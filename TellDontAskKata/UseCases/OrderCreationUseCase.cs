﻿using System;
using System.Collections.Generic;
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
                    var itemQuantity = itemRequest.Quantity;
                    
                    decimal unitaryTaxedAmount = decimal.Round(product.Price + product.CalculateUnitaryTax(), 2, MidpointRounding.AwayFromZero); // .setScale(2, HALF_UP)
                    decimal taxedAmount = decimal.Round(unitaryTaxedAmount * itemQuantity, 2, MidpointRounding.AwayFromZero); // .setScale(2, HALF_UP)
                    decimal taxAmount = product.CalculateUnitaryTax() * itemQuantity;

                    var orderItem = new OrderItem
                    {
                        Product = product,
                        Quantity = itemQuantity,
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
}
