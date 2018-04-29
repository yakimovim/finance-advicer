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
            CurrencyConverter.ClearRates();
            CurrencyConverter.One(Dollars).Costs(2.Rubles());

            _prices = new Dictionary<string, Money>
            {
                {"AGG", 20.Dollars()},
                {"VTT", 15.Dollars()}
            };
        }

        [Fact]
        public void Constructor_NoAssets()
        {
            var allocation = new AssetAllocation();

            Assert.NotNull(allocation);
        }

        [Fact]
        public void Constructor_PortionsDoesNotSumToOne()
        {
            Assert.Throws<ArgumentException>(() => new AssetAllocation(
                new AssetPortion("AGG", 0.45M),
                new AssetPortion("VTT", 0.45M)
                ));
        }

        [Fact]
        public void Constructor_SameTickers()
        {
            Assert.Throws<ArgumentException>(() => new AssetAllocation(
                new AssetPortion("AGG", 0.45M),
                new AssetPortion("AGG", 0.55M)
            ));
        }

        [Fact]
        public void GetPortionWithSmallestReminder_PortfolioCostIsTooSmall()
        {
            var allocation = new AssetAllocation(new AssetPortion("AGG", 1.0M));

            var assetSize = allocation.GetAssetWithSmallestReminder(10.Dollars(), _prices);

            Assert.Equal("AGG", assetSize.Ticker);
            Assert.Equal(0, assetSize.Size);
        }

        [Fact]
        public void GetPortionWithSmallestReminder_OneAsset()
        {
            var allocation = new AssetAllocation(new AssetPortion("AGG", 1.0M));

            var assetSize = allocation.GetAssetWithSmallestReminder(30.Dollars(), _prices);

            Assert.Equal("AGG", assetSize.Ticker);
            Assert.Equal(1, assetSize.Size);
        }

        [Fact]
        public void GetPortionWithSmallestReminder_SeveralAssets()
        {
            var allocation = new AssetAllocation(
                new AssetPortion("AGG", 0.55M),
                new AssetPortion("VTT", 0.45M));

            var assetSize = allocation.GetAssetWithSmallestReminder(100.Dollars(), _prices);

            Assert.Equal("VTT", assetSize.Ticker);
            Assert.Equal(3, assetSize.Size);
        }
    }
}