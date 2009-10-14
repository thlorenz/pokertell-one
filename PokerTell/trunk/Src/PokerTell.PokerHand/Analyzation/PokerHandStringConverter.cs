namespace PokerTell.PokerHand.Analyzation
{
    using System;
    using System.Reflection;
    using System.Text.RegularExpressions;

    using log4net;

    using PokerTell.Infrastructure.Enumerations.PokerHand;
    using PokerTell.Infrastructure.Interfaces.PokerHand;

    /// <summary>
    /// Decorates ConvertedPokerAction with ToSql method
    /// and a constructor that takes an Sql string as input
    /// </summary>
    public class PokerHandStringConverter : IPokerHandStringConverter
    {
        static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
       
        #region Constants and Fields

        const string PatSqlActionWithId = @"^\[(?<playerID>\d+)\](?<whatAndRatio>.+)";

        #endregion

        #region Implemented Interfaces

        #region IPokerHandStringConverter

        public string BuildSqlStringFrom(IConvertedPokerRound convertedRound)
        {
            string sqlString = string.Empty;

            for (int i = 0; i < convertedRound.Count; i++)
            {
                try
                {
                    IConvertedPokerAction theAction = convertedRound[i];

                    if (theAction is IConvertedPokerActionWithId)
                    {
                        sqlString = sqlString + BuildSqlStringFrom((IConvertedPokerActionWithId)theAction);
                    }
                    else
                    {
                        sqlString = sqlString + BuildSqlStringFrom(theAction);
                    }

                    sqlString += ",";
                }
                catch (Exception excep)
                {
                    Log.DebugFormat("Unhandled {0}", excep);
                }
            }

            return sqlString.TrimStart(',').TrimEnd(',');
        }

        /// <summary>
        /// Converts an action string in Database format into a PokerRound
        /// </summary>
        /// <param name="csvRound">Comma separated String representation of Bettinground
        /// for instance as read from the database</param>
        public IConvertedPokerRound ConvertedRoundFrom(string csvRound)
        {
            var convertedRound = new ConvertedPokerRound();

            if (! string.IsNullOrEmpty(csvRound))
            {
                // Determine if actions contain player IDs
                bool actionsContainId = Regex.Match(csvRound, PatSqlActionWithId).Success;

                string[] theActions = csvRound.Split(',');

                foreach (string sqlActionString in theActions)
                {
                    if (actionsContainId)
                    {
                        convertedRound.AddAction(ConvertedActionWithIdFrom(sqlActionString));
                    }
                    else
                    {
                        convertedRound.AddAction(ConvertedActionFrom(sqlActionString));
                    }
                }
            }

            return convertedRound;
        }

        #endregion

        #endregion

        #region Methods

        /// <summary>
        /// Gives a string representation of the action to be entered into the database
        /// </summary>
        static string BuildSqlStringFrom(IConvertedPokerAction convertedAction)
        {
            switch (convertedAction.What)
            {
                case ActionTypes.A:
                case ActionTypes.X:
                case ActionTypes.F:
                   return string.Format("{0}", convertedAction.What);
                case ActionTypes.E:
                case ActionTypes.N:
                case ActionTypes.P:
                case ActionTypes.U:
                        Log.DebugFormat("Encountered this illegal action {0}, returning empty string.", convertedAction.What);
                    return string.Empty;
                default:
                    return string.Format("{0}{1:F1}", convertedAction.What, convertedAction.Ratio);
            }
        }

        static string BuildSqlStringFrom(IConvertedPokerActionWithId convertedActionWithId)
        {
            return string.Format(
                "[{0}]{1}", convertedActionWithId.Id, BuildSqlStringFrom(convertedActionWithId as ConvertedPokerAction));
        }

        static IConvertedPokerAction ConvertedActionFrom(string sqlAction)
        {
            ActionTypes theWhat = PokerAction.GetActionWhatFromSql(sqlAction.Substring(0, 1));

            double parsedRatio;
            double theRatio = double.TryParse(sqlAction.Substring(1), out parsedRatio) ? parsedRatio : 1.0;

            return new ConvertedPokerAction(theWhat, theRatio);
        }

        static IConvertedPokerActionWithId ConvertedActionWithIdFrom(string sqlAction)
        {
            Match m = Regex.Match(sqlAction, PatSqlActionWithId);

            if (! (m.Groups["playerID"].Success && m.Groups["whatAndRatio"].Success))
            {
                throw new FormatException("Given sqlAction cannot be matched: " + sqlAction);
            }

            string whatAndRatio = m.Groups["whatAndRatio"].Value;

            int playerId = int.Parse(m.Groups["playerID"].Value);

            IConvertedPokerAction convertedAction = ConvertedActionFrom(whatAndRatio);

            return new ConvertedPokerActionWithId(convertedAction, playerId);
        }

        #endregion
    }
}