using System.Collections.Generic;
using Xunit;

namespace ActiveSelection.Test
{
    public class PortfolioTest
    {
        private Dictionary<string, Money> _prices;

        public PortfolioTest()
        {
            _prices = new Dictionary<string, Money>();
        }

        [Fact]
        public void OnlyMoney()
        {
            var portfolio = new Portfolio(100.Rubles(), new Asset[0]);

            Assert.Equal(100.Rubles(), portfolio.Cost(_prices));
        }
    }
}