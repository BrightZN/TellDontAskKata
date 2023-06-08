using System;
using System.Collections.Generic;
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

        foreach(var itemRequest in request.Requests)
        {
            var product = await _productCatalog.GetByNameAsync(itemRequest.Name);

            if(product == null)
            {
                throw new UnknownProductException();
            }
            else
            {
                // need to find the C# equivalent of Java BigDecimal.setScale(2, HALF_UP)
                    
                var itemRequestQuantity = itemRequest.Quantity;
                
                var unitaryTax =  decimal.Round(
                    product.Price / 100.00M * product.Category.TaxPercentage, 2, MidpointRounding.AwayFromZero);
                
                var unitaryTaxedAmount =  decimal.Round(
                    product.Price + unitaryTax, 2, MidpointRounding.AwayFromZero);
                
                var taxedAmount = decimal.Round(
                    unitaryTaxedAmount * itemRequestQuantity, 2, MidpointRounding.AwayFromZero);
                
                var taxAmount = unitaryTax * itemRequestQuantity;

                var orderItem = new OrderItem
                {
                    Product = product,
                    Quantity = itemRequestQuantity,
                    Tax = taxAmount,
                    TaxedAmount = taxedAmount
                };

                order.Items.Add(orderItem);

                order.Total += taxedAmount;
                order.Tax += taxAmount;
            }
        }

        await _orderRepository.SaveAsync(order);
    }
}