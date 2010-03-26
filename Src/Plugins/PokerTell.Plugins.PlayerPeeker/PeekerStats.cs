namespace PokerTell.Plugins.PlayerPeeker
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Text.RegularExpressions;

    using log4net;

    /// <summary>
    /// Description of PeekerStats.
    /// </summary>
    public class PeekerStats
    {
        public MonStruct AvgBuyin;

        public AvgStruct AvgEntrants;

        public TextStruct AvgFinish;

        public MonStruct AvgProfit;

        public AvgStruct GamesPlayed;

        public TextStruct Name;

        public PercStruct PercFinalTables;

        public PercStruct PercITM;

        public PercStruct PercROI;

        public PercStruct PercWins;

        public MonStruct Profit;

        public TextStruct Site;

        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public PeekerStats(string htmlTagText)
        {
            Name = new TextStruct(@"StatsName: (?<Q>.+) Average", htmlTagText);

            Site = new TextStruct(@"Site: (?<Q>\w+) Average", htmlTagText);

            AvgFinish = new TextStruct(@"Average Finish: (?<Q>\d+/\d+)", htmlTagText);

            AvgEntrants = new AvgStruct(@"Average Entrants: *(?<Q>[0-9,.]+)", htmlTagText);

            AvgBuyin = new MonStruct(@"Average Buyin: *\$(?<Q>[0-9,.]+)", htmlTagText);

            AvgProfit = new MonStruct(@"Average Profit: *(?<Q>[-$0-9,.]+)", htmlTagText);

            Profit = new MonStruct(@"Est\. Profit: *(?<Q>[-$0-9,.]+)", htmlTagText);

            PercROI = new PercStruct(@"ROI:  *(?<Q>[-0-9,.]+)%", htmlTagText);

            PercITM = new PercStruct(@"ITM:  *(?<Q>[-0-9,.]+)%", htmlTagText);

            PercFinalTables = new PercStruct(@"Final Tables:  *\d+ *\((?<Q>[-0-9,.]+)%\)", htmlTagText);

            PercWins = new PercStruct(@"Wins:  *\d+ *\((?<Q>[-0-9,.]+)%\)", htmlTagText);

            GamesPlayed = new AvgStruct(@"Games Played: *(?<Q>[0-9,.]+)", htmlTagText);
        }

        public struct AvgStruct
        {
            public readonly string Pattern;

            public readonly int Value;

            readonly Match _match;

            public AvgStruct(string pattern, string toParse)
            {
                Pattern = pattern;
                _match = Regex.Match(toParse, pattern, RegexOptions.IgnoreCase);

                try
                {
                    if (_match.Success)
                    {
                        if (!int.TryParse(_match.Groups["Q"].Value.Replace(",", string.Empty), out Value))
                        {
                            Value = -1;
                        }
                    }
                    else
                    {
                        Value = -1;
                    }
                }
                catch (Exception excep)
                {
                    Log.Error("Unexpected", excep);
                    Value = -1;
                }
            }

            public override string ToString()
            {
                return string.Format("{0:0,0}", Value);
            }
        }

        public struct MonStruct
        {
            public readonly string Pattern;

            public readonly double Value;

            readonly Match _match;

            public MonStruct(string pattern, string toParse)
            {
                string temp;
                Pattern = pattern;
                _match = Regex.Match(toParse, pattern, RegexOptions.IgnoreCase);

                try
                {
                    if (_match.Success)
                    {
                        temp = _match.Groups["Q"].Value.Replace("$", string.Empty);
                        temp = temp.Replace(",", string.Empty);

                        if (!double.TryParse(temp, out Value))
                        {
                            Value = -1;
                        }
                    }
                    else
                    {
                        Value = -1;
                    }
                }
                catch (Exception excep)
                {
                    Log.Error("Unexpected", excep);
                    Value = -1;
                }
            }

            public override string ToString()
            {
                return string.Format("{00:#,0.00}$", Value);
            }
        }

        public struct PercStruct
        {
            public readonly string Pattern;

            public readonly int Value;

            readonly Match _match;

            public PercStruct(string pattern, string toParse)
            {
                Pattern = pattern;
                _match = Regex.Match(toParse, pattern, RegexOptions.IgnoreCase);
                try
                {
                    if (_match.Success)
                    {
                        if (!int.TryParse(_match.Groups["Q"].Value, out Value))
                        {
                            Value = -1;
                        }
                    }
                    else
                    {
                        Value = -1;
                    }
                }
                catch (Exception excep)
                {
                    Log.Error("Unexpected", excep);
                    Value = -1;
                }
            }

            public override string ToString()
            {
                return string.Format("{00}%", Value);
            }
        }

        public struct TextStruct
        {
            public readonly string Pattern;

            public readonly string Value;

            readonly Match _match;

            public TextStruct(string pattern, string toParse)
            {
                Pattern = pattern;
                _match = Regex.Match(toParse, pattern, RegexOptions.IgnoreCase);
                Value = _match.Groups["Q"].Value;
            }

            public override string ToString()
            {
                return Value;
            }
        }
    }
}