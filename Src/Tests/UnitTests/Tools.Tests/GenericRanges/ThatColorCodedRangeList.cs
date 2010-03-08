//Date: 5/8/2009

using NUnit.Framework;
using System.Drawing;
using Tools.GenericRanges;

namespace Tools.Tests.GenericRanges
{
	[TestFixture]
	public class ThatColorCodedRangeList
	{
		[Test]
		public void CanAddAndRemove()
		{
			ColorCodedGenericRangeList<int> lstRanges = new ColorCodedGenericRangeList<int>();
			
			IColorCodedRange<int> r1 = new ColorCodedRange<int>(1, 6, Color.Beige);
			IColorCodedRange<int> r2 = new ColorCodedRange<int>(7, 9, Color.Red);
			BlinkingColorCodedRange<int> br = 
				new BlinkingColorCodedRange<int>(10, 19, Color.Blue, Color.DarkBlue);
			
			lstRanges.Add(r1);
			lstRanges.Add(r2);
			lstRanges.Add(br);
			
			Assert.That(lstRanges.Count, Is.EqualTo(3));
			
			lstRanges.Remove(r2);
			Assert.That(lstRanges[1] == br);
		}
		
		[Test] public void CanSort()
		{
			ColorCodedGenericRangeList<int> lstRanges = new ColorCodedGenericRangeList<int>();
			
			ColorCodedRange<int> r1 = new ColorCodedRange<int>(1, 6, Color.Beige);
			ColorCodedRange<int> r2 = new ColorCodedRange<int>(7, 9, Color.Red);
			BlinkingColorCodedRange<int> br = 
				new BlinkingColorCodedRange<int>(10, 19, Color.Blue, Color.DarkBlue);
			
			lstRanges.Add(br);
			lstRanges.Add(r2);
			lstRanges.Add(r1);
			
			lstRanges.Sort();
			
			
			Assert.That(lstRanges[0], Is.EqualTo(r1));
			
		}
		
		[Test] public void CanGetRangeThatIncludesValue()
		{
			ColorCodedGenericRangeList<int> lstRanges = new ColorCodedGenericRangeList<int>();
			
			IColorCodedRange<int> r1 = new ColorCodedRange<int>(1, 6, Color.Beige);
			IColorCodedRange<int> r2 = new ColorCodedRange<int>(7, 9, Color.Red);
			IColorCodedRange<int> br = 
				new BlinkingColorCodedRange<int>(10, 19, Color.Blue, Color.DarkBlue);
			
			lstRanges.Add(br);
			lstRanges.Add(r2);
			lstRanges.Add(r1);
			
			IColorCodedRange<int> get1 = lstRanges.GetRangeThatIncludes(r1.MinValue);
			IColorCodedRange<int> get2 = lstRanges.GetRangeThatIncludes(br.MaxValue);
			
			Assert.That(get1, Is.EqualTo(r1));
			Assert.That(get2, Is.Not.EqualTo(r1));
			Assert.That(get2, Is.EqualTo(br));
		}
	}
}
