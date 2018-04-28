using System;
using System.Collections.Generic;
using System.Linq;

namespace ActiveSelection
{
    public class AssetAllocation
    {
        public AssetAllocation(IReadOnlyList<AssetPortion> portions)
        {
            Portions = portions ?? throw new ArgumentNullException(nameof(portions));
            if(Portions.Count > 0 && Portions.Select(p => p.Portion).Sum() != 1.0M)
                throw new ArgumentException("Sum of all portions must be 1.");
        }

        public AssetAllocation(params AssetPortion[] portions)
        {
            Portions = portions ?? throw new ArgumentNullException(nameof(portions));
            if(Portions.Count > 0 && Portions.Select(p => p.Portion).Sum() != 1.0M)
                throw new ArgumentException("Sum of all portions must be 1.");
        }

        public IReadOnlyList<AssetPortion> Portions { get; }

        public AssetAllocation Remove(string ticker)
        {
            if (ticker == null) throw new ArgumentNullException(nameof(ticker));
            var newPortions = Portions.Where(p => p.Ticker == ticker).ToList();
            var total = newPortions.Select(p => p.Portion).Sum();
            return new AssetAllocation(newPortions.Select(p => new AssetPortion(p.Ticker, p.Portion / total)).ToArray());
        }
    }

    public class AssetPortion : ValueObject
    {
        public AssetPortion(string ticker, decimal portion)
        {
            Ticker = ticker ?? throw new ArgumentNullException(nameof(ticker));
            if(portion < 0.0M || portion > 1.0M) 
                throw new ArgumentOutOfRangeException(nameof(portion), "Portion must be between 0 and 1.");
            Portion = portion;
        }

        public string Ticker { get; }
        public decimal Portion { get; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Ticker;
            yield return Portion;
        }
    }

    public class AssetSize : ValueObject
    {
        public AssetSize(string ticker, int size)
        {
            Ticker = ticker ?? throw new ArgumentNullException(nameof(ticker));
            Size = size;
        }

        public string Ticker { get; }
        public int Size { get; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Ticker;
            yield return Size;
        }
    }

    public static class AssetPortionExtensions
    {
        public static AssetSize GetAssetWithSmallestReminder(
            this AssetAllocation allocation,
            Money portfolioCost, 
            Dictionary<string, Money> prices)
        {
            AssetSize size = null;
            decimal bestReminder = decimal.MaxValue;

            foreach (var portion in allocation.Portions)
            {
                var ticker = portion.Ticker;
                var targetCost = portfolioCost * portion.Portion;

                var targetNumber = Math.Floor(targetCost / prices[ticker]);

                var rest = (targetCost - targetNumber * prices[ticker]).As(Currency.Rubles).Value;

                if (bestReminder > rest)
                {
                    bestReminder = rest;
                    size = new AssetSize(portion.Ticker, Convert.ToInt32(targetNumber));
                }
            }

            return size;
        }
    }
}