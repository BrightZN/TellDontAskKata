using TellDontAskKata.UseCases;

namespace TellDontAskKata.Domain
{
    public class OrderItem
    {
        private OrderItem(Product product, int quantity)
        {
            Product = product;
            Quantity = quantity;
        }

        public Product Product { get; }
        public int Quantity { get; }
        public decimal TaxedAmount => Product.CalculateTaxedAmount(Quantity);
        public decimal Tax => Product.CalculateUnitaryTax(Quantity);

        public static OrderItem Create(string itemName, int itemQuantity, ProductList productList)
        {
            if (productList.Missing(itemName))
                throw new UnknownProductException();

            return new OrderItem(productList.GetByName(itemName), itemQuantity);
        }
    }
}
