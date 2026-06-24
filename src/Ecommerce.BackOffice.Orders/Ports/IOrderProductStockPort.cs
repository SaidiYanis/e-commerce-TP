using Ecommerce.BackOffice.SharedKernel.Results;

namespace Ecommerce.BackOffice.Orders.Ports;

public interface IOrderProductStockPort
{
    Task<OrderProductSnapshot?> GetByIdAsync(Guid productId, CancellationToken cancellationToken = default);

    Task<Result> DecreaseStockAsync(Guid productId, int quantity, CancellationToken cancellationToken = default);
}
