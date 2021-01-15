using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TellDontAskKata.Domain;
using TellDontAskKata.Repositories;

namespace TellDontAskKata.UseCases
{
    public class OrderCreationUseCase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductCatalog _productCatalog;

        public OrderCreationUseCase(IOrderRepository orderRepository, IProductCatalog productCatalog)
        {
            _orderRepository = orderRepository;
            _productCatalog = productCatalog;
        }

        public async Task RunAsync(SellItemsRequest request)
        {
            var productList = await _productCatalog.GetForNamesAsync(request.ProductNames);

            var order = new Order(
                currency: "EUR", 
                items: CreateOrderItems(request, productList));

            await _orderRepository.SaveAsync(order);
        }

        private static IEnumerable<OrderItem> CreateOrderItems(SellItemsRequest request, ProductList productList)
        {
            return request.Requests
                .Select(r => OrderItem.Create(r.Name, r.Quantity, productList))
                .ToList();
        }
    }
}
