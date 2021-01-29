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
            var orderItems = await CreateOrderItems(request);

            var order = new Order
            {
                Status = OrderStatus.Created,
                Items = orderItems,
                Currency = "EUR"
            };

            await _orderRepository.SaveAsync(order);
        }

        private async Task<List<OrderItem>> CreateOrderItems(SellItemsRequest request)
        {
            ProductList productList = await _productCatalog.GetListByNamesAsync(request.Requests.Select(r => r.Name));
            
            var orderItems = new List<OrderItem>();

            foreach (var itemRequest in request.Requests)
            {
                var product = await _productCatalog.GetByNameAsync(itemRequest.Name);

                if (product == null)
                    throw new UnknownProductException();

                var itemQuantity = itemRequest.Quantity;

                var orderItem = new OrderItem
                {
                    Product = product,
                    Quantity = itemQuantity
                };

                orderItems.Add(orderItem);
            }

            return orderItems;
        }
    }
}
