using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TellDontAskKata.Domain;
using TellDontAskKata.Repositories;

namespace TellDontAskKata.UseCases;

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
            Currency = "EUR",
            Total = 0.00M,
            Tax = 0.00M
        };

        var productNames = request.Requests.Select(r => r.Name).ToArray();

        var products = await _productCatalog.GetByNamesAsync(productNames);

        foreach(var itemRequest in request.Requests)
        {
            var product = products.FindByName(itemRequest.Name);

            if(product is null)
                throw new UnknownProductException();
            
            // need to find the C# equivalent of Java BigDecimal.setScale(2, HALF_UP)
                    
            var quantity = itemRequest.Quantity;

            var orderItem = new OrderItem(product, quantity);

            order.Items.Add(orderItem);

            order.Total += orderItem.TaxedAmount;
            order.Tax += orderItem.Tax;
        }

        await _orderRepository.SaveAsync(order);
    }
}