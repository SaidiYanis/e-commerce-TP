using Ecommerce.BackOffice.Catalog.Domain;

namespace Ecommerce.BackOffice.Catalog.Tests;

public sealed class CategoryAndProductDomainTests
{
    [Fact]
    public void Given_CategoryTitleTooShort_When_Create_Then_FailureIsReturned()
    {
        var result = Category.Create(Guid.NewGuid(), "ab", "Valid description", "blue");

        Assert.True(result.IsFailure);
        Assert.Equal("Category title must contain at least 3 characters.", result.Error);
    }

    [Fact]
    public void Given_ValidCategory_When_Create_Then_CategoryIsCreated()
    {
        var result = Category.Create(Guid.NewGuid(), "Tech", "Technology products", "blue");

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal("Tech", result.Value!.Title);
    }

    [Fact]
    public void Given_CategoryDescriptionTooShort_When_Create_Then_FailureIsReturned()
    {
        var result = Category.Create(Guid.NewGuid(), "Tech", "ab", "blue");

        Assert.True(result.IsFailure);
        Assert.Equal("Category description must contain at least 3 characters.", result.Error);
    }

    [Fact]
    public void Given_CategoryTitleTooLong_When_Create_Then_FailureIsReturned()
    {
        var result = Category.Create(Guid.NewGuid(), new string('a', 101), "Valid description", "blue");

        Assert.True(result.IsFailure);
        Assert.Equal("Category title must contain at most 100 characters.", result.Error);
    }

    [Fact]
    public void Given_CategoryDescriptionTooLong_When_Create_Then_FailureIsReturned()
    {
        var result = Category.Create(Guid.NewGuid(), "Tech", new string('a', 501), "blue");

        Assert.True(result.IsFailure);
        Assert.Equal("Category description must contain at most 500 characters.", result.Error);
    }

    [Fact]
    public void Given_CategoryColorEmpty_When_Create_Then_FailureIsReturned()
    {
        var result = Category.Create(Guid.NewGuid(), "Tech", "Valid description", string.Empty);

        Assert.True(result.IsFailure);
        Assert.Equal("Category color is required.", result.Error);
    }

    [Fact]
    public void Given_PromotionalPriceGreaterThanPrice_When_CreateProduct_Then_FailureIsReturned()
    {
        var result = Product.Create(
            Guid.NewGuid(),
            "Laptop",
            "A powerful laptop",
            1000m,
            1200m,
            5,
            Guid.NewGuid());

        Assert.True(result.IsFailure);
        Assert.Equal("Promotional price must be lower than price.", result.Error);
    }

    [Fact]
    public void Given_NegativeStock_When_CreateProduct_Then_FailureIsReturned()
    {
        var result = Product.Create(
            Guid.NewGuid(),
            "Laptop",
            "A powerful laptop",
            1000m,
            900m,
            -1,
            Guid.NewGuid());

        Assert.True(result.IsFailure);
        Assert.Equal("Product stock cannot be negative.", result.Error);
    }

    [Fact]
    public void Given_ProductPriceAtZero_When_CreateProduct_Then_FailureIsReturned()
    {
        var result = Product.Create(
            Guid.NewGuid(),
            "Laptop",
            "A powerful laptop",
            0m,
            null,
            5,
            Guid.NewGuid());

        Assert.True(result.IsFailure);
        Assert.Equal("Product price must be greater than 0.", result.Error);
    }

    [Fact]
    public void Given_ProductPriceAt30000_When_CreateProduct_Then_FailureIsReturned()
    {
        var result = Product.Create(
            Guid.NewGuid(),
            "Laptop",
            "A powerful laptop",
            30000m,
            null,
            5,
            Guid.NewGuid());

        Assert.True(result.IsFailure);
        Assert.Equal("Product price must be lower than 30000.", result.Error);
    }

    [Fact]
    public void Given_ProductPromotionalPriceEqualToPrice_When_CreateProduct_Then_FailureIsReturned()
    {
        var result = Product.Create(
            Guid.NewGuid(),
            "Laptop",
            "A powerful laptop",
            1000m,
            1000m,
            5,
            Guid.NewGuid());

        Assert.True(result.IsFailure);
        Assert.Equal("Promotional price must be lower than price.", result.Error);
    }

    [Fact]
    public void Given_ProductTitleTooLong_When_CreateProduct_Then_FailureIsReturned()
    {
        var result = Product.Create(
            Guid.NewGuid(),
            new string('a', 101),
            "A powerful laptop",
            1000m,
            null,
            5,
            Guid.NewGuid());

        Assert.True(result.IsFailure);
        Assert.Equal("Product title must contain at most 100 characters.", result.Error);
    }

    [Fact]
    public void Given_ProductDescriptionTooLong_When_CreateProduct_Then_FailureIsReturned()
    {
        var result = Product.Create(
            Guid.NewGuid(),
            "Laptop",
            new string('a', 501),
            1000m,
            null,
            5,
            Guid.NewGuid());

        Assert.True(result.IsFailure);
        Assert.Equal("Product description must contain at most 500 characters.", result.Error);
    }

    [Fact]
    public void Given_ProductCategoryEmpty_When_CreateProduct_Then_FailureIsReturned()
    {
        var result = Product.Create(
            Guid.NewGuid(),
            "Laptop",
            "A powerful laptop",
            1000m,
            null,
            5,
            Guid.Empty);

        Assert.True(result.IsFailure);
        Assert.Equal("Product category is required.", result.Error);
    }
}
