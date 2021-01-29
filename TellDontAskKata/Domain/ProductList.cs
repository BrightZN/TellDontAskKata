using System.Collections.Generic;
using System.Linq;
using TellDontAskKata.Domain;

namespace TellDontAskKata.UseCases
{
    public class ProductList
    {
        private List<Product> _products;

        public ProductList(List<Product> products)
        {
            _products = products;
        }

        public bool Missing(string name)
        {
            return _products.All(p => p.Name != name);
        }

        public Product GetByName(string name)
        {
            return _products.Single(p => p.Name == name);
        }
    }
}