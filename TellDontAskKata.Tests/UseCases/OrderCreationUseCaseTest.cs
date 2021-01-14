using System.Collections.Generic;
using System.Threading.Tasks;
using TellDontAskKata.Domain;
using TellDontAskKata.Repositories;
using TellDontAskKata.Tests.Doubles;
using TellDontAskKata.UseCases;
using Xunit;

namespace TellDontAskKata.Tests.UseCases
{
    public class OrderCreationUseCaseTest
    {
        private readonly TestOrderRepository _orderRepository;
        private readonly Category _food;
        private readonly IProductCatalog _productCatalog;
        private readonly OrderCreationUseCase _useCase;

        public OrderCreationUseCaseTest()
        {
            _orderRepository = new TestOrderRepository();

            _food = new Category(name: "food", taxPercentage: 10.00M);

            _productCatalog = new InMemoryProductCatalog(new List<Product> { 
                new Product 
                {
                    Name = "salad",
                    Price = 3.56M,
                    Category = _food
                },
                new Product
                {
                    Name = "tomato",
                    Price = 4.65M,
                    Category = _food
                }
            });

            _useCase = new OrderCreationUseCase(_orderRepository, _productCatalog);
        }

        [Fact]
        public async Task Sell_Multiple_Items()
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

            var createdOrder = _orderRepository.SavedOrder;

            Assert.Equal(OrderStatus.Created, createdOrder.Status);
            Assert.Equal(23.20M, createdOrder.Total);
            Assert.Equal("EUR", createdOrder.Currency);
            Assert.Equal(2, createdOrder.Items.Count);

            var firstItem = createdOrder.Items[0];

            Assert.Equal("salad", firstItem.Product.Name);
            Assert.Equal(3.56M, firstItem.Product.Price);
            Assert.Equal(2, firstItem.Quantity);
            Assert.Equal(7.84M, firstItem.TaxedAmount);
            Assert.Equal(0.72M, firstItem.Tax);

            var secondItem = createdOrder.Items[1];

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
        }
    }
}
