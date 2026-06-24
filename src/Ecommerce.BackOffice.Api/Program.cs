using Ecommerce.BackOffice.Catalog.Application.Categories;
using Ecommerce.BackOffice.Catalog.Application.Products;
using Ecommerce.BackOffice.Catalog.Ports;
using Ecommerce.BackOffice.Infrastructure.Repositories;
using Ecommerce.BackOffice.Orders.Application;
using Ecommerce.BackOffice.Orders.Ports;
using Ecommerce.BackOffice.SharedKernel.Results;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ICategoryRepository, InMemoryCategoryRepository>();
builder.Services.AddSingleton<IProductRepository, InMemoryProductRepository>();
builder.Services.AddSingleton<IOrderRepository, InMemoryOrderRepository>();
builder.Services.AddSingleton<IOrderProductStockPort, InMemoryOrderProductStockPort>();

builder.Services.AddSingleton<CategoryService>();
builder.Services.AddSingleton<ProductService>();
builder.Services.AddSingleton<OrderService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => Results.Ok(new
{
    Name = "Ecommerce BackOffice API",
    Modules = new[] { "Catalog", "Orders" }
}));

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
    return ToTypedHttpResult(result, dto => $"/categories/{dto.Id}");
});

categories.MapPut("/{id:guid}", async (Guid id, UpdateCategoryCommand command, CategoryService service, CancellationToken cancellationToken) =>
{
    var result = await service.UpdateAsync(id, command, cancellationToken);
    return ToTypedHttpResult(result);
});

categories.MapDelete("/{id:guid}", async (Guid id, CategoryService service, CancellationToken cancellationToken) =>
{
    var result = await service.DeleteAsync(id, cancellationToken);
    return ToHttpResult(result);
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
    return ToTypedHttpResult(result, dto => $"/products/{dto.Id}");
});

products.MapPut("/{id:guid}", async (Guid id, UpdateProductCommand command, ProductService service, CancellationToken cancellationToken) =>
{
    var result = await service.UpdateAsync(id, command, cancellationToken);
    return ToTypedHttpResult(result);
});

products.MapDelete("/{id:guid}", async (Guid id, ProductService service, CancellationToken cancellationToken) =>
{
    var result = await service.DeleteAsync(id, cancellationToken);
    return ToHttpResult(result);
});

var orders = app.MapGroup("/orders");
orders.MapGet("/", async (OrderService service, CancellationToken cancellationToken) =>
{
    var result = await service.GetAllAsync(cancellationToken);
    return Results.Ok(result);
});

orders.MapGet("/{id:guid}", async (Guid id, OrderService service, CancellationToken cancellationToken) =>
{
    var result = await service.GetByIdAsync(id, cancellationToken);
    return result is null ? Results.NotFound() : Results.Ok(result);
});

orders.MapPost("/", async (CreateOrderCommand command, OrderService service, CancellationToken cancellationToken) =>
{
    var result = await service.CreateAsync(command, cancellationToken);
    return ToTypedHttpResult(result, dto => $"/orders/{dto.Id}");
});

orders.MapPost("/{id:guid}/lines", async (Guid id, AddOrderLineCommand command, OrderService service, CancellationToken cancellationToken) =>
{
    var result = await service.AddProductAsync(id, command, cancellationToken);
    return ToTypedHttpResult(result);
});

orders.MapPost("/{id:guid}/pay", async (Guid id, OrderService service, CancellationToken cancellationToken) =>
{
    var result = await service.PayAsync(id, cancellationToken);
    return ToTypedHttpResult(result);
});

orders.MapPost("/{id:guid}/cancel", async (Guid id, OrderService service, CancellationToken cancellationToken) =>
{
    var result = await service.CancelAsync(id, cancellationToken);
    return ToTypedHttpResult(result);
});

orders.MapPost("/{id:guid}/deliver", async (Guid id, OrderService service, CancellationToken cancellationToken) =>
{
    var result = await service.DeliverAsync(id, cancellationToken);
    return ToTypedHttpResult(result);
});

app.Run();

static IResult ToHttpResult(Result result)
{
    if (result.IsSuccess)
    {
        return Results.NoContent();
    }

    if (result.Error?.Contains("not found", StringComparison.OrdinalIgnoreCase) == true)
    {
        return Results.NotFound(new { error = result.Error });
    }

    return Results.BadRequest(new { error = result.Error });
}

static IResult ToTypedHttpResult<T>(Result<T> result, Func<T, string>? locationFactory = null)
{
    if (result.IsFailure || result.Value is null)
    {
        if (result.Error?.Contains("not found", StringComparison.OrdinalIgnoreCase) == true)
        {
            return Results.NotFound(new { error = result.Error });
        }

        return Results.BadRequest(new { error = result.Error });
    }

    return locationFactory is null
        ? Results.Ok(result.Value)
        : Results.Created(locationFactory(result.Value), result.Value);
}
