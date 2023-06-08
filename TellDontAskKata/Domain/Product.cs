using System;

namespace TellDontAskKata.Domain;

public class Product
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public Category Category { get; set; }

    public decimal CalculateTaxedAmount(int quantity)
    {
        var unitaryTax = Category.CalculateUnitaryTax(Price);
        
        var unitaryTaxedAmount = decimal.Round(
            Price + unitaryTax, 2, MidpointRounding.AwayFromZero);

        return decimal.Round(
            unitaryTaxedAmount * quantity, 2, MidpointRounding.AwayFromZero);
    }

    public decimal CalculateTax(int quantity)
    {
        return Category.CalculateUnitaryTax(Price) * quantity;
    }
}