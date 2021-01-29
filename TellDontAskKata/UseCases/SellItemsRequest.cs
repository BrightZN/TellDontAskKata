using System.Collections.Generic;
using System.Linq;

namespace TellDontAskKata.UseCases
{
    public class SellItemsRequest
    {
        public List<SellItemRequest> Requests { get; set; }

        public IEnumerable<string> ProductNames => Requests.Select(r => r.Name);
    }
}
