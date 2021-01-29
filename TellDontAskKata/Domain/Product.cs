using System;

namespace TellDontAskKata.Domain
{
    public class Product
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public Category Category { get; set; }

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
