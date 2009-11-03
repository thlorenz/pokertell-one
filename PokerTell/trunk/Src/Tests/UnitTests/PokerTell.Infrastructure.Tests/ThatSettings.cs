//Date: 4/22/2009

namespace PokerTell.Infrastructure.Tests
{
    using System;
    using System.Drawing;

    using Interfaces;

    using NUnit.Framework;

    using UnitTests;

    /// <summary>
    /// Description of Settings_Test.
    /// </summary>
    [TestFixture]
    public class ThatSettings : TestWithLog
    {
        ISettings _settings;

        #region Public Methods

        [SetUp]
        public void _Init()
        {
            _settings = new Settings(new MockUserConfiguration());    
        }

        [Test]
        public void CanPersistBool()
        {
            const bool boolSave = true;
            _settings.Save("boolSaved", boolSave);

            bool boolPersist;
            _settings.Persist("boolSaved", out boolPersist);

            Assert.That(boolPersist, Is.EqualTo(boolSave), "Persisting Value with known Key");

            _settings.Persist("UNKNOWN_KEY", out boolPersist);
            Assert.That(boolPersist, Is.EqualTo(false), "Persisting Value with unknown Key");

            const bool Default = true;
            _settings.Persist("UNKNOWN_KEY", out boolPersist, Default);
            Assert.That(boolPersist, Is.EqualTo(Default), "Persisting Value with unknown Key and given default value");
        }

        [Test]
        public void CanPersistColorFromArgb()
        {
            Color colorSave = Color.FromArgb(1, 2, 3, 4);
            _settings.Save("colorSaved", colorSave);

            Color colorPersist;
            _settings.Persist("colorSaved", out colorPersist);

            Assert.That(colorPersist, Is.EqualTo(colorSave), "Persisting Value with known Key");

            _settings.Persist("UNKNOWN_KEY", out colorPersist);
            Assert.That(colorPersist, Is.EqualTo(Color.Empty), "Persisting Value with unknown Key");

            Color Default = Color.AliceBlue;
            _settings.Persist("UNKNOWN_KEY", out colorPersist, Default);
            Assert.That(colorPersist, Is.EqualTo(Default), "Persisting Value with unknown Key and given default value");
        }

        [Test]
        public void CanPersistDouble()
        {
            double doubleSave = 56.00576;
            _settings.Save("doubleSaved", doubleSave);

            double doublePersist;
            _settings.Persist("doubleSaved", out doublePersist);

            Assert.That(doublePersist, Is.EqualTo(doubleSave), "Persisting Value with known Key");

            _settings.Persist("UNKNOWN_KEY", out doublePersist);
            Assert.That(doublePersist, Is.EqualTo(double.MinValue), "Perstising Value with unknown Key");

            double Default = 2.3333;
            _settings.Persist("UNKNOWN_KEY", out doublePersist, Default);
            Assert.That(doublePersist, Is.EqualTo(Default), "Persisting Value with unknown Key and given default value");
        }

        [Test]
        public void CanPersistInt()
        {
            int intSave = 5;
            _settings.Save("intSaved", intSave);

            int intPersist;
            _settings.Persist("intSaved", out intPersist);

            Assert.That(intPersist, Is.EqualTo(intSave), "Persisting Value with known Key");

            _settings.Persist("UNKNOWN_KEY", out intPersist);
            Assert.That(intPersist, Is.EqualTo(int.MinValue), "Persisting Value with unknown Key");

            int Default = 2;
            _settings.Persist("UNKNOWN_KEY", out intPersist, Default);
            Assert.That(intPersist, Is.EqualTo(Default), "Persisting Value with unknown Key and given default value");
        }

        [Test]
        public void CanPersistKnownColor()
        {
            Color colorSave = Color.AliceBlue;
            _settings.Save("colorSaved", colorSave);

            Color colorPersist;
            _settings.Persist("colorSaved", out colorPersist);

            Assert.That(colorPersist, Is.EqualTo(colorSave), "Persisting Value with known Key");

            _settings.Persist("UNKNOWN_KEY", out colorPersist);
            Assert.That(colorPersist, Is.EqualTo(Color.Empty), "Persisting Value with unknown Key");

            Color Default = Color.AliceBlue;
            _settings.Persist("UNKNOWN_KEY", out colorPersist, Default);
            Assert.That(colorPersist, Is.EqualTo(Default), "Persisting Value with unknown Key and given default value");
        }

        [Test]
        public void CanPersistPoint()
        {
            var pointSave = new Point(-10,  100);
            _settings.Save("pointSaved", pointSave);

            Point pointPersist;
            _settings.Persist("pointSaved", out pointPersist);

            Assert.That(pointPersist, Is.EqualTo(pointSave),  "Persisting Value with known Key");

            _settings.Persist("UNKNOWN_KEY",   out pointPersist);
            Assert.That(pointPersist,  Is.EqualTo(Point.Empty),  "Persisting Value with unknown Key");

            var Default = new Point(9,  9);
            _settings.Persist("UNKNOWN_KEY",   out pointPersist, Default);
            Assert.That(
                pointPersist,  Is.EqualTo(Default),  "Persisting Value with unknown Key and given default value");
        }

        [Test]
        public void CanPersistSize()
        {
            var sizeSave = new Size(10, 100);
            _settings.Save("SizeSaved", sizeSave);

            Size sizePersist;
            _settings.Persist("SizeSaved", out sizePersist);

            Assert.That(sizePersist, Is.EqualTo(sizeSave), "Persising Value with known Key");

            _settings.Persist("UNKNOWN_KEY", out sizePersist);
            Assert.That(sizePersist, Is.EqualTo(Size.Empty), "Persisting Value with unknown Key");

            var Default = new Size(10, 10);
            _settings.Persist("UNKNOWN_KEY", out sizePersist, Default);
            Assert.That(sizePersist, Is.EqualTo(Default), "Persisting Value with unknown Key and given default value");
        }

        [Test]
        public void CanPersistString()
        {
            string stringSave = "Test String";
            _settings.Save("stringSaved", stringSave);

            string stringPersist;
            _settings.Persist("stringSaved", out stringPersist);

            Assert.That(stringPersist, Is.EqualTo(stringSave), "Persisting Value with known Key");

            _settings.Persist("UNKNOWN_KEY", out stringPersist);
            Assert.That(stringPersist, Is.EqualTo(string.Empty), "Persisting Value with unknown Key");

            string Default = "Text";
            _settings.Persist("UNKNOWN_KEY", out stringPersist, Default);
            Assert.That(stringPersist, Is.EqualTo(Default), "Persisting Value with unknown Key and given default value");
        }

        #endregion
    }
}