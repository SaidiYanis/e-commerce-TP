using Ecommerce.BackOffice.Catalog.Ports;
using Ecommerce.BackOffice.SharedKernel.Results;

namespace Ecommerce.BackOffice.Catalog.Application.Products;

public sealed class ProductService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IProductRepository _productRepository;

    public ProductService(ICategoryRepository categoryRepository, IProductRepository productRepository)
    {
        _categoryRepository = categoryRepository;
        _productRepository = productRepository;
    }

    public async Task<IReadOnlyCollection<ProductDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var products = await _productRepository.GetAllAsync(cancellationToken);
        return products.Select(product => product.ToDto()).ToArray();
    }

    public async Task<ProductDto?> GetByIdAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetByIdAsync(productId, cancellationToken);
        return product?.ToDto();
    }

    public async Task<Result<ProductDto>> CreateAsync(CreateProductCommand command, CancellationToken cancellationToken = default)
    {
        var categoryExists = await _categoryRepository.ExistsAsync(command.CategoryId, cancellationToken);
        if (!categoryExists)
        {
            return Result<ProductDto>.Failure("Product category must exist.");
        }

        var productResult = Domain.Product.Create(
            Guid.NewGuid(),
            command.Title,
            command.Description,
            command.Price,
            command.PromotionalPrice,
            command.Stock,
            command.CategoryId);

        if (productResult.IsFailure)
        {
            return Result<ProductDto>.Failure(productResult.Error!);
        }

        await _productRepository.AddAsync(productResult.Value!, cancellationToken);
        return Result<ProductDto>.Success(productResult.Value!.ToDto());
    }

    public async Task<Result<ProductDto>> UpdateAsync(Guid productId, UpdateProductCommand command, CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetByIdAsync(productId, cancellationToken);
        if (product is null)
        {
            return Result<ProductDto>.Failure("Product not found.");
        }

        var categoryExists = await _categoryRepository.ExistsAsync(command.CategoryId, cancellationToken);
        if (!categoryExists)
        {
            return Result<ProductDto>.Failure("Product category must exist.");
        }

        var updateResult = product.Update(
            command.Title,
            command.Description,
            command.Price,
            command.PromotionalPrice,
            command.Stock,
            command.CategoryId);

        if (updateResult.IsFailure)
        {
            return Result<ProductDto>.Failure(updateResult.Error!);
        }

        await _productRepository.UpdateAsync(product, cancellationToken);
        return Result<ProductDto>.Success(product.ToDto());
    }

    public async Task<Result> DeleteAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        var exists = await _productRepository.ExistsAsync(productId, cancellationToken);
        if (!exists)
        {
            return Result.Failure("Product not found.");
        }

        await _productRepository.DeleteAsync(productId, cancellationToken);
        return Result.Success();
    }
}
