using System;
using System.Collections.Generic;
using Xunit;

namespace ActiveSelection.Test
{
    using static Currency;

    public class AssetAllocationTest
    {
        private readonly Dictionary<string, Money> _prices;

        public AssetAllocationTest()
        {
            CurrencyConverter.ConvertionRates.Clear();
            CurrencyConverter.ConvertionRates[Tuple.Create(Dollars, Rubles)] = 2.0M;

            _prices = new Dictionary<string, Money>
            {
                {"AGG", 20.Dollars()},
                {"VTT", 15.Dollars()}
            };
        }

        [Fact]
        public void GetPortionWithSmallestReminder_PortfolioCostIsTooSmall()
        {
            var allocation = new AssetAllocation(new AssetPortion("AGG", 1.0M));

            var assetSize = allocation.GetAssetWithSmallestReminder(10.Dollars(), _prices);

            Assert.Equal("AGG", assetSize.Ticker);
        }
    }
}