using System.ComponentModel.DataAnnotations.Schema;

namespace Business.Dtos;

public class OrderDto
{
    public int Id { get; set; }

    public DateTime? OrderDate { get; set; }

    public decimal OrderPrice { get; set; }

    public string CustomerId { get; set; } = null!;
}
