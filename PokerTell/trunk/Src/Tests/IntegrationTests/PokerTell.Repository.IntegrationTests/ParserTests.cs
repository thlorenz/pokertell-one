namespace PokerTell.Repository.IntegrationTests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;

    using Infrastructure.Interfaces.Repository;

    using Microsoft.Practices.Unity;

    using NUnit.Framework;

    using PokerHand.Analyzation;
    using PokerHand.Services;

    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.Infrastructure.Interfaces.PokerHandParsers;
    using PokerTell.PokerHand.Aquisition;

    using UnitTests;

    public class ParserTests : TestWithLog
    {
        IUnityContainer _container;

        [SetUp]
        public void _Init()
        {

            _container = new UnityContainer();
            _container
                .RegisterConstructor<IAquiredPokerAction, AquiredPokerAction>()
                .RegisterConstructor<IAquiredPokerRound, AquiredPokerRound>()
                .RegisterConstructor<IAquiredPokerPlayer, AquiredPokerPlayer>()
                .RegisterConstructor<IAquiredPokerHand, AquiredPokerHand>()

                .RegisterConstructor<IConvertedPokerAction, ConvertedPokerAction>()
                .RegisterConstructor<IConvertedPokerActionWithId, ConvertedPokerActionWithId>()
                .RegisterConstructor<IConvertedPokerRound, ConvertedPokerRound>()
                .RegisterConstructor<IConvertedPokerPlayer, ConvertedPokerPlayer>()
                .RegisterConstructor<IConvertedPokerHand, ConvertedPokerHand>()

                .RegisterType<IPokerActionConverter, PokerActionConverter>()
                .RegisterType<IPokerRoundsConverter, PokerRoundsConverter>()
                .RegisterType<IPokerHandConverter, PokerHandConverter>()

                .RegisterType<RepositoryParser>();
        }
        
        [Test]
        public void Parsing_EntireDirectoryWithPokerStarsHandHistories_EncountersNoParsingErrors()
        {
            _container
                .RegisterInstance<IPokerHandParsers>(
                new PokerHandParserToUse(
                    _container.Resolve<PokerHandParsers.PokerStars.PokerHandParser>()));

            var repositoryParser = _container.Resolve<RepositoryParser>();
            
            const string directory = @"C:\Program Files\PokerStars\HandHistory\renniweg\";
           
            ParseDirectoryWithParser(directory, repositoryParser);
        }

        [Test]
        public void Parsing_EntireDirectoryWithFullTiltHandHistories_EncountersNoParsingErrors()
        {
            _container
                .RegisterInstance<IPokerHandParsers>(
                new PokerHandParserToUse(
                    _container.Resolve<PokerHandParsers.PokerStars.PokerHandParser>()));

            var repositoryParser = _container.Resolve<RepositoryParser>();

            const string directory = @"C:\SD\PokerTell\TestData\HandHistories\FullTilt\";

            ParseDirectoryWithParser(directory, repositoryParser);
        }

        static void ParseDirectoryWithParser(string directory, RepositoryParser repositoryParser)
        {
            var dirInfo = new DirectoryInfo(directory);

            var files = dirInfo.GetFiles("*.txt");

            Console.WriteLine("Parsing {0} files.", files.Length);

            int fileCounter = 0;
            foreach (var file in files)
            {
                try
                {
                    string handHistories = new StreamReader(file.OpenRead()).ReadToEnd();
                    repositoryParser.RetrieveAndConvert(handHistories, file.FullName);
                    fileCounter++;
                }
                catch (Exception excep)
                {
                    Console.WriteLine("File #{0} name is {1}", fileCounter, file.Name);
                    Console.WriteLine(excep);
                }
            }
            Console.WriteLine("Finished Files #{0}", fileCounter);
        }

        class PokerHandParserToUse : IPokerHandParsers
        {
            readonly IList<IPokerHandParser> _parsers;

            public PokerHandParserToUse(IPokerHandParser parser)
            {
                parser.LogVerbose = true;
                _parsers = new List<IPokerHandParser> { parser };
            }

            public IEnumerator<IPokerHandParser> GetEnumerator()
            {
                return _parsers.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}
