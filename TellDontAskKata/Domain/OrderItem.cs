namespace TellDontAskKata.Domain
{
    public class OrderItem
    {
        public OrderItem(Product product, int quantity)
        {
            Product = product;
            Quantity = quantity;
        }

        public Product Product { get; }
        public int Quantity { get; }
        public decimal TaxedAmount => Product.CalculateTaxedAmount(Quantity);
        public decimal Tax => Product.CalculateTax(Quantity);
    }
}
