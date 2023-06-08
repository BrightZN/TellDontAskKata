using System;

namespace TellDontAskKata.Domain;

public class OrderItem
{
    public Product Product { get; }
    public int Quantity { get; }
    public decimal TaxedAmount { get; }
    public decimal Tax { get; }

    public OrderItem(Product product, int quantity)
    {
        Product = product;
        Quantity = quantity;
        Tax = product.CalculateTax(quantity);
        TaxedAmount = product.CalculateTaxedAmount(quantity);
    }

    /// <summary>
    /// This constructor is primarily for preserving historical information -
    /// changes to product prices will not affect historical tax amounts.
    /// </summary>
    /// <param name="product"></param>
    /// <param name="quantity"></param>
    /// <param name="taxedAmount"></param>
    /// <param name="tax"></param>
    // ReSharper disable once UnusedMember.Global
    public OrderItem(Product product, int quantity, decimal taxedAmount, decimal tax)
    {
        Product = product;
        Quantity = quantity;
        TaxedAmount = taxedAmount;
        Tax = tax;
    }
}