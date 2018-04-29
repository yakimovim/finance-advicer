using System;
using System.Collections.Generic;
using Xunit;

namespace ActiveSelection.Test
{
    using static Currency;

    public class PortfolioTest
    {
        private readonly Dictionary<string, Money> _prices;

        public PortfolioTest()
        {
            _prices = new Dictionary<string, Money>();

            CurrencyConverter.ClearRates();
            CurrencyConverter.One(Dollars).Costs(2.Rubles());
        }

        [Fact]
        public void Constructor_NegativeMoney()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Portfolio(
                (-10).Dollars(),
                new AssetSize("AGG", 3)
            ));
        }

        [Fact]
        public void Constructor_SameTickers()
        {
            Assert.Throws<ArgumentException>(() => new Portfolio(
                0.Dollars(),
                new AssetSize("AGG", 3),
                new AssetSize("AGG", 4)
            ));
        }

        [Fact]
        public void Cost_OnlyMoney()
        {
            var portfolio = new Portfolio(100.Rubles());

            Assert.Equal(100.Rubles(), portfolio.Cost(_prices).As(Rubles));
        }

        [Fact]
        public void Cost_OneAsset()
        {
            _prices["AGG"] = 3.Rubles();

            var portfolio = new Portfolio(10.Rubles(), new AssetSize("AGG", 3));

            Assert.Equal(19.Rubles(), portfolio.Cost(_prices).As(Rubles));
        }

        [Fact]
        public void Cost_MulticurrencyAssets()
        {
            _prices["AGG"] = 3.Rubles();
            _prices["VTT"] = 2.Dollars();

            var portfolio = new Portfolio(10.Rubles(), new AssetSize("AGG", 3), new AssetSize("VTT", 5));

            Assert.Equal(39.Rubles(), portfolio.Cost(_prices).As(Rubles));
        }
    }
}