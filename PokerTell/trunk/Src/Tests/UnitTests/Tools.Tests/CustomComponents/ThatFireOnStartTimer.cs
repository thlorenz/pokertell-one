/*
 * User: Thorsten Lorenz
 * Date: 7/1/2009
 * 
 */

using NUnit.Framework;

namespace Tools.Tests.CustomComponents
{
    [TestFixture]
    public class ThatFireOnStartTimer
    {
        [Test] public void Fires_on_Start()
        {
            var mockTimer = new FireOnStartTimer();
            
            bool elapsedFired = false;
            mockTimer.Elapsed += (sender, e) => elapsedFired = true;
            mockTimer.Start();
            
            Assert.That(elapsedFired);
        }
        
        
    }

}
