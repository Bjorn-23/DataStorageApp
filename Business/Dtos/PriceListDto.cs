namespace Business.Dtos;

public class PriceListDto
{
    public int Id { get; set; }

    public decimal Price { get; set; }

    public decimal? DiscountPrice { get; set; }

    public string UnitType { get; set; } = null!;
}
