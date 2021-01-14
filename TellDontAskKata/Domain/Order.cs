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

        public void Approve(bool approve)
        {
            if (Shipped())
                throw new ShippedOrdersCannotBeChangedException();

            if (approve)
            {
                if (Rejected())
                    throw new RejectedOrderCannotBeApprovedException();

                Status = OrderStatus.Approved;
            }
            else
            {
                if (Approved())
                    throw new ApprovedOrderCannotBeRejectedException();

                Status = OrderStatus.Rejected;
            }
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

        private bool Approved() => Status == OrderStatus.Approved;

        private bool Rejected() => Status == OrderStatus.Rejected;

        private bool NewOrRejected() => Created() || Rejected();

        private bool Created() => Status == OrderStatus.Created;

        private bool Shipped() => Status == OrderStatus.Shipped;
    }
}
