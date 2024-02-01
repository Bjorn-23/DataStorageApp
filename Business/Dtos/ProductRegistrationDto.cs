namespace Business.Dtos
{
    public class ProductRegistrationDto
    {
        public string ArticleNumber { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string? Ingress { get; set; }
        public string? Description { get; set; }
        public string CategoryName { get; set; } = null!;
        public string Unit { get; set; } = null!;
        public int Stock { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; } = null!;
        public decimal? DiscountPrice { get; set; }
    }
}