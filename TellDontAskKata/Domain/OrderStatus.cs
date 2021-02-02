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
            public override void ShouldBeShippable() => throw new OrderCannotBeShippedException();

            public override void ShouldBeApprovable(bool isApproved)
            {
                if(isApproved)
                    throw new RejectedOrderCannotBeApprovedException();
            }
        }

        private class ShippedStatus : OrderStatus
        {
            public override void ShouldBeShippable() => throw new OrderCannotBeShippedTwiceException();

            public override void ShouldBeApprovable(bool _) => throw new ShippedOrdersCannotBeChangedException();
        }

        private class CreatedStatus : OrderStatus
        {
            public override void ShouldBeShippable() => throw new OrderCannotBeShippedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="OrderCannotBeShippedException"></exception>
        /// <exception cref="OrderCannotBeShippedTwiceException"></exception>
        public virtual void ShouldBeShippable()
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="ApprovedOrderCannotBeRejectedException"></exception>
        /// <exception cref="RejectedOrderCannotBeApprovedException"></exception>
        /// <exception cref="ShippedOrdersCannotBeChangedException"></exception>
        /// <param name="isApproved"></param>
        public virtual void ShouldBeApprovable(bool isApproved)
        {
            
        }
    }
}
