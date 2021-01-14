using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TellDontAskKata.Domain;
using TellDontAskKata.Repositories;
using TellDontAskKata.Services;
using TellDontAskKata.Tests.Doubles;
using TellDontAskKata.UseCases;
using Xunit;

namespace TellDontAskKata.Tests.UseCases
{
    public class OrderExpiditedShipmentUseCaseTest
    {
        private readonly Category _food;

        public OrderExpiditedShipmentUseCaseTest()
        {
            _food = new Category(name: "food", taxPercentage: 10.00M);
        }

        [Fact]
        public async Task Creates_Approves_And_Ships_Order()
        {
            var request = new SellItemsRequest
            {
                Requests = new List<SellItemRequest>
                {
                    new SellItemRequest
                    {
                        Name = "salad",
                        Quantity = 2
                    },
                    new SellItemRequest
                    {
                        Name = "tomato",
                        Quantity = 3
                    }
                }
            };

            IOrderRepository orderRepository = new TestOrderRepository();

            IProductCatalog productCatalog = new InMemoryProductCatalog(new List<Product> {
                new Product(name: "salad", price: 3.56M, category: _food),
                new Product(name: "tomato", price: 4.65M, category: _food)
            });

            IShipmentService shipmentService = new TestShipmentService();

            var useCase = new OrderExpiditedShipmentUseCase(orderRepository, productCatalog, shipmentService);

            await useCase.RunAsync(request);
        }
    }
}
