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

            CurrencyConverter.ConvertionRates.Clear();
            CurrencyConverter.ConvertionRates[Tuple.Create(Dollars, Rubles)] = 2.0M;
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

            var portfolio = new Portfolio(10.Rubles(), new Asset("AGG", 3));

            Assert.Equal(19.Rubles(), portfolio.Cost(_prices).As(Rubles));
        }

        [Fact]
        public void Cost_MulticurrencyAssets()
        {
            _prices["AGG"] = 3.Rubles();
            _prices["VTT"] = 2.Dollars();

            var portfolio = new Portfolio(10.Rubles(), new Asset("AGG", 3), new Asset("VTT", 5));

            Assert.Equal(39.Rubles(), portfolio.Cost(_prices).As(Rubles));
        }
    }
}