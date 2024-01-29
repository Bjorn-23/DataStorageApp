using Infrastructure.Entities;

namespace Business.Dtos;

public class OrderRowDto
{
    public int Id { get; set; }

    public int Quantity { get; set; }

    public decimal OrderRowPrice { get; set; }

    public string ArticleNumber { get; set; } = null!;

    public int OrderId { get; set; }

    public virtual ProductEntity ArticleNumberNavigation { get; set; } = null!; // Do I need this?

    public virtual OrderEntity Order { get; set; } = null!; // Do I need this?
}

