using Ecommerce.BackOffice.SharedKernel.Abstractions;
using Ecommerce.BackOffice.SharedKernel.Results;

namespace Ecommerce.BackOffice.Catalog.Domain;

public sealed class Product : IEntity<Guid>
{
    private Product(
        Guid id,
        string title,
        string description,
        decimal price,
        decimal? promotionalPrice,
        int stock,
        Guid categoryId)
    {
        Id = id;
        Title = title;
        Description = description;
        Price = price;
        PromotionalPrice = promotionalPrice;
        Stock = stock;
        CategoryId = categoryId;
    }

    public Guid Id { get; }

    public string Title { get; private set; }

    public string Description { get; private set; }

    public decimal Price { get; private set; }

    public decimal? PromotionalPrice { get; private set; }

    public int Stock { get; private set; }

    public Guid CategoryId { get; private set; }

    public decimal CurrentUnitPrice => PromotionalPrice ?? Price;

    public static Result<Product> Create(
        Guid id,
        string title,
        string description,
        decimal price,
        decimal? promotionalPrice,
        int stock,
        Guid categoryId)
    {
        var validation = Validate(title, description, price, promotionalPrice, stock, categoryId);
        if (validation.IsFailure)
        {
            return Result<Product>.Failure(validation.Error!);
        }

        if (id == Guid.Empty)
        {
            return Result<Product>.Failure("Product id is required.");
        }

        return Result<Product>.Success(new Product(
            id,
            title.Trim(),
            description.Trim(),
            price,
            promotionalPrice,
            stock,
            categoryId));
    }

    public Result Update(
        string title,
        string description,
        decimal price,
        decimal? promotionalPrice,
        int stock,
        Guid categoryId)
    {
        var validation = Validate(title, description, price, promotionalPrice, stock, categoryId);
        if (validation.IsFailure)
        {
            return validation;
        }

        Title = title.Trim();
        Description = description.Trim();
        Price = price;
        PromotionalPrice = promotionalPrice;
        Stock = stock;
        CategoryId = categoryId;

        return Result.Success();
    }

    public Result DecreaseStock(int quantity)
    {
        if (quantity <= 0)
        {
            return Result.Failure("Stock decrement quantity must be greater than 0.");
        }

        if (Stock < quantity)
        {
            return Result.Failure("Insufficient stock.");
        }

        Stock -= quantity;
        return Result.Success();
    }

    private static Result Validate(
        string title,
        string description,
        decimal price,
        decimal? promotionalPrice,
        int stock,
        Guid categoryId)
    {
        if (string.IsNullOrWhiteSpace(title) || title.Trim().Length < 3)
        {
            return Result.Failure("Product title must contain at least 3 characters.");
        }

        if (title.Trim().Length > 100)
        {
            return Result.Failure("Product title must contain at most 100 characters.");
        }

        if (string.IsNullOrWhiteSpace(description) || description.Trim().Length < 3)
        {
            return Result.Failure("Product description must contain at least 3 characters.");
        }

        if (description.Trim().Length > 500)
        {
            return Result.Failure("Product description must contain at most 500 characters.");
        }

        if (price <= 0m)
        {
            return Result.Failure("Product price must be greater than 0.");
        }

        if (price >= 30000m)
        {
            return Result.Failure("Product price must be lower than 30000.");
        }

        if (promotionalPrice.HasValue && promotionalPrice.Value >= price)
        {
            return Result.Failure("Promotional price must be lower than price.");
        }

        if (stock < 0)
        {
            return Result.Failure("Product stock cannot be negative.");
        }

        if (categoryId == Guid.Empty)
        {
            return Result.Failure("Product category is required.");
        }

        return Result.Success();
    }
}
