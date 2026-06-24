namespace Ecommerce.BackOffice.Catalog.Application.Categories;

public sealed record CreateCategoryCommand(
    string Title,
    string Description,
    string Color);
