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
            var productNames = GetProductNames(request);
            var productList = await _productCatalog.GetForNamesAsync(productNames);

            var order = new Order
            {
                Status = OrderStatus.Created,
                Currency = "EUR",
                Items = CreateOrderItems(request, productList)
            };

            await _orderRepository.SaveAsync(order);
        }

        private static List<OrderItem> CreateOrderItems(SellItemsRequest request, ProductList productList)
        {
            var items = new List<OrderItem>();

            foreach (var itemRequest in request.Requests)
            {
                if (productList.Missing(itemRequest.Name))
                {
                    throw new UnknownProductException();
                }
                else
                {
                    var orderItem = new OrderItem
                    {
                        Product = productList.GetProductByName(itemRequest.Name),
                        Quantity = itemRequest.Quantity
                    };

                    items.Add(orderItem);
                }
            }

            return items;
        }

        private static IEnumerable<string> GetProductNames(SellItemsRequest request)
        {
            return request.Requests.Select(r => r.Name);
        }
    }
}
