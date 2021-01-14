using System;
using System.Linq;
using System.Threading.Tasks;
using TellDontAskKata.Domain;
using TellDontAskKata.Tests.Doubles;
using TellDontAskKata.UseCases;
using Xunit;

namespace TellDontAskKata.Tests.UseCases
{
    public class OrderApprovalUseCaseTest
    {
        private readonly TestOrderRepository _orderRepository;
        private readonly OrderApprovalUseCase _useCase;

        public OrderApprovalUseCaseTest()
        {
            _orderRepository = new TestOrderRepository();
            _useCase = new OrderApprovalUseCase(_orderRepository);
        }

        [Theory]
        [InlineData(true, OrderStatus.Approved)]
        [InlineData(false, OrderStatus.Rejected)]
        public async Task Approves_Existing_Order(bool approved, OrderStatus expectedStatus)
        {
            var initialOrder = new Order(1, string.Empty, OrderStatus.Created, Enumerable.Empty<OrderItem>());

            _orderRepository.AddOrder(initialOrder);

            var request = new OrderApprovalRequest
            {
                OrderId = 1,
                Approved = approved
            };

            await _useCase.RunAsync(request);

            var savedOrder = _orderRepository.SavedOrder;

            Assert.Equal(expectedStatus, savedOrder.Status);
        }

        [Theory]
        [InlineData(OrderStatus.Approved, false, typeof(ApprovedOrderCannotBeRejectedException))]
        [InlineData(OrderStatus.Rejected, true, typeof(RejectedOrderCannotBeApprovedException))]
        [InlineData(OrderStatus.Shipped, true, typeof(ShippedOrdersCannotBeChangedException))]
        [InlineData(OrderStatus.Shipped, false, typeof(ShippedOrdersCannotBeChangedException))]
        public async Task Cannot_Approve_Order_With_Status(OrderStatus initialStatus, bool approved, Type expectedException)
        {
            var initialOrder = new Order(1, string.Empty, initialStatus, Enumerable.Empty<OrderItem>());

            _orderRepository.AddOrder(initialOrder);

            var request = new OrderApprovalRequest
            {
                OrderId = 1,
                Approved = approved
            };

            await Assert.ThrowsAsync(expectedException, () => _useCase.RunAsync(request));

            Assert.Null(_orderRepository.SavedOrder);
        }
    }
}
