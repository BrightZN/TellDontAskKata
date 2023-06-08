using System.Threading.Tasks;
using TellDontAskKata.Domain;

namespace TellDontAskKata.Repositories
{
    public interface IOrderRepository
    {
        Task<Order?> GetByIdAsync(int orderId);
        Task SaveAsync(Order order);
    }
}
