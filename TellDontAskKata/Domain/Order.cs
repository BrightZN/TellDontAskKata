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
            if (Status == OrderStatus.Shipped)
                throw new ShippedOrdersCannotBeChangedException();

            if (approved && Status == OrderStatus.Rejected)
                throw new RejectedOrderCannotBeApprovedException();

            if (!approved && Status == OrderStatus.Approved)
                throw new ApprovedOrderCannotBeRejectedException();

            Status = approved ? OrderStatus.Approved : OrderStatus.Rejected;
        }
    }
}
