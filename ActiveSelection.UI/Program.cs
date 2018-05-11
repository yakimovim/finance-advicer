using System;

namespace ActiveSelection.UI
{
    using static Currency;

    class Program
    {
        static void Main()
        {
            CurrencyConverter.ClearRates();
            CurrencyConverter.One(Dollars).Costs(61.76M.Rubles());

            var myPortfolio = new Portfolio(133150.91M.Rubles());

            var myAssetAllocation = new AssetAllocation(
                new AssetPortion("AGG", 0.45M),
                new AssetPortion("VT", 0.35M),
                new AssetPortion("VWO", 0.10M),
                new AssetPortion("VNQI", 0.10M)
                );

            var prices = new[]
            {
                new AssetPrice("AGG", 1.Dollars()),
                new AssetPrice("VT", 1.Dollars()),
                new AssetPrice("VWO", 1.Dollars()),
                new AssetPrice("VNQI", 1.Dollars())
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

            Console.WriteLine();
            Console.WriteLine("Press ENTER to exit...");
            Console.ReadLine();
        }
    }
}
