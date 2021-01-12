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

            if (order.Status == OrderStatus.Shipped)
                throw new ShippedOrdersCannotBeChangedException();

            if (request.Approved && order.Status == OrderStatus.Rejected)
                throw new RejectedOrderCannotBeApprovedException();

            if (!request.Approved && order.Status == OrderStatus.Approved)
                throw new ApprovedOrderCannotBeRejectedException();

            order.Status = request.Approved ? OrderStatus.Approved : OrderStatus.Rejected;

            await _orderRepository.SaveAsync(order);
        }
    }
}
