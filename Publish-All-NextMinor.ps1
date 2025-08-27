# Publish-All-NextMinor.ps1
Set-StrictMode -Version Latest





$ErrorActionPreference = 'Stop'

# --- Config: bump all three ---
$projectNames = @('Maturix','Maturix.Sample','Maturix.Tests')
$nugetSource  = 'https://api.nuget.org/v3/index.json'

function Get-ApiKeyFromUserSecrets($maturixCsprojPath) {
    $list = & dotnet user-secrets --project $maturixCsprojPath list
    foreach ($line in $list) {
        if ($line -match 'NuGet:ApiKey\s*=\s*(.+)') { return $Matches[1].Trim() }
    }
    throw 'NuGet:ApiKey not found in Maturix user-secrets.'
}

function Bump-Minor([string]$versionText) {
    if ([string]::IsNullOrWhiteSpace($versionText)) { $versionText = '0.0.0' }
    $parts = $versionText.Split('.')
    if ($parts.Count -lt 2) { throw "Invalid version '$versionText'." }
    $major = [int]$parts[0]
    $minor = [int]$parts[1]
    '{0}.{1}.0' -f $major, ($minor + 1)
}

function Get-Or-Create-VersionNode([xml]$xml) {
    # namespace-agnostic selection of <Project>/<PropertyGroup>/<Version>
    $versionNode = $xml.SelectSingleNode('/*[local-name()="Project"]/*[local-name()="PropertyGroup"]/*[local-name()="Version"]')
    if ($versionNode) { return $versionNode }

    $projectNode = $xml.SelectSingleNode('/*[local-name()="Project"]')
    if (-not $projectNode) { throw 'Invalid csproj: <Project> not found.' }

    $pg = $xml.SelectSingleNode('/*[local-name()="Project"]/*[local-name()="PropertyGroup"]')
    if (-not $pg) {
        $pg = $xml.CreateElement('PropertyGroup', $projectNode.NamespaceURI)
        [void]$projectNode.AppendChild($pg)
    }

    $versionNode = $xml.CreateElement('Version', $projectNode.NamespaceURI)
    $versionNode.InnerText = '0.0.0'
    [void]$pg.AppendChild($versionNode)
    $versionNode
}

# Find Maturix.csproj to read API key scope
$maturixCsproj = Get-ChildItem -Recurse -Filter 'Maturix.csproj' | Select-Object -First 1
if (-not $maturixCsproj) { throw 'Maturix.csproj not found.' }
$apiKey = Get-ApiKeyFromUserSecrets $maturixCsproj.FullName

$packagesToPush = @()

foreach ($name in $projectNames) {
    $csproj = Get-ChildItem -Recurse -Filter "$name.csproj" | Select-Object -First 1
    if (-not $csproj) { Write-Warning "Skipping $name (csproj not found)."; continue }

    [xml]$xml = Get-Content -LiteralPath $csproj.FullName

    # read IsPackable (default true for libraries)
    $isPackableNode = $xml.SelectSingleNode('/*[local-name()="Project"]/*[local-name()="PropertyGroup"]/*[local-name()="IsPackable"]')
    $isPackable = $true
    if ($isPackableNode -and $isPackableNode.InnerText.Trim()) {
        $isPackable = [System.Convert]::ToBoolean($isPackableNode.InnerText.Trim())
    }

    # ensure <Version> exists and bump (uses your Get-Or-Create-VersionNode + Bump-Minor)
    $versionNode = Get-Or-Create-VersionNode $xml
    $oldVersion = ($versionNode.InnerText ?? '').Trim()
    $newVersion = Bump-Minor $oldVersion
    $versionNode.InnerText = $newVersion
    $xml.Save($csproj.FullName)
    Write-Host "[$name] $oldVersion -> $newVersion"

    # restore & build first so pack finds the DLL
    & dotnet restore $csproj.FullName | Out-Host
    & dotnet build   $csproj.FullName -c Release -v minimal | Out-Host

    if (-not $isPackable) {
        Write-Host "[$name] IsPackable=false -> skipping pack."
        continue
    }

    # determine PackageId (defaults to project name)
    $pkgIdNode = $xml.SelectSingleNode('/*[local-name()="Project"]/*[local-name()="PropertyGroup"]/*[local-name()="PackageId"]')
    $packageId = if ($pkgIdNode -and $pkgIdNode.InnerText.Trim()) { $pkgIdNode.InnerText.Trim() } else { $name }

    # pack without rebuilding
    & dotnet pack $csproj.FullName -c Release --no-build | Out-Host

    $pkg = Join-Path $csproj.DirectoryName ("bin\Release\{0}.{1}.nupkg" -f $packageId, $newVersion)
    if (Test-Path $pkg) { $packagesToPush += $pkg } else { Write-Host "[$name] Pack produced no nupkg." }
}


if ($packagesToPush.Count -eq 0) { Write-Host 'No packages to push.'; exit 0 }

foreach ($pkg in $packagesToPush) {
    Write-Host "Pushing $([IO.Path]::GetFileName($pkg)) ..."
    & dotnet nuget push $pkg --api-key $apiKey --source $nugetSource --skip-duplicate | Out-Host
}

Write-Host 'Done.'