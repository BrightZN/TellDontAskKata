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
    }
}
