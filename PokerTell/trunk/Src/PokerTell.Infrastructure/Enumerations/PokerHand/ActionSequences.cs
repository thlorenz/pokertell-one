namespace PokerTell.Infrastructure.Enumerations.PokerHand
{
    using System;
    using System.Collections.Generic;

    using Tools.FunctionalCSharp;

    /// <summary>
    ///   Enumeration of possible action sequences
    /// </summary>
    public enum ActionSequences
    {
        /// <summary>
        ///   Identifies Sequence as a non standard sequence that can not be included in detailed analysis
        /// </summary>
        NonStandard,

        /// <summary>
        ///   Identifies Sequence as Hero acts ActionSequence collection to DetailStatistic, applies to in and out of position
        /// </summary>
        HeroActs,

        /// <summary>
        ///   Hero checks, applies to in and out of position
        /// </summary>
        HeroX,

        /// <summary>
        ///   Hero bets, applies to in and out of position
        /// </summary>
        HeroB,

        /// <summary>
        ///   Identifies it as Opponent bets ActionSequence collection to DetailStatistics, applies to in and out of position
        /// </summary>
        OppB,

        /// <summary>
        ///   Opponent bets and Hero folds, applies to in and out of position
        /// </summary>
        OppBHeroF,

        /// <summary>
        ///   Opponent bets and Hero calls, applies to in and out of position
        /// </summary>
        OppBHeroC,

        /// <summary>
        ///   Opponent bets and Hero raises, applies to in and out of position
        /// </summary>
        OppBHeroR,

        /// <summary>
        ///   Identifies it as Hero checks ActionSequence collection to DetailStatistics
        /// </summary>
        HeroXOppB,

        /// <summary>
        ///   Hero checks, opponent bets and Hero folds, applies only to out of position
        /// </summary>
        HeroXOppBHeroF,

        /// <summary>
        ///   Hero checks, opponent bets and Hero calls, applies only to out of position
        /// </summary>
        HeroXOppBHeroC,

        /// <summary>
        ///   Hero checks, opponent bets and Hero raises, applies only to out of position
        /// </summary>
        HeroXOppBHeroR,

        /// <summary>
        ///   Identifies the Preflop ActionSequence to have occured in unraised pot
        /// </summary>
        PreFlopNoFrontRaise,

        /// <summary>
        ///   Hero folds Preflop
        /// </summary>
        HeroF,

        /// <summary>
        ///   Hero calls Preflop
        /// </summary>
        HeroC,

        /// <summary>
        ///   Hero raises Preflop
        /// </summary>
        HeroR,

        /// <summary>
        ///   Identifies the Preflop ActionSequence (Reaction) to have occured in raised pot
        /// </summary>
        PreFlopFrontRaise,

        /// <summary>
        ///   Opponent raises, Hero folds Preflop
        /// </summary>
        OppRHeroF,

        /// <summary>
        ///   Opponent raises, Hero calls Preflop
        /// </summary>
        OppRHeroC,

        /// <summary>
        ///   Opponent raises, Hero raises Preflop
        /// </summary>
        OppRHeroR
    }

    /// <summary>
    ///   The action sequences utility.
    /// </summary>
    public static class ActionSequencesUtility
    {
        /// <summary>Gets Bets.</summary>
        public static IList<ActionSequences> Bets
        {
            get { return new[] { ActionSequences.HeroB, ActionSequences.OppB, ActionSequences.HeroXOppB }; }
        }

        /// <summary>Gets Calls.</summary>
        public static IList<ActionSequences> Calls
        {
            get
            {
                return new[] {
                    ActionSequences.HeroC, ActionSequences.OppBHeroC, ActionSequences.OppRHeroC,
                    ActionSequences.HeroXOppBHeroC
                };
            }
        }

        /// <summary>Gets Checks.</summary>
        public static IList<ActionSequences> Checks
        {
            get { return new[] { ActionSequences.HeroX }; }
        }

        /// <summary>Gets Folds.</summary>
        public static IList<ActionSequences> Folds
        {
            get
            {
                return new[] {
                    ActionSequences.HeroF, ActionSequences.OppBHeroF, ActionSequences.OppRHeroF,
                    ActionSequences.HeroXOppBHeroF
                };
            }
        }

        /// <summary>Gets GetAll.</summary>
        public static IEnumerable<ActionSequences> GetAll
        {
            get
            {
                foreach (ActionSequences item in GetAllPreflop)
                {
                    yield return item;
                }

                foreach (ActionSequences item in GetAllPostFlop)
                {
                    yield return item;
                }
            }
        }

        /// <summary>
        ///   Gets GetAllPostFlop.
        /// </summary>
        public static IEnumerable<ActionSequences> GetAllPostFlop
        {
            get
            {
                foreach (ActionSequences item in GetHeroActs)
                {
                    yield return item;
                }

                foreach (ActionSequences item in GetHeroReacts)
                {
                    yield return item;
                }

                foreach (ActionSequences item in GetHeroChecksThenReacts)
                {
                    yield return item;
                }
            }
        }

        /// <summary>
        ///   Gets GetAllPreflop.
        /// </summary>
        public static IEnumerable<ActionSequences> GetAllPreflop
        {
            get
            {
                foreach (ActionSequences item in GetPreflopUnraised)
                {
                    yield return item;
                }

                foreach (ActionSequences item in GetPreflopRaised)
                {
                    yield return item;
                }
            }
        }

        /// <summary>
        ///   Gets GetHeroActs.
        /// </summary>
        public static IList<ActionSequences> GetHeroActs
        {
            get { return new[] { ActionSequences.HeroX, ActionSequences.HeroB }; }
        }

        /// <summary>
        ///   Gets GetHeroChecksThenReacts.
        /// </summary>
        public static IList<ActionSequences> GetHeroChecksThenReacts
        {
            get
            {
                return new[] {
                    ActionSequences.HeroXOppBHeroF, ActionSequences.HeroXOppBHeroC, ActionSequences.HeroXOppBHeroR
                };
            }
        }

        /// <summary>
        ///   Gets GetHeroReacts.
        /// </summary>
        public static IList<ActionSequences> GetHeroReacts
        {
            get { return new[] { ActionSequences.OppBHeroF, ActionSequences.OppBHeroC, ActionSequences.OppBHeroR }; }
        }

        /// <summary>
        ///   Gets GetPreflopRaised.
        /// </summary>
        public static IList<ActionSequences> GetPreflopRaised
        {
            get { return new[] { ActionSequences.OppRHeroF, ActionSequences.OppRHeroC, ActionSequences.OppRHeroR }; }
        }

        /// <summary>
        ///   Gets GetPreflopUnraised.
        /// </summary>
        public static IList<ActionSequences> GetPreflopUnraised
        {
            get { return new[] { ActionSequences.HeroF, ActionSequences.HeroC, ActionSequences.HeroR }; }
        }

        /// <summary>Gets Raises.</summary>
        public static IList<ActionSequences> Raises
        {
            get
            {
                return new[] {
                    ActionSequences.HeroR, ActionSequences.OppBHeroR, ActionSequences.OppRHeroR,
                    ActionSequences.HeroXOppBHeroR
                };
            }
        }

        /// <summary>
        ///   The get last action in.
        /// </summary>
        /// <param name="actionSequence">
        ///   The action sequence.
        /// </param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static ActionTypes GetLastActionIn(ActionSequences actionSequence)
        {
            if (Folds.Contains(actionSequence))
            {
                return ActionTypes.F;
            }

            if (Calls.Contains(actionSequence))
            {
                return ActionTypes.C;
            }

            if (Raises.Contains(actionSequence))
            {
                return ActionTypes.R;
            }

            if (Checks.Contains(actionSequence))
            {
                return ActionTypes.X;
            }

            if (Bets.Contains(actionSequence))
            {
                return ActionTypes.B;
            }

            throw new ArgumentException(actionSequence + " has no last Action");
        }

        /// <summary>
        ///   The name last action in sequence.
        /// </summary>
        /// <param name="actionSequence">
        ///   The action sequence.
        /// </param>
        /// <returns>
        ///   The name last action in sequence.
        /// </returns>
        /// <exception cref="NotSupportedException"></exception>
        public static string NameLastActionInSequence(ActionSequences actionSequence)
        {
            if (Folds.Contains(actionSequence))
            {
                return "Fold";
            }

            if (Calls.Contains(actionSequence))
            {
                return "Call";
            }

            if (Raises.Contains(actionSequence))
            {
                return "Raise";
            }

            if (Bets.Contains(actionSequence))
            {
                return "Bet";
            }

            if (actionSequence == ActionSequences.HeroX)
            {
                return "Check";
            }

            throw new NotSupportedException("Cannot name last action of " + actionSequence);
        }

        public static string NamePastTenseOfLastActionSequence(ActionSequences actionSequence)
        {
            ActionTypes lastAction = GetLastActionIn(actionSequence);
            return
                lastAction.Match()
                    .With(a => a == ActionTypes.F, _ => "folded")
                    .With(a => a == ActionTypes.C, _ => "called")
                    .With(a => a == ActionTypes.R, _ => "raised")
                    .Do();
        }
    }
}