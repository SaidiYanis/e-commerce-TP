namespace Ecommerce.BackOffice.Catalog.Application.Products;

public sealed record CreateProductCommand(
    string Title,
    string Description,
    decimal Price,
    decimal? PromotionalPrice,
    int Stock,
    Guid CategoryId);
