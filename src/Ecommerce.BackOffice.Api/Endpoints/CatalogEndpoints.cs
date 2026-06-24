using Ecommerce.BackOffice.Api.Http;
using Ecommerce.BackOffice.Catalog.Application.Categories;
using Ecommerce.BackOffice.Catalog.Application.Products;

namespace Ecommerce.BackOffice.Api.Endpoints;

public static class CatalogEndpoints
{
    public static IEndpointRouteBuilder MapCatalogEndpoints(this IEndpointRouteBuilder app)
    {
        var categories = app.MapGroup("/categories");
        categories.MapGet("/", async (CategoryService service, CancellationToken cancellationToken) =>
        {
            var result = await service.GetAllAsync(cancellationToken);
            return Results.Ok(result);
        });

        categories.MapGet("/{id:guid}", async (Guid id, CategoryService service, CancellationToken cancellationToken) =>
        {
            var result = await service.GetByIdAsync(id, cancellationToken);
            return result is null ? Results.NotFound() : Results.Ok(result);
        });

        categories.MapPost("/", async (CreateCategoryCommand command, CategoryService service, CancellationToken cancellationToken) =>
        {
            var result = await service.CreateAsync(command, cancellationToken);
            return result.ToHttpResult(dto => $"/categories/{dto.Id}");
        });

        categories.MapPut("/{id:guid}", async (Guid id, UpdateCategoryCommand command, CategoryService service, CancellationToken cancellationToken) =>
        {
            var result = await service.UpdateAsync(id, command, cancellationToken);
            return result.ToHttpResult();
        });

        categories.MapDelete("/{id:guid}", async (Guid id, CategoryService service, CancellationToken cancellationToken) =>
        {
            var result = await service.DeleteAsync(id, cancellationToken);
            return result.ToHttpResult();
        });

        var products = app.MapGroup("/products");
        products.MapGet("/", async (ProductService service, CancellationToken cancellationToken) =>
        {
            var result = await service.GetAllAsync(cancellationToken);
            return Results.Ok(result);
        });

        products.MapGet("/{id:guid}", async (Guid id, ProductService service, CancellationToken cancellationToken) =>
        {
            var result = await service.GetByIdAsync(id, cancellationToken);
            return result is null ? Results.NotFound() : Results.Ok(result);
        });

        products.MapPost("/", async (CreateProductCommand command, ProductService service, CancellationToken cancellationToken) =>
        {
            var result = await service.CreateAsync(command, cancellationToken);
            return result.ToHttpResult(dto => $"/products/{dto.Id}");
        });

        products.MapPut("/{id:guid}", async (Guid id, UpdateProductCommand command, ProductService service, CancellationToken cancellationToken) =>
        {
            var result = await service.UpdateAsync(id, command, cancellationToken);
            return result.ToHttpResult();
        });

        products.MapDelete("/{id:guid}", async (Guid id, ProductService service, CancellationToken cancellationToken) =>
        {
            var result = await service.DeleteAsync(id, cancellationToken);
            return result.ToHttpResult();
        });

        return app;
    }
}
