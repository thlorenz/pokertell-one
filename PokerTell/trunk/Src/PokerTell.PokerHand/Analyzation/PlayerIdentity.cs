namespace PokerTell.PokerHand.Analyzation
{
    using System;
    using System.Text;

    using Iesi.Collections.Generic;

    using Infrastructure.Interfaces.PokerHand;

    [Serializable]
    public class PlayerIdentity : IPlayerIdentity
    {
       public PlayerIdentity()
       {
           _convertedPlayers = new HashedSet<IConvertedPokerPlayer>();
       }
        
        public PlayerIdentity(string name, string site)
            : this()
        {
           InitializeWith(name, site); 
        }

        string _name;

        public string Name
        {
            get { return _name; }
        }

        string _site;

        public string Site
        {
            get { return _site; }
        }

        public int Id { get; private set; }

        readonly ISet<IConvertedPokerPlayer> _convertedPlayers;

        public ISet<IConvertedPokerPlayer> ConvertedPlayers
        {
            get { return _convertedPlayers; }
        }

        public IPlayerIdentity InitializeWith(string name, string site)
        {
            _name = name;
            _site = site;

            return this;
        }

        public bool Equals(PlayerIdentity other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return Equals(other.Name, Name) && Equals(other.Site, Site);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
           
            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((PlayerIdentity)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Name != null ? Name.GetHashCode() : 0) * 397) ^ (Site != null ? Site.GetHashCode() : 0);
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder(string.Format("Id: {0}, Name: {1} Site: {2}\n", Id, Name, Site));
            foreach (var item in ConvertedPlayers)
            {
                sb.AppendLine(item.ToString());
            }

            return sb.ToString();
        }
    }
}