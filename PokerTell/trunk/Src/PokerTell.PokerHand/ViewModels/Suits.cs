namespace PokerTell.PokerHand.ViewModels
{
    using System.Windows.Controls;

    using Tools.WPF;

    public static class Suits
    {
        #region Properties

        public static Image Club
        {
            get
            {
                return Utilities.ImageFromResource("../Images/club.gif");
            }
        }

        public static Image Diamond
        {
            get
            {
                return Utilities.ImageFromResource("../Images/diamond.gif");
            }
        }

        public static Image Heart
        {
            get
            {
                return Utilities.ImageFromResource("../Images/heart.gif");
            }
        }

        public static Image Spade
        {
            get
            {
                return Utilities.ImageFromResource("../Images/spade.gif");
            }
        }

        public static Image Unknown
        {
            get
            {
                return Utilities.ImageFromResource("../Images/unknown.gif");
            }
        }

        #endregion

        #region Public Methods

        public static Image GetSuitImageFrom(string suitString)
        {
            if (string.IsNullOrEmpty(suitString))
            {
                return Unknown;
            }

            switch (suitString)
            {
                case "c":
                    return Club;
                case "s":
                    return Spade;
                case "h":
                    return Heart;
                case "d":
                    return Diamond;
                case "?":
                    return Unknown;
                default:
                    {
                        return Unknown;
                    }
            }
        }

        #endregion
    }
}