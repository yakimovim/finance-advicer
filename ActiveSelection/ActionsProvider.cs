using System;
using System.Collections.Generic;
using System.Linq;

namespace ActiveSelection
{
    public class ActionsProvider
    {
        public IReadOnlyList<Action> GetActions(
            Portfolio portfolio,
            AssetAllocation allocation,
            IReadOnlyList<Price> prices)
        {
            var pricesMap = prices.ToDictionary(p => p.Ticker, p => p.Value);

            foreach (var ticker in portfolio.Assets.Select(a => a.Ticker))
            {
                if(!pricesMap.ContainsKey(ticker))
                    throw new ArgumentException($"There is no price for ticker '{ticker}' in portfolio.");
            }

            foreach (var ticker in allocation.Portions.Select(p => p.Ticker))
            {
                if(!pricesMap.ContainsKey(ticker))
                    throw new ArgumentException($"There is no price for ticker '{ticker}' in asset allocation.");
            }

            var portfolioCost = portfolio.Cost(pricesMap);

            while (allocation.Portions.Count > 0)
            {
                var assetSize = allocation.GetAssetWithSmallestReminder(portfolioCost, pricesMap);
                allocation = allocation.Remove(assetSize.Ticker);
            }

            return null;
        }
    }
}