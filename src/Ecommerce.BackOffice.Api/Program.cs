using Ecommerce.BackOffice.Api.Endpoints;
using Ecommerce.BackOffice.Catalog.Ports;
using Ecommerce.BackOffice.Infrastructure.Repositories;
using Ecommerce.BackOffice.Orders.Ports;
using Ecommerce.BackOffice.Catalog.Application.Categories;
using Ecommerce.BackOffice.Catalog.Application.Products;
using Ecommerce.BackOffice.Orders.Application;

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

app.MapCatalogEndpoints();
app.MapOrderEndpoints();

app.Run();
