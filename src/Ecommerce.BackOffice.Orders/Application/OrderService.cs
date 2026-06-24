using Ecommerce.BackOffice.Orders.Ports;
using Ecommerce.BackOffice.SharedKernel.Results;

namespace Ecommerce.BackOffice.Orders.Application;

public sealed class OrderService
{
    private readonly IOrderProductStockPort _productStockPort;
    private readonly IOrderRepository _orderRepository;

    public OrderService(IOrderProductStockPort productStockPort, IOrderRepository orderRepository)
    {
        _productStockPort = productStockPort;
        _orderRepository = orderRepository;
    }

    public async Task<IReadOnlyCollection<OrderDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var orders = await _orderRepository.GetAllAsync(cancellationToken);
        return orders.Select(order => order.ToDto()).ToArray();
    }

    public async Task<OrderDto?> GetByIdAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        var order = await _orderRepository.GetByIdAsync(orderId, cancellationToken);
        return order?.ToDto();
    }

    public async Task<Result<OrderDto>> CreateAsync(CreateOrderCommand command, CancellationToken cancellationToken = default)
    {
        var product = await _productStockPort.GetByIdAsync(command.ProductId, cancellationToken);
        if (product is null)
        {
            return Result<OrderDto>.Failure("Product not found.");
        }

        if (product.Stock <= 0)
        {
            return Result<OrderDto>.Failure("Product is out of stock.");
        }

        var orderResult = Domain.Order.Create(Guid.NewGuid(), DateTime.UtcNow, product.Id, product.CurrentUnitPrice);
        if (orderResult.IsFailure)
        {
            return Result<OrderDto>.Failure(orderResult.Error!);
        }

        await _orderRepository.AddAsync(orderResult.Value!, cancellationToken);
        return Result<OrderDto>.Success(orderResult.Value!.ToDto());
    }

    public async Task<Result<OrderDto>> AddProductAsync(Guid orderId, AddOrderLineCommand command, CancellationToken cancellationToken = default)
    {
        var order = await _orderRepository.GetByIdAsync(orderId, cancellationToken);
        if (order is null)
        {
            return Result<OrderDto>.Failure("Order not found.");
        }

        var product = await _productStockPort.GetByIdAsync(command.ProductId, cancellationToken);
        if (product is null)
        {
            return Result<OrderDto>.Failure("Product not found.");
        }

        var existingQuantity = order.Lines
            .Where(line => line.ProductId == command.ProductId)
            .Select(line => line.Quantity)
            .SingleOrDefault();

        if (product.Stock <= existingQuantity)
        {
            return Result<OrderDto>.Failure("Product is out of stock.");
        }

        var addResult = order.AddProduct(product.Id, product.CurrentUnitPrice);
        if (addResult.IsFailure)
        {
            return Result<OrderDto>.Failure(addResult.Error!);
        }

        await _orderRepository.UpdateAsync(order, cancellationToken);
        return Result<OrderDto>.Success(order.ToDto());
    }

    public async Task<Result<OrderDto>> PayAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        var order = await _orderRepository.GetByIdAsync(orderId, cancellationToken);
        if (order is null)
        {
            return Result<OrderDto>.Failure("Order not found.");
        }

        foreach (var line in order.Lines)
        {
            var product = await _productStockPort.GetByIdAsync(line.ProductId, cancellationToken);
            if (product is null)
            {
                return Result<OrderDto>.Failure("Product not found.");
            }

            if (product.Stock < line.Quantity)
            {
                return Result<OrderDto>.Failure("Insufficient stock at payment time.");
            }
        }

        var payResult = order.Pay(DateTime.UtcNow);
        if (payResult.IsFailure)
        {
            return Result<OrderDto>.Failure(payResult.Error!);
        }

        foreach (var line in order.Lines)
        {
            var stockResult = await _productStockPort.DecreaseStockAsync(line.ProductId, line.Quantity, cancellationToken);
            if (stockResult.IsFailure)
            {
                return Result<OrderDto>.Failure(stockResult.Error!);
            }
        }

        await _orderRepository.UpdateAsync(order, cancellationToken);
        return Result<OrderDto>.Success(order.ToDto());
    }

    public async Task<Result<OrderDto>> CancelAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        var order = await _orderRepository.GetByIdAsync(orderId, cancellationToken);
        if (order is null)
        {
            return Result<OrderDto>.Failure("Order not found.");
        }

        var cancelResult = order.Cancel(DateTime.UtcNow);
        if (cancelResult.IsFailure)
        {
            return Result<OrderDto>.Failure(cancelResult.Error!);
        }

        await _orderRepository.UpdateAsync(order, cancellationToken);
        return Result<OrderDto>.Success(order.ToDto());
    }

    public async Task<Result<OrderDto>> DeliverAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        var order = await _orderRepository.GetByIdAsync(orderId, cancellationToken);
        if (order is null)
        {
            return Result<OrderDto>.Failure("Order not found.");
        }

        var deliverResult = order.Deliver();
        if (deliverResult.IsFailure)
        {
            return Result<OrderDto>.Failure(deliverResult.Error!);
        }

        await _orderRepository.UpdateAsync(order, cancellationToken);
        return Result<OrderDto>.Success(order.ToDto());
    }
}
