using Ecommerce.BackOffice.SharedKernel.Abstractions;
using Ecommerce.BackOffice.SharedKernel.Results;

namespace Ecommerce.BackOffice.Catalog.Domain;

public sealed class Category : IEntity<Guid>
{
    private Category(Guid id, string title, string description, string color)
    {
        Id = id;
        Title = title;
        Description = description;
        Color = color;
    }

    public Guid Id { get; }

    public string Title { get; private set; }

    public string Description { get; private set; }

    public string Color { get; private set; }

    public static Result<Category> Create(Guid id, string title, string description, string color)
    {
        var validation = Validate(title, description, color);
        if (validation.IsFailure)
        {
            return Result<Category>.Failure(validation.Error!);
        }

        if (id == Guid.Empty)
        {
            return Result<Category>.Failure("Category id is required.");
        }

        return Result<Category>.Success(new Category(
            id,
            title.Trim(),
            description.Trim(),
            color.Trim()));
    }

    public Result Update(string title, string description, string color)
    {
        var validation = Validate(title, description, color);
        if (validation.IsFailure)
        {
            return validation;
        }

        Title = title.Trim();
        Description = description.Trim();
        Color = color.Trim();

        return Result.Success();
    }

    private static Result Validate(string title, string description, string color)
    {
        if (string.IsNullOrWhiteSpace(title) || title.Trim().Length < 3)
        {
            return Result.Failure("Category title must contain at least 3 characters.");
        }

        if (title.Trim().Length > 100)
        {
            return Result.Failure("Category title must contain at most 100 characters.");
        }

        if (string.IsNullOrWhiteSpace(description) || description.Trim().Length < 3)
        {
            return Result.Failure("Category description must contain at least 3 characters.");
        }

        if (description.Trim().Length > 500)
        {
            return Result.Failure("Category description must contain at most 500 characters.");
        }

        if (string.IsNullOrWhiteSpace(color))
        {
            return Result.Failure("Category color is required.");
        }

        return Result.Success();
    }
}
