using Ecommerce.BackOffice.Orders.Domain;

namespace Ecommerce.BackOffice.Orders.Tests;

public sealed class OrderDomainTests
{
    [Fact]
    public void Given_ExistingLine_When_AddingSameProductAgain_Then_QuantityIsIncremented()
    {
        var productId = Guid.NewGuid();
        var order = Order.Create(Guid.NewGuid(), DateTime.UtcNow, productId, 100m).Value!;

        var result = order.AddProduct(productId, 200m);

        Assert.True(result.IsSuccess);
        Assert.Single(order.Lines);
        Assert.Equal(2, order.Lines.Single().Quantity);
        Assert.Equal(100m, order.Lines.Single().UnitPrice);
    }

    [Fact]
    public void Given_ProductAlreadyAddedTwice_When_AddingSameProductAgain_Then_FailureIsReturned()
    {
        var productId = Guid.NewGuid();
        var order = Order.Create(Guid.NewGuid(), DateTime.UtcNow, productId, 100m).Value!;
        order.AddProduct(productId, 100m);

        var result = order.AddProduct(productId, 100m);

        Assert.True(result.IsFailure);
        Assert.Equal("A product can be added at most 2 times in the same order.", result.Error);
    }

    [Fact]
    public void Given_CartWithFiveDifferentProducts_When_AddingSixthProduct_Then_FailureIsReturned()
    {
        var order = Order.Create(Guid.NewGuid(), DateTime.UtcNow, Guid.NewGuid(), 10m).Value!;

        order.AddProduct(Guid.NewGuid(), 20m);
        order.AddProduct(Guid.NewGuid(), 30m);
        order.AddProduct(Guid.NewGuid(), 40m);
        order.AddProduct(Guid.NewGuid(), 50m);

        var result = order.AddProduct(Guid.NewGuid(), 60m);

        Assert.True(result.IsFailure);
        Assert.Equal("An order can contain at most 5 different products.", result.Error);
    }

    [Fact]
    public void Given_PaidOrder_When_Deliver_Then_StatusBecomesDelivered()
    {
        var order = Order.Create(Guid.NewGuid(), DateTime.UtcNow, Guid.NewGuid(), 100m).Value!;
        order.Pay(DateTime.UtcNow);

        var result = order.Deliver();

        Assert.True(result.IsSuccess);
        Assert.Equal(OrderStatus.Delivered, order.Status);
    }

    [Fact]
    public void Given_CancelledOrder_When_Pay_Then_FailureIsReturned()
    {
        var order = Order.Create(Guid.NewGuid(), DateTime.UtcNow, Guid.NewGuid(), 100m).Value!;
        order.Cancel(DateTime.UtcNow);

        var result = order.Pay(DateTime.UtcNow);

        Assert.True(result.IsFailure);
        Assert.Equal("Only a cart can be paid.", result.Error);
    }

    [Fact]
    public void Given_PaidOrder_When_PayAgain_Then_FailureIsReturned()
    {
        var order = Order.Create(Guid.NewGuid(), DateTime.UtcNow, Guid.NewGuid(), 100m).Value!;
        order.Pay(DateTime.UtcNow);

        var result = order.Pay(DateTime.UtcNow);

        Assert.True(result.IsFailure);
        Assert.Equal("Only a cart can be paid.", result.Error);
    }

    [Fact]
    public void Given_PaidOrder_When_Cancel_Then_FailureIsReturned()
    {
        var order = Order.Create(Guid.NewGuid(), DateTime.UtcNow, Guid.NewGuid(), 100m).Value!;
        order.Pay(DateTime.UtcNow);

        var result = order.Cancel(DateTime.UtcNow);

        Assert.True(result.IsFailure);
        Assert.Equal("Only a cart can be cancelled.", result.Error);
    }

    [Fact]
    public void Given_CartOrder_When_Deliver_Then_FailureIsReturned()
    {
        var order = Order.Create(Guid.NewGuid(), DateTime.UtcNow, Guid.NewGuid(), 100m).Value!;

        var result = order.Deliver();

        Assert.True(result.IsFailure);
        Assert.Equal("Only a paid order can be delivered.", result.Error);
    }

    [Fact]
    public void Given_PaidOrder_When_AddingProduct_Then_FailureIsReturned()
    {
        var order = Order.Create(Guid.NewGuid(), DateTime.UtcNow, Guid.NewGuid(), 100m).Value!;
        order.Pay(DateTime.UtcNow);

        var result = order.AddProduct(Guid.NewGuid(), 50m);

        Assert.True(result.IsFailure);
        Assert.Equal("Only a cart can be modified.", result.Error);
    }
}
