//Date: 4/22/2009
namespace PokerTell.User.Tests
{
    using System;
    using System.Drawing;

    using Infrastructure.Interfaces;

    using NUnit.Framework;

    using UnitTests;
    using UnitTests.Fakes;
    using UnitTests.Tools;

    /// <summary>
    /// Description of Settings_Test.
    /// </summary>
    [TestFixture]
    public class SettingsTests : TestWithLog
    {
        ISettings _settings;

        [SetUp]
        public void _Init()
        {
            _settings = new Settings(new MockUserConfiguration());    
        }

        [Test]
        public void CanRetrieveBool()
        {
            const bool boolSave = true;
            _settings.Set("boolSaved", boolSave);

            bool boolPersist = _settings.RetrieveBool("boolSaved");

            Assert.That(boolPersist, Is.EqualTo(boolSave), "Persisting Value with known Key");

            boolPersist = _settings.RetrieveBool("UNKNOWN_KEY");
            Assert.That(boolPersist, Is.EqualTo(false), "Persisting Value with unknown Key");

            const bool defaultValue = true;
            boolPersist = _settings.RetrieveBool("UNKNOWN_KEY", defaultValue);
            Assert.That(boolPersist, Is.EqualTo(defaultValue), "Persisting Value with unknown Key and given default value");
        }

        [Test]
        public void CanRetrieveColorFromArgb()
        {
            Color colorSave = Color.FromArgb(1, 2, 3, 4);
            _settings.Set("colorSaved", colorSave);

            Color colorPersist = _settings.RetrieveColor("colorSaved");

            Assert.That(colorPersist, Is.EqualTo(colorSave), "Persisting Value with known Key");

            colorPersist = _settings.RetrieveColor("UNKNOWN_KEY");
            Assert.That(colorPersist, Is.EqualTo(Color.Empty), "Persisting Value with unknown Key");

            Color defaultValue = Color.AliceBlue;
            colorPersist = _settings.RetrieveColor("UNKNOWN_KEY", defaultValue);
            Assert.That(colorPersist, Is.EqualTo(defaultValue), "Persisting Value with unknown Key and given default value");
        }

        [Test]
        public void CanRetrieveDouble()
        {
            const double doubleSave = 56.00576;
            _settings.Set("doubleSaved", doubleSave);

            double doublePersist = _settings.RetrieveDouble("doubleSaved");

            Assert.That(doublePersist, Is.EqualTo(doubleSave), "Persisting Value with known Key");

            doublePersist = _settings.RetrieveDouble("UNKNOWN_KEY");
            Assert.That(doublePersist, Is.EqualTo(double.MinValue), "Perstising Value with unknown Key");

            const double defaultValue = 2.3333;
            doublePersist = _settings.RetrieveDouble("UNKNOWN_KEY", defaultValue);
            Assert.That(doublePersist, Is.EqualTo(defaultValue), "Persisting Value with unknown Key and given default value");
        }

        [Test]
        public void CanRetrieveInt()
        {
            const int intSave = 5;
            _settings.Set("intSaved", intSave);

            int intPersist = _settings.RetrieveInt("intSaved");

            Assert.That(intPersist, Is.EqualTo(intSave), "Persisting Value with known Key");

            intPersist = _settings.RetrieveInt("UNKNOWN_KEY");
            Assert.That(intPersist, Is.EqualTo(int.MinValue), "Persisting Value with unknown Key");

            const int defaultValue = 2;
            intPersist = _settings.RetrieveInt("UNKNOWN_KEY", defaultValue);
            Assert.That(intPersist, Is.EqualTo(defaultValue), "Persisting Value with unknown Key and given default value");
        }

        [Test]
        public void CanRetrieveKnownColor()
        {
            Color colorSave = Color.AliceBlue;
            _settings.Set("colorSaved", colorSave);

            Color colorPersist = _settings.RetrieveColor("colorSaved");

            Assert.That(colorPersist, Is.EqualTo(colorSave), "Persisting Value with known Key");

            colorPersist = _settings.RetrieveColor("UNKNOWN_KEY");
            Assert.That(colorPersist, Is.EqualTo(Color.Empty), "Persisting Value with unknown Key");

            Color defaultValue = Color.AliceBlue;
            colorPersist = _settings.RetrieveColor("UNKNOWN_KEY", defaultValue);
            Assert.That(colorPersist, Is.EqualTo(defaultValue), "Persisting Value with unknown Key and given default value");
        }

        [Test]
        public void CanRetrievePoint()
        {
            var pointSave = new Point(-10,  100);
            _settings.Set("pointSaved", pointSave);

            Point pointPersist = _settings.RetrievePoint("pointSaved");

            Assert.That(pointPersist, Is.EqualTo(pointSave),  "Persisting Value with known Key");

            pointPersist = _settings.RetrievePoint("UNKNOWN_KEY");
            Assert.That(pointPersist,  Is.EqualTo(Point.Empty),  "Persisting Value with unknown Key");

            var defaultValue = new Point(9, 9);
            pointPersist = _settings.RetrievePoint("UNKNOWN_KEY", defaultValue);
            Assert.That(
                pointPersist, Is.EqualTo(defaultValue),  "Persisting Value with unknown Key and given default value");
        }

        [Test]
        public void CanRetrieveSize()
        {
            var sizeSave = new Size(10, 100);
            _settings.Set("SizeSaved", sizeSave);

            Size sizePersist = _settings.RetrieveSize("SizeSaved");

            Assert.That(sizePersist, Is.EqualTo(sizeSave), "Persising Value with known Key");

            sizePersist = _settings.RetrieveSize("UNKNOWN_KEY");
            Assert.That(sizePersist, Is.EqualTo(Size.Empty), "Persisting Value with unknown Key");

            var defaultValue = new Size(10, 10);
            sizePersist = _settings.RetrieveSize("UNKNOWN_KEY", defaultValue);
            Assert.That(sizePersist, Is.EqualTo(defaultValue), "Persisting Value with unknown Key and given default value");
        }

        [Test]
        public void CanRetrieveRectangle()
        {
            const string key = "RectangleSaved";
            var rectangleSaved = new Rectangle(1, 2, 3, 4);
            
            _settings.Set(key, rectangleSaved);

            var rectanglePersisted = _settings.RetrieveRectangle(key);
            rectanglePersisted.ShouldBeEqualTo(rectangleSaved);

            var defaultValue = new Rectangle(2, 5, 4, 6);
            rectanglePersisted = _settings.RetrieveRectangle("UnknownKey", defaultValue);
            rectanglePersisted.ShouldBeEqualTo(defaultValue);
        }

        [Test]
        public void CanRetrieveString()
        {
            const string stringSave = "Test String";
            _settings.Set("stringSaved", stringSave);

            string stringPersist = _settings.RetrieveString("stringSaved");

            Assert.That(stringPersist, Is.EqualTo(stringSave), "Persisting Value with known Key");

            stringPersist = _settings.RetrieveString("UNKNOWN_KEY");
            Assert.That(stringPersist, Is.EqualTo(string.Empty), "Persisting Value with unknown Key");

            const string defaultValue = "Text";
            stringPersist = _settings.RetrieveString("UNKNOWN_KEY", defaultValue);
            Assert.That(stringPersist, Is.EqualTo(defaultValue), "Persisting Value with unknown Key and given default value");
        }

    }
}