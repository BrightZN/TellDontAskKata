using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TellDontAskKata.Services;

namespace TellDontAskKata.Domain
{
    public class Order
    {
        public Order(int id, string currency, OrderStatus status, IEnumerable<OrderItem> items)
        {
            Id = id;
            Currency = currency;
            Status = status;
            Items = items;
        }

        public Order(string currency, IEnumerable<OrderItem> items)
            : this(0, currency, OrderStatus.Created, items)
        {
        }

        public int Id { get; }
        public string Currency { get; }
        public OrderStatus Status { get; private set; }
        public IEnumerable<OrderItem> Items { get; }

        public decimal Total => Items.Sum(i => i.TaxedAmount);

        public decimal Tax => Items.Sum(i => i.Tax);

        public void Approve()
        {
            if (Shipped())
                throw new ShippedOrdersCannotBeChangedException();

            if (Rejected())
                throw new RejectedOrderCannotBeApprovedException();

            Status = OrderStatus.Approved;
        }

        public void Reject()
        {
            if (Shipped())
                throw new ShippedOrdersCannotBeChangedException();

            if (Approved())
                throw new ApprovedOrderCannotBeRejectedException();

            Status = OrderStatus.Rejected;
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
