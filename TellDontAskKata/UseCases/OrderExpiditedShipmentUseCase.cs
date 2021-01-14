using System;
using System.Threading.Tasks;
using TellDontAskKata.Repositories;
using TellDontAskKata.Services;

namespace TellDontAskKata.UseCases
{
    public class OrderExpiditedShipmentUseCase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductCatalog _productCatalog;
        private readonly IShipmentService _shipmentService;

        public OrderExpiditedShipmentUseCase()
        {
        }

        public OrderExpiditedShipmentUseCase(IOrderRepository orderRepository, IProductCatalog productCatalog, IShipmentService shipmentService)
        {
            _orderRepository = orderRepository;
            _productCatalog = productCatalog;
            _shipmentService = shipmentService;
        }

        public async Task RunAsync(SellItemsRequest request)
        {


            //throw new NotImplementedException();
        }
    }
}