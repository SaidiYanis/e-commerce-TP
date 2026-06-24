using Ecommerce.BackOffice.Catalog.Domain;

namespace Ecommerce.BackOffice.Catalog.Application.Categories;

internal static class CategoryMappings
{
    public static CategoryDto ToDto(this Category category) =>
        new(category.Id, category.Title, category.Description, category.Color);
}
