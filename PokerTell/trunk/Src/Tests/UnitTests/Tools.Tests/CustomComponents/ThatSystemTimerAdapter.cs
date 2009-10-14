/*
 * User: Thorsten Lorenz
 * Date: 7/2/2009
 * 
 */
using NUnit.Framework;
using Tools;

namespace Tools.Tests.CustomComponents
{
    [TestFixture]
    public class ThatSystemTimerAdapter
    {
        [Test] public void Implements_ITimer_Interface()
        {
            ITimer timer = new SystemTimerAdapter(1);
            Assert.That(timer, Is.Not.Null);
        }
    }
}
