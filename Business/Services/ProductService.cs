using Business.Dtos;
using Business.Factories;
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

    /// <summary>
    /// Checks if logged in User is Admin and that a product with the sarticle number input doesnt exist in database. If true creates new product.
    /// </summary>
    /// <param name="product"></param>
    /// <returns>ProductDto</returns>
    public ProductDto CreateProduct(ProductRegistrationDto product) // Requires user to be logged in and have "Admin" as UserRoleName
    {
        try
        {
            var checkRole = _userService.isUserActive();
            if (checkRole.UserRole.RoleName == "Admin")
            {
                var existingProduct = _productRepository.GetOne(x => x.ArticleNumber == product.ArticleNumber);
                if (existingProduct != null)
                {
                    return ProductFactory.Create(existingProduct);
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
                        CategoryId = categoryName.Id

                    });
                    if (newProduct != null)
                    {
                        return ProductFactory.Create(newProduct);
                    }
                }
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }
    
    /// <summary>
    /// Gets a product from database..
    /// </summary>
    /// <param name="dto"></param>
    /// <returns>ProductRegistrationDto</returns>
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
                    CategoryName = existingProduct.Category.CategoryName
                };
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;        
    }

    /// <summary>
    /// Gets all products from database.
    /// </summary>
    /// <returns>List of ProductRegistrationDto</returns>
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
                    CategoryName = product.Category.CategoryName
                    };

                    productList.Add(productDto);
                
                }
                return productList;
            }

        }
         catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return new List<ProductRegistrationDto>();
    }

    /// <summary>
    /// Checks if User is Admin and that products exists in database, if true updates product with new details.
    /// </summary>
    /// <param name="product"></param>
    /// <returns></returns>
    public ProductDto UpdateProduct(ProductRegistrationDto product) // Requires user to be logged in and have "Admin" as UserRoleName
    {
        try
        {
            var checkRole = _userService.isUserActive();
            if (checkRole !=null && checkRole.UserRole.RoleName == "Admin")
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

                    var categoryName = _categoryService.GetOrCreateCategory(!string.IsNullOrWhiteSpace(product.CategoryName) ? product : new ProductRegistrationDto() { CategoryName = existingProduct.Category.CategoryName });
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
                        CategoryId = categoryName != null ? categoryName.Id : existingProduct.Category.Id,

                    };

                    var result = _productRepository.Update(existingProduct, updatedProduct);
                    if (result != null)
                    {
                        return ProductFactory.Create(updatedProduct);
                    }
                }
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

    /// <summary>
    /// Updates a products stock property if it exists in database.
    /// </summary>
    /// <param name="product"></param>
    /// <returns>ProductDto</returns>
    public ProductDto UpdateProductStock(ProductRegistrationDto product) // Requires user to be logged in and have "Admin" as UserRoleName
    {
        try
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

                ProductEntity updatedProduct = new()
                {
                    ArticleNumber = existingProduct.ArticleNumber,
                    Title = existingProduct.Title,
                    Ingress = existingProduct.Ingress,
                    Description = existingProduct.Description,
                    PriceId = existingProduct.PriceId,
                    Unit = existingProduct.Unit,
                    Stock = productStock,
                    CategoryId = existingProduct.Category.Id,

                };

                var result = _productRepository.Update(existingProduct, updatedProduct);
                if (result != null)
                {
                    return ProductFactory.Create(updatedProduct);
                }
            }
            
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

    /// <summary>
    /// Checks if User is Admin and that products exists in database, if true deletes.
    /// </summary>
    /// <param name="product"></param>
    /// <returns></returns>
    public ProductDto DeleteProduct(ProductDto product)
    {
        try
        {
            var checkRole = _userService.isUserActive();
            if (checkRole.UserRole.RoleName == "Admin")
            {
                var existingProduct = _productRepository.GetOne(x => x.ArticleNumber == product.ArticleNumber);
                if (existingProduct != null)
                {
                    var result = _productRepository.Delete(existingProduct);
                    if (result)
                    {
                        return ProductFactory.Create(existingProduct);
                    }
                }            
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }
}
