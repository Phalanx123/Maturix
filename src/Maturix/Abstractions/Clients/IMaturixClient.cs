using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Maturix.Clients;
using Maturix.Models;
using OneOf;

namespace Maturix.Abstractions.Clients
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
        /// Creates a new <see cref="IMaturixClient"/> instance scoped to a specific location.
        /// </summary>
        /// <param name="locationId"></param>
        /// <returns></returns>
        IMaturixClient ForLocation(string locationId);
        
        /// <summary>
        /// Retrieves all quality reports for a specific location.
        /// </summary>
        /// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
        /// <returns>
        /// A task that resolves to a <see cref="OneOf{T0, T1}"/> containing either
        /// a read‑only list of <see cref="QualityReport"/> objects or an
        /// <see cref="ApiError"/> if the operation fails.
        /// </returns>
        Task<OneOf<IReadOnlyList<QualityReport>, ApiError>> GetQualityReportsAsync(
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves statistics and sensor data for a specific production unit.
        /// </summary>
        /// <param name="productionId">The identifier of the production unit.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
        /// <returns>
        /// A task that resolves to a <see cref="OneOf{T0, T1}"/> containing either
        /// a <see cref="ProductionStats"/> or an <see cref="ApiError"/>.
        /// </returns>
        Task<OneOf<ProductionUnit, ApiError>> GetProductionUnitAsync(
            string productionId,
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Retrieves all sensors.
        /// </summary>
        /// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
        /// <returns>
        /// A task that resolves to a <see cref="OneOf{T0, T1}"/> containing either
        /// a <see cref="Sensor"/> or an <see cref="ApiError"/>.
        /// </returns>
        Task<OneOf<IReadOnlyList<Sensor>, ApiError>> GetSensorsAsync(
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves production data from sensors.
        /// </summary>
        /// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
        /// <returns>
        /// A task that resolves to a <see cref="OneOf{T0, T1}"/> containing either
        /// a <see cref="SensorProductionData"/> or an <see cref="ApiError"/>.
        /// </returns>
        Task<OneOf<IReadOnlyList<SensorProductionData>, ApiError>> GetSensorProductionData(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Retrieves all compounds.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>
        /// A task that resolves to a <see cref="OneOf{T0, T1}"/> containing either
        /// a read‑only list of <see cref="Compound"/> objects or an
        /// <see cref="ApiError"/> if the operation fails.
        /// </returns>
        Task<OneOf<IReadOnlyList<Compound>, ApiError>> GetCompoundsAsync(CancellationToken cancellationToken = default);
    }
}