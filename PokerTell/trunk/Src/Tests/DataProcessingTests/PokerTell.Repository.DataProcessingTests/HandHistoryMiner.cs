namespace PokerTell.Repository.DataProcessingTests
{
    using System;
    using System.IO;
    using System.Text.RegularExpressions;

    public class HandHistoryMiner
    {
        public void ExtractHandsFromPhpFolder()
        {
            const Sites site = Sites.FullTilt;
            const int maxPlayers = 2;
            const bool tournament = true;
            const string gameType = "No ";


            const string handHistoryStartPattern = @"<pre>";
            const string handHistoryEndPattern = @"</pre>";

            const string directory = @"C:\SD\PokerTell\TestData\HandHistories\PHP";

            const string needsToContain = "SUMMARY";

            string targetFile = string.Format(
                @"C:\SD\PokerTell\TestData\HandHistories\{0}\Batches\{1} {2} Limit {3}-max.txt",
                site,
                tournament ? "Tournament" : "CashGame",
                gameType,
                maxPlayers);

            var dir = new DirectoryInfo(directory);
            FileInfo[] allPhpFiles = dir.GetFiles("*.php");

            Console.WriteLine("Extracting from {0} files.", allPhpFiles.Length);

            string allHandHistories = string.Empty;
            foreach (FileInfo file in allPhpFiles)
            {
                string allText = new StreamReader(file.OpenRead()).ReadToEnd();

                Match matchHistoryStart = Regex.Match(allText, handHistoryStartPattern, RegexOptions.IgnoreCase);
                Match matchHistoryEnd = Regex.Match(allText, handHistoryEndPattern, RegexOptions.IgnoreCase);

                if (matchHistoryStart.Success && matchHistoryEnd.Success)
                {
                    int startIndex = matchHistoryStart.Index + handHistoryStartPattern.Length;
                    int length = matchHistoryEnd.Index - startIndex;
                    var newHandHistory = allText.Substring(startIndex, length);
                    if (newHandHistory.Contains(needsToContain))
                    {
                        allHandHistories += newHandHistory + "\n\n\n\n";
                    }
                    else
                    {
                        Console.WriteLine("\nDisqualified (did not contain {0})\n<{1}>", needsToContain, newHandHistory);
                    }
                   
                }
            }

            File.AppendAllText(targetFile, allHandHistories);
        }

        enum Sites
        {
            PokerStars, FullTilt
        }
    }
}