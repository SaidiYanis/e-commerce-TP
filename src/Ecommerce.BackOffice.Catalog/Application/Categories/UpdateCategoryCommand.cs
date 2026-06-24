namespace Ecommerce.BackOffice.Catalog.Application.Categories;

public sealed record UpdateCategoryCommand(
    string Title,
    string Description,
    string Color);
