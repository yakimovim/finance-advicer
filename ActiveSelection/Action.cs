using System;

namespace ActiveSelection
{
    public class Action
    {
        public Action(string ticker, int size, ActionType type)
        {
            Ticker = ticker ?? throw new ArgumentNullException(nameof(ticker));
            Type = type;
            Size = size;
        }

        public string Ticker { get; }
        public int Size { get; }
        public ActionType Type { get; }

        public override string ToString()
        {
            return $"{Type} {Size} items of '{Ticker}'.";
        }
    }

    public enum ActionType
    {
        Sell,
        Buy,
    }
}