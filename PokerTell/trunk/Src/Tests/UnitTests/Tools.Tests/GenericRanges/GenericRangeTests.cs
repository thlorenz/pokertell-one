//Date: 5/7/2009

#region Using Directives

using System;
using NUnit.Framework;

using System.Collections.Generic;
using System.Drawing;
using Tools.GenericRanges;

#endregion

namespace Tools.Tests.GenericRanges
{
    using PokerTell.UnitTests.Tools;

    [TestFixture]
	public class GenericRangeTests
	{
		[Test] public void HandlesDoubles()
		{
			double minVal = 2.3;
			double maxVal = 7.8;
			
			IGenericRange<double> genR = new GenericRange<double>(minVal, maxVal);
			
			Assert.That(genR.MinValue, Is.EqualTo(minVal));
			Assert.That(genR.MaxValue , Is.EqualTo(maxVal));
			
			double inRange = 4.5;
			double equalToMax = maxVal;
			double toLarge = maxVal * 2.0;
			double toSmall = minVal / 2.0;
			
			Assert.That(genR.IncludesValue(inRange));
			Assert.That(genR.IncludesValue(equalToMax));
			Assert.That(! genR.IncludesValue(toLarge));
			Assert.That(! genR.IncludesValue(toSmall));
			
		}
		
		[Test] public void HandlesDateTimes()
		{
			int year = 2009;
			int month = 5;
			int day = 12;
			
			int hour = 4;
			int minute = 14;
			int second = 30;
			
			DateTime minVal = new DateTime(year, month, day, hour, minute, second);
			DateTime maxVal = new DateTime(year, month, day, hour + 1, minute, second);
			
			IGenericRange<DateTime> genR = new GenericRange<DateTime>(minVal, maxVal);
			
			DateTime inRange = new DateTime(year, month, day, hour, minute + 30, second);
			DateTime equalToMax = new DateTime(year, month, day, hour + 1, minute, second);
			DateTime toLarge = new DateTime(year, month, day, hour + 1, minute, second + 1);
			DateTime toSmall = new DateTime(year, month, day, hour, minute, second - 1);
			
			Assert.That(genR.IncludesValue(inRange));
			Assert.That(genR.IncludesValue(equalToMax));
			Assert.That(! genR.IncludesValue(toLarge));
			Assert.That(! genR.IncludesValue(toSmall));
			
		}
		
		[Test] public void CanSortListOfRanges()
		{
			
			double minVal = 2.3;
			double maxVal = 7.8;
			List<IGenericRange<double>> lstRanges = new List<IGenericRange<double>>();
			
			IGenericRange<double> smallerRange = new GenericRange<double>(minVal, maxVal);
			IGenericRange<double> biggerRange = new GenericRange<double>(minVal + 1.0, maxVal * 2.0);
			
			lstRanges.Add(biggerRange);
			lstRanges.Add(smallerRange);
			
			Assert.That(lstRanges[0], Is.EqualTo(biggerRange));
			
			lstRanges.Sort();
			
			Assert.That(lstRanges[0], Is.EqualTo(smallerRange));
			
		}
		
		[Test] public void HandlesColorCodedGenericRanges()
		{
			Color myColor = Color.AliceBlue;
			ColorCodedRange<int> genRange=
				new ColorCodedRange<int>(2, 4, myColor);
			
			Assert.That(genRange.ColorCode, Is.EqualTo(myColor));
			
		}
		
		[Test] public void HandlesBlinkingColorCodedGenericRanges()
		{
			Color myColor = Color.AliceBlue;
			Color altColor = Color.Aqua;
			
			BlinkingColorCodedRange<int> genRange =
				new BlinkingColorCodedRange<int>(2, 4, myColor, altColor);
			
			Assert.That(genRange.ColorCode, Is.EqualTo(myColor));
			Assert.That(genRange.AlternateColor, Is.EqualTo(altColor));
			
		}

	    [Test]
	    public void Constructor_MinGreaterThanMax_ThrowsArgumentException()
	    {
	        Assert.Throws<ArgumentException>(() => new GenericRange<int>(1, 0));
	    }

	    [Test]
	    public void Equals_TwoRangesWithSameMinAndMax_ReturnsTrue()
	    {
	        const int min = 0;
	        const int max = 1;
            var genR1 = new GenericRange<int>(min, max);
	        var genR2 = new GenericRange<int>(min, max);

	        genR1.IsEqualTo(genR2);   
	    }

        [Test]
        public void Equals_TwoRangesWithSameMinAndDifferentMax_ReturnsFalsee()
        {
            const int min = 0;
            const int max = 1;
        
            var genR1 = new GenericRange<int>(min, max);
            var genR2 = new GenericRange<int>(min, max + 1);

            genR1.IsNotEqualTo(genR2);
        }

        [Test]
        public void Equals_TwoRangesWithDifferentMinAndSameMax_ReturnsFalsee()
        {
            const int min = 0;
            const int max = 1;

            var genR1 = new GenericRange<int>(min, max);
            var genR2 = new GenericRange<int>(min - 1, max);

            genR1.IsNotEqualTo(genR2);
        }
	}
}
