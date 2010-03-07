namespace PokerTell.Repository.DataProcessingTests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;

    using Infrastructure.Interfaces.PokerHand;
    using Infrastructure.Interfaces.PokerHandParsers;

    using Microsoft.Practices.Unity;

    using NUnit.Framework;

    using PokerHand.Analyzation;
    using PokerHand.Aquisition;
    using PokerHand.Services;

    using PokerHandParsers.PokerStars;

    public class ParserTests 
    {
        #region Constants and Fields

        IUnityContainer _container;

        #endregion

        #region Public Methods

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
        public void Parsing_EntireDirectory_FullTilt()
        {
            const bool printConvertedHands = false;
            
            _container
                .RegisterInstance<IPokerHandParsers>(
                new PokerHandParserToUse(
                    _container.Resolve<PokerHandParsers.FullTiltPoker.PokerHandParser>()));

            var repositoryParser = _container.Resolve<RepositoryParser>();

            const string directory = @"C:\SD\PokerTell\TestData\HandHistories\FullTilt\Batches\New\";

            ParseDirectoryWithParser(directory, repositoryParser, printConvertedHands);
        }

        [Test]
        public void Parsing_EntireDirectory_PokerStars()
        {
            const bool printConvertedHands = false;

            _container
                .RegisterInstance<IPokerHandParsers>(
                new PokerHandParserToUse(
                    _container.Resolve<PokerHandParser>()));

            var repositoryParser = _container.Resolve<RepositoryParser>();

            const string directory = @"C:\Program Files\PokerStars\HandHistory\renniweg\";

            ParseDirectoryWithParser(directory, repositoryParser, printConvertedHands);
        }

        #endregion

        #region Methods

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

        #endregion

        class PokerHandParserToUse : IPokerHandParsers
        {
            #region Constants and Fields

            readonly IList<IPokerHandParser> _parsers;

            #endregion

            #region Constructors and Destructors

            public PokerHandParserToUse(IPokerHandParser parser)
            {
                parser.LogVerbose = false;
                _parsers = new List<IPokerHandParser> { parser };
            }

            #endregion

            #region Implemented Interfaces

            #region IEnumerable

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            #endregion

            #region IEnumerable<IPokerHandParser>

            public IEnumerator<IPokerHandParser> GetEnumerator()
            {
                return _parsers.GetEnumerator();
            }

            #endregion

            #endregion
        }
    }
}