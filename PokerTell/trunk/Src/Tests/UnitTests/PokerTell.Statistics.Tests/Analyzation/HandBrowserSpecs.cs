namespace PokerTell.Statistics.Tests.Analyzation
{
    public class HandBrowserSpecs
    {
        /*
         *  Specification
         *  Subject is HandBrowser
         *  
         *  Initialization
         *       
         *      given 0 analyzable players
         *          It should throw an ArgumentException
         *      
         *      given 1 analyzable player
         *          It should not retrieve any hands yet
         *          It should have 1 potential hand
         *      
         *      when the first hand is accessed for the first time
         *          It should request it from the repostirory
         *          It should return the hand returned by the repository
         *          
         *      when the first hand is accessed for the second time
         *          It should not request it again from the repository
         *          It should return the hand that was previously returned by the repository
         *      
         */
    }
}