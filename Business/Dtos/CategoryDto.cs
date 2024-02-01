using Infrastructure.Entities;

namespace Business.Dtos;

public class CategoryDto
{
    public int Id { get; set; }

    public string CategoryName { get; set; } = null!;

    public virtual ICollection<ProductEntity> Products { get; set; } = new List<ProductEntity>();
}