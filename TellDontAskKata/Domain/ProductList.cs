using System;
using System.Collections.Generic;
using System.Linq;

namespace TellDontAskKata.Domain
{
    public class ProductList
    {
        private readonly IEnumerable<Product> _products;

        public ProductList(IEnumerable<Product> products) => _products = products;

        public bool Missing(string name) => !Contains(name);

        private bool Contains(string name) => _products.Any(p => p.Name == name);

        public Product GetProductByName(string name) => _products.Single(p => p.Name == name);
    }
}