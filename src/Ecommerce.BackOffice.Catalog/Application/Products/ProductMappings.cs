using Ecommerce.BackOffice.Catalog.Domain;

namespace Ecommerce.BackOffice.Catalog.Application.Products;

internal static class ProductMappings
{
    public static ProductDto ToDto(this Product product) =>
        new(
            product.Id,
            product.Title,
            product.Description,
            product.Price,
            product.PromotionalPrice,
            product.Stock,
            product.CategoryId,
            product.CurrentUnitPrice);
}
