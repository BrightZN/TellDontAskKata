using System.Collections.Generic;
using System.Threading.Tasks;
using TellDontAskKata.Domain;

namespace TellDontAskKata.Repositories;

public interface IProductCatalog
{
    Task<Product?> GetByNameAsync(string name);

    Task<ProductList> GetByNamesAsync(IReadOnlyCollection<string> names);
}