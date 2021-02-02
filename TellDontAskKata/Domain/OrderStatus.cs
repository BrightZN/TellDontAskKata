namespace TellDontAskKata.Domain
{
    public abstract class OrderStatus
    {
        public static readonly OrderStatus Approved = new ApprovedStatus();
        public static readonly OrderStatus Rejected = new RejectedStatus();
        public static readonly OrderStatus Shipped = new ShippedStatus();
        public static readonly OrderStatus Created = new CreatedStatus();

        private class ApprovedStatus : OrderStatus
        {

            public override void CanBeApproved(bool isApproved)
            {
                if (isApproved)
                    return;
                
                throw new ApprovedOrderCannotBeRejectedException();
            }
        }

        private class RejectedStatus : OrderStatus
        {
            public override void CanBeShipped()
            {
                throw new OrderCannotBeShippedException();
            }

            public override void CanBeApproved(bool isApproved)
            {
                if(isApproved)
                    throw new RejectedOrderCannotBeApprovedException();
            }
        }

        private class ShippedStatus : OrderStatus
        {
            public override void CanBeShipped()
            {
                throw new OrderCannotBeShippedTwiceException();
            }

            public override void CanBeApproved(bool isApproved)
            {
                throw new ShippedOrdersCannotBeChangedException();
            }
        }

        private class CreatedStatus : OrderStatus
        {
            public override void CanBeShipped()
            {
                throw new OrderCannotBeShippedException();
            }
        }

        public virtual void CanBeShipped()
        {
            
        }

        public virtual void CanBeApproved(bool isApproved)
        {
            
        }
    }
}
