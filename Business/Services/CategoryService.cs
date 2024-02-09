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

    /// <summary>
    /// Checks for CategoryName in database, returns if exists, else creates new CategoryEntity.
    /// </summary>
    /// <param name="product"></param>
    /// <returns>CategoryEntity/returns>
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

    /// <summary>
    /// Creates a new CategoryEntity in database. Requires Rolename of activeUser == "Admin".
    /// </summary>
    /// <param name="category"></param>
    /// <returns>CategoryDto</returns>
    public CategoryDto CreateCategory(CategoryDto category)
    {
        try
        {
            var activeUser = _userService.isUserActive();
            var exisitingCategoryName = _categoryRepository.GetOne(x => x.CategoryName == category.CategoryName);
            if (exisitingCategoryName == null && activeUser.UserRole.RoleName == "Admin")
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

    /// <summary>
    /// Checks database for matching category and returns it.
    /// </summary>
    /// <param name="category"></param>
    /// <returns>CategoryDto</returns>
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

    /// <summary>
    /// Checks database for categories and returns all entries.
    /// </summary>
    /// <returns>List of CategoryDto</returns>
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

    /// <summary>
    /// Updates existing categoryEntity in database with new values. 
    /// </summary>
    /// <param name="existingDtoName"></param>
    /// <param name="updatedDtoName"></param>
    /// <returnsCategoryDto></returns>
    public CategoryDto UpdateCategory(CategoryDto existingDtoName, CategoryDto updatedDtoName)
    {
        try
        {
            var activeUser = _userService.isUserActive();
            var existingCategory = _categoryRepository.GetOne(x => x.Id == existingDtoName.Id);
            if (existingCategory != null && activeUser.UserRole.RoleName == "Admin")
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

    /// <summary>
    /// Deletes existing category from database.
    /// </summary>
    /// <param name="category"></param>
    /// <returns>CategoryDto</returns>
    public CategoryDto DeleteCategory(CategoryDto category)
    {
        try
        {
            var activeUser = _userService.isUserActive();
            var existingCategory = _categoryRepository.GetOne(x => x.CategoryName == category.CategoryName && x.Id == category.Id);
            if (existingCategory != null && activeUser.UserRole.RoleName == "Admin")
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
