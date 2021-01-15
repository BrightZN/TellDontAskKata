using System.Collections.Generic;
using System.Linq;
using TellDontAskKata.Domain;

namespace TellDontAskKata.UseCases
{
    public static class SellItemsRequestExtensions
    {
        public static IEnumerable<OrderItem> ToOrderItems(this SellItemsRequest request, ProductList productList)
        {
            return request.Requests
                .Select(r => OrderItem.Create(r.Name, r.Quantity, productList))
                .ToList();
        }
    }
}
