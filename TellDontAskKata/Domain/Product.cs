using System;
using static TellDontAskKata.Domain.CurrencyRounding;


namespace TellDontAskKata.Domain
{
    public class Product
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public Category Category { get; set; }

        public decimal UnitaryTax => Category.CalculateUnitaryTax(Price);

        public decimal UnitaryTaxedAmount 
        { 
            get
            {
                decimal unitaryTaxedAmount = Price + UnitaryTax;

                return Round(unitaryTaxedAmount);
            }
        }

        public decimal CalculateTax(int quantity) => UnitaryTax * quantity;

        public decimal CalculateTaxedAmount(int quantity)
        {
            decimal taxedAmount = UnitaryTaxedAmount * quantity;

            return Round(taxedAmount);
        }
    }
}
