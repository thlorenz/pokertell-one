namespace PokerTell.Infrastructure.Interfaces.PokerHand
{
    using System;

    public interface IHandHistoriesFilter
    {
        string HeroName { get; set; }

        bool SelectHero { get; set; }

        bool ShowAll { get; set; }

        bool ShowMoneyInvested { get; set; }

        bool ShowPreflopFolds { get; set; }

        bool ShowSawFlop { get; set; }

        bool ShowSelectedOnly { get; set; }

        event Action HeroNameChanged;

        event Action SelectHeroChanged;

        event Action ShowAllChanged;

        event Action ShowMoneyInvestedChanged;

        event Action ShowPreflopFoldsChanged;

        event Action ShowSawFlopChanged;

        event Action ShowSelectedOnlyChanged;
    }
}