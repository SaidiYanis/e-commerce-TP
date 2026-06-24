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
}
