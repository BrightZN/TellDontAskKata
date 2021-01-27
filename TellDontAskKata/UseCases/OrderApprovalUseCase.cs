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

            order.Approve(isApproved);

            await _orderRepository.SaveAsync(order);
        }
    }
}
