namespace PokerTell.PokerHandParsers.Tests.Patterns
{
    using System;
    using System.Text.RegularExpressions;

    using NUnit.Framework;

    public class RatioPatternTests
    {
        [Test]
        public void Match_TwoDigitNumber_ExtractsIt()
        {
            const int twoDigitNumber = 10;
            Match match =  Regex.Match(twoDigitNumber.ToString(), SharedPatterns.RatioPattern, RegexOptions.IgnoreCase);
            int extractedNumber = Convert.ToInt32(match.Groups["Ratio"].Value);
            Assert.That(extractedNumber, Is.EqualTo(twoDigitNumber));
        }

        [Test]
        public void Match_TwoDigitNumberWithDollarSign_ExtractsIt()
        {
            const int twoDigitNumber = 10;
            Match match = Regex.Match("$" + twoDigitNumber.ToString(), SharedPatterns.RatioPattern, RegexOptions.IgnoreCase);
            int extractedNumber = Convert.ToInt32(match.Groups["Ratio"].Value);
            Assert.That(extractedNumber, Is.EqualTo(twoDigitNumber));
        }

        [Test]
        public void Match_DoubleNumber_ExtractsIt()
        {
            const double doubleNumber = 1.5;
            Match match = Regex.Match(doubleNumber.ToString(), SharedPatterns.RatioPattern, RegexOptions.IgnoreCase);
            double extractedNumber = Convert.ToDouble(match.Groups["Ratio"].Value);
            Assert.That(extractedNumber, Is.EqualTo(doubleNumber));
        }

        [Test]
        public void Match_DoubleNumberWithDollarSign_ExtractsIt()
        {
            const double doubleNumber = 1.5;
            Match match = Regex.Match("$" + doubleNumber.ToString(), SharedPatterns.RatioPattern, RegexOptions.IgnoreCase);
            double extractedNumber = Convert.ToDouble(match.Groups["Ratio"].Value);
            Assert.That(extractedNumber, Is.EqualTo(doubleNumber));
        }

        [Test]
        public void Match_BigNumberCommaSeparated_ExtractsIt()
        {
            const int multiDigitNumber = 1001000;
            Match match = Regex.Match("1,001,000", SharedPatterns.RatioPattern, RegexOptions.IgnoreCase);
            int extractedNumber = Convert.ToInt32(match.Groups["Ratio"].Value.Replace(",",string.Empty));
            Assert.That(extractedNumber, Is.EqualTo(multiDigitNumber));
        }

        [Test]
        public void Match_BigDoubleNumberCommaSeparated_ExtractsIt()
        {
            const double multiDigitDoubleNumber = 1001000.23;
            Match match = Regex.Match("1,001,000.23", SharedPatterns.RatioPattern, RegexOptions.IgnoreCase);
            double extractedNumber = Convert.ToDouble(match.Groups["Ratio"].Value.Replace(",",string.Empty));
            Assert.That(extractedNumber, Is.EqualTo(multiDigitDoubleNumber));
        }
    }
}