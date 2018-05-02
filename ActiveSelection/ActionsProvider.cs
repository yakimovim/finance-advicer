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
            IReadOnlyList<AssetPrice> prices)
        {
            var pricesMap = prices.ToDictionary(p => p.Ticker, p => p.Value);

            ValidateThatProcesForAllTickersAreAvailable(portfolio, allocation, pricesMap);

            var targetAssetSizes = CalculateTargetAssetSizes(portfolio, allocation, pricesMap);

            var actions = GetActions(portfolio, targetAssetSizes).OrderBy(a => a.Type).ToArray();

            ValidateActions(portfolio, actions, pricesMap);

            return actions;
        }

        private static void ValidateThatProcesForAllTickersAreAvailable(Portfolio portfolio, AssetAllocation allocation,
            Dictionary<string, Money> pricesMap)
        {
            foreach (var ticker in portfolio.Assets.Select(a => a.Ticker))
            {
                if (!pricesMap.ContainsKey(ticker))
                    throw new ArgumentException($"There is no price for ticker '{ticker}' in portfolio.");
            }

            foreach (var ticker in allocation.Portions.Select(p => p.Ticker))
            {
                if (!pricesMap.ContainsKey(ticker))
                    throw new ArgumentException($"There is no price for ticker '{ticker}' in asset allocation.");
            }
        }

        private static Dictionary<string, int> CalculateTargetAssetSizes(Portfolio portfolio, AssetAllocation allocation,
            Dictionary<string, Money> pricesMap)
        {
            var portfolioCost = portfolio.Cost(pricesMap);

            var targetAssetSizes = new Dictionary<string, int>();

            while (allocation.Portions.Count > 0)
            {
                var assetSize = allocation.GetAssetWithSmallestReminder(portfolioCost, pricesMap);
                allocation = allocation.Remove(assetSize.Ticker);

                targetAssetSizes[assetSize.Ticker] = assetSize.Size;

                portfolioCost = portfolioCost - assetSize.Size * pricesMap[assetSize.Ticker];
            }

            return targetAssetSizes;
        }

        private static List<Action> GetActions(Portfolio portfolio, Dictionary<string, int> targetAssetSizes)
        {
            var actions = new List<Action>();

            foreach (var asset in portfolio.Assets)
            {
                if (!targetAssetSizes.ContainsKey(asset.Ticker))
                {
                    actions.Add(new Action(asset.Ticker, asset.Size, ActionType.Sell));
                }
                else
                {
                    var targetSize = targetAssetSizes[asset.Ticker];

                    if (targetSize > asset.Size)
                    {
                        actions.Add(new Action(asset.Ticker, targetSize - asset.Size, ActionType.Buy));
                    }
                    else if (targetSize < asset.Size)
                    {
                        actions.Add(new Action(asset.Ticker, asset.Size - targetSize, ActionType.Sell));
                    }

                    targetAssetSizes.Remove(asset.Ticker);
                }
            }

            foreach (var ticker in targetAssetSizes.Keys)
            {
                var targetSize = targetAssetSizes[ticker];

                if (targetSize > 0)
                {
                    actions.Add(new Action(ticker, targetSize, ActionType.Buy));
                }
            }
            return actions;
        }

        private void ValidateActions(Portfolio portfolio, IReadOnlyList<Action> actions, Dictionary<string, Money> pricesMap)
        {
            if(actions.GroupBy(a => a.Ticker).Any(g => g.Count() > 1))
                throw new InvalidOperationException("There must be only one action for ticker.");

            var freeMoney = portfolio.Cash;

            foreach (var action in actions)
            {
                switch (action.Type)
                {
                    case ActionType.Sell:
                        freeMoney += action.Size * pricesMap[action.Ticker];
                        break;
                    case ActionType.Buy:
                        freeMoney -= action.Size * pricesMap[action.Ticker];
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                if(freeMoney < 0.Rubles())
                    throw new InvalidOperationException("There is no money to execute action.");
            }
        }
    }
}