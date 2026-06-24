namespace Ecommerce.BackOffice.Catalog.Application.Products;

public sealed record UpdateProductCommand(
    string Title,
    string Description,
    decimal Price,
    decimal? PromotionalPrice,
    int Stock,
    Guid CategoryId);
