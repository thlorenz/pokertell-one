namespace Tools.Tests.Serialization
{
    using System;

    [Serializable]
    public class InfoToSaveClass
    {
        #region Constructors and Destructors

        public InfoToSaveClass()
            : this("DefaultConstructedString", 1)
        {
        }

        public InfoToSaveClass(string theString, int theInt)
        {
            this.theString = theString;
            this.theInt = theInt;
        }

        #endregion

        #region Properties

        public int theInt { get; set; }

        public string theString { get; set; }

        #endregion

        #region Public Methods

        public override bool Equals(object obj)
        {
            if (obj == null || ! (obj is InfoToSaveClass))
            {
                return false;
            }

            var sndInfo = (InfoToSaveClass)obj;

            return GetHashCode().Equals(sndInfo.GetHashCode());
        }

        public override int GetHashCode()
        {
            return theString.GetHashCode() ^ theInt.ToString().GetHashCode();
        }

        public override string ToString()
        {
            return "theString: " + theString + "theInt: " + theInt.ToString();
        }

        #endregion
    }
}