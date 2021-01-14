﻿using System;

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
                return decimal.Round(Price + UnitaryTax, 2, MidpointRounding.AwayFromZero);
            }
        }

        public decimal CalculateTax(int quantity) => UnitaryTax * quantity;

        public decimal CalculateTaxedAmount(int quantity)
        {
            return decimal.Round(UnitaryTaxedAmount * quantity, 2, MidpointRounding.AwayFromZero);
        }
    }
}
