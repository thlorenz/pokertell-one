namespace PokerTell.PokerHand.Analyzation
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    using Infrastructure;
    using Infrastructure.Enumerations.PokerHand;

    /// <summary>
    /// Converts an Action Sequence string into the appropriate ActionSequence and BetSizeIndex if applies
    /// </summary>
    public class SequenceStringConverter
    {
        string _sequenceString;

        public IList<int> StandardizedBetSizes { get; private set; }

        public ActionSequences ActionSequence { get; private set; }

        public int BetSizeIndex { get; private set; }

        public SequenceStringConverter()
        {
            StandardizedBetSizes = new List<int>();

            foreach (var betSizeKey in ApplicationProperties.BetSizeKeys)
            {
                StandardizedBetSizes.Add((int)(betSizeKey * 10));
            }
        }

        public void Convert(string sequenceString)
        {
            BetSizeIndex = 0;
            
            if (string.IsNullOrEmpty(sequenceString))
            {
                ActionSequence = ActionSequences.NonStandard;
            }
            else
            {
                _sequenceString = sequenceString;

                switch (_sequenceString)
                {
                    case "X":
                        ActionSequence = ActionSequences.HeroX;
                        break;

                    case "F":
                        ActionSequence = ActionSequences.HeroF;
                        break;

                    case "C":
                        ActionSequence = ActionSequences.HeroC;
                        break;

                    case "R":
                        ActionSequence = ActionSequences.HeroR;
                        break;

                    case "RF":
                        ActionSequence = ActionSequences.OppRHeroF;
                        break;

                    case "RC":
                        ActionSequence = ActionSequences.OppRHeroC;
                        break;

                    case "RR":
                        ActionSequence = ActionSequences.OppRHeroR;
                        break;

                    default:
                        MatchStringContainingBetSizeKey();
                        break;
                }
            }
        }

        void MatchStringContainingBetSizeKey()
        {
            const string betSizePattern = @"(?<BetSizeKey>\d{1,2})";

            const string HeroBPattern = @"^" + betSizePattern + @"\b";

            const string OppBHeroFPattern = @"^" + betSizePattern + @"F\b";
            const string OppBHeroCPattern = @"^" + betSizePattern + @"C\b";
            const string OppBHeroRPattern = @"^" + betSizePattern + @"R\b";

            const string HeroXOppBHeroFPattern = @"^X" + betSizePattern + @"F\b";
            const string HeroXOppBHeroCPattern = @"^X" + betSizePattern + @"C\b";
            const string HeroXOppBHeroRPattern = @"^X" + betSizePattern + @"R\b";

            if (FoundMatchFor(HeroBPattern, ActionSequences.HeroB))
            {
                return;
            }

            if (FoundMatchFor(OppBHeroFPattern, ActionSequences.OppBHeroF))
            {
                return;
            }

            if (FoundMatchFor(OppBHeroCPattern, ActionSequences.OppBHeroC))
            {
                return;
            }

            if (FoundMatchFor(OppBHeroRPattern, ActionSequences.OppBHeroR))
            {
                return;
            }

            if (FoundMatchFor(HeroXOppBHeroFPattern, ActionSequences.HeroXOppBHeroF))
            {
                return;
            }

            if (FoundMatchFor(HeroXOppBHeroCPattern, ActionSequences.HeroXOppBHeroC))
            {
                return;
            }

            if (FoundMatchFor(HeroXOppBHeroRPattern, ActionSequences.HeroXOppBHeroR))
            {
                return;
            }

            ActionSequence = ActionSequences.NonStandard;
        }

        bool FoundMatchFor(string pattern, ActionSequences actionSequence)
        {
            Match match = Regex.Match(_sequenceString, pattern, RegexOptions.IgnoreCase);
            if (match.Success)
            {
                ActionSequence = actionSequence;
                BetSizeIndex = StandardizedBetSizes.IndexOf(System.Convert.ToInt32(match.Groups["BetSizeKey"].Value));
                return true;
            }

            return false;
        }
    }
  
}