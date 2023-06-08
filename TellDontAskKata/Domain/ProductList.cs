using System.Collections.Generic;
using System.Linq;

namespace TellDontAskKata.Domain;

public class ProductList
{
    private readonly IReadOnlyCollection<Product> _products;

    public ProductList(IReadOnlyCollection<Product> products) => 
        _products = products;

    public Product? FindByName(string name) => 
        _products.SingleOrDefault(p => p.Name.Equals(name));
}