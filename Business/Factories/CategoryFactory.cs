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

    public static IEnumerable<CategoryDto> Create(IEnumerable<CategoryEntity> entities)
    {
        try
        {
            List<CategoryDto> list = new();

            foreach (var entity in entities)
            {
                list.Add(Create(entity));
            }

            return list;
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }


}
