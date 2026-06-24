using Ecommerce.BackOffice.SharedKernel.Abstractions;
using Ecommerce.BackOffice.SharedKernel.Results;

namespace Ecommerce.BackOffice.Orders.Domain;

public sealed class Order : IEntity<Guid>
{
    private readonly List<OrderLine> _lines = new();

    private Order(Guid id, DateTime createdAt, OrderLine firstLine)
    {
        Id = id;
        CreatedAt = createdAt;
        Status = OrderStatus.Cart;
        _lines.Add(firstLine);
    }

    public Guid Id { get; }

    public DateTime CreatedAt { get; }

    public OrderStatus Status { get; private set; }

    public DateTime? PaidAt { get; private set; }

    public DateTime? CancelledAt { get; private set; }

    public IReadOnlyCollection<OrderLine> Lines => _lines.AsReadOnly();

    public decimal TotalPrice => _lines.Sum(line => line.TotalPrice);

    public static Result<Order> Create(Guid id, DateTime createdAt, Guid productId, decimal unitPrice)
    {
        var lineResult = CreateLine(productId, unitPrice);
        if (lineResult.IsFailure)
        {
            return Result<Order>.Failure(lineResult.Error!);
        }

        if (id == Guid.Empty)
        {
            return Result<Order>.Failure("Order id is required.");
        }

        return Result<Order>.Success(new Order(id, createdAt, lineResult.Value!));
    }

    public Result AddProduct(Guid productId, decimal unitPrice)
    {
        if (Status != OrderStatus.Cart)
        {
            return Result.Failure("Only a cart can be modified.");
        }

        var lineResult = CreateLine(productId, unitPrice);
        if (lineResult.IsFailure)
        {
            return lineResult;
        }

        var existingLine = _lines.SingleOrDefault(line => line.ProductId == productId);
        if (existingLine is not null)
        {
            if (!existingLine.CanIncrement())
            {
                return Result.Failure("A product can be added at most 2 times in the same order.");
            }

            existingLine.Increment();
            return Result.Success();
        }

        if (_lines.Count >= 5)
        {
            return Result.Failure("An order can contain at most 5 different products.");
        }

        _lines.Add(lineResult.Value!);
        return Result.Success();
    }

    public Result Pay(DateTime paidAt)
    {
        if (Status != OrderStatus.Cart)
        {
            return Result.Failure("Only a cart can be paid.");
        }

        Status = OrderStatus.Paid;
        PaidAt = paidAt;

        return Result.Success();
    }

    public Result Cancel(DateTime cancelledAt)
    {
        if (Status != OrderStatus.Cart)
        {
            return Result.Failure("Only a cart can be cancelled.");
        }

        Status = OrderStatus.Cancelled;
        CancelledAt = cancelledAt;

        return Result.Success();
    }

    public Result Deliver()
    {
        if (Status != OrderStatus.Paid)
        {
            return Result.Failure("Only a paid order can be delivered.");
        }

        Status = OrderStatus.Delivered;
        return Result.Success();
    }

    private static Result<OrderLine> CreateLine(Guid productId, decimal unitPrice)
    {
        if (productId == Guid.Empty)
        {
            return Result<OrderLine>.Failure("Order line product id is required.");
        }

        if (unitPrice <= 0m)
        {
            return Result<OrderLine>.Failure("Order line unit price must be greater than 0.");
        }

        return Result<OrderLine>.Success(new OrderLine(productId, unitPrice, 1));
    }
}
