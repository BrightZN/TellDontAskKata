using System;
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
            var initialOrder = new Order
            {
                Id = 1,
                Status = OrderStatus.Approved
            };

            _orderRepository.AddOrder(initialOrder);

            var request = new OrderShipmentRequest
            {
                OrderId = 1
            };

            await _useCase.RunAsync(request);

            Assert.Equal(OrderStatus.Shipped, _orderRepository.SavedOrder.Status);
            Assert.Equal(_shipmentService.ShippedOrder, initialOrder);
        }

        [Theory]
        [InlineData(OrderStatus.Created, typeof(OrderCannotBeShippedException))]
        [InlineData(OrderStatus.Rejected, typeof(OrderCannotBeShippedException))]
        [InlineData(OrderStatus.Shipped, typeof(OrderCannotBeShippedTwiceException))]
        public async Task Cannot_Ship_Order_With_Status(OrderStatus initialStatus, Type expectedException)
        {
            var initialOrder = new Order
            {
                Id = 1,
                Status = initialStatus
            };

            _orderRepository.AddOrder(initialOrder);

            var request = new OrderShipmentRequest
            {
                OrderId = 1
            };

            await Assert.ThrowsAsync(expectedException, () => _useCase.RunAsync(request));

            Assert.Null(_orderRepository.SavedOrder);
            Assert.Null(_shipmentService.ShippedOrder);
        }
    }
}
