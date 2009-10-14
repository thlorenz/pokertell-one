//Date: 5/7/2009

using System;
using System.Drawing;

namespace Tools.GenericRanges
{
    /// <summary>
    /// Description of ColorCodedGenericRange.
    /// </summary>
    public class ColorCodedRange<T> : GenericRange<T>, IColorCodedRange<T> where T : struct, IComparable
    {
        public ColorCodedRange(T minValue, T maxValue, Color colorCode) 
            : base(minValue, maxValue)
        {
            this.ColorCode = colorCode;
        }

        public Color ColorCode {
            get;
            protected set;
        }

    }
}