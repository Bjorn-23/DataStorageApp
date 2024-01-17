using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Entities;

public class CategoryEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string CategoryName { get; set; } = null!;
    public ICollection<ProductEntity> Product { get; set; } = new HashSet<ProductEntity>();
}