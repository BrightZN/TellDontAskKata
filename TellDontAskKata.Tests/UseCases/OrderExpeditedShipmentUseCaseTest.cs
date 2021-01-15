using System;
using System.Collections.Generic;
using System.Linq;
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
    public class OrderExpeditedShipmentUseCaseTest
    {
        private readonly TestOrderRepository _orderRepository;
        private readonly IProductCatalog _productCatalog;
        private readonly TestShipmentService _shipmentService;
        private readonly Category _food;

        private readonly OrderExpeditedShipmentUseCase _useCase;

        public OrderExpeditedShipmentUseCaseTest()
        {
            _orderRepository = new TestOrderRepository();
            _shipmentService = new TestShipmentService();

            _food = new Category(name: "food", taxPercentage: 10.00M);

            _productCatalog = new InMemoryProductCatalog(new List<Product> {
                new Product(name: "salad", price: 3.56M, category: _food),
                new Product(name: "tomato", price: 4.65M, category: _food)
            });

            _useCase = new OrderExpeditedShipmentUseCase(_orderRepository, _productCatalog, _shipmentService);
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

            await _useCase.RunAsync(request);

            var shippedOrder = _shipmentService.ShippedOrder;

            Assert.NotNull(shippedOrder);
            Assert.Equal(OrderStatus.Shipped, shippedOrder.Status);

            var createdOrder = _orderRepository.SavedOrder;

            Assert.Equal(shippedOrder, createdOrder);

            Assert.Equal(23.20M, createdOrder.Total);
            Assert.Equal("EUR", createdOrder.Currency);
            Assert.Equal(2, createdOrder.Items.Count());

            var firstItem = createdOrder.Items.First();

            Assert.Equal("salad", firstItem.Product.Name);
            Assert.Equal(3.56M, firstItem.Product.Price);
            Assert.Equal(2, firstItem.Quantity);
            Assert.Equal(7.84M, firstItem.TaxedAmount);
            Assert.Equal(0.72M, firstItem.Tax);

            var secondItem = createdOrder.Items.Skip(1).First();

            Assert.Equal("tomato", secondItem.Product.Name);
            Assert.Equal(4.65M, secondItem.Product.Price);
            Assert.Equal(3, secondItem.Quantity);
            Assert.Equal(15.36M, secondItem.TaxedAmount);
            Assert.Equal(1.41M, secondItem.Tax);
        }

        [Fact]
        public async Task Cannot_Create_Order_With_Unknown_Product()
        {
            var request = new SellItemsRequest
            {
                Requests = new List<SellItemRequest>
                {
                    new SellItemRequest
                    {
                        Name = "unknown product"
                    }
                }
            };

            await Assert.ThrowsAsync<UnknownProductException>(() => _useCase.RunAsync(request));

            Assert.Null(_orderRepository.SavedOrder);
        }
    }
}
