using System.Collections.Generic;
using System.Linq;
using TellDontAskKata.Domain;

namespace TellDontAskKata.UseCases
{
    public class ProductList
    {
        private readonly IEnumerable<Product> _products;

        public ProductList(IEnumerable<Product> products) => _products = products;

        public bool Missing(string name) => _products.All(p => p.Name != name);

        public Product GetByName(string name) => _products.Single(p => p.Name == name);
    }
}