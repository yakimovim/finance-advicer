using System;
using System.Collections.Generic;
using System.Linq;

namespace ActiveSelection
{
    public class Portfolio
    {
        public Portfolio(Money cash, IReadOnlyList<AssetSize> assets)
        {
            Cash = cash ?? throw new ArgumentNullException(nameof(cash));
            if(Cash < 0.Dollars())
                throw new ArgumentOutOfRangeException("Amount of money must be non-negative.");
            Assets = assets ?? throw new ArgumentNullException(nameof(assets));
            if(Assets.Count != Assets.Select(a => a.Ticker).Distinct().Count())
                throw new ArgumentException("All tickers must be different.");
        }

        public Portfolio(Money cash, params AssetSize[] assets)
        {
            Cash = cash ?? throw new ArgumentNullException(nameof(cash));
            if (Cash < 0.Dollars())
                throw new ArgumentOutOfRangeException("Amount of money must be non-negative.");
            Assets = assets ?? throw new ArgumentNullException(nameof(assets));
            if (Assets.Count != Assets.Select(a => a.Ticker).Distinct().Count())
                throw new ArgumentException("All tickers must be different.");
        }

        public Money Cash { get; }

        public IReadOnlyList<AssetSize> Assets { get; }

        public Money Cost(IDictionary<string, Money> prices)
        {
            return Cash + Assets
                       .Select(a => a.Size * prices[a.Ticker])
                       .Aggregate(new Money(0, Currency.Rubles), (seed, money) => seed + money);
        }
    }
}