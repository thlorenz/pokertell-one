//Date: 4/22/2009

namespace PokerTell.Infrastructure.Tests
{
    using System.Drawing;

    using NUnit.Framework;

    using UnitTests;

    /// <summary>
    /// Description of Settings_Test.
    /// </summary>
    [TestFixture]
    public class ThatSettings : TestWithLog
    {
        #region Public Methods

        [Test]
        public void CanPersistBool()
        {
            bool boolSave = true;
            Settings.Save("boolSaved", boolSave);

            bool boolPersist;
            Settings.Persist("boolSaved", out boolPersist);

            Assert.That(boolPersist, Is.EqualTo(boolSave), "Persisting Value with known Key");

            Settings.Persist("UNKNOWN_KEY", out boolPersist);
            Assert.That(boolPersist, Is.EqualTo(false), "Persisting Value with unknown Key");

            bool Default = true;
            Settings.Persist("UNKNOWN_KEY", out boolPersist, Default);
            Assert.That(boolPersist, Is.EqualTo(Default), "Persisting Value with unknown Key and given default value");
        }

        [Test]
        public void CanPersistColorFromArgb()
        {
            Color colorSave = Color.FromArgb(1, 2, 3, 4);
            Settings.Save("colorSaved", colorSave);

            Color colorPersist;
            Settings.Persist("colorSaved", out colorPersist);

            Assert.That(colorPersist, Is.EqualTo(colorSave), "Persisting Value with known Key");

            Settings.Persist("UNKNOWN_KEY", out colorPersist);
            Assert.That(colorPersist, Is.EqualTo(Color.Empty), "Persisting Value with unknown Key");

            Color Default = Color.AliceBlue;
            Settings.Persist("UNKNOWN_KEY", out colorPersist, Default);
            Assert.That(colorPersist, Is.EqualTo(Default), "Persisting Value with unknown Key and given default value");
        }

        [Test]
        public void CanPersistDouble()
        {
            double doubleSave = 56.00576;
            Settings.Save("doubleSaved", doubleSave);

            double doublePersist;
            Settings.Persist("doubleSaved", out doublePersist);

            Assert.That(doublePersist, Is.EqualTo(doubleSave), "Persisting Value with known Key");

            Settings.Persist("UNKNOWN_KEY", out doublePersist);
            Assert.That(doublePersist, Is.EqualTo(double.MinValue), "Perstising Value with unknown Key");

            double Default = 2.3333;
            Settings.Persist("UNKNOWN_KEY", out doublePersist, Default);
            Assert.That(doublePersist, Is.EqualTo(Default), "Persisting Value with unknown Key and given default value");
        }

        [Test]
        public void CanPersistInt()
        {
            int intSave = 5;
            Settings.Save("intSaved", intSave);

            int intPersist;
            Settings.Persist("intSaved", out intPersist);

            Assert.That(intPersist, Is.EqualTo(intSave), "Persisting Value with known Key");

            Settings.Persist("UNKNOWN_KEY", out intPersist);
            Assert.That(intPersist, Is.EqualTo(int.MinValue), "Persisting Value with unknown Key");

            int Default = 2;
            Settings.Persist("UNKNOWN_KEY", out intPersist, Default);
            Assert.That(intPersist, Is.EqualTo(Default), "Persisting Value with unknown Key and given default value");
        }

        [Test]
        public void CanPersistKnownColor()
        {
            Color colorSave = Color.AliceBlue;
            Settings.Save("colorSaved", colorSave);

            Color colorPersist;
            Settings.Persist("colorSaved", out colorPersist);

            Assert.That(colorPersist, Is.EqualTo(colorSave), "Persisting Value with known Key");

            Settings.Persist("UNKNOWN_KEY", out colorPersist);
            Assert.That(colorPersist, Is.EqualTo(Color.Empty), "Persisting Value with unknown Key");

            Color Default = Color.AliceBlue;
            Settings.Persist("UNKNOWN_KEY", out colorPersist, Default);
            Assert.That(colorPersist, Is.EqualTo(Default), "Persisting Value with unknown Key and given default value");
        }

        [Test]
        public void CanPersistPoint()
        {
            var pointSave = new Point(-10,  100);
            Settings.Save("pointSaved", pointSave);

            Point pointPersist;
            Settings.Persist("pointSaved", out pointPersist);

            Assert.That(pointPersist, Is.EqualTo(pointSave),  "Persisting Value with known Key");

            Settings.Persist("UNKNOWN_KEY",   out pointPersist);
            Assert.That(pointPersist,  Is.EqualTo(Point.Empty),  "Persisting Value with unknown Key");

            var Default = new Point(9,  9);
            Settings.Persist("UNKNOWN_KEY",   out pointPersist, Default);
            Assert.That(
                pointPersist,  Is.EqualTo(Default),  "Persisting Value with unknown Key and given default value");
        }

        [Test]
        public void CanPersistSize()
        {
            var sizeSave = new Size(10, 100);
            Settings.Save("SizeSaved", sizeSave);

            Size sizePersist;
            Settings.Persist("SizeSaved", out sizePersist);

            Assert.That(sizePersist, Is.EqualTo(sizeSave), "Persising Value with known Key");

            Settings.Persist("UNKNOWN_KEY", out sizePersist);
            Assert.That(sizePersist, Is.EqualTo(Size.Empty), "Persisting Value with unknown Key");

            var Default = new Size(10, 10);
            Settings.Persist("UNKNOWN_KEY", out sizePersist, Default);
            Assert.That(sizePersist, Is.EqualTo(Default), "Persisting Value with unknown Key and given default value");
        }

        [Test]
        public void CanPersistString()
        {
            string stringSave = "Test String";
            Settings.Save("stringSaved", stringSave);

            string stringPersist;
            Settings.Persist("stringSaved", out stringPersist);

            Assert.That(stringPersist, Is.EqualTo(stringSave), "Persisting Value with known Key");

            Settings.Persist("UNKNOWN_KEY", out stringPersist);
            Assert.That(stringPersist, Is.EqualTo(string.Empty), "Persisting Value with unknown Key");

            string Default = "Text";
            Settings.Persist("UNKNOWN_KEY", out stringPersist, Default);
            Assert.That(stringPersist, Is.EqualTo(Default), "Persisting Value with unknown Key and given default value");
        }

        #endregion
    }
}