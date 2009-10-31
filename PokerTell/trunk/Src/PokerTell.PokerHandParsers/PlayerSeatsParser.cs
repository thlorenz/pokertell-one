namespace PokerTell.PokerHandParsers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    using Infrastructure.Interfaces;

    public abstract class PlayerSeatsParser 
    {
        public bool IsValid { get; protected set; }

        public IDictionary<int, PlayerData> PlayerSeats { get; protected set; }

        public abstract void Parse(string handHistory);

        [EditorBrowsable(EditorBrowsableState.Never)]
        public class PlayerData 
        {
            public readonly double Ratio;

            public readonly string Name;

            public PlayerData(string name, double ratio)
            {
                Name = name;
                Ratio = ratio;
            }

            [EditorBrowsable(EditorBrowsableState.Never)]
            public override bool Equals(object obj)
            {
                return obj.GetHashCode().Equals(GetHashCode());
            }

            [EditorBrowsable(EditorBrowsableState.Never)]
            public override int GetHashCode()
            {
                unchecked
                {
                    return (Ratio.GetHashCode() * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                }
            }

            [EditorBrowsable(EditorBrowsableState.Never)]
            public override string ToString()
            {
                return string.Format("Ratio: {0}, Name: {1}", Ratio, Name);
            }

            [EditorBrowsable(EditorBrowsableState.Never)]
            public new Type GetType() 
            {
                return base.GetType(); 
            }
        }
    }
}