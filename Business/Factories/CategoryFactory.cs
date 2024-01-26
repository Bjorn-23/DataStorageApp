using Business.Dtos;
using Infrastructure.Entities;
using System.Diagnostics;

namespace Business.Factories;

public static class CategoryFactory
{
    public static CategoryDto Create(CategoryEntity entity)
    {
        try
        {
            return new CategoryDto
            {
                CategoryName = entity.CategoryName,
            };
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

    public static CategoryEntity Create(CategoryDto dto)
    {
        try
        {
            return new CategoryEntity
            {
                CategoryName = dto.CategoryName,
            };
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

}
