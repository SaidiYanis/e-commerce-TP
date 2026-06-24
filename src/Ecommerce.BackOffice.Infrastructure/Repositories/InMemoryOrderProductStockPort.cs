using Ecommerce.BackOffice.Catalog.Ports;
using Ecommerce.BackOffice.Orders.Ports;
using Ecommerce.BackOffice.SharedKernel.Results;

namespace Ecommerce.BackOffice.Infrastructure.Repositories;

public sealed class InMemoryOrderProductStockPort : IOrderProductStockPort
{
    private readonly IProductRepository _productRepository;

    public InMemoryOrderProductStockPort(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<OrderProductSnapshot?> GetByIdAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetByIdAsync(productId, cancellationToken);
        if (product is null)
        {
            return null;
        }

        return new OrderProductSnapshot(product.Id, product.CurrentUnitPrice, product.Stock);
    }

    public async Task<Result> DecreaseStockAsync(Guid productId, int quantity, CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetByIdAsync(productId, cancellationToken);
        if (product is null)
        {
            return Result.Failure("Product not found.");
        }

        var decreaseResult = product.DecreaseStock(quantity);
        if (decreaseResult.IsFailure)
        {
            return decreaseResult;
        }

        await _productRepository.UpdateAsync(product, cancellationToken);
        return Result.Success();
    }
}
