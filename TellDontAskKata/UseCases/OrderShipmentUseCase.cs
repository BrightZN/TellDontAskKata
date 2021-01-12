using System.Threading.Tasks;
using TellDontAskKata.Domain;
using TellDontAskKata.Repositories;
using TellDontAskKata.Services;

namespace TellDontAskKata.UseCases
{
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

            await Ship(order);

            await _orderRepository.SaveAsync(order);
        }

        private async Task Ship(Order order)
        {
            if (order.Status == OrderStatus.Created || order.Status == OrderStatus.Rejected)
                throw new OrderCannotBeShippedException();

            if (order.Status == OrderStatus.Shipped)
                throw new OrderCannotBeShippedTwiceException();

            await _shipmentService.ShipAsync(order);

            order.Status = OrderStatus.Shipped;
        }
    }
}
