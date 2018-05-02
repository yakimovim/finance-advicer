using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ActiveSelection.Test
{
    using static Currency;
    using static ActionType;

    public class ActionsProviderTest
    {
        private static readonly IReadOnlyList<AssetPrice> Prices = new[]
        {
            new AssetPrice("AGG", 100.Dollars()),
            new AssetPrice("VTT", 70.Dollars()),
            new AssetPrice("VTA", 80.Dollars()),
        };

        private readonly ActionsProvider _provider;

        public ActionsProviderTest()
        {
            CurrencyConverter.ClearRates();
            CurrencyConverter.One(Dollars).Costs(60.Rubles());

            _provider = new ActionsProvider();
        }

        [Fact]
        public void EmptyPortfolio()
        {
            var actions = _provider.GetActions(
                new Portfolio(0.Rubles()),
                new AssetAllocation(
                    new AssetPortion("AGG", 0.45M),
                    new AssetPortion("VTT", 0.55M)
                ),
                Prices
            );

            Assert.NotNull(actions);
            Assert.Equal(0, actions.Count);
        }

        [Fact]
        public void EmptyAllocation()
        {
            var actions = _provider.GetActions(
                new Portfolio(100.Dollars()),
                new AssetAllocation(),
                Prices
            );

            Assert.NotNull(actions);
            Assert.Equal(0, actions.Count);
        }

        [Fact]
        public void OneAsset_Buy()
        {
            var actions = _provider.GetActions(
                new Portfolio(101.Dollars(),
                    new AssetSize("AGG", 3)),
                new AssetAllocation(
                    new AssetPortion("AGG", 1.0M)
                    ),
                Prices
            );

            Assert.NotNull(actions);
            Assert.Equal(1, actions.Count);
            Assert.Equal("AGG", actions[0].Ticker);
            Assert.Equal(1, actions[0].Size);
            Assert.Equal(Buy, actions[0].Type);
        }

        [Fact]
        public void OneAsset_DoNothing()
        {
            var actions = _provider.GetActions(
                new Portfolio(99.Dollars(),
                    new AssetSize("AGG", 3)),
                new AssetAllocation(
                    new AssetPortion("AGG", 1.0M)
                ),
                Prices
            );

            Assert.NotNull(actions);
            Assert.Equal(0, actions.Count);
        }

        [Fact]
        public void TwoAssets_BuyBoth()
        {
            var actions = _provider.GetActions(
                new Portfolio(1000.Dollars()),
                new AssetAllocation(
                    new AssetPortion("AGG", 0.45M),
                    new AssetPortion("VTT", 0.55M)
                ),
                Prices
            );

            Assert.NotNull(actions);
            Assert.Equal(2, actions.Count);

            var aggAction = actions.Single(a => a.Ticker == "AGG");
            var vttAction = actions.Single(a => a.Ticker == "VTT");

            Assert.Equal(Buy, aggAction.Type);
            Assert.Equal(4, aggAction.Size);

            Assert.Equal(Buy, vttAction.Type);
            Assert.Equal(8, vttAction.Size);
        }

        [Fact]
        public void TwoAssets_SellAndBuy()
        {
            var actions = _provider.GetActions(
                new Portfolio(0.Dollars(),
                    new AssetSize("AGG", 10)
                ),
                new AssetAllocation(
                    new AssetPortion("AGG", 0.45M),
                    new AssetPortion("VTT", 0.55M)
                ),
                Prices
            );

            Assert.NotNull(actions);
            Assert.Equal(2, actions.Count);

            var aggAction = actions.Single(a => a.Ticker == "AGG");
            var vttAction = actions.Single(a => a.Ticker == "VTT");

            Assert.Equal(Sell, aggAction.Type);
            Assert.Equal(6, aggAction.Size);

            Assert.Equal(Buy, vttAction.Type);
            Assert.Equal(8, vttAction.Size);
        }

        [Fact]
        public void SellAssetWhichIsNotInAllocation()
        {
            var actions = _provider.GetActions(
                new Portfolio(0.Dollars(),
                    new AssetSize("VTA", 10)
                ),
                new AssetAllocation(
                    new AssetPortion("AGG", 1.0M)
                ),
                Prices
            );

            Assert.NotNull(actions);
            Assert.Equal(2, actions.Count);

            var aggAction = actions.Single(a => a.Ticker == "AGG");
            var vtaAction = actions.Single(a => a.Ticker == "VTA");

            Assert.Equal(Buy, aggAction.Type);
            Assert.Equal(8, aggAction.Size);

            Assert.Equal(Sell, vtaAction.Type);
            Assert.Equal(10, vtaAction.Size);
        }
    }
}