using System;

namespace TellDontAskKata.Domain
{
    public static class CurrencyRounding
    {
        public static decimal Round(decimal value) => decimal.Round(value, 2, MidpointRounding.AwayFromZero);
    }
}
