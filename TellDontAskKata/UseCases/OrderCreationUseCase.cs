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
            var orderItems = await CreateOrderItemsAsync(request);

            var order = new Order("EUR", orderItems);

            await _orderRepository.SaveAsync(order);
        }

        private async Task<IEnumerable<OrderItem>> CreateOrderItemsAsync(SellItemsRequest request)
        {
            var productList = await _productCatalog.GetListByNamesAsync(request.ProductNames);

            return request.Requests
                .Select(itemRequest => OrderItem.Create(itemRequest.Name, itemRequest.Quantity, productList))
                .ToList();
        }
    }
}
