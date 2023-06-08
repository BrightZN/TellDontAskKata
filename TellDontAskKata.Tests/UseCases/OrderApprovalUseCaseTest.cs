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

        [Fact]
        public async Task Approves_Existing_Order()
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
                Approved = true
            };

            await _useCase.RunAsync(request);

            var savedOrder = _orderRepository.SavedOrder;

            Assert.Equal(OrderStatus.Approved, savedOrder.Status);
        }

        [Fact]
        public async Task Rejects_Existing_Order()
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
                Approved = false
            };

            await _useCase.RunAsync(request);

            var savedOrder = _orderRepository.SavedOrder;

            Assert.Equal(OrderStatus.Rejected, savedOrder.Status);
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

        [Fact]
        public async Task Throws_Exception_When_Order_Is_Null()
        {
            var request = new OrderApprovalRequest
            {
                OrderId = 1,
                Approved = true
            };

            await Assert.ThrowsAsync<OrderNotFoundException>(() => _useCase.RunAsync(request));

            Assert.Null(_orderRepository.SavedOrder);
        }
    }
}
