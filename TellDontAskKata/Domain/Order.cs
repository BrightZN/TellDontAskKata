using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TellDontAskKata.Services;
using TellDontAskKata.UseCases;

namespace TellDontAskKata.Domain;

public class Order
{
    public int Id { get; set; }
    public string Currency { get; set; } = string.Empty;
    public List<OrderItem> Items { get; } = new();
    public OrderStatus Status { get; set; }
    public decimal Total => Items.Sum(i => i.TaxedAmount);
    public decimal Tax => Items.Sum(i => i.Tax);

    public async Task Ship(IShipmentService shipmentService)
    {
        if (Status is OrderStatus.Created or OrderStatus.Rejected)
            throw new OrderCannotBeShippedException();

        if (Status is OrderStatus.Shipped)
            throw new OrderCannotBeShippedTwiceException();

        await shipmentService.ShipAsync(this);

        Status = OrderStatus.Shipped;
    }

    public void Approve(bool approved)
    {
        if (Status == OrderStatus.Shipped)
            throw new ShippedOrdersCannotBeChangedException();

        if (approved && Status == OrderStatus.Rejected)
            throw new RejectedOrderCannotBeApprovedException();

        if (!approved && Status == OrderStatus.Approved)
            throw new ApprovedOrderCannotBeRejectedException();

        Status = approved ? OrderStatus.Approved : OrderStatus.Rejected;
    }

    public void AddItem(Product product, int quantity)
    {
        var orderItem = new OrderItem(product, quantity);

        Items.Add(orderItem);
    }
}