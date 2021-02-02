using System;

namespace TellDontAskKata.Domain
{
    public class Product
    {
        public string Name { get; }
        public decimal Price { get; }
        public Category Category { get; }

        public Product(string name, decimal price, Category category)
        {
            Name = name;
            Price = price;
            Category = category;
        }

        private decimal CalculateUnitaryTax() => Category.CalculateUnitaryTax(Price);

        public decimal CalculateUnitaryTax(int itemQuantity) => CalculateUnitaryTax() * itemQuantity;

        private decimal CalculateUnitaryTaxedAmount()
        {
            return decimal.Round(Price + CalculateUnitaryTax(), 2, MidpointRounding.AwayFromZero);
        }

        public decimal CalculateTaxedAmount(int itemQuantity)
        {
            return decimal.Round(CalculateUnitaryTaxedAmount() * itemQuantity, 2, MidpointRounding.AwayFromZero);
        }
    }
}
