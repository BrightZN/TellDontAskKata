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
            if (CannotBeShippedYet())
                throw new OrderCannotBeShippedException();

            if (Shipped())
                throw new OrderCannotBeShippedTwiceException();

            await shipmentService.ShipAsync(this);

            Status = OrderStatus.Shipped;
        }

        private bool Shipped()
        {
            return Status == OrderStatus.Shipped;
        }

        private bool CannotBeShippedYet()
        {
            return Status == OrderStatus.Created || Status == OrderStatus.Rejected;
        }

        public void Approve(bool isApproved)
        {
            if (Status == OrderStatus.Shipped)
                throw new ShippedOrdersCannotBeChangedException();

            if (isApproved && Status == OrderStatus.Rejected)
                throw new RejectedOrderCannotBeApprovedException();

            if (!isApproved && Status == OrderStatus.Approved)
                throw new ApprovedOrderCannotBeRejectedException();

            Status = isApproved ? OrderStatus.Approved : OrderStatus.Rejected;
        }
    }
}
