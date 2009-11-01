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

        public abstract PlayerSeatsParser Parse(string handHistory);

        [EditorBrowsable(EditorBrowsableState.Never)]
        public class PlayerData 
        {
            public readonly double Stack;

            public readonly string Name;

            public PlayerData(string name, double stack)
            {
                Name = name;
                Stack = stack;
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
                    return (Stack.GetHashCode() * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                }
            }

            [EditorBrowsable(EditorBrowsableState.Never)]
            public override string ToString()
            {
                return string.Format("stack: {0}, Name: {1}", Stack, Name);
            }

            [EditorBrowsable(EditorBrowsableState.Never)]
            public new Type GetType() 
            {
                return base.GetType(); 
            }
        }
    }
}