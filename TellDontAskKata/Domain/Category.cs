﻿using static TellDontAskKata.Domain.CurrencyRounding;

namespace TellDontAskKata.Domain
{
    public class Category
    {
        public string Name { get; }
        public decimal TaxPercentage { get; }

        public Category(string name, decimal taxPercentage)
        {
            Name = name;
            TaxPercentage = taxPercentage;
        }

        public decimal CalculateUnitaryTax(decimal price) => Round(price / 100.00M * TaxPercentage);
    }
}
