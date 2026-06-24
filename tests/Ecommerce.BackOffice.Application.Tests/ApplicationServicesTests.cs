using Ecommerce.BackOffice.Catalog.Application.Categories;
using Ecommerce.BackOffice.Catalog.Application.Products;
using Ecommerce.BackOffice.Catalog.Ports;
using Ecommerce.BackOffice.Infrastructure.Repositories;
using Ecommerce.BackOffice.Orders.Application;
using Ecommerce.BackOffice.Orders.Ports;

namespace Ecommerce.BackOffice.Application.Tests;

public sealed class ApplicationServicesTests
{
    [Fact]
    public async Task Given_MissingCategory_When_CreatingProduct_Then_FailureIsReturned()
    {
        ICategoryRepository categoryRepository = new InMemoryCategoryRepository();
        IProductRepository productRepository = new InMemoryProductRepository();
        var service = new ProductService(categoryRepository, productRepository);

        var result = await service.CreateAsync(new CreateProductCommand(
            "Laptop",
            "A powerful laptop",
            1000m,
            900m,
            3,
            Guid.NewGuid()));

        Assert.True(result.IsFailure);
        Assert.Equal("Product category must exist.", result.Error);
    }

    [Fact]
    public async Task Given_OutOfStockProduct_When_CreatingOrder_Then_FailureIsReturned()
    {
        IProductRepository productRepository = new InMemoryProductRepository();
        IOrderRepository orderRepository = new InMemoryOrderRepository();
        IOrderProductStockPort stockPort = new InMemoryOrderProductStockPort(productRepository);

        var product = Ecommerce.BackOffice.Catalog.Domain.Product.Create(
            Guid.NewGuid(),
            "Laptop",
            "A powerful laptop",
            1000m,
            null,
            0,
            Guid.NewGuid()).Value!;

        await productRepository.AddAsync(product);

        var service = new OrderService(stockPort, orderRepository);
        var result = await service.CreateAsync(new CreateOrderCommand(product.Id));

        Assert.True(result.IsFailure);
        Assert.Equal("Product is out of stock.", result.Error);
    }

    [Fact]
    public async Task Given_PaidOrder_When_PayingOrder_Then_ProductStockIsDecremented()
    {
        ICategoryRepository categoryRepository = new InMemoryCategoryRepository();
        IProductRepository productRepository = new InMemoryProductRepository();
        IOrderRepository orderRepository = new InMemoryOrderRepository();
        IOrderProductStockPort stockPort = new InMemoryOrderProductStockPort(productRepository);

        var categoryService = new CategoryService(categoryRepository);
        var category = await categoryService.CreateAsync(new CreateCategoryCommand("Tech", "Technology", "blue"));

        var productService = new ProductService(categoryRepository, productRepository);
        var product = await productService.CreateAsync(new CreateProductCommand(
            "Laptop",
            "A powerful laptop",
            1000m,
            900m,
            2,
            category.Value!.Id));

        var orderService = new OrderService(stockPort, orderRepository);
        var order = await orderService.CreateAsync(new CreateOrderCommand(product.Value!.Id));

        var payment = await orderService.PayAsync(order.Value!.Id);
        var storedProduct = await productRepository.GetByIdAsync(product.Value!.Id);

        Assert.True(payment.IsSuccess);
        Assert.NotNull(storedProduct);
        Assert.Equal(1, storedProduct!.Stock);
    }

    [Fact]
    public async Task Given_StockBecomesInsufficientAfterCartCreation_When_PayingOrder_Then_FailureIsReturned()
    {
        ICategoryRepository categoryRepository = new InMemoryCategoryRepository();
        IProductRepository productRepository = new InMemoryProductRepository();
        IOrderRepository orderRepository = new InMemoryOrderRepository();
        IOrderProductStockPort stockPort = new InMemoryOrderProductStockPort(productRepository);

        var categoryService = new CategoryService(categoryRepository);
        var category = await categoryService.CreateAsync(new CreateCategoryCommand("Tech", "Technology", "blue"));

        var productService = new ProductService(categoryRepository, productRepository);
        var product = await productService.CreateAsync(new CreateProductCommand(
            "Laptop",
            "A powerful laptop",
            1000m,
            null,
            1,
            category.Value!.Id));

        var orderService = new OrderService(stockPort, orderRepository);
        var order = await orderService.CreateAsync(new CreateOrderCommand(product.Value!.Id));

        var storedProduct = await productRepository.GetByIdAsync(product.Value!.Id);
        Assert.NotNull(storedProduct);

        var stockUpdate = storedProduct!.DecreaseStock(1);
        Assert.True(stockUpdate.IsSuccess);
        await productRepository.UpdateAsync(storedProduct);

        var payment = await orderService.PayAsync(order.Value!.Id);

        Assert.True(payment.IsFailure);
        Assert.Equal("Insufficient stock at payment time.", payment.Error);
    }
}
