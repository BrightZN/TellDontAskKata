using System;
using System.Threading.Tasks;
using TellDontAskKata.Domain;
using TellDontAskKata.Repositories;
using TellDontAskKata.Services;

namespace TellDontAskKata.UseCases
{
    public class OrderExpeditedShipmentUseCase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductCatalog _productCatalog;
        private readonly IShipmentService _shipmentService;

        public OrderExpeditedShipmentUseCase(IOrderRepository orderRepository, IProductCatalog productCatalog, IShipmentService shipmentService)
        {
            _orderRepository = orderRepository;
            _productCatalog = productCatalog;
            _shipmentService = shipmentService;
        }

        public async Task RunAsync(SellItemsRequest request)
        {
            var productList = await _productCatalog.GetForNamesAsync(request.ProductNames);
            
            var order = new Order(
                currency: "EUR",
                items: request.ToOrderItems(productList));

            order.Approve();

            await order.ShipAsync(_shipmentService);

            await _orderRepository.SaveAsync(order);
        }
    }
}