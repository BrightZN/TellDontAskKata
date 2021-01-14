using System;

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

        public static OrderItem Create(string name, int quantity, ProductList productList)
        {
            if (productList.Missing(name))
                throw new UnknownProductException();
            else
            {
                return new OrderItem(
                    product: productList.GetProductByName(name),
                    quantity: quantity);
            }
        }
    }
}
