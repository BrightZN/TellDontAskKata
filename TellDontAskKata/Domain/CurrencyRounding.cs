using System;
using System.Collections.Generic;
using System.Text;

namespace TellDontAskKata.Domain
{
    public static class CurrencyRounding
    {
        public static decimal Round(decimal value)
        {
            return decimal.Round(value, 2, MidpointRounding.AwayFromZero);
        }
    }
}
