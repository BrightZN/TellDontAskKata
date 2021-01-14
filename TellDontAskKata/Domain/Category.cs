using static TellDontAskKata.Domain.CurrencyRounding;

namespace TellDontAskKata.Domain
{
    public class Category
    {
        public string Name { get; set; }
        public decimal TaxPercentage { get; set; }

        public decimal CalculateUnitaryTax(decimal price)
        {
            return Round(price / 100.00M * TaxPercentage);
        }
    }
}
