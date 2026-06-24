using Ecommerce.BackOffice.Catalog.Ports;
using Ecommerce.BackOffice.SharedKernel.Results;

namespace Ecommerce.BackOffice.Catalog.Application.Categories;

public sealed class CategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<IReadOnlyCollection<CategoryDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var categories = await _categoryRepository.GetAllAsync(cancellationToken);
        return categories.Select(category => category.ToDto()).ToArray();
    }

    public async Task<CategoryDto?> GetByIdAsync(Guid categoryId, CancellationToken cancellationToken = default)
    {
        var category = await _categoryRepository.GetByIdAsync(categoryId, cancellationToken);
        return category?.ToDto();
    }

    public async Task<Result<CategoryDto>> CreateAsync(CreateCategoryCommand command, CancellationToken cancellationToken = default)
    {
        var categoryResult = Domain.Category.Create(Guid.NewGuid(), command.Title, command.Description, command.Color);
        if (categoryResult.IsFailure)
        {
            return Result<CategoryDto>.Failure(categoryResult.Error!);
        }

        await _categoryRepository.AddAsync(categoryResult.Value!, cancellationToken);

        return Result<CategoryDto>.Success(categoryResult.Value!.ToDto());
    }

    public async Task<Result<CategoryDto>> UpdateAsync(Guid categoryId, UpdateCategoryCommand command, CancellationToken cancellationToken = default)
    {
        var category = await _categoryRepository.GetByIdAsync(categoryId, cancellationToken);
        if (category is null)
        {
            return Result<CategoryDto>.Failure("Category not found.");
        }

        var updateResult = category.Update(command.Title, command.Description, command.Color);
        if (updateResult.IsFailure)
        {
            return Result<CategoryDto>.Failure(updateResult.Error!);
        }

        await _categoryRepository.UpdateAsync(category, cancellationToken);
        return Result<CategoryDto>.Success(category.ToDto());
    }

    public async Task<Result> DeleteAsync(Guid categoryId, CancellationToken cancellationToken = default)
    {
        var exists = await _categoryRepository.ExistsAsync(categoryId, cancellationToken);
        if (!exists)
        {
            return Result.Failure("Category not found.");
        }

        await _categoryRepository.DeleteAsync(categoryId, cancellationToken);
        return Result.Success();
    }
}
