namespace PokerTell.UnitTests.Tools
{
    using System;
    using System.Collections;
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

        public static T IsEqualTo<T>(this T me, T other)
        {
            Assert.That(me, Is.EqualTo(other));
            return me;
        }

        public static void IsFalse(this bool? me)
        {
            Assert.That(me, Is.False);
        }

        public static void IsFalse(this bool me)
        {
            Assert.That(me, Is.False);
        }

        public static IComparable<T> IsGreaterThan<T>(this IComparable<T> me, IComparable<T> other)
        {
            Assert.That(me, Is.GreaterThan(other));
            return me;
        }

        public static IComparable<T> IsNotGreaterThan<T>(this IComparable<T> me, IComparable<T> other)
        {
            Assert.That(me, Is.Not.GreaterThan(other));
            return me;
        }

        public static T IsNotEqualTo<T>(this T me, T other)
        {
            Assert.That(me, Is.Not.EqualTo(other));
            return me;
        }

        public static T IsNotNull<T>(this T me) where T : class
        {
            Assert.That(me, Is.Not.Null);
            return me;
        }

        public static T IsNotSameAs<T>(this T me, T other) where T : class
        {
            Assert.That(me, Is.Not.SameAs(other));
            return me;
        }

        public static T IsNull<T>(this T me) where T : class
        {
            Assert.That(me, Is.Null);
            return me;
        }

        public static T IsSameAs<T>(this T me, T other) where T : class
        {
            Assert.That(me, Is.SameAs(other));
            return me;
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

        public static IList<T> DoesContain<T>(this IList<T> me, T item)
        {
            Assert.That(me, Has.Some.EqualTo(item));
            return me;
        }

        public static IList<T> DoesNotContain<T>(this IList<T> me, T item)
        {
            Assert.That(me, Has.None.EqualTo(item));
            return me;
        }

        public static IEnumerable<T> DoesContain<T>(this IEnumerable<T> me, T item)
        {
            me.ToList().DoesContain(item);
            return me;
        }

        public static IEnumerable<T> DoesNotContain<T>(this IEnumerable<T> me, T item)
        {
            me.ToList().DoesNotContain(item);
            return me;
        }

        public static IEnumerable<T> DoesContain<T>(this IEnumerable<T> me, Func<T, bool> expected)
        {
            me.Single(expected).IsNotEqualTo(default(T));
            return me;
        }

        public static IEnumerable<T> DoesNotContain<T>(this IEnumerable<T> me, Func<T, bool> expected)
        {
            me.Single(expected).IsEqualTo(default(T));
            return me;
        }

        public static IEnumerable<T> IsEmpty<T>(this IEnumerable<T> me)
        {
            Assert.That(me, Is.Empty, "Contained " + me.Count() + " elements.");
            return me;
        }

        public static IEnumerable<T> IsNotEmpty<T>(this IEnumerable<T> me)
        {
            Assert.That(me, Is.Not.Empty);
            return me;
        }

        public static IEnumerable<T> HasCount<T>(this IEnumerable<T> me, int expectedCount)
        {
            me.Count().IsEqualTo(expectedCount);
            return me;
        }

        public static void IsEmpty(this ICollection collection)
        {
            Assert.IsEmpty(collection);
        }

        public static void IsEmpty(this string aString)
        {
            Assert.IsEmpty(aString);
        }

        public static void IsNotEmpty(this string aString)
        {
            Assert.IsNotEmpty(aString);
        }



        #endregion
    }
}