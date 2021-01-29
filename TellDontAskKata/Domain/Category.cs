using System;

namespace TellDontAskKata.Domain
{
    public class Category
    {
        public Category(string name, decimal taxPercentage)
        {
            Name = name;
            TaxPercentage = taxPercentage;
        }

        public string Name { get; }
        public decimal TaxPercentage { get; }

        public decimal CalculateUnitaryTax(decimal price)
        {
            return decimal.Round(price / 100.00M * TaxPercentage, 2, MidpointRounding.AwayFromZero);
        }
    }
}
