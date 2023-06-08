using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TellDontAskKata.Domain;
using TellDontAskKata.Repositories;

namespace TellDontAskKata.Tests.Doubles;

public class TestOrderRepository : IOrderRepository
{
    private readonly List<Order> _orders = new List<Order>();

    public Order SavedOrder { get; private set; }

    public void AddOrder(Order order) => _orders.Add(order);

    public Task<Order> GetByIdAsync(int orderId)
    {
        return Task.FromResult(_orders.FirstOrDefault(o => o.Id == orderId));
    }

    public Task SaveAsync(Order order)
    {
        SavedOrder = order;

        return Task.CompletedTask;
    }
}