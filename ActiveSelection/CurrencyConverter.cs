using System;
using System.Collections.Generic;

namespace ActiveSelection
{
    public static class CurrencyConverter
    {
        public sealed class CurrencylessValue
        {
            private readonly decimal _value;

            internal CurrencylessValue(decimal value)
            {
                _value = value;
            }


            public ValueWithCurrency From(Currency currency)
            {
                return new ValueWithCurrency(_value, currency);
            }
        }

        public class ValueWithCurrency
        {
            private readonly decimal _value;
            private readonly Currency _fromCurrency;

            public ValueWithCurrency(decimal value, Currency currency)
            {
                _value = value;
                _fromCurrency = currency;
            }

            public decimal To(Currency toCurrency)
            {
                if (_fromCurrency == toCurrency)
                    return _value;

                if (ConvertionRates.TryGetValue(Tuple.Create(_fromCurrency, toCurrency),
                    out decimal rate))
                {
                    return _value * rate;
                }

                if (ConvertionRates.TryGetValue(Tuple.Create(toCurrency, _fromCurrency),
                    out rate))
                {
                    return _value / rate;
                }

                throw new ArgumentOutOfRangeException($"Can't convert from {_fromCurrency} to {toCurrency}.");
            }
        }

        public static readonly Dictionary<Tuple<Currency, Currency>, decimal> ConvertionRates = new Dictionary<Tuple<Currency, Currency>, decimal>();

        public static CurrencylessValue Convert(decimal value)
        {
            return new CurrencylessValue(value);
        }
    }

}