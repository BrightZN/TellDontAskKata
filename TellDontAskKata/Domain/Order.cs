using System;
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
            if (Status == OrderStatus.Created || Status == OrderStatus.Rejected)
                throw new OrderCannotBeShippedException();

            if (Status == OrderStatus.Shipped)
                throw new OrderCannotBeShippedTwiceException();

            await shipmentService.ShipAsync(this);

            Status = OrderStatus.Shipped;
        }

        private bool RejectingApprovedOrder(bool approved) => !approved && Status == OrderStatus.Approved;

        private bool ApprovingRejectedOrder(bool approved) => approved && Status == OrderStatus.Rejected;

        private bool Shipped() => Status == OrderStatus.Shipped;
    }
}
