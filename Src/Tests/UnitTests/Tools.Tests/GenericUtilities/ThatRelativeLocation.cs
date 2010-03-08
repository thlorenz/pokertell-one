/*
 * User: Thorsten Lorenz
 * Date: 7/10/2009
 * 
*/

using System;
using NUnit.Framework;
using System.Drawing;
using Tools.GenericUtilities;

namespace Tools.Tests.GenericUtilities
{
    [TestFixture]
    public class ThatRelativeLocation
    {
        [Test] public void constructsCorrectlyGivenLocationAndSize()
        {
            Point location = new Point(20, 20);
            Size size = new Size(100, 200);
            
            var expectedRelLoc = new RelativeLocation(0.2, 0.1);
            var constructedRelLoc = new RelativeLocation(location, size);
            
            Assert.That(constructedRelLoc, Is.EqualTo(expectedRelLoc));
        }
    }
}
