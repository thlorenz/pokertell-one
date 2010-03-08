namespace PokerTell.SessionReview
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text;

    using log4net;

    using PokerTell.Infrastructure.Enumerations.PokerHand;
    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.SessionReview.Properties;

    /// <summary>
    /// Description of HtmlStringBuilder.
    /// </summary>
    internal class HtmlStringBuilder
    {
        #region Constants and Fields

        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region Public Methods

        /// <summary>
        /// Gives a html representation of the action
        /// </summary>
        /// <returns>String representation of the action type and ratio</returns>
        public static string BuildFrom(IConvertedPokerAction convertedAction)
        {
            string htmlString;

            switch (convertedAction.What)
            {
                case ActionTypes.P:
                case ActionTypes.B:
                case ActionTypes.C:
                case ActionTypes.W:
                case ActionTypes.R:
                    htmlString = string.Format("{0} {1:F1}", convertedAction.What, convertedAction.Ratio);
                    break;
                case ActionTypes.A:
                case ActionTypes.X:
                case ActionTypes.F:
                case ActionTypes.E:
                    htmlString = string.Format("{0}&nbsp&nbsp&nbsp&nbsp&nbsp", convertedAction.What);
                    break;
                default:
                    htmlString = string.Format("{0} {1:F1}", convertedAction.What, convertedAction.Ratio);
                    break;
            }

            return htmlString;
        }

        /// <summary>
        /// Gives string representation for an Html Cell containing this betting round
        /// <param name="indent">If true, this is Preflop and player is SB or BB, so we need to indent</param>
        /// </summary>
        /// <returns>Html Cell string</returns>
        public static string BuildFrom(IConvertedPokerRound convertedRound, bool indent)
        {
            string strIndent = indent ? " &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp " : string.Empty;

            string roundString = string.Empty;
            foreach (IConvertedPokerAction iConvertedAction in convertedRound)
            {
                roundString = roundString + BuildFrom(iConvertedAction) + " &nbsp ";
            }

            return string.Format("\t\t\t\t<td><font size=\"2\">{0}{1}</td>", strIndent, roundString);
        }

        /// <summary>
        /// Gives string representation for an Html Row containing this Players information
        /// </summary>
        public static string BuildFrom(IConvertedPokerPlayer convertedPlayer, bool markRow)
        {
            var strToHtml = new StringBuilder();
            string strMarkRow = string.Empty;
            bool Indent;

            if (markRow)
            {
                strMarkRow = " bgcolor=\"#DCDCDC\"";
            }

            // Header
            strToHtml.AppendLine("\t\t\t<tr valign=\"top\" halign=\"left\"" + strMarkRow + ">");

            // Position
            strToHtml.AppendLine(
                string.Format(
                    "\t\t\t\t<td align=\"center\"><font size=\"2\">{0}</td>", convertedPlayer.StrategicPosition));

            // Name
            strToHtml.AppendLine(string.Format("\t\t\t\t<td><font size=\"2\">{0}</td>", convertedPlayer.Name));

            // Cards
            strToHtml.AppendLine(
                string.Format(
                    "\t\t\t\t<td align=\"center\"><font size=\"2\">{0}</td>", HoleCardsToHtml(convertedPlayer.Holecards)));

            // BB
            strToHtml.AppendLine(
                string.Format("\t\t\t\t<td align=\"center\"><font size=\"2\">{0}</td>", convertedPlayer.MBefore));

            // Bettingrounds
            foreach (IConvertedPokerRound round in convertedPlayer)
            {
                Indent = convertedPlayer.StrategicPosition <= StrategicPositions.BB 
                    && convertedPlayer.Rounds.IndexOf(round) == (int)Streets.PreFlop;
                
                strToHtml.AppendLine(BuildFrom(round, Indent));
            }

            // Footer
            strToHtml.AppendLine("\t\t\t</tr>");

            return strToHtml.ToString();
        }

        /// <summary>
        /// Creates a string containg Html Code for and Html Table
        /// representing this Poker Hand
        /// </summary>
        /// <param name="convertedHand"></param>
        /// <param name="showInactives">If true, inactive folds will be included in the table</param>
        /// <param name="heroName"></param>
        /// <param name="note">The note the user entered about the hand.</param>
        /// <returns>Html Code for Table of this hand</returns>
        public static string BuildFrom(IConvertedPokerHand convertedHand, bool showInactives, string heroName, string note)
        {
            var strToHtml = new StringBuilder();

            // Header
            strToHtml.AppendLine("<div align = \"left\">");

            // Table Header
            strToHtml.AppendLine(
                "\t\t<table cellspacing=\"0\" cellpadding=\"2\" width =\"700\" BORDER=4 name=\"tabActions\">");

            // HandInfo
            strToHtml.AppendLine("\t\t\t<tr align=\"center\">");
            strToHtml.AppendLine(string.Format("\t\t\t\t<td ><font size=\"2\">{0}</td>", convertedHand.DateAsString));
            strToHtml.AppendLine(string.Format("\t\t\t\t<td><font size=\"2\">{0}</td>", convertedHand.TimeAsString));
            strToHtml.AppendLine(
                string.Format("\t\t\t\t<td><font size=\"2\">{0}</td>", BoardToHtml(convertedHand.Board)));
            strToHtml.AppendLine("\t\t\t\t<td Colspan = \"5\"><font size=\"2\">");
            strToHtml.AppendLine(
                string.Format(
                    "\t\t\t\t\t Table: {0} #{1}" + " Players: {2} Blinds:{3}/{4}", 
                    convertedHand.TableName, 
                    convertedHand.GameId, 
                    convertedHand.TotalPlayers, 
                    convertedHand.SB, 
                    convertedHand.BB));
            strToHtml.AppendLine("\t\t\t\t</td>");
            strToHtml.AppendLine("\t\t\t</tr>");

            // Column Header Row
            strToHtml.AppendLine("\t\t\t<tr valign=\"top\" halign=\"left\">");
            foreach (string colHeader in new[] { "Pos", "Name", "Cards", "M", "Preflop", "Flop", "Turn", "River" })
            {
                strToHtml.AppendLine(string.Format("\t\t\t\t<th><font size=\"2\">{0}</th>", colHeader));
            }

            strToHtml.AppendLine("\t\t\t</tr>");

            foreach (IConvertedPokerPlayer convertedPlayer in convertedHand)
            {
                // Was player active?
                if (showInactives ||
                    (convertedPlayer[Streets.PreFlop] != null &&
                     convertedPlayer[Streets.PreFlop][0].What != ActionTypes.F))
                {
                    strToHtml.AppendLine(BuildFrom(convertedPlayer, convertedPlayer.Name.Equals(heroName)));
                }
            }

            // Add a row that contains the note if it was added
            if (!string.IsNullOrEmpty(note))
            {
                // Show linebreaks in the html representation
                var strHtmlNote = new StringBuilder();
               
                string[] linesInNote = note.Split('\n');
               
                foreach (string lineInNote in linesInNote)
                {
                    strHtmlNote.AppendLine(lineInNote + "<BR>");
                }

                // Add the row
                strToHtml.AppendLine("\t\t\t<tr align=\"left\">");
                strToHtml.AppendLine(string.Format("\t\t\t\t<th><font size=\"2\">{0}</th>", "Notes: "));
                strToHtml.AppendLine("\t\t\t\t<td Colspan = \"7\"><font size=\"2\">");
                strToHtml.AppendLine("\t\t\t\t" + strHtmlNote + " </td>");
                strToHtml.AppendLine("\t\t\t</tr>");
            }

            // Table Footer
            strToHtml.AppendLine("\t\t</table>");

            // Footer
            strToHtml.AppendLine("</div><div align = \"left\"></div><BR>");

            return strToHtml.ToString();
        }

        /// <summary>
        /// Creates a string containg Html Code for an Html Tables
        /// representing each Poker Hand in the selected Hand Histories
        /// </summary>
        /// <returns>Html Code for Table of the hands</returns>
        public static string BuildFrom(IEnumerable<IHandHistoryViewModel> selectedHandHistories, bool showInactives, string heroName)
        {
            string html = string.Empty;
            foreach (var selectedHandHistory in selectedHandHistories)
            {
                html = html + BuildFrom(
                                  selectedHandHistory.Hand,
                                  showInactives,
                                  heroName,
                                  selectedHandHistory.Note);
            }
           
            return html;
        }

        public static string SuitToHtml(char suit)
        {
            string imagePath = Resources.Url_Suits_Path;

            if (string.IsNullOrEmpty(imagePath))
            {
                imagePath = @"http://www.flopturnriver.com/phpBB2/images/smiles/";
            }

            string imageSrcStart = "<img src=\"" + imagePath;
            string imageSrcEnd = "\" alt=\"" + suit + "\" width=\"13\" height=\"13\"/>";

            var htmlSuit = new SortedList<char, string>
                {
                    { 'd', Resources.Url_Suits_Diamond }, 
                    { 's', Resources.Url_Suits_Spade }, 
                    { 'h', Resources.Url_Suits_Heart }, 
                    { 'c', Resources.Url_Suits_Club }
                };

            return imageSrcStart + htmlSuit[suit] + imageSrcEnd;
        }

        #endregion

        #region Methods

        static string BoardToHtml(string theBoard)
        {
            string htmlString = string.Empty;
            for (int i = 0; i < theBoard.Length; i += 3)
            {
                htmlString = htmlString + theBoard[i] + SuitToHtml(theBoard[i + 1]);
            }

            return htmlString;
        }

        static string HoleCardsToHtml(string theHolecards)
        {
            if (theHolecards.Contains("?"))
            {
                return string.Empty;
            }
            else
            {
                try
                {
                    if (theHolecards.Length < 5)
                    {
                        throw new FormatException("Length of HoleCards: " + theHolecards);
                    }

                    return theHolecards[0] + SuitToHtml(theHolecards[1]) + " " + theHolecards[3] +
                           SuitToHtml(theHolecards[4]);
                }
                catch (FormatException excep)
                {
                    Log.Error("Unhandled", excep);
                    return string.Empty;
                }
            }
        }

        #endregion
    }
}