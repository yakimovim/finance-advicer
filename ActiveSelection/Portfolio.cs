using System;
using System.Collections.Generic;
using System.Linq;

namespace ActiveSelection
{
    public class Portfolio
    {
        public Portfolio(Money cash, IReadOnlyList<Asset> assets)
        {
            Cash = cash ?? throw new ArgumentNullException(nameof(cash));
            Assets = assets ?? throw new ArgumentNullException(nameof(assets));
        }

        public Money Cash { get; }

        public IReadOnlyList<Asset> Assets { get; }

        public Money Cost(IDictionary<string, Money> prices)
        {
            return Cash + Assets
                       .Select(a => a.Number * prices[a.Ticker])
                       .Aggregate(new Money(0, Currency.Rubles), (seed, money) => seed + money);
        }
    }
}