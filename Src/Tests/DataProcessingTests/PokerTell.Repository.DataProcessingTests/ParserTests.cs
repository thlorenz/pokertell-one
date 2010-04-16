namespace PokerTell.Repository.DataProcessingTests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;

    using Microsoft.Practices.Unity;

    using NUnit.Framework;

    using PokerHandParsers.Interfaces;
    using PokerHandParsers.Interfaces.Parsers;

    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.Infrastructure.Interfaces.PokerHandParsers;
    using PokerTell.PokerHand.Analyzation;
    using PokerTell.PokerHand.Aquisition;
    using PokerTell.PokerHand.Interfaces;
    using PokerTell.PokerHand.Services;

    public class ParserTests
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
                .RegisterTypeAndConstructor<IPokerHandConverter, PokerHandConverter>(() => _container.Resolve<IPokerHandConverter>())

                // PokerStars
                .RegisterType<IPokerStarsAnteParser, PokerHandParsers.PokerStars.AnteParser>()
                .RegisterType<IPokerStarsBlindsParser, PokerHandParsers.PokerStars.BlindsParser>()
                .RegisterType<IPokerStarsBoardParser, PokerHandParsers.PokerStars.BoardParser>()
                .RegisterType<IPokerStarsGameTypeParser, PokerHandParsers.PokerStars.GameTypeParser>()
                .RegisterType<IPokerStarsHandHeaderParser, PokerHandParsers.PokerStars.HandHeaderParser>()
                .RegisterType<IPokerStarsHeroNameParser, PokerHandParsers.PokerStars.HeroNameParser>()
                .RegisterType<IPokerStarsHoleCardsParser, PokerHandParsers.PokerStars.HoleCardsParser>()
                .RegisterType<IPokerStarsPlayerActionsParser, PokerHandParsers.PokerStars.PlayerActionsParser>()
                .RegisterType<IPokerStarsPlayerSeatsParser, PokerHandParsers.PokerStars.PlayerSeatsParser>()
                .RegisterType<IPokerStarsSmallBlindPlayerNameParser, PokerHandParsers.PokerStars.SmallBlindPlayerNameParser>()
                .RegisterType<IPokerStarsStreetsParser, PokerHandParsers.PokerStars.StreetsParser>()
                .RegisterType<IPokerStarsTableNameParser, PokerHandParsers.PokerStars.TableNameParser>()
                .RegisterType<IPokerStarsTimeStampParser, PokerHandParsers.PokerStars.TimeStampParser>()
                .RegisterType<IPokerStarsTotalPotParser, PokerHandParsers.PokerStars.TotalPotParser>()
                .RegisterType<IPokerStarsTotalSeatsParser, PokerHandParsers.PokerStars.TotalSeatsParser>()

                // FullTiltPoker
                .RegisterType<IFullTiltPokerAnteParser, PokerHandParsers.FullTiltPoker.AnteParser>()
                .RegisterType<IFullTiltPokerBlindsParser, PokerHandParsers.FullTiltPoker.BlindsParser>()
                .RegisterType<IFullTiltPokerBoardParser, PokerHandParsers.FullTiltPoker.BoardParser>()
                .RegisterType<IFullTiltPokerGameTypeParser, PokerHandParsers.FullTiltPoker.GameTypeParser>()
                .RegisterType<IFullTiltPokerHandHeaderParser, PokerHandParsers.FullTiltPoker.HandHeaderParser>()
                .RegisterType<IFullTiltPokerHeroNameParser, PokerHandParsers.FullTiltPoker.HeroNameParser>()
                .RegisterType<IFullTiltPokerHoleCardsParser, PokerHandParsers.FullTiltPoker.HoleCardsParser>()
                .RegisterType<IFullTiltPokerPlayerActionsParser, PokerHandParsers.FullTiltPoker.PlayerActionsParser>()
                .RegisterType<IFullTiltPokerPlayerSeatsParser, PokerHandParsers.FullTiltPoker.PlayerSeatsParser>()
                .RegisterType<IFullTiltPokerSmallBlindPlayerNameParser, PokerHandParsers.FullTiltPoker.SmallBlindPlayerNameParser>()
                .RegisterType<IFullTiltPokerStreetsParser, PokerHandParsers.FullTiltPoker.StreetsParser>()
                .RegisterType<IFullTiltPokerTableNameParser, PokerHandParsers.FullTiltPoker.TableNameParser>()
                .RegisterType<IFullTiltPokerTimeStampParser, PokerHandParsers.FullTiltPoker.TimeStampParser>()
                .RegisterType<IFullTiltPokerTotalPotParser, PokerHandParsers.FullTiltPoker.TotalPotParser>()
                .RegisterType<IFullTiltPokerTotalSeatsParser, PokerHandParsers.FullTiltPoker.TotalSeatsParser>()

                .RegisterType<ITotalSeatsForTournamentsRecordKeeper, PokerHandParsers.TotalSeatsForTournamentsRecordKeeper>(new ContainerControlledLifetimeManager())
                .RegisterType<RepositoryParser>();
        }

        [Test]
        public void Parsing_EntireDirectory_FullTilt()
        {
            const bool printConvertedHands = true;

            _container
                .RegisterInstance<IPokerHandParsers>(
                new PokerHandParserToUse(
                    _container.Resolve<PokerHandParsers.FullTiltPoker.PokerHandParser>()));

            var repositoryParser = _container.Resolve<RepositoryParser>();

            const string batchDirectory = @"C:\SD\PokerTell\TestData\HandHistories\FullTilt\OneFile\";

            ParseDirectoryWithParser(batchDirectory, repositoryParser, printConvertedHands);
        }

        [Test]
        public void Parsing_EntireDirectory_PokerStars()
        {
            const bool printConvertedHands = true;

            _container
                .RegisterInstance<IPokerHandParsers>(
                new PokerHandParserToUse(
                    _container.Resolve<PokerHandParsers.PokerStars.PokerHandParser>()));

            var repositoryParser = _container.Resolve<RepositoryParser>();

            const string directory = @"C:\Program Files\PokerStars\HandHistory\";

            ParseDirectoryWithParser(directory, repositoryParser, printConvertedHands);
        }

        static void ParseDirectoryWithParser(string directory, RepositoryParser repositoryParser, bool printConverted)
        {
            var dirInfo = new DirectoryInfo(directory);

            FileInfo[] files = dirInfo.GetFiles("*.txt");

            Console.WriteLine("Parsing {0} files.", files.Length);

            int fileCounter = 0;
            foreach (FileInfo file in files)
            {
                try
                {
                    string handHistories = new StreamReader(file.OpenRead()).ReadToEnd();

                    IEnumerable<IConvertedPokerHand> convertedPokerHands =
                        repositoryParser.RetrieveAndConvert(handHistories, file.FullName);
                    if (printConverted)
                    {
                        PrintToConsole(convertedPokerHands);
                    }

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

        static void PrintToConsole(IEnumerable<IConvertedPokerHand> hands)
        {
            foreach (IConvertedPokerHand hand in hands)
            {
                Console.WriteLine(hand);
            }
        }

        class PokerHandParserToUse : IPokerHandParsers
        {
            readonly IList<IPokerHandParser> _parsers;

            public PokerHandParserToUse(IPokerHandParser parser)
            {
                parser.LogVerbose = false;
                _parsers = new List<IPokerHandParser> { parser };
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public IEnumerator<IPokerHandParser> GetEnumerator()
            {
                return _parsers.GetEnumerator();
            }
        }
    }
}