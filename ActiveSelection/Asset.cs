using System;

namespace ActiveSelection
{
    public class Asset
    {
        public Asset(string ticker, int number)
        {
            if(number < 0) throw new ArgumentOutOfRangeException(nameof(number));
            Ticker = ticker ?? throw new ArgumentNullException(nameof(ticker));
            Number = number;
        }

        public string Ticker { get; }
        public int Number { get; }
    }
}