using System;
using System.Collections.Generic;

namespace ActiveSelection
{
    public class AssetSize : ValueObject
    {
        public AssetSize(string ticker, int size)
        {
            Ticker = ticker ?? throw new ArgumentNullException(nameof(ticker));
            Size = size;
            if(Size < 0)
                throw new ArgumentOutOfRangeException(nameof(size), "Size of asset should be non-negative.");
        }

        public string Ticker { get; }
        public int Size { get; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Ticker;
            yield return Size;
        }
    }
}