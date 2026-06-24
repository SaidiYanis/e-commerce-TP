namespace Ecommerce.BackOffice.Orders.Domain;

public sealed class OrderLine
{
    internal OrderLine(Guid productId, decimal unitPrice, int quantity)
    {
        ProductId = productId;
        UnitPrice = unitPrice;
        Quantity = quantity;
    }

    public Guid ProductId { get; }

    public decimal UnitPrice { get; }

    public int Quantity { get; private set; }

    public decimal TotalPrice => UnitPrice * Quantity;

    internal bool CanIncrement() => Quantity < 2;

    internal void Increment() => Quantity++;
}
