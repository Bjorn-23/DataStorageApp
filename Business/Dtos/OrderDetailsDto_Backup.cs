using Infrastructure.Entities;

namespace Business.Dtos;

public class OrderDetailsDto_Backup
{
    public int OrderId { get; set; }

    public DateTime? OrderDate { get; set; }

    public decimal OrderPrice { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public int OrderRowId { get; set; }

    public int OrderRowQuantity { get; set; }

    public decimal OrderRowPrice { get; set; }

    public string ArticleNumber { get; set; } = null!;

    public virtual ProductEntity ArticleNumberNavigation { get; set; } = null!;

    public virtual CustomerEntity CustomerNavigation { get; private set; } = null!;
}
