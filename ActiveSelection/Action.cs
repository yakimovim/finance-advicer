using System;

namespace ActiveSelection
{
    public class Action
    {
        public Action(string ticker, ActionType type)
        {
            Ticker = ticker ?? throw new ArgumentNullException(nameof(ticker));
            Type = type;
        }

        public string Ticker { get; }
        public ActionType Type { get; }
    }

    public enum ActionType
    {
        Sell,
        Buy,
    }
}