using System.Threading.Tasks;
using TellDontAskKata.Domain;
using TellDontAskKata.Repositories;
using TellDontAskKata.Services;

namespace TellDontAskKata.UseCases;

public class OrderShipmentUseCase
{
    private readonly IOrderRepository _orderRepository;
    private readonly IShipmentService _shipmentService;

    public OrderShipmentUseCase(IOrderRepository orderRepository, IShipmentService shipmentService)
    {
        _orderRepository = orderRepository;
        _shipmentService = shipmentService;
    }

    public async Task RunAsync(OrderShipmentRequest request)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId);

        if (order is null)
            throw new OrderNotFoundException();

        await order.Ship(_shipmentService);

        await _orderRepository.SaveAsync(order);
    }
}