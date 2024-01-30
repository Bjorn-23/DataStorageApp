using Business.Dtos;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using System.Diagnostics;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Business.Services;

public class CategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public CategoryEntity GetOrCreateCategory(ProductRegistrationDto product)
    {
        try
        {
            var exisitingCategoryName = _categoryRepository.GetOne(x => x.CategoryName == product.CategoryName);
            if (exisitingCategoryName == null)
            {
                var newCategoryName = _categoryRepository.Create(new CategoryEntity()
                {
                    CategoryName = product.CategoryName
                });
                return newCategoryName;
            }
            else
                return exisitingCategoryName;
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

    public CategoryDto GetOrCreateCategory(CategoryDto category)
    {
        try
        {
            var exisitingCategoryName = _categoryRepository.GetOne(x => x.CategoryName == category.CategoryName);
            if (exisitingCategoryName == null)
            {
                var newCategoryName = _categoryRepository.Create(new CategoryEntity()
                {
                    CategoryName = category.CategoryName
                });
                return Factories.CategoryFactory.Create(newCategoryName);
            }
            else
                return Factories.CategoryFactory.Create(exisitingCategoryName);
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }
    
    public IEnumerable<CategoryDto> GetAllCategories()
    {
        try
        {
            var result = _categoryRepository.GetAll();
            if (result != null)
                return Factories.CategoryFactory.Create(result);
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return new List<CategoryDto>();
    }

    public CategoryDto UpdateCategory(CategoryDto existingDtoName, CategoryDto updatedDtoName)
    {
        try
        {
            var existingCategory = _categoryRepository.GetOne(x => x.CategoryName == existingDtoName.CategoryName);
            if (existingCategory != null)
            {
                CategoryEntity updatedEntity = new()
                {
                    Id = existingCategory.Id,
                    CategoryName = !string.IsNullOrEmpty(updatedDtoName.CategoryName) ? updatedDtoName.CategoryName : existingDtoName.CategoryName,
                };

                var result = _categoryRepository.Update(existingCategory, updatedEntity);
                if (result != null)
                {
                    return Factories.CategoryFactory.Create(result);
                }
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

    public CategoryDto DeleteCategory(CategoryDto category)
    {
        try
        {
            var existingCategory = _categoryRepository.GetOne(x => x.CategoryName == category.CategoryName && x.Id == category.Id);
            if (existingCategory != null)
            {
                var result = _categoryRepository.Delete(existingCategory);
                if (result)
                {
                    return Factories.CategoryFactory.Create(existingCategory);
                }
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

}
