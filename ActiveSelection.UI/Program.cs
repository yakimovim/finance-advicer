using System;
using System.Linq;

namespace ActiveSelection.UI
{
    using static Currency;

    internal class Program
    {
        private static void Main()
        {
            CurrencyConverter.ClearRates();
            CurrencyConverter.One(Dollars).Costs(73.67M.Rubles());

            var myPortfolio = new Portfolio(27070.4M.Rubles(),
                new AssetSize("FXMM", 265),
                new AssetSize("FXUS", 54),
                new AssetSize("FXDE", 62),
                new AssetSize("FXCN", 25),
                new AssetSize("FXGD", 106));

            var myAssetAllocation = new AssetAllocation(
                new AssetPortion("FXMM", 0.40M),
                new AssetPortion("FXUS", 0.25M),
                new AssetPortion("FXDE", 0.15M),
                new AssetPortion("FXCN", 0.10M),
                new AssetPortion("FXGD", 0.10M)
                );

            var prices = new[]
            {
                new AssetPrice("FXMM", 1646.8M.Rubles()),
                new AssetPrice("FXUS", 4949M.Rubles()),
                new AssetPrice("FXDE", 2668.5M.Rubles()),
                new AssetPrice("FXCN", 3917.5M.Rubles()),
                new AssetPrice("FXGD", 926.6M.Rubles())
            };

            var actions = new ActionsProvider().GetActions(
                myPortfolio,
                myAssetAllocation,
                prices
            );

            foreach (var action in actions)
            {
                Console.WriteLine(action);
            }

            var restCash = myPortfolio.Cash;

            foreach (var action in actions)
            {
                restCash = restCash - ((action.Type == ActionType.Buy)
                    ? action.Size * prices.Single(ap => ap.Ticker == action.Ticker).Value
                    : -action.Size * prices.Single(ap => ap.Ticker == action.Ticker).Value);
            }

            Console.WriteLine($"Rest of cash: {restCash}");

            Console.WriteLine();
            Console.WriteLine("Press ENTER to exit...");
            Console.ReadLine();
        }
    }
}