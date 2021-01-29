using System.Collections.Generic;
using System.Threading.Tasks;
using TellDontAskKata.Domain;
using TellDontAskKata.UseCases;

namespace TellDontAskKata.Repositories
{
    public interface IProductCatalog
    {
        Task<Product> GetByNameAsync(string name);
        Task<ProductList> GetListByNamesAsync(IEnumerable<string> names);
    }
}
