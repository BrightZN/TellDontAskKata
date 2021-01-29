using System;

namespace TellDontAskKata.Domain
{
    public class Product
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public Category Category { get; set; }

        public decimal CalculateUnitaryTax()
        {
            return decimal.Round(this.Price / 100.00M * this.Category.TaxPercentage, 2, MidpointRounding.AwayFromZero);
        }

        public decimal CalculateUnitaryTax(int itemQuantity)
        {
            return this.CalculateUnitaryTax() * itemQuantity;
        }

        public decimal CalculateUnitaryTaxedAmount()
        {
            return decimal.Round(this.Price + this.CalculateUnitaryTax(), 2, MidpointRounding.AwayFromZero);
        }

        public decimal CalculateTaxedAmount(int itemQuantity)
        {
            return decimal.Round(this.CalculateUnitaryTaxedAmount() * itemQuantity, 2, MidpointRounding.AwayFromZero);
        }
    }
}
