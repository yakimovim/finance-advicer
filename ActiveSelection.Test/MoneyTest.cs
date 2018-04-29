using Xunit;

namespace ActiveSelection.Test
{
    using static Currency;

    public class MoneyTest
    {
        public MoneyTest()
        {
            CurrencyConverter.ClearRates();
            CurrencyConverter.One(Dollars).Costs(60.Rubles());
        }

        [Fact]
        public void TestDollars()
        {
            var money = 1.Dollars();

            Assert.Equal(1M, money.Value);
            Assert.Equal(Dollars, money.Currency);
        }

        [Fact]
        public void TestRubles()
        {
            var money = 10.Rubles();

            Assert.Equal(10M, money.Value);
            Assert.Equal(Rubles, money.Currency);
        }

        [Fact]
        public void TestMultiplication()
        {
            var money = 5.Rubles();

            var mult = 3 * money;

            Assert.Equal(15M, mult.Value);
            Assert.Equal(mult.Currency, money.Currency);
        }

        [Fact]
        public void TestDivision()
        {
            var money = 15.Rubles();

            var div = money / 3;

            Assert.Equal(5M, div.Value);
            Assert.Equal(div.Currency, money.Currency);
        }

        [Fact]
        public void TestMoneyDivision()
        {
            var x = 15.Rubles();
            var y = 5.Rubles();

            var div = x / y;

            Assert.Equal(3.0M, div);
        }

        [Fact]
        public void TestAddition()
        {
            var x = 15.Rubles();
            var y = 3.Dollars();

            var money = x + y;

            Assert.Equal(195M, money.Value);
            Assert.Equal(money.Currency, x.Currency);
        }

        [Fact]
        public void TestSubtraction()
        {
            var x = 15.Rubles();
            var y = 3.Dollars();

            var money = y - x;

            Assert.Equal(2.75M, money.Value);
            Assert.Equal(money.Currency, y.Currency);
        }

        [Fact]
        public void TestConversion()
        {
            var dollars = 5.Dollars();

            var rubles = dollars.As(Rubles);

            Assert.Equal(300M, rubles.Value);
            Assert.Equal(Rubles, rubles.Currency);
        }

        [Fact]
        public void TestComparison()
        {
            var x = 5.Dollars();
            var y = 5.Rubles();

            Assert.True(y < x);
            Assert.True(x > y);
        }
    }
}