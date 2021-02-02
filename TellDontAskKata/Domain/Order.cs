using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TellDontAskKata.Services;

namespace TellDontAskKata.Domain
{
    public class Order
    {
        public Order(int id, OrderStatus status)
        {
            Id = id;
            Status = status;
            Items = Enumerable.Empty<OrderItem>();
        }

        public Order(string currency, IEnumerable<OrderItem> items)
        {
            Id = 0;
            Currency = currency;
            Items = items;
            Status = OrderStatus.Created;
        }

        public int Id { get; }
        public decimal Total => Items.Sum(i => i.TaxedAmount);
        public string Currency { get; }
        public IEnumerable<OrderItem> Items { get; }
        public decimal Tax => Items.Sum(i => i.Tax);
        public OrderStatus Status { get; private set; }

        public async Task ShipAsync(IShipmentService shipmentService)
        {
            Status.ShouldBeShippable();

            await shipmentService.ShipAsync(this);

            Status = OrderStatus.Shipped;
        }

        public void Approve(bool isApproved)
        {
            Status.ShouldBeApprovable(isApproved);

            Status = isApproved ? OrderStatus.Approved : OrderStatus.Rejected;
        }
    }
}
