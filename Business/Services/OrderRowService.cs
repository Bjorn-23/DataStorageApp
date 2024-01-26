using Infrastructure.Interfaces;

namespace Business.Services;

public class OrderRowService
{
    private readonly IOrderRowRepository _orderRowRepository;

    public OrderRowService(IOrderRowRepository orderRowRepository)
    {
        _orderRowRepository = orderRowRepository;
    }
}
