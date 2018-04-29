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

        public sealed class ValueWithCurrency
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

                if (ConversionRates.TryGetValue(Tuple.Create(_fromCurrency, toCurrency),
                    out decimal rate))
                {
                    return _value * rate;
                }

                if (ConversionRates.TryGetValue(Tuple.Create(toCurrency, _fromCurrency),
                    out rate))
                {
                    return _value / rate;
                }

                throw new ArgumentOutOfRangeException($"Can't convert from {_fromCurrency} to {toCurrency}.");
            }
        }

        public sealed class ConversionRateSetter
        {
            private readonly Dictionary<Tuple<Currency, Currency>, decimal> _conversionRates;
            private readonly Currency _oneCurrency;

            public ConversionRateSetter(Dictionary<Tuple<Currency, Currency>, decimal> conversionRates, Currency oneCurrency)
            {
                _conversionRates = conversionRates;
                _oneCurrency = oneCurrency;
            }

            public void Costs(Money value)
            {
                if(value.Currency == _oneCurrency)
                    throw new ArgumentException("Target currency should be different");

                _conversionRates[Tuple.Create(_oneCurrency, value.Currency)] = value.Value;
            }
        }

        private static readonly Dictionary<Tuple<Currency, Currency>, decimal> InternalConvertionRates =
            new Dictionary<Tuple<Currency, Currency>, decimal>();

        public static readonly IReadOnlyDictionary<Tuple<Currency, Currency>, decimal> ConversionRates =
            InternalConvertionRates;

        public static CurrencylessValue Convert(decimal value)
        {
            return new CurrencylessValue(value);
        }

        public static ConversionRateSetter One(Currency currency)
        {
            return new ConversionRateSetter(InternalConvertionRates, currency);
        }

        public static void ClearRates()
        {
            InternalConvertionRates.Clear();
        }
    }

}