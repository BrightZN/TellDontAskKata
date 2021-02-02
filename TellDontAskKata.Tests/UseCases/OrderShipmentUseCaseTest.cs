using System.Threading.Tasks;
using TellDontAskKata.Domain;
using TellDontAskKata.Tests.Doubles;
using TellDontAskKata.UseCases;
using Xunit;

namespace TellDontAskKata.Tests.UseCases
{
    public class OrderShipmentUseCaseTest
    {
        private readonly TestOrderRepository _orderRepository;
        private readonly TestShipmentService _shipmentService;
        private readonly OrderShipmentUseCase _useCase;

        public OrderShipmentUseCaseTest()
        {
            _orderRepository = new TestOrderRepository();
            _shipmentService = new TestShipmentService();

            _useCase = new OrderShipmentUseCase(_orderRepository, _shipmentService);
        }

        [Fact]
        public async Task Ships_Approved_Order()
        {
            var initialOrder = new Order(1, OrderStatus.Approved);

            _orderRepository.AddOrder(initialOrder);

            var request = new OrderShipmentRequest
            {
                OrderId = 1
            };

            await _useCase.RunAsync(request);

            Assert.Equal(OrderStatus.Shipped, _orderRepository.SavedOrder.Status);
            Assert.Equal(_shipmentService.ShippedOrder, initialOrder);
        }

        [Fact]
        public async Task Created_Order_Cannot_Be_Shipped()
        {
            var initialOrder = new Order(1, OrderStatus.Created);

            _orderRepository.AddOrder(initialOrder);

            var request = new OrderShipmentRequest
            {
                OrderId = 1
            };

            await Assert.ThrowsAsync<OrderCannotBeShippedException>(() => _useCase.RunAsync(request));

            Assert.Null(_orderRepository.SavedOrder);
            Assert.Null(_shipmentService.ShippedOrder);
        }

        [Fact]
        public async Task Rejected_Order_Cannot_Be_Shipped()
        {
            var initialOrder = new Order(1, OrderStatus.Rejected);

            _orderRepository.AddOrder(initialOrder);

            var request = new OrderShipmentRequest
            {
                OrderId = 1
            };

            await Assert.ThrowsAsync<OrderCannotBeShippedException>(() => _useCase.RunAsync(request));

            Assert.Null(_orderRepository.SavedOrder);
            Assert.Null(_shipmentService.ShippedOrder);
        }

        [Fact]
        public async Task Shipped_Order_Cannot_Be_Shipped_Again()
        {
            var initialOrder = new Order(1, OrderStatus.Shipped);

            _orderRepository.AddOrder(initialOrder);

            var request = new OrderShipmentRequest
            {
                OrderId = 1
            };

            await Assert.ThrowsAsync<OrderCannotBeShippedTwiceException>(() => _useCase.RunAsync(request));

            Assert.Null(_orderRepository.SavedOrder);
            Assert.Null(_shipmentService.ShippedOrder);
        }
    }
}
