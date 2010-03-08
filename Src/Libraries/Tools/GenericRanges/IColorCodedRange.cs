//Date: 5/7/2009

using System;
using System.Drawing;

namespace Tools.GenericRanges
{
    public interface IColorCodedRange<T> : IGenericRange<T> where T : IComparable
    {
        Color ColorCode { get; }
			
    }
}