using Business.Dtos;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using System.Diagnostics;

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

    public CategoryEntity GetOrCreateCategory(CategoryDto category) // REFACTOR WITH FACTORIES!!
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
                return newCategoryName;
            }
            else
                return exisitingCategoryName;
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }
}
