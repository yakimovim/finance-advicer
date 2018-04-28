using System;

namespace ActiveSelection
{
    public class Price
    {
        public Price(string ticker, Money value)
        {
            Ticker = ticker ?? throw new ArgumentNullException(nameof(ticker));
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string Ticker { get; }
        public Money Value { get; }
    }
}