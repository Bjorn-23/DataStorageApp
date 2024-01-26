using System.Globalization;

namespace Business.Dtos;

public class ProductDto
{
    public string ArticleNumber { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string? Ingress { get; set; }
    public string? Description { get; set; }
    public int PriceId {  get; set; }
    public string Unit { get; set; } = null!;
    public int Stock {  get; set; }
    public string CategoryName { get; set; } = null!;
}
