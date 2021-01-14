using System;

namespace TellDontAskKata.Domain
{
    public class Category
    {
        public string Name { get; set; }
        public decimal TaxPercentage { get; set; }

        public decimal CalculateUnitaryTax(decimal price)
        {
            return decimal.Round(price / 100.00M * TaxPercentage, 2, MidpointRounding.AwayFromZero);
        }
    }
}
