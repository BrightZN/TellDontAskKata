using System.Threading.Tasks;
using TellDontAskKata.Domain;

namespace TellDontAskKata.Repositories;

public interface IProductCatalog
{
    Task<Product?> GetByNameAsync(string name);
}