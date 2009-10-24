//Date: 6/19/2009

namespace PokerTell.UnitTests
{
//    using System;
//    using System.Collections.Generic;
//
//    using Infrastructure.Enumerations.PokerHand;
//    using Infrastructure.Interfaces.PokerHand;
//
//    using Moq;

//  TODO: ReImplement
//    using PokerTell.Datalayer;
//    using PokerTell.Infrastructure;
//    using PokerTell.PokerHand.Analyzation;
//    using PokerTell.Statistics;
//    using PokerTell.Statistics.Detailed;
//    using PokerTell.Statistics.Interfaces;
//
//    /// <summary>
//    /// Description of Samples.
//    /// </summary>
//    public class SampleFactory
//    {
//        #region Constants and Fields
//
//        static readonly Random RandHowMany = new Random();
//
//        #endregion
//
//        #region Public Methods
//
//        public static IConvertedPokerHand CreateHandHeader_9Max()
//        {
//            return new ConvertedPokerHand("Pokerstars", 123456, DateTime.Parse("01/01/2009 12:00:00 pm"), 20, 10, 9);
//        }
//
//        public static OverlayStatisticsTexts CreateOverlayStatisticsTextsSampleWith(string playerName, int totalHands)
//        {
//            return CreateOverlayStatisticsTextsSampleWith(playerName, totalHands, string.Empty);
//        }
//
//        public static OverlayStatisticsTexts CreateOverlayStatisticsTextsSampleWith(
//            string playerName, int totalHands, string changeText)
//        {
//            var s1 = new string[3];
//            var s2 = new string[3];
//            var s3 = new string[3];
//
//            var s4 = new string[3];
//            var s5 = new string[3];
//            for (int i = 0; i < 3; i++)
//            {
//                s1[i] = changeText + "35";
//                s2[i] = changeText + "00-00";
//                s3[i] = changeText + "??x??";
//
//                s4[i] = changeText + "67";
//                s5[i] = changeText + "67-12";
//            }
//
//            return new OverlayStatisticsTexts(
//                playerName, 
//                changeText + totalHands, 
//                changeText + "20-12", 
//                changeText + "10-05", 
//                changeText + "s23", 
//                s1, 
//                s2, 
//                s3, 
//                s4, 
//                s5);
//        }
//
//        public static IConvertedPokerPlayer CreatePlayer(string playerName, int totalPlayers)
//        {
//            return new ConvertedPokerPlayer(playerName, 10, 12, 2, totalPlayers, "As Kd");
//        }
//
//        public static IPlayerStatistics CreatePlayerStatisticsWithInjectedRandomDatabaseThourough(string playerName)
//        {
//            var databaseMock = new Mock<IDatabaseQueryable>();
//            foreach (ActionSequences sequence in ActionSequencesUtility.GetAllPreflop)
//            {
//                databaseMock.Setup(
//                    foo => foo.ExecuteQueryGetFirstColumn<int>(It.IsRegex(SequenceCodes.GetCode(sequence, 0)))).Returns(
//                    CreateRandomListOfIntegers());
//            }
//
//            foreach (Streets street in StreetsUtility.GetAllPostFlop)
//            {
//                for (int i = 0; i < ApplicationProperties.BetSizeKeys.Length; i++)
//                {
//                    double ratio = ApplicationProperties.BetSizeKeys[i];
//
//                    foreach (ActionSequences sequence in ActionSequencesUtility.GetAllPostFlop)
//                    {
//                        string sqlStreet = street.ToString().ToLower();
//                        string patReg = string.Format(
//                            ".*inpos{0} = 0.*sequence{1}.*{2}", 
//                            sqlStreet, 
//                            (int)street, 
//                            SequenceCodes.GetCode(sequence, ratio));
//                        databaseMock.Setup(foo => foo.ExecuteQueryGetFirstColumn<int>(It.IsRegex(patReg))).Returns(
//                            CreateRandomListOfIntegers());
//
//                        patReg = string.Format(
//                            ".*inpos{0} = 1.*sequence{1}.*{2}", 
//                            sqlStreet, 
//                            (int)street, 
//                            SequenceCodes.GetCode(sequence, ratio));
//                        databaseMock.Setup(foo => foo.ExecuteQueryGetFirstColumn<int>(It.IsRegex(patReg))).Returns(
//                            CreateRandomListOfIntegers());
//                    }
//                }
//            }
//
//            return new PlayerStatistics(databaseMock.Object, playerName);
//        }
//
//        public DetailedStatisticsInfo CreateDetailedStatisticsInfo(string playerName)
//        {
//            return new DetailedStatisticsInfo(playerName, ActionSequences.HeroB, true, Streets.Flop);
//        }
//
//        public IConvertedPokerHand CreateHandHeaderWithSequences_9Max(string hero)
//        {
//            IConvertedPokerHand hand = CreateHandHeader_9Max();
//
//            hand.AddPlayer(CreatePlayer(hero, 9));
//            var round = new ConvertedPokerRound();
//            round.Add(new ConvertedPokerActionWithId(new ConvertedPokerAction(ActionTypes.B, 1.0), 0));
//
//            for (var street = (int)Streets.PreFlop; street <= (int)Streets.River; street++)
//            {
//                hand.AddSequence(round);
//            }
//
//            return hand;
//        }
//
//        public OverlayStatisticsTexts CreateOverlayStatisticsTextsSampleWith(int totalHands)
//        {
//            return CreateOverlayStatisticsTextsSampleWith("Test Player", totalHands);
//        }
//
//        public IPlayerStatistics CreatePlayerStatisticsWith(string playerName, int totalHands)
//        {
//            return new PlayerStatistics
//                {
//                    PlayerName = playerName, 
//                    OverlayText = CreateOverlayStatisticsTextsSampleWith(playerName, totalHands)
//                };
//        }
//
//        public IPlayerStatistics CreatePlayerStatisticsWith(int totalHands)
//        {
//            return CreatePlayerStatisticsWith("Test Player", totalHands);
//        }
//
//        public IPlayerStatistics CreatePlayerStatisticsWithInjectedRandomDatabaseFast(string playerName)
//        {
//            var databaseMock = new Mock<IDatabaseQueryable>();
//            databaseMock.Setup(foo => foo.ExecuteQueryGetFirstColumn<int>(It.IsAny<string>())).Returns(
//                CreateRandomListOfIntegers());
//
//            return new PlayerStatistics(databaseMock.Object, playerName);
//        }
//
//        public IPlayerStatistics CreatePlayerStatisticsWithInjectedRandomDatabaseSameForAllPostFlopAndPosition(
//            string playerName)
//        {
//            var databaseMock = new Mock<IDatabaseQueryable>();
//            foreach (ActionSequences sequence in ActionSequencesUtility.GetAllPreflop)
//            {
//                databaseMock.Setup(
//                    foo => foo.ExecuteQueryGetFirstColumn<int>(It.IsRegex(SequenceCodes.GetCode(sequence, 0)))).Returns(
//                    CreateRandomListOfIntegers());
//            }
//
//            for (int i = 0; i < ApplicationProperties.BetSizeKeys.Length; i++)
//            {
//                double ratio = ApplicationProperties.BetSizeKeys[i];
//
//                foreach (ActionSequences sequence in ActionSequencesUtility.GetAllPostFlop)
//                {
//                    databaseMock.Setup(
//                        foo => foo.ExecuteQueryGetFirstColumn<int>(It.IsRegex(SequenceCodes.GetCode(sequence, ratio)))).
//                        Returns(CreateRandomListOfIntegers());
//
//                    string patReg = string.Format(
//                        ".*inpos(flop|turn|river) = 1.*{0}", SequenceCodes.GetCode(sequence, ratio));
//
//                    databaseMock.Setup(foo => foo.ExecuteQueryGetFirstColumn<int>(It.IsRegex(patReg))).Returns(
//                        CreateRandomListOfIntegers());
//                }
//            }
//
//            return new PlayerStatistics(databaseMock.Object, playerName);
//        }
//
//        #endregion
//
//        #region Methods
//
//        static List<int> CreateRandomListOfIntegers()
//        {
//            var list = new List<int>();
//
//            const int minValue = 1;
//            const int maxValue = 9999;
//
//            int howMany = RandHowMany.Next(20);
//
//            var rand = new Random();
//
//            for (int i = 0; i < howMany; i++)
//            {
//                int value = rand.Next(minValue, maxValue);
//                if (!list.Contains(value))
//                {
//                    list.Add(value);
//                }
//            }
//
//            return list;
//        }
//
//        #endregion
//    }
}