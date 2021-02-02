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
            if (CannotBeShippedYet())
                throw new OrderCannotBeShippedException();

            if (Shipped())
                throw new OrderCannotBeShippedTwiceException();

            await shipmentService.ShipAsync(this);

            Status = OrderStatus.Shipped;
        }

        public void Approve(bool isApproved)
        {
            if (Shipped())
                throw new ShippedOrdersCannotBeChangedException();

            if (isApproved)
            {
                if (Rejected())
                    throw new RejectedOrderCannotBeApprovedException();

                Status = OrderStatus.Approved;
            }
            else
            {
                if(Approved())
                    throw new ApprovedOrderCannotBeRejectedException();
                
                Status = OrderStatus.Rejected;
            }
        }

        private bool Shipped() => Status == OrderStatus.Shipped;

        private bool CannotBeShippedYet() => Status == OrderStatus.Created || Rejected();

        private bool Approved() => Status == OrderStatus.Approved;

        private bool Rejected() => Status == OrderStatus.Rejected;
    }
}
