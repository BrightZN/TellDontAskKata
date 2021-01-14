using System;
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
            var initialOrder = new Order
            {
                Status = OrderStatus.Created,
                Id = 1
            };

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

        [Fact]
        public async Task Cannot_Approve_Rejected_Order()
        {
            var initialOrder = new Order
            {
                Status = OrderStatus.Rejected,
                Id = 1
            };

            _orderRepository.AddOrder(initialOrder);

            var request = new OrderApprovalRequest
            {
                OrderId = 1,
                Approved = true
            };

            await Assert.ThrowsAsync<RejectedOrderCannotBeApprovedException>(() => _useCase.RunAsync(request));

            Assert.Null(_orderRepository.SavedOrder);
        }

        [Theory]
        [InlineData(OrderStatus.Approved, false, typeof(ApprovedOrderCannotBeRejectedException))]
        [InlineData(OrderStatus.Shipped, true, typeof(ShippedOrdersCannotBeChangedException))]
        [InlineData(OrderStatus.Shipped, false, typeof(ShippedOrdersCannotBeChangedException))]
        public async Task Cannot_Approve_Order_With_Status(OrderStatus initialStatus, bool approved, Type expectedException)
        {
            var initialOrder = new Order
            {
                Status = initialStatus,
                Id = 1
            };

            _orderRepository.AddOrder(initialOrder);

            var request = new OrderApprovalRequest
            {
                OrderId = 1,
                Approved = approved
            };

            await Assert.ThrowsAsync(expectedException, () => _useCase.RunAsync(request));

            Assert.Null(_orderRepository.SavedOrder);
        }

        /*
        [Fact]
        public async Task Cannot_Reject_Approved_Order()
        {
            var initialOrder = new Order
            {
                Status = OrderStatus.Approved,
                Id = 1
            };

            _orderRepository.AddOrder(initialOrder);

            var request = new OrderApprovalRequest
            {
                OrderId = 1,
                Approved = false
            };

            await Assert.ThrowsAsync<ApprovedOrderCannotBeRejectedException>(() => _useCase.RunAsync(request));

            Assert.Null(_orderRepository.SavedOrder);
        }

        [Fact]
        public async Task Cannot_Approve_Shipped_Order()
        {
            var initialOrder = new Order
            {
                Status = OrderStatus.Shipped,
                Id = 1
            };

            _orderRepository.AddOrder(initialOrder);

            var request = new OrderApprovalRequest
            {
                OrderId = 1,
                Approved = true
            };

            await Assert.ThrowsAsync<ShippedOrdersCannotBeChangedException>(() => _useCase.RunAsync(request));

            Assert.Null(_orderRepository.SavedOrder);
        }

        [Fact]
        public async Task Cannot_Reject_Shipped_Order()
        {
            var initialOrder = new Order
            {
                Status = OrderStatus.Shipped,
                Id = 1
            };

            _orderRepository.AddOrder(initialOrder);

            var request = new OrderApprovalRequest
            {
                OrderId = 1,
                Approved = false
            };

            await Assert.ThrowsAsync<ShippedOrdersCannotBeChangedException>(() => _useCase.RunAsync(request));

            Assert.Null(_orderRepository.SavedOrder);
        }
        */
    }
}
