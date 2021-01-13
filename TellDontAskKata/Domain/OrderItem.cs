namespace TellDontAskKata.Domain
{
    public class OrderItem
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal TaxedAmount => Product.CalculateTaxedAmount(Quantity);
        public decimal Tax => Product.CalculateTax(Quantity);
    }
}
