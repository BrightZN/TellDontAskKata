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

        public async Task Ship(IShipmentService shipmentService)
        {
            if (Status is OrderStatus.Created or OrderStatus.Rejected)
                throw new OrderCannotBeShippedException();

            if (Status is OrderStatus.Shipped)
                throw new OrderCannotBeShippedTwiceException();

            await shipmentService.ShipAsync(this);

            Status = OrderStatus.Shipped;
        }
    }
}
