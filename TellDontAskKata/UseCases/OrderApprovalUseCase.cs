using System.Threading.Tasks;
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

            //order.Approve(request.Approved);
            if (request.Approved)
                order.Approve();
            else
                order.Reject();

            await _orderRepository.SaveAsync(order);
        }
    }
}
