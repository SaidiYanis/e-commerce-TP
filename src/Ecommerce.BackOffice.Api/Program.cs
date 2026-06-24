using System.Text.Json.Serialization;
using Ecommerce.BackOffice.Api.Endpoints;
using Ecommerce.BackOffice.Orders.Domain;
using Ecommerce.BackOffice.Catalog.Ports;
using Ecommerce.BackOffice.Infrastructure.Repositories;
using Ecommerce.BackOffice.Orders.Ports;
using Ecommerce.BackOffice.Catalog.Application.Categories;
using Ecommerce.BackOffice.Catalog.Application.Products;
using Ecommerce.BackOffice.Orders.Application;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.MapType<OrderStatus>(() => new OpenApiSchema
    {
        Type = "string",
        Enum = Enum.GetNames<OrderStatus>()
            .Select(name => (IOpenApiAny)new OpenApiString(name))
            .ToList()
    });
});

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

app.MapCatalogEndpoints();
app.MapOrderEndpoints();

app.Run();
