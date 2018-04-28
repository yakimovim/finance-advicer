using System;
using System.Collections.Generic;

namespace ActiveSelection
{
    public class AssetAllocation
    {
        public AssetAllocation(IReadOnlyList<AssertPortion> portions)
        {
            Portions = portions ?? throw new ArgumentNullException(nameof(portions));
        }

        public IReadOnlyList<AssertPortion> Portions { get; }
    }

    public class AssertPortion
    {
        public AssertPortion(string ticker, decimal portion)
        {
            Ticker = ticker ?? throw new ArgumentNullException(nameof(ticker));
            Portion = portion;
        }

        public string Ticker { get; }
        public decimal Portion { get; }
    }
}