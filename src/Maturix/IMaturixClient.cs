using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Maturix.Models;
using OneOf;

namespace Maturix
{
    /// <summary>
    /// Defines the contract for a client capable of retrieving data from the
    /// Maturix API. Consumers should depend on this interface rather than
    /// directly instantiating <see cref="MaturixClient"/> to enable
    /// substitutability and testing.
    /// </summary>
    public interface IMaturixClient
    {
        /// <summary>
        /// Retrieves all quality reports for a specific location.
        /// </summary>
        /// <param name="locationId">The identifier of the location for which to retrieve reports.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
        /// <returns>
        /// A task that resolves to a <see cref="OneOf{T0, T1}"/> containing either
        /// a read‑only list of <see cref="QualityReport"/> objects or an
        /// <see cref="ApiError"/> if the operation fails.
        /// </returns>
        Task<OneOf<IReadOnlyList<QualityReport>, ApiError>> GetQualityReportsAsync(
            string locationId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves statistics and sensor data for a specific production unit.
        /// </summary>
        /// <param name="productionId">The identifier of the production unit.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
        /// <returns>
        /// A task that resolves to a <see cref="OneOf{T0, T1}"/> containing either
        /// a <see cref="ProductionUnitDashboard"/> or an <see cref="ApiError"/>.
        /// </returns>
        Task<OneOf<ProductionUnitDashboard, ApiError>> GetProductionUnitDashboardAsync(
            string productionId,
            CancellationToken cancellationToken = default);
    }
}