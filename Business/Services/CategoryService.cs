using Business.Dtos;
using Business.Factories;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using System.Diagnostics;

namespace Business.Services;

public class CategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly UserService _userService;

    public CategoryService(ICategoryRepository categoryRepository, UserService userService)
    {
        _categoryRepository = categoryRepository;
        _userService = userService;
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

    public CategoryDto CreateCategory(CategoryDto category)
    {
        try
        {
            var activeUser = _userService.FindRoleOfActiveUser();
            var exisitingCategoryName = _categoryRepository.GetOne(x => x.CategoryName == category.CategoryName);
            if (exisitingCategoryName == null && activeUser.UserRoleName == "Admin")
            {
                var newCategoryName = _categoryRepository.Create(new CategoryEntity()
                {
                    CategoryName = category.CategoryName
                });
                return CategoryFactory.Create(newCategoryName);
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

    public CategoryDto GetCategory(CategoryDto category)
    {
        try
        {
            var exisitingCategory = _categoryRepository.GetOne(x => x.Id == category.Id);
            if (exisitingCategory != null)
            {
                return CategoryFactory.Create(exisitingCategory);
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

    public IEnumerable<CategoryDto> GetAllCategories()
    {
        try
        {
            var exisitingCategories = _categoryRepository.GetAll();
            if (exisitingCategories != null)
                return CategoryFactory.Create(exisitingCategories);
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return new List<CategoryDto>();
    }

    public CategoryDto UpdateCategory(CategoryDto existingDtoName, CategoryDto updatedDtoName)
    {
        try
        {
            var activeUser = _userService.FindRoleOfActiveUser();
            var existingCategory = _categoryRepository.GetOne(x => x.Id == existingDtoName.Id);
            if (existingCategory != null && activeUser.UserRoleName == "Admin")
            {
                CategoryEntity updatedEntity = new()
                {
                    Id = existingCategory.Id,
                    CategoryName = !string.IsNullOrEmpty(updatedDtoName.CategoryName) ? updatedDtoName.CategoryName : existingCategory.CategoryName,
                };

                var result = _categoryRepository.Update(existingCategory, updatedEntity);
                if (result != null)
                {
                    return CategoryFactory.Create(result);
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
            var activeUser = _userService.FindRoleOfActiveUser();
            var existingCategory = _categoryRepository.GetOne(x => x.CategoryName == category.CategoryName && x.Id == category.Id);
            if (existingCategory != null && activeUser.UserRoleName == "Admin")
            {
                var result = _categoryRepository.Delete(existingCategory);
                if (result)
                {
                    return CategoryFactory.Create(existingCategory);
                }
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

}
