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
        public List<OrderItem> Items { get; set; }
        public decimal Tax => Items.Sum(i => i.Tax);
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
            return Status == OrderStatus.Created || Rejected();
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

        private bool Approved()
        {
            return Status == OrderStatus.Approved;
        }

        private bool Rejected()
        {
            return Status == OrderStatus.Rejected;
        }
    }
}
