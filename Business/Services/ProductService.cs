using Business.Dtos;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using System.Diagnostics;

namespace Business.Services;

public class ProductService
{
    private readonly IProductRepository _productRepository;
    private readonly CategoryService _categoryService;
    private readonly PriceListService _priceListService;


    public ProductService(IProductRepository productRepository, CategoryService categoryService, PriceListService priceListService)
    {
        _productRepository = productRepository;
        _categoryService = categoryService;
        _priceListService = priceListService;
    }

    public ProductDto CreateProduct(ProductRegistrationDto product) // REFACTOR WITH FACTORIES!!
    {
        try
        {
            var existingProduct = _productRepository.GetOne(x => x.ArticleNumber == product.ArticleNumber);
            if (existingProduct != null)
            {
                return Factories.ProductFactory.Create(existingProduct);
            }
            else
            {
                var categoryName = _categoryService.GetOrCreateCategory(product);
                var priceId = _priceListService.GetOrCreatePriceList(product);
                
                var newProduct = _productRepository.Create(new ProductEntity()
                {
                    ArticleNumber = product.ArticleNumber,
                    Title = product.Title,
                    Ingress = product.Ingress,
                    Description = product.Description,
                    PriceId = priceId.Id,
                    Unit = product.Unit,
                    Stock = product.Stock,
                    CategoryName = categoryName.CategoryName

                });
                if (newProduct != null)
                {
                    return Factories.ProductFactory.Create(newProduct);
                }
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }


}
