//Date: 5/8/2009
using System;
using System.Text;
using System.Collections.Generic;

namespace Tools.GenericRanges
{
    /// <summary>
    /// Description of ColorCodedGenericRangeList.
    /// </summary>
    public class ColorCodedGenericRangeList<T> : IEnumerable<IColorCodedRange<T>> where T : IComparable
    {
        List<IColorCodedRange<T>> lstRanges;
		
        public ColorCodedGenericRangeList()
        {
            this.lstRanges = new List<IColorCodedRange<T>>();
        }
		
        public IColorCodedRange<T> this[int index]
        {
            get{
                return lstRanges[index];
            }
        }
		
        public int Count {
            get { 
                return lstRanges.Count;
            }
        }
		
        public void Add(IColorCodedRange<T> theRange)
        {
            this.lstRanges.Add(theRange);
        }
		
        public bool Remove(IColorCodedRange<T> theRange)
        {
            return this.lstRanges.Remove(theRange);
        }
		
        public void Sort()
        {
            lstRanges.Sort();
        }
		
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
			
            foreach(IColorCodedRange<T> currentRange in this)
            {
                sb.AppendLine(currentRange.ToString());
            }
			
            return sb.ToString();
        }
		
        public IColorCodedRange<T> GetRangeThatIncludes(T value)
        {
            for(int i =0; i < lstRanges.Count; i++)
            {
                if (lstRanges[i].IncludesValue(value))
                    return lstRanges[i];
            }
			
            return null;
			
        }
		
        IEnumerator<IColorCodedRange<T>> IEnumerable<IColorCodedRange<T>>.GetEnumerator()
        {
            return this.lstRanges.GetEnumerator();
        }
		
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.lstRanges.GetEnumerator();
        }
    }
}