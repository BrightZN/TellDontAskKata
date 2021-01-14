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
                return Round(Price + UnitaryTax);
            }
        }

        public decimal CalculateTax(int quantity) => UnitaryTax * quantity;

        public decimal CalculateTaxedAmount(int quantity)
        {
            return Round(UnitaryTaxedAmount * quantity);
        }
    }
}
