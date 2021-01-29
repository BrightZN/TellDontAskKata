using TellDontAskKata.UseCases;

namespace TellDontAskKata.Domain
{
    public class OrderItem
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal TaxedAmount => Product.CalculateTaxedAmount(Quantity);
        public decimal Tax => Product.CalculateUnitaryTax(Quantity);

        public static OrderItem Create(string itemName, int itemQuantity, ProductList productList)
        {
            if (productList.Missing(itemName))
                throw new UnknownProductException();
            
            var orderItem = new OrderItem
            {
                Product = productList.GetByName(itemName),
                Quantity = itemQuantity
            };
            
            return orderItem;
        }
    }
}
