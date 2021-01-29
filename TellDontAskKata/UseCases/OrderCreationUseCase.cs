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
            var order = new Order
            {
                Status = OrderStatus.Created,
                Items = new List<OrderItem>(),
                Currency = "EUR"
            };

            foreach(var itemRequest in request.Requests)
            {
                var product = await _productCatalog.GetByNameAsync(itemRequest.Name);

                if(product == null)
                {
                    throw new UnknownProductException();
                }

                var itemQuantity = itemRequest.Quantity;

                var orderItem = new OrderItem
                {
                    Product = product,
                    Quantity = itemQuantity
                };

                order.Items.Add(orderItem);
            }

            await _orderRepository.SaveAsync(order);
        }
    }
}
