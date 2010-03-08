namespace PokerTell.PokerHandParsers.Tests.PokerStars
{
    using System.IO;

    using Infrastructure.Interfaces.PokerHand;
    using Infrastructure.Interfaces.PokerHandParsers;

    using Microsoft.Practices.Unity;

    using NUnit.Framework;

    using PokerHand.Aquisition;

    using UnitTests;

    public class PokerHandParserTests : TestWithLog
    {
        const string FileName =
            @"C:\Program Files\PokerStars\HandHistory\renniweg\0202200802084914_HH20080108 T73124117 No Limit Hold'em $5 + $0.50.txt";

        IUnityContainer _container;

        IPokerHandParser _parser;
        [SetUp]
        public void _Init()
        {
          
            _container = new UnityContainer();
            _container
                .RegisterConstructor<IAquiredPokerAction, AquiredPokerAction>()
                .RegisterConstructor<IAquiredPokerRound, AquiredPokerRound>()
                .RegisterConstructor<IAquiredPokerPlayer, AquiredPokerPlayer>()
                .RegisterConstructor<IAquiredPokerHand, AquiredPokerHand>();
           _parser = _container.Resolve<PokerTell.PokerHandParsers.PokerStars.PokerHandParser>();
        }
        
        [Test]
        public void Parse_ParsesHand_Correctly()
        {
            string handHistory = new StreamReader(File.OpenRead(FileName)).ReadToEnd();
            _parser.LogVerbose = true;
            _parser.ParseHand(handHistory);
        }
    }
}