using System.Threading.Tasks;
using TellDontAskKata.Domain;
using TellDontAskKata.Repositories;

namespace TellDontAskKata.UseCases
{
    public class OrderApprovalUseCase
    {
        private readonly IOrderRepository _orderRepository;

        public OrderApprovalUseCase(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task RunAsync(OrderApprovalRequest request)
        {
            var isApproved = request.Approved;
            
            var order = await _orderRepository.GetByIdAsync(request.OrderId);

            Approve(order, isApproved);

            await _orderRepository.SaveAsync(order);
        }

        private void Approve(Order order, bool isApproved)
        {
            if (order.Status == OrderStatus.Shipped)
                throw new ShippedOrdersCannotBeChangedException();

            if (isApproved && order.Status == OrderStatus.Rejected)
                throw new RejectedOrderCannotBeApprovedException();

            if (!isApproved && order.Status == OrderStatus.Approved)
                throw new ApprovedOrderCannotBeRejectedException();

            order.Status = isApproved ? OrderStatus.Approved : OrderStatus.Rejected;
        }
    }
}
