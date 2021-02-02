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
            
        }

        private class RejectedStatus : OrderStatus
        {
            
        }

        private class ShippedStatus : OrderStatus
        {
            
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
    }
}
