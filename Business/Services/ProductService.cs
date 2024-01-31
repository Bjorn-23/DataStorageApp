using Business.Dtos;
using Business.Factories;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Migrations;
using System.Diagnostics;
using System.Linq.Expressions;

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
            var existingProduct = _productRepository.GetOne(x => x.ArticleNumber == dto.ArticleNumber);

            if (existingProduct != null!)
            {
                return new ProductRegistrationDto()
                {
                    ArticleNumber = existingProduct.ArticleNumber,
                    Title = existingProduct.Title,
                    Ingress = existingProduct.Ingress,
                    Description = existingProduct.Description,
                    Price = existingProduct.Price.Price,
                    Currency = existingProduct.Price.UnitType,
                    DiscountPrice = existingProduct.Price.DiscountPrice,
                    Unit = existingProduct.Unit,
                    Stock = existingProduct.Stock,
                    CategoryName = existingProduct.CategoryName
                };
            }

            //var priceId = _priceListService.GetPriceList(dto);
            //if (priceId.Price >= 1 && priceId.UnitType != null!)
            //{
            //    return new ProductRegistrationDto()
            //    {
            //        ArticleNumber = dto.ArticleNumber,
            //        Title = dto.Title,
            //        Ingress = dto.Ingress,
            //        Description = dto.Description,
            //        Price = priceId.Price,
            //        Currency = priceId.UnitType,
            //        DiscountPrice = priceId.DiscountPrice,
            //        Unit = dto.Unit,
            //        Stock = dto.Stock,
            //        CategoryName = dto.CategoryName
            //    };
            //}
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
        
    }

    public IEnumerable<ProductRegistrationDto> GetAllProducts()
    {
        try
        {
            var products = _productRepository.GetAll();
            if (products.Any())
            {
                List<ProductRegistrationDto> productList = new();
                foreach (var product in products)
                {
                    ProductRegistrationDto productDto = new()
                    {
                    ArticleNumber = product.ArticleNumber,
                    Title = product.Title,
                    Ingress = product.Ingress,
                    Description = product.Description,
                    Price = product.Price.Price,
                    Currency = product.Price.UnitType,
                    DiscountPrice = product.Price.DiscountPrice,
                    Unit = product.Unit,
                    Stock = product.Stock,
                    CategoryName = product.CategoryName
                    };

                    productList.Add(productDto);
                
                }
                return productList;
            }

        }
         catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return new List<ProductRegistrationDto>();
    }

    public ProductDto UpdateProduct(ProductRegistrationDto product) // Requires user to be logged in and have "Admin" as UserRoleName
    {
        try
        {
            var checkRole = _userService.FindRoleOfActiveUser();
            if (checkRole.UserRoleName == "Admin")
            {
                var existingProduct = _productRepository.GetOne(x => x.ArticleNumber == product.ArticleNumber);
                var productStock = existingProduct.Stock;
                if (existingProduct == null)
                {
                    return null!;
                }
                else
                {
                    productStock += product.Stock;

                    var categoryName = _categoryService.GetOrCreateCategory(!string.IsNullOrWhiteSpace(product.CategoryName) ? product : new ProductRegistrationDto() { CategoryName = existingProduct.CategoryName });
                    var priceId = _priceListService.GetOrCreatePriceList(product);

                    ProductEntity updatedProduct = new()
                    {
                        ArticleNumber = existingProduct.ArticleNumber,
                        Title = !string.IsNullOrWhiteSpace(product.Title) ? product.Title : existingProduct.Title,
                        Ingress = !string.IsNullOrWhiteSpace(product.Ingress) ? product.Ingress : existingProduct.Ingress,
                        Description = !string.IsNullOrWhiteSpace(product.Description) ? product.Description : existingProduct.Description,
                        PriceId = priceId != null ? priceId.Id : existingProduct.PriceId,
                        Unit = !string.IsNullOrWhiteSpace(product.Unit) ? product.Unit : existingProduct.Unit,
                        Stock = productStock,
                        CategoryName = categoryName != null ? categoryName.CategoryName : existingProduct.CategoryName,

                    };

                    var result = _productRepository.Update(existingProduct, updatedProduct);
                    if (result != null)
                    {
                        return Factories.ProductFactory.Create(updatedProduct);
                    }
                }
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

    public ProductDto DeleteProduct(ProductDto product)
    {
        try
        {
            var checkRole = _userService.FindRoleOfActiveUser();
            if (checkRole.UserRoleName == "Admin")
            {
                var existingProduct = _productRepository.GetOne(x => x.ArticleNumber == product.ArticleNumber);
                if (existingProduct != null)
                {
                    var result = _productRepository.Delete(existingProduct);
                    if (result)
                    {
                        return Factories.ProductFactory.Create(existingProduct);
                    }
                }            
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }
}
