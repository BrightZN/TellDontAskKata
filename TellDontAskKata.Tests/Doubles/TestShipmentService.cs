using System.Threading.Tasks;
using TellDontAskKata.Domain;
using TellDontAskKata.Services;

namespace TellDontAskKata.Tests.Doubles
{
    public class TestShipmentService : IShipmentService
    {
        public Order ShippedOrder { get; private set; }

        public Task ShipAsync(Order order)
        {
            ShippedOrder = order;

            return Task.CompletedTask;
        }
    }
}
