namespace Ecommerce.BackOffice.Catalog.Application.Products;

public sealed record ProductDto(
    Guid Id,
    string Title,
    string Description,
    decimal Price,
    decimal? PromotionalPrice,
    int Stock,
    Guid CategoryId,
    decimal CurrentUnitPrice);
