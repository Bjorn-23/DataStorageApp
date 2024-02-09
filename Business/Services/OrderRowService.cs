using Business.Dtos;
using Business.Factories;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using System.Diagnostics;

namespace Business.Services;

public class OrderRowService
{
    private readonly IOrderRowRepository _orderRowRepository;
    private readonly ProductService _productService;
    private readonly OrderService _orderService;
    private readonly ICustomerRepository _customerRepository;

    public OrderRowService(IOrderRowRepository orderRowRepository, ProductService productService, OrderService orderService, ICustomerRepository customerRepository)
    {
        _orderRowRepository = orderRowRepository;
        _productService = productService;
        _orderService = orderService;
        _customerRepository = customerRepository;
    }

    /// <summary>
    /// Creates a new orderRow in an order.
    /// </summary>
    /// <param name="orderRow"></param>
    /// <returns>OrderRowDto</returns>
    public OrderRowDto CreateOrderRow(OrderRowDto orderRow)
    {
        try
        {
            var order = GetOrderId(orderRow);
            var existingOrderRow = _orderRowRepository.GetOne(x => x.Id == order.Id && x.ArticleNumber == orderRow.ArticleNumber);
            if (existingOrderRow == null)
            {
                // Gets product from articleNumber to be able to set price.
                var product = _productService.GetProductDisplay(new ProductDto(){ ArticleNumber = orderRow.ArticleNumber });

                var price = GetPriceOrDiscountPrice(product);

                var newOrderRow = new OrderRowEntity()
                {
                    Quantity = orderRow.Quantity,
                    OrderRowPrice = (price * orderRow.Quantity),
                    ArticleNumber = product.ArticleNumber,
                    OrderId = order.Id
                };

                var createdOrderRow = _orderRowRepository.Create(newOrderRow);
                if (createdOrderRow != null)
                {
                    _orderService.UpdateOrder(new OrderDto()
                    {
                        Id = newOrderRow.OrderId,
                        OrderPrice = newOrderRow.OrderRowPrice,
                    });

                    _productService.UpdateProductStock(new ProductRegistrationDto
                    {
                        ArticleNumber = product.ArticleNumber,
                        Stock = -newOrderRow.Quantity
                    });

                    return OrderRowFactory.Create(createdOrderRow);
                }
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

    /// <summary>
    /// Fetches all orders asscociated with the currents user.
    /// </summary>
    /// <returns>List of orderRows</returns>
    public IEnumerable<OrderRowDto> GetAllOrderRows()
    {
        try
        {
            var order = _orderService.GetUsersOrder();
            if (order != null)
            {
                var orderRows = _orderRowRepository.GetAllWithPredicate(x => x.OrderId == order.Id);
                return OrderRowFactory.Create(orderRows);
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return new List<OrderRowDto>();
    }

    /// <summary>
    /// Updates an existing OrderRow with new quantity of the same product, then updates Total Price for order and amount of product in stock.
    /// </summary>
    /// <param name="orderRow"></param>
    /// <returns>OrderRowDto</returns>
    public OrderRowDto UpdateOrderRow(OrderRowDto orderRow)
    {
        try
        {
            // Checks that an updated value has been put in for order quantity, if less than 0, null or empty then delete OrderRow.
            if (orderRow.Quantity <= 0 || string.IsNullOrWhiteSpace(orderRow.Quantity.ToString()))
            {
                DeleteOrderRow(orderRow);
                return null!;
            }                

            var existingOrderRow = _orderRowRepository.GetOne(x => x.Id == orderRow.Id);
            var oldOrderRowPrice = existingOrderRow.OrderRowPrice; // saves old price to new variable as existing one will be updated.
            var oldOrderRowQuantity = existingOrderRow.Quantity; // saves old price to new variable as existing one will be updated.
            if (existingOrderRow != null)
            {
                var updatedOrderRowDetails = new OrderRowEntity
                {
                    Id = existingOrderRow.Id,
                    Quantity = orderRow.Quantity,
                    OrderRowPrice = (existingOrderRow.OrderRowPrice / existingOrderRow.Quantity) * orderRow.Quantity,
                    ArticleNumber = existingOrderRow.ArticleNumber,
                    OrderId = existingOrderRow.OrderId,
                };

                var updatedOrderRow = _orderRowRepository.Update(existingOrderRow, updatedOrderRowDetails);
                if (updatedOrderRow != null)
                {
                    // Sets price to update order with.
                    OrderDto orderPriceUpdate = new()
                    {
                        Id = existingOrderRow.OrderId,
                        OrderPrice = updatedOrderRowDetails.OrderRowPrice - oldOrderRowPrice,
                    };
                    var orderPriceResult = _orderService.UpdateOrder(orderPriceUpdate); // updates order total with new price.

                    // Sets quantity to update product stock with.
                    ProductRegistrationDto stockUpdate = new()
                    {
                        ArticleNumber = existingOrderRow.ArticleNumber,
                        Stock = oldOrderRowQuantity - updatedOrderRowDetails.Quantity
                    };
                    var productStockResult = _productService.UpdateProductStock(stockUpdate); // updates product stock with new amount.

                    return OrderRowFactory.Create(updatedOrderRow);
                }
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

    /// <summary>
    /// Deletes an exisiting orderRow and updates stock quantity on product and Total Price of order.
    /// </summary>
    /// <param name="orderRow"></param>
    /// <returns></returns>
    public OrderRowDto DeleteOrderRow(OrderRowDto orderRow)
    {
        try
        {
            var existingOrderRow = _orderRowRepository.GetOne(x => x.Id == orderRow.Id);
            var oldOrderRowPrice = existingOrderRow.OrderRowPrice;
            var oldOrderRowQuantity = existingOrderRow.Quantity;
            if (existingOrderRow != null)
            {
                var result = _orderRowRepository.Delete(existingOrderRow);
                if (result)
                {
                    // Sets price to update order with.
                    OrderDto orderPriceUpdate = new()
                    {
                        Id = existingOrderRow.OrderId,
                        OrderPrice = -oldOrderRowPrice,
                    };
                    var OrderUpdateResult = _orderService.UpdateOrder(orderPriceUpdate); // updates order total with new price.

                    // Sets quantity to update product stock with.
                    ProductRegistrationDto stockUpdate = new()
                    {
                        ArticleNumber = existingOrderRow.ArticleNumber,
                        Stock = oldOrderRowQuantity
                    };
                    var prodcutUpdateResult = _productService.UpdateProductStock(stockUpdate); // updates product stock with new amount.

                    return OrderRowFactory.Create(existingOrderRow);
                }
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }


    // Helpers
    /// <summary>
    /// Checks if Dicountprice has a value equal or greater than 1, if so then uses that price, else regular price.
    /// </summary>
    /// <param name="product"></param>
    /// <returns></returns>
    public decimal GetPriceOrDiscountPrice(ProductRegistrationDto product)
    {
        try
        {
            // Checks for discountprice and applies if it not null
            decimal? price = 0;
            if (product.DiscountPrice != null && product.DiscountPrice >= 1)
                price = product.DiscountPrice;
            else
                price = product.Price;

            return price.Value;
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return 0;
    }

    /// <summary>
    /// Either gets current users orderId or creates a new order for user.
    /// </summary>
    /// <param name="orderRow"></param>
    /// <returns></returns>
    public OrderDto GetOrderId(OrderRowDto orderRow)
    {
        try
        {
            var order = new OrderDto();

            // Checks if orderRow.OrderId is null, if it is then either gets the first order in a list or creates a new order.
            if (orderRow.OrderId <= 0)
            {
                order = _orderService.GetUsersOrder(); // first order associated to logged in customer.
                if (order == null)
                {
                    order = _orderService.CreateOrder(); // If no order exists then create new order.
                }
            }
            else
            {
                order.Id = orderRow.OrderId; // return the OrderId supplied in orderRow.OrderId
            }

            return order;
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

}
