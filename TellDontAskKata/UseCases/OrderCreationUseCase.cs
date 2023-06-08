using System.Linq;
using System.Threading.Tasks;
using TellDontAskKata.Domain;
using TellDontAskKata.Repositories;

namespace TellDontAskKata.UseCases;

public class OrderCreationUseCase
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductCatalog _productCatalog;

    public OrderCreationUseCase(
        IOrderRepository orderRepository, IProductCatalog productCatalog)
    {
        _orderRepository = orderRepository;
        _productCatalog = productCatalog;
    }

    public async Task RunAsync(SellItemsRequest request)
    {
        var productNames = GetProductNames(request);
        var products = await _productCatalog.GetByNamesAsync(productNames);

        var order = CreateOrder(request, products);

        await _orderRepository.SaveAsync(order);
    }

    private static Order CreateOrder(SellItemsRequest request, ProductList products)
    {
        var order = new Order
        {
            Status = OrderStatus.Created,
            Currency = "EUR",
        };

        foreach (var itemRequest in request.Requests)
        {
            var product = products.FindByName(itemRequest.Name);

            if (product is null)
                throw new UnknownProductException();

            order.AddItem(product, itemRequest.Quantity);
        }

        return order;
    }

    private static string[] GetProductNames(SellItemsRequest request)
    {
        return request.Requests.Select(r => r.Name).ToArray();
    }
}