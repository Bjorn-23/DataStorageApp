using Business.Dtos;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using System.Diagnostics;

namespace Business.Services;

public class OrderRowService
{
    private readonly IOrderRowRepository _orderRowRepository;
    private readonly ProductService _productService;
    private readonly OrderService _orderService;

    public OrderRowService(IOrderRowRepository orderRowRepository, ProductService prorductService, OrderService orderService)
    {
        _orderRowRepository = orderRowRepository;
        _productService = prorductService;
        _orderService = orderService;
    }

    public OrderRowDto CreateOrderRow(OrderRowDto orderRow)
    {
        try
        {
            var order = GetOrderId(orderRow);
            var existingOrderRow = _orderRowRepository.GetOne(x => x.Id == order.Id && x.ArticleNumber == orderRow.ArticleNumber);
            if (existingOrderRow == null)
            {
                // Gets product from articleNumber to be able to set price.
                var product = _productService.GetProduct(x => x.ArticleNumber == orderRow.ArticleNumber);

                var price = GetPrice(product);

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
                    return Factories.OrderRowFactory.Create(createdOrderRow);
                }

            }
            else if (existingOrderRow != null)
            {
                return null!;// Better to return null here?
            }
            
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

    public IEnumerable<OrderRowDto> GetAllOrderRows()
    {
        try
        {
            var order = _orderService.GetAllOrders().FirstOrDefault();
            if(order != null)
            {
                var orderRows = _orderRowRepository.GetAllWithPredicate(x => x.OrderId == order.Id);
                return Factories.OrderRowFactory.Create(orderRows);
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return new List<OrderRowDto>();
    }
    
    public OrderRowDto UpdateOrderRow(OrderRowDto orderRow)
    {
        try
        {
            // Checks that an updated value has been put in for order quantity, if less than 0, null or empty, return null.
            if (orderRow.Quantity <= 0 || string.IsNullOrWhiteSpace(orderRow.Quantity.ToString()))
                return null!;

            var existingOrderRow = _orderRowRepository.GetOne(x => x.Id == orderRow.Id);
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
                    return Factories.OrderRowFactory.Create(updatedOrderRow);
                } 
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }

    
    // Helpers
    private decimal GetPrice(ProductRegistrationDto product)
    {
        // Checks for discountprice and applies if it not null
        decimal? price = 0;
        if (product.DiscountPrice != null)
            price = product.DiscountPrice;
        else
            price = product.Price;

        return price.Value;
    }

    private OrderDto GetOrderId(OrderRowDto orderRow)
    {
        var order = new OrderDto();

        // Checks if orderRow.OrderId is null, if it is then either gets the first order in a list or creates a new order.
        if (orderRow.OrderId <= 0)
        {
            order = _orderService.GetAllOrders().FirstOrDefault(); // first order associated to logged in customer.
            if (order == null)
            {
                var result = _orderService.CreateOrder(); // If no order exists then create new order.
                order = Factories.OrderFactory.Create(result);
            }
        }
        else
        {
            order.Id = orderRow.OrderId; // return the OrderId supplied in orderRow.OrderId
        }

        return order;
    }


}
