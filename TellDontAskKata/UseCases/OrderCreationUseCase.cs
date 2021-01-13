﻿using System;
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
            var order = new Order
            {
                Status = OrderStatus.Created,
                Items = new List<OrderItem>(),
                Currency = "EUR",
                Total = 0.00M,
                Tax = 0.00M
            };

            var productNames = GetProductNames(request);
            var productList = await _productCatalog.GetForNamesAsync(productNames);

            foreach (var itemRequest in request.Requests)
            {
                //var product = await _productCatalog.GetByNameAsync(itemRequest.Name);

                if (productList.Missing(itemRequest.Name))
                {
                    throw new UnknownProductException();
                }
                else
                {
                    // need to find the C# equivalent of Java BigDecimal.setScale(2, HALF_UP)
                    var product = productList.GetByName(itemRequest.Name);

                    int quantity = itemRequest.Quantity;

                    decimal taxedAmount = CalculateTaxedAmount(product, quantity); // .setScale(2, HALF_UP)
                    decimal taxAmount = CalculateTax(product, quantity);

                    var orderItem = new OrderItem
                    {
                        Product = product,
                        Quantity = quantity,
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

        private static decimal CalculateTax(Product product, int quantity)
        {
            return GetUnitaryTax(product) * quantity;
        }

        private static decimal CalculateTaxedAmount(Product product, int quantity)
        {
            return decimal.Round(GetUnitaryTaxedAmount(product) * quantity, 2, MidpointRounding.AwayFromZero);
        }

        private static decimal GetUnitaryTaxedAmount(Product product)
        {
            return decimal.Round(product.Price + GetUnitaryTax(product), 2, MidpointRounding.AwayFromZero);
            // .setScale(2, HALF_UP)
        }

        private static decimal GetUnitaryTax(Product product)
        {
            return decimal.Round(product.Price / 100.00M * product.Category.TaxPercentage, 2, MidpointRounding.AwayFromZero);
            // .setScale(2, HALF_UP)
        }

        private static IEnumerable<string> GetProductNames(SellItemsRequest request)
        {
            return request.Requests.Select(r => r.Name);
        }
    }
}
