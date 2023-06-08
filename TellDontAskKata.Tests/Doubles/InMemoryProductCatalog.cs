using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TellDontAskKata.Domain;
using TellDontAskKata.Repositories;

namespace TellDontAskKata.Tests.Doubles;

public class InMemoryProductCatalog : IProductCatalog
{
    private readonly List<Product> _products;

    public InMemoryProductCatalog(List<Product> products)
    {
        _products = products;
    }

    public Task<Product> GetByNameAsync(string name)
    {
        return Task.FromResult(_products.FirstOrDefault(p => p.Name == name));
    }
}