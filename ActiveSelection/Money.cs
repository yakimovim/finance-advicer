using System.Collections.Generic;

namespace ActiveSelection
{
    public sealed class Money : ValueObject
    {
        public decimal Value { get; }
        public Currency Currency { get; }

        public Money(decimal value, Currency currency)
        {
            Value = value;
            Currency = currency;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
            yield return Currency;
        }

        public Money As(Currency currency)
        {
            if (currency == Currency)
                return this;

            var newValue = CurrencyConverter.Convert(Value).From(Currency).To(currency);

            return new Money(newValue, currency);
        }

        public static Money operator* (decimal mult, Money money)
        {
            return new Money(mult * money.Value, money.Currency);
        }

        public static Money operator* (int mult, Money money)
        {
            return new Money(mult * money.Value, money.Currency);
        }

        public static Money operator* (Money money, decimal mult)
        {
            return new Money(mult * money.Value, money.Currency);
        }

        public static Money operator* (Money money, int mult)
        {
            return new Money(mult * money.Value, money.Currency);
        }

        public static Money operator/ (Money money, decimal div)
        {
            return new Money(money.Value / div, money.Currency);
        }

        public static Money operator/ (Money money, int div)
        {
            return new Money(money.Value / div, money.Currency);
        }

        public static decimal operator/ (Money x, Money y)
        {
            var newYValue = CurrencyConverter.Convert(y.Value).From(y.Currency).To(x.Currency);
            
            return x.Value / newYValue;
        }

        public static Money operator+ (Money x, Money y)
        {
            var newYValue = CurrencyConverter.Convert(y.Value).From(y.Currency).To(x.Currency);

            return new Money(x.Value + newYValue, x.Currency);
        }

        public static Money operator- (Money x, Money y)
        {
            var newYValue = CurrencyConverter.Convert(y.Value).From(y.Currency).To(x.Currency);

            return new Money(x.Value - newYValue, x.Currency);
        }

        public static bool operator <(Money x, Money y)
        {
            var newYValue = CurrencyConverter.Convert(y.Value).From(y.Currency).To(x.Currency);

            return x.Value < newYValue;
        }

        public static bool operator >(Money x, Money y)
        {
            var newYValue = CurrencyConverter.Convert(y.Value).From(y.Currency).To(x.Currency);

            return x.Value > newYValue;
        }

        public static bool operator <=(Money x, Money y)
        {
            var newYValue = CurrencyConverter.Convert(y.Value).From(y.Currency).To(x.Currency);

            return x.Value <= newYValue;
        }

        public static bool operator >=(Money x, Money y)
        {
            var newYValue = CurrencyConverter.Convert(y.Value).From(y.Currency).To(x.Currency);

            return x.Value >= newYValue;
        }

        public static bool operator ==(Money x, Money y)
        {
            if (y == null)
                return false;

            var newYValue = CurrencyConverter.Convert(y.Value).From(y.Currency).To(x.Currency);

            return x.Value == newYValue;
        }

        public static bool operator !=(Money x, Money y)
        {
            return !(x == y);
        }
    }

    public enum Currency
    {
        Rubles,
        Dollars
    }

    public static class MoneyHelpers
    {
        public static Money Dollars(this decimal value)
        {
            return new Money(value, Currency.Dollars);
        }

        public static Money Rubles(this decimal value)
        {
            return new Money(value, Currency.Rubles);
        }

        public static Money Dollars(this int value)
        {
            return new Money(value, Currency.Dollars);
        }

        public static Money Rubles(this int value)
        {
            return new Money(value, Currency.Rubles);
        }
    }
}