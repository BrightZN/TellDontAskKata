using static TellDontAskKata.Domain.CurrencyRounding;


namespace TellDontAskKata.Domain
{
    public class Product
    {
        public Product(string name, decimal price, Category category)
        {
            Name = name;
            Price = price;
            Category = category;
        }

        public string Name { get; }
        public decimal Price { get; }
        public Category Category { get; }

        public decimal UnitaryTax => Category.CalculateUnitaryTax(Price);

        public decimal UnitaryTaxedAmount => Round(Price + UnitaryTax);

        public decimal CalculateTax(int quantity) => UnitaryTax * quantity;

        public decimal CalculateTaxedAmount(int quantity) => Round(UnitaryTaxedAmount * quantity);
    }
}
