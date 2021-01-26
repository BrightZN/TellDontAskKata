using System.Collections.Generic;
using System.Threading.Tasks;
using TellDontAskKata.Services;
using TellDontAskKata.UseCases;

namespace TellDontAskKata.Domain
{
    public class Order
    {
        public int Id { get; set; }
        public decimal Total { get; set; }
        public string Currency { get; set; }
        public List<OrderItem> Items { get; set; }
        public decimal Tax { get; set; }
        public OrderStatus Status { get; set; }

        public async Task ShipAsync(IShipmentService shipmentService)
        {
            if (Status == OrderStatus.Created || Status == OrderStatus.Rejected)
                throw new OrderCannotBeShippedException();

            if (Status == OrderStatus.Shipped)
                throw new OrderCannotBeShippedTwiceException();

            await shipmentService.ShipAsync(this);

            Status = OrderStatus.Shipped;
        }
    }
}
