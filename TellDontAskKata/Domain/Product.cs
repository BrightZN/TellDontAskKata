using System;

namespace TellDontAskKata.Domain
{
    public class Product
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public Category Category { get; set; }

        public decimal UnitaryTax
        {
            get
            {
                return decimal.Round(Price / 100.00M * Category.TaxPercentage, 2, MidpointRounding.AwayFromZero);
            }
        }

        public decimal UnitaryTaxedAmount 
        { 
            get
            {
                return decimal.Round(Price + UnitaryTax, 2, MidpointRounding.AwayFromZero);
            }
        }

        public decimal CalculateTax(int quantity)
        {
            return UnitaryTax * quantity;
        }

        public decimal CalculateTaxedAmount(int quantity)
        {
            return decimal.Round(UnitaryTaxedAmount * quantity, 2, MidpointRounding.AwayFromZero);
        }
    }
}
