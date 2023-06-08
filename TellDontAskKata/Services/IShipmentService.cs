using System.Threading.Tasks;
using TellDontAskKata.Domain;

namespace TellDontAskKata.Services;

public interface IShipmentService
{
    Task ShipAsync(Order order);
}