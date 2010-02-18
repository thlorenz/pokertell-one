namespace PokerTell.Statistics.Analyzation
{
    using System;

    using Interfaces;

    public class StartingHand : IStartingHand
    {
        public StartingHand(int top, int left, int sideLength, string display)
        {
            Top = top;
            Left = left;
            SideLength = sideLength;
            Display = display;
        }

        public int SideLength { get; protected set; } 
      

        public int Left { get; protected set; }

        public int Top { get; protected set; }

        public int Count { get; set; }

        public double FillHeight { get; set; }

        public string Display { get; protected set; }

        public override string ToString()
        {
            return string.Format("Left: {0}, Top: {1}, Count: {2}, FillHeight: {3}, Display: {4}", Left, Top, Count, FillHeight, Display);
        }
    }
}