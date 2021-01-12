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
            var order = await _orderRepository.GetByIdAsync(request.OrderId);

            Approve(order, request.Approved);

            await _orderRepository.SaveAsync(order);
        }

        private static void Approve(Order order, bool approved)
        {
            if (order.Status == OrderStatus.Shipped)
                throw new ShippedOrdersCannotBeChangedException();

            if (approved && order.Status == OrderStatus.Rejected)
                throw new RejectedOrderCannotBeApprovedException();

            if (!approved && order.Status == OrderStatus.Approved)
                throw new ApprovedOrderCannotBeRejectedException();

            order.Status = approved ? OrderStatus.Approved : OrderStatus.Rejected;
        }
    }
}
