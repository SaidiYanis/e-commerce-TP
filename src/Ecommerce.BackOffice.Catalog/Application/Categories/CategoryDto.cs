namespace Ecommerce.BackOffice.Catalog.Application.Categories;

public sealed record CategoryDto(
    Guid Id,
    string Title,
    string Description,
    string Color);
