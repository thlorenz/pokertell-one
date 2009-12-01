namespace PokerTell.UnitTests.Tools
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using NUnit.Framework;

    using global::Tools.Extensions;

    public static class UnitTestingExtensions
    {
        #region Public Methods

        public static void AreEqualTo<T>(this IEnumerable<T> me, IEnumerable<T> other)
        {
            Assert.That(me.ToArray().EqualsArray(other.ToArray(), true), Is.True);
        }

        public static void IsEqualTo<T>(this T me, T other)
        {
            Assert.That(me, Is.EqualTo(other));
        }

        public static void IsFalse(this bool? me)
        {
            Assert.That(me, Is.False);
        }

        public static void IsFalse(this bool me)
        {
            Assert.That(me, Is.False);
        }

        public static void IsGreaterThan<T>(this T me, T other) where T : struct
        {
            Assert.That(me, Is.GreaterThan(other));
        }

        public static void IsNotEqualTo<T>(this T me, T other)
        {
            Assert.That(me, Is.Not.EqualTo(other));
        }

        public static void IsNotNull<T>(this T me) where T : class
        {
            Assert.That(me, Is.Not.Null);
        }

        public static void IsNotSameAs<T>(this T me, T other) where T : class
        {
            Assert.That(me, Is.Not.SameAs(other));
        }

        public static void IsNull<T>(this T me) where T : class
        {
            Assert.That(me, Is.Null);
        }

        public static void IsSameAs<T>(this T me, T other) where T : class
        {
            Assert.That(me, Is.SameAs(other));
        }

        public static void IsTrue(this bool? me)
        {
            Assert.That(me, Is.True);
        }

        public static void IsTrue(this bool me)
        {
            Assert.That(me, Is.True);
        }

        public static void Throws<T>(this Action codeBlock) where T : Exception
        {
            Assert.Throws(typeof(T), codeBlock.Invoke);
        }

        #endregion
    }
}