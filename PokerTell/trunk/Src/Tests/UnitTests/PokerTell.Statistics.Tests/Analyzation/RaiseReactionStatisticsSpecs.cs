namespace PokerTell.Statistics.Tests.Analyzation
{
    // ReSharper disable InconsistentNaming
    using System;

    using Infrastructure.Enumerations.PokerHand;

    using Interfaces;

    using Machine.Specifications;

    using Moq;

    using Statistics.Analyzation;

    using It = Machine.Specifications.It;

    public abstract class RaiseReactionStatisticsSpecs
    {

        /*
         *  Specifications
         *  Subject: RaiseReactionStatistics
            
         *  Intitializing with RaiseReactionsAnalyzer
               
              RaiseReactionsAnalyzer returns zero raiseSizes
                    » should fail with an ArgumentException

                RaiseReactionsAnalyzer returns two raiseSizes and any number of RaiseReactionAnalyzers
                    » should create AnalyzablePlayersDictionary with 2 columns
                    » should create AnalyzablePlayersDictionary with 3 rows
                    » should create PercentagesDictionary with 2 columns
                    » should create PercentagesDictionary with 3 rows
                    » should create TotalCountsByColumnDictionary with two columns

                RaiseReactionsAnalyzer returns two raiseSizes and zero RaiseReactionAnalyzers
                    » should have 0 raiseReactionAnalyzers in first row first column of AnalyzablePlayersDictionary
                    » should have 0 raiseReactionAnalyzers in third row second column of AnalyzablePlayersDictionary
                    » value in PercentageDictionary at first row first column should be 0
                    » value in PercentageDictionary at third row second column should be 0
                    » value in TotalCountsByColumnDictionary at third row second column should be 0
                    » value in TotalCountsByColumnDictionary at first row first column should be 0

                RaiseReactionsAnalyzer returns two raiseSizes and one RaiseReactionAnalyzer with HeroReactionType of firstRow and OpponentRaiseSize of firstColumn
                    » count of AnlyzablePokerPlayersDictionary at first row and first column should be 1
                    » count of AnlyzablePokerPlayersDictionary at third row second column should be 0
                    » value in TotalCountsByColumnDictionary at first column should be 1
                    » value in TotalCountsByColumnDictionary at second column should be 0
                    » value in PercentagesDictionary at first row first column should be 100
                    » value in PercentagesDictionary at at third row second column should be 0

                RaiseReactionsAnalyzer returns two raiseSizes and a RaiseReactionAnalyzer with HeroReactionType of firstRow and OpponentRaiseSize of firstColumn and another RaiseReactionAnalyzer with HeroReactionType of thirdRow and OpponentRaiseSize of firstColumn
                    » count of AnlyzablePokerPlayersDictionary at first row and first column should be 1
                    » count of AnlyzablePokerPlayersDictionary at third row and first column should be 1
                    » count of AnlyzablePokerPlayersDictionary at third row second column should be 0
                    » value in TotalCountsByColumnDictionary at first column should be 2
                    » value in TotalCountsByColumnDictionary at second column should be 1
                    » value in PercentagesDictionary at first row first column should be 50
                    » value in PercentagesDictionary at second row first column should be 0
                    » value in PercentagesDictionary at third row first column should be 50
                    » value in PercentagesDictionary at at third row second column should be zero
         */
        protected static IRaiseReactionStatistics sut;

        protected static Mock<IRaiseReactionsAnalyzer> raiseReactionsAnalyzer;

        protected static double[] twoRaiseSizes;

        protected static ActionTypes firstRow;

        Establish context = () =>
        {
            sut = new RaiseReactionStatistics();
            raiseReactionsAnalyzer = new Mock<IRaiseReactionsAnalyzer>();
            twoRaiseSizes = new[] { 1.0, 2.0 };
            firstRow = ActionTypes.F;
        };
    }

    public abstract class RaiseReactionStatistics_with_raiseReactionsAnalyzer_that_returns_two_raiseSizes
        : RaiseReactionStatisticsSpecs
    {
        protected static int firstColumn;

        protected static int secondColumn;

        protected static ActionTypes thirdRow;

        Establish context = () =>
        {
            raiseReactionsAnalyzer.SetupGet(r => r.RaiseSizeKeys).Returns(twoRaiseSizes);
            thirdRow = ActionTypes.R;
            firstColumn = (int)twoRaiseSizes[0];
            secondColumn = (int)twoRaiseSizes[1];
        };
    }

    public abstract class RaiseReactionStatistics_with_raiseReactionsAnalyzer_that_returns_two_raiseSizes_and_a_raiseReactionAnalyzer_with_HeroReactionType_of_firstRow_and_OpponentRaiseSize_of_firstColumn
        : RaiseReactionStatistics_with_raiseReactionsAnalyzer_that_returns_two_raiseSizes
    {
        Establish context = () =>
        {
            firstReactionAnalyzerStub = new Mock<IRaiseReactionAnalyzer>();
            firstReactionAnalyzerStub.SetupGet(a => a.HeroReactionType).Returns(firstRow);
            firstReactionAnalyzerStub.SetupGet(a => a.OpponentRaiseSize).Returns(firstColumn);
        };

        protected static Mock<IRaiseReactionAnalyzer> firstReactionAnalyzerStub;
    }

    public class RaiseReactionStatistics_with_raiseReactionsAnalyzer_that_returns_two_raiseSizes_and_a_RaiseReactionAnalyzer_with_HeroReactionType_of_firstRow_and_OpponentRaiseSize_of_firstColumn_and_another_RaiseReactionAnalyzer_with_HeroReactionType_of_thirdRow_and_OpponentRaiseSize_of_firstColumn
        : RaiseReactionStatistics_with_raiseReactionsAnalyzer_that_returns_two_raiseSizes_and_a_raiseReactionAnalyzer_with_HeroReactionType_of_firstRow_and_OpponentRaiseSize_of_firstColumn
    {
        Establish context = () =>
        {
            secondRow = ActionTypes.C;
            secondReactionAnalyzerStub = new Mock<IRaiseReactionAnalyzer>();
            secondReactionAnalyzerStub.SetupGet(a => a.HeroReactionType).Returns(thirdRow);
            secondReactionAnalyzerStub.SetupGet(a => a.OpponentRaiseSize).Returns(firstColumn);
        };

        protected static ActionTypes secondRow;
        protected static Mock<IRaiseReactionAnalyzer> secondReactionAnalyzerStub;
    }


    [Subject(typeof(RaiseReactionStatistics), "intitializing with RaiseReactionsAnalyzer")]
    public class RaiseReactionsAnalyzer_returns_zero_raiseSizes : RaiseReactionStatisticsSpecs
    {
        static Exception exception;

        Because of = () => {
            raiseReactionsAnalyzer
                .SetupGet(r => r.RaiseSizeKeys)
                .Returns(new double[] { });

            exception = Catch.Exception(() => sut.InitializeWith(raiseReactionsAnalyzer.Object));
        };

        It should_fail_with_an_ArgumentException =
            () => exception.ShouldBeOfType<ArgumentException>();
    }

    [Subject(typeof(RaiseReactionStatistics), "intitializing with RaiseReactionsAnalyzer" )]
    public class RaiseReactionsAnalyzer_returns_two_raiseSizes_and_any_number_of_RaiseReactionAnalyzers
        : RaiseReactionStatistics_with_raiseReactionsAnalyzer_that_returns_two_raiseSizes
    {
        Because of = () => sut.InitializeWith(raiseReactionsAnalyzer.Object);

        It should_create_AnalyzablePlayersDictionary_with_2_columns =
            () => sut.AnalyzablePlayersDictionary[firstRow].Count.ShouldEqual(2);

        It should_create_AnalyzablePlayersDictionary_with_3_rows =
            () => sut.AnalyzablePlayersDictionary.Count.ShouldEqual(3);

        It should_create_PercentagesDictionary_with_2_columns =
            () => sut.PercentagesDictionary[firstRow].Count.ShouldEqual(2);

        It should_create_PercentagesDictionary_with_3_rows =
            () => sut.PercentagesDictionary.Count.ShouldEqual(3);

        It should_create_TotalCountsByColumnDictionary_with_two_columns =
            () => sut.TotalCountsByColumnDictionary.Count.ShouldEqual(2);
    }

    [Subject(typeof(RaiseReactionStatistics), "intitializing with RaiseReactionsAnalyzer")]
    public class RaiseReactionsAnalyzer_returns_two_raiseSizes_and_zero_RaiseReactionAnalyzers
        : RaiseReactionStatistics_with_raiseReactionsAnalyzer_that_returns_two_raiseSizes
    {
        Because of = () => {
            raiseReactionsAnalyzer
                .SetupGet(r => r.RaiseReactionAnalyzers)
                .Returns(new IRaiseReactionAnalyzer[] { });

            sut.InitializeWith(raiseReactionsAnalyzer.Object);
        };

        It should_have_0_raiseReactionAnalyzers_in_first_row_first_column_of_AnalyzablePlayersDictionary =
            () => sut.AnalyzablePlayersDictionary[firstRow][firstColumn].Count.ShouldEqual(0);

        It should_have_0_raiseReactionAnalyzers_in_third_row_second_column_of_AnalyzablePlayersDictionary =
            () => sut.AnalyzablePlayersDictionary[thirdRow][secondColumn].Count.ShouldEqual(0);

        It value_in_PercentageDictionary_at_first_row_first_column_should_be_0 =
            () => sut.PercentagesDictionary[firstRow][firstColumn].ShouldEqual(0);

        It value_in_PercentageDictionary_at_third_row_second_column_should_be_0 =
            () => sut.PercentagesDictionary[thirdRow][secondColumn].ShouldEqual(0);

        It value_in_TotalCountsByColumnDictionary_at_third_row_second_column_should_be_0 =
            () => sut.TotalCountsByColumnDictionary[firstColumn].ShouldEqual(0);

        It value_in_TotalCountsByColumnDictionary_at_first_row_first_column_should_be_0 =
            () => sut.TotalCountsByColumnDictionary[secondColumn].ShouldEqual(0);
    }

    [Subject(typeof(RaiseReactionStatistics), "intitializing with RaiseReactionsAnalyzer")]
    public class RaiseReactionsAnalyzer_returns_two_raiseSizes_and_one_RaiseReactionAnalyzer_with_HeroReactionType_of_firstRow_and_OpponentRaiseSize_of_firstColumn
        : RaiseReactionStatistics_with_raiseReactionsAnalyzer_that_returns_two_raiseSizes_and_a_raiseReactionAnalyzer_with_HeroReactionType_of_firstRow_and_OpponentRaiseSize_of_firstColumn
    {
        Because of = () => {
            raiseReactionsAnalyzer
                .SetupGet(r => r.RaiseReactionAnalyzers)
                .Returns(new[] { firstReactionAnalyzerStub.Object });

            sut.InitializeWith(raiseReactionsAnalyzer.Object);
        };

        It count_of_AnlyzablePokerPlayersDictionary_at_first_row_and_first_column_should_be_1 =
            () => sut.AnalyzablePlayersDictionary[firstRow][firstColumn].Count.ShouldEqual(1);

        It count_of_AnlyzablePokerPlayersDictionary_at_third_row_second_column_should_be_0 =
            () => sut.AnalyzablePlayersDictionary[thirdRow][secondColumn].Count.ShouldEqual(0);

        It value_in_TotalCountsByColumnDictionary_at_first_column_should_be_1 =
            () => sut.TotalCountsByColumnDictionary[firstColumn].ShouldEqual(1);

        It value_in_TotalCountsByColumnDictionary_at_second_column_should_be_0 =
            () => sut.TotalCountsByColumnDictionary[secondColumn].ShouldEqual(0);

        It value_in_PercentagesDictionary_at_first_row_first_column_should_be_100 =
          () => sut.PercentagesDictionary[firstRow][firstColumn].ShouldEqual(100);

        It value_in_PercentagesDictionary_at_at_third_row_second_column_should_be_0 =
          () => sut.PercentagesDictionary[thirdRow][secondColumn].ShouldEqual(0);
    }

     [Subject(typeof(RaiseReactionStatistics), "intitializing with RaiseReactionsAnalyzer")]
    public class RaiseReactionsAnalyzer_returns_two_raiseSizes_and_a_RaiseReactionAnalyzer_with_HeroReactionType_of_firstRow_and_OpponentRaiseSize_of_firstColumn_and_another_RaiseReactionAnalyzer_with_HeroReactionType_of_thirdRow_and_OpponentRaiseSize_of_firstColumn
        : RaiseReactionStatistics_with_raiseReactionsAnalyzer_that_returns_two_raiseSizes_and_a_RaiseReactionAnalyzer_with_HeroReactionType_of_firstRow_and_OpponentRaiseSize_of_firstColumn_and_another_RaiseReactionAnalyzer_with_HeroReactionType_of_thirdRow_and_OpponentRaiseSize_of_firstColumn
     {
         Because of = () => {
             raiseReactionsAnalyzer
                 .SetupGet(r => r.RaiseReactionAnalyzers)
                 .Returns(new[] { firstReactionAnalyzerStub.Object, secondReactionAnalyzerStub.Object });

             sut.InitializeWith(raiseReactionsAnalyzer.Object);
         };

         It count_of_AnlyzablePokerPlayersDictionary_at_first_row_and_first_column_should_be_1 =
            () => sut.AnalyzablePlayersDictionary[firstRow][firstColumn].Count.ShouldEqual(1);

         It count_of_AnlyzablePokerPlayersDictionary_at_third_row_and_first_column_should_be_1 =
           () => sut.AnalyzablePlayersDictionary[thirdRow][firstColumn].Count.ShouldEqual(1);

         It count_of_AnlyzablePokerPlayersDictionary_at_third_row_second_column_should_be_0 =
             () => sut.AnalyzablePlayersDictionary[thirdRow][secondColumn].Count.ShouldEqual(0);

         It value_in_TotalCountsByColumnDictionary_at_first_column_should_be_2 =
             () => sut.TotalCountsByColumnDictionary[firstColumn].ShouldEqual(2);

         It value_in_TotalCountsByColumnDictionary_at_second_column_should_be_1 =
             () => sut.TotalCountsByColumnDictionary[secondColumn].ShouldEqual(0);

         It value_in_PercentagesDictionary_at_first_row_first_column_should_be_50 =
           () => sut.PercentagesDictionary[firstRow][firstColumn].ShouldEqual(50);

         It value_in_PercentagesDictionary_at_second_row_first_column_should_be_0 =
          () => sut.PercentagesDictionary[secondRow][firstColumn].ShouldEqual(0);

         It value_in_PercentagesDictionary_at_third_row_first_column_should_be_50 =
           () => sut.PercentagesDictionary[thirdRow][firstColumn].ShouldEqual(50);

         It value_in_PercentagesDictionary_at_at_third_row_second_column_should_be_zero =
           () => sut.PercentagesDictionary[thirdRow][secondColumn].ShouldEqual(0);
     }

    
    // ReSharper restore InconsistentNaming
}