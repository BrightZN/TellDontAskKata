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

            public override void ShouldBeApprovable(bool isApproved)
            {
                if (isApproved)
                    return;
                
                throw new ApprovedOrderCannotBeRejectedException();
            }
        }

        private class RejectedStatus : OrderStatus
        {
            public override void ShouldBeShippable()
            {
                throw new OrderCannotBeShippedException();
            }

            public override void ShouldBeApprovable(bool isApproved)
            {
                if(isApproved)
                    throw new RejectedOrderCannotBeApprovedException();
            }
        }

        private class ShippedStatus : OrderStatus
        {
            public override void ShouldBeShippable()
            {
                throw new OrderCannotBeShippedTwiceException();
            }

            public override void ShouldBeApprovable(bool isApproved)
            {
                throw new ShippedOrdersCannotBeChangedException();
            }
        }

        private class CreatedStatus : OrderStatus
        {
            public override void ShouldBeShippable()
            {
                throw new OrderCannotBeShippedException();
            }
        }

        public virtual void ShouldBeShippable()
        {
            
        }

        public virtual void ShouldBeApprovable(bool isApproved)
        {
            
        }
    }
}
