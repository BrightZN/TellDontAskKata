using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TellDontAskKata.Services;
using TellDontAskKata.UseCases;

namespace TellDontAskKata.Domain
{
    public class Order
    {
        public int Id { get; set; }
        public decimal Total => Items.Sum(i => i.TaxedAmount);
        public string Currency { get; set; }
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
        public decimal Tax => Items.Sum(i => i.Tax);
        public OrderStatus Status { get; set; }

        public void Approve(bool approved)
        {
            if (Shipped())
                throw new ShippedOrdersCannotBeChangedException();

            if (ApprovingRejectedOrder(approved))
                throw new RejectedOrderCannotBeApprovedException();

            if (RejectingApprovedOrder(approved))
                throw new ApprovedOrderCannotBeRejectedException();

            Status = approved ? OrderStatus.Approved : OrderStatus.Rejected;
        }

        public async Task ShipAsync(IShipmentService shipmentService)
        {
            if (NewOrRejected())
                throw new OrderCannotBeShippedException();

            if (Shipped())
                throw new OrderCannotBeShippedTwiceException();

            await shipmentService.ShipAsync(this);

            Status = OrderStatus.Shipped;
        }

        private bool NewOrRejected() => Status == OrderStatus.Created || Status == OrderStatus.Rejected;

        private bool RejectingApprovedOrder(bool approved) => !approved && Status == OrderStatus.Approved;

        private bool ApprovingRejectedOrder(bool approved) => approved && Status == OrderStatus.Rejected;

        private bool Shipped() => Status == OrderStatus.Shipped;
    }
}
