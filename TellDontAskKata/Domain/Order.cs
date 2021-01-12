using System;
using System.Collections.Generic;
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

        private bool RejectingApprovedOrder(bool approved)
        {
            return !approved && Status == OrderStatus.Approved;
        }

        private bool ApprovingRejectedOrder(bool approved)
        {
            return approved && Status == OrderStatus.Rejected;
        }

        private bool Shipped()
        {
            return Status == OrderStatus.Shipped;
        }
    }
}
