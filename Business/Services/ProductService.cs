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
    private readonly UserService _userService;


    public ProductService(IProductRepository productRepository, CategoryService categoryService, PriceListService priceListService, UserService userService)
    {
        _productRepository = productRepository;
        _categoryService = categoryService;
        _priceListService = priceListService;
        _userService = userService;
    }

    public ProductDto CreateProduct(ProductRegistrationDto product) // Requires user to be logged in and have "Admin" as UserRoleName
    {
        try
        {
            var checkRole = _userService.FindRoleOfActiveUser();
            if (checkRole.UserRoleName == "Admin")
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
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

    public ProductRegistrationDto GetProductDisplay(ProductDto dto)
    {
        try
        {
            var priceId = _priceListService.GetPriceList(dto);
            if (priceId.Price >= 1 && priceId.UnitType != null!)
            {
                return new ProductRegistrationDto()
                {
                    ArticleNumber = dto.ArticleNumber,
                    Title = dto.Title,
                    Ingress = dto.Ingress,
                    Description = dto.Description,
                    Price = priceId.Price,
                    Currency = priceId.UnitType,
                    DiscountPrice = priceId.DiscountPrice,
                    Unit = dto.Unit,
                    Stock = dto.Stock,
                    CategoryName = dto.CategoryName
                };
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
        
    }

    public IEnumerable<ProductRegistrationDto> GetAllProducts()
    {
        List<ProductRegistrationDto> productList = new();

        var products = _productRepository.GetAll();
        foreach (var product in products)
        {
            var priceId = _priceListService.GetPriceList(product);
            var result = Factories.ProductFactory.Create(product, priceId.UnitType, priceId.Price, priceId.DiscountPrice);
            productList.Add(result);
        }
        return productList;
    }
}
