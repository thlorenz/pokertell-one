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

        public static void ShouldBeEqualTo<T>(this IEnumerable<T> me, IEnumerable<T> other)
        {
            Assert.That(me.ToArray().EqualsArray(other.ToArray(), true), Is.True);
        }

        public static T ShouldBeEqualTo<T>(this T me, T other)
        {
            Assert.That(me, Is.EqualTo(other));
            return me;
        }

        public static void ShouldBeFalse(this bool? me)
        {
            Assert.That(me, Is.False);
        }

        public static void ShouldBeFalse(this bool me)
        {
            Assert.That(me, Is.False);
        }

        public static IComparable<T> ShouldBeGreaterThan<T>(this IComparable<T> me, IComparable<T> other)
        {
            Assert.That(me, Is.GreaterThan(other));
            return me;
        }

        public static IComparable<T> ShouldNotBeGreaterThan<T>(this IComparable<T> me, IComparable<T> other)
        {
            Assert.That(me, Is.Not.GreaterThan(other));
            return me;
        }

        public static T ShouldNotBeEqualTo<T>(this T me, T other)
        {
            Assert.That(me, Is.Not.EqualTo(other));
            return me;
        }

        public static T ShouldNotBeNull<T>(this T me) where T : class
        {
            Assert.That(me, Is.Not.Null);
            return me;
        }

        public static T ShouldNotBeSameAs<T>(this T me, T other) where T : class
        {
            Assert.That(me, Is.Not.SameAs(other));
            return me;
        }

        public static T ShouldBeNull<T>(this T me) where T : class
        {
            Assert.That(me, Is.Null);
            return me;
        }

        public static T ShouldBeSameAs<T>(this T me, T other) where T : class
        {
            Assert.That(me, Is.SameAs(other));
            return me;
        }

        public static void ShouldBeTrue(this bool? me)
        {
            Assert.That(me, Is.True);
        }

        public static void ShouldBeTrue(this bool me)
        {
            Assert.That(me, Is.True);
        }

        public static void ShouldThrow<TException>(this TestDelegate codeBlockThatThrows) where TException : Exception
        {
            Assert.Throws<TException>(codeBlockThatThrows);
        }

        public static IList<T> ShouldContain<T>(this IList<T> me, T item)
        {
            Assert.That(me, Has.Some.EqualTo(item));
            return me;
        }

        public static IList<T> ShouldNotContain<T>(this IList<T> me, T item)
        {
            Assert.That(me, Has.None.EqualTo(item));
            return me;
        }

        public static IEnumerable<T> ShouldContain<T>(this IEnumerable<T> me, T item)
        {
            me.ToList().ShouldContain(item);
            return me;
        }

        public static IEnumerable<T> ShouldNotContain<T>(this IEnumerable<T> me, T item)
        {
            me.ToList().ShouldNotContain(item);
            return me;
        }

        public static IEnumerable<T> ShouldContain<T>(this IEnumerable<T> me, Func<T, bool> expected)
        {
            me.Single(expected).ShouldNotBeEqualTo(default(T));
            return me;
        }

        public static IEnumerable<T> ShouldNotContain<T>(this IEnumerable<T> me, Func<T, bool> expected)
        {
            me.Single(expected).ShouldBeEqualTo(default(T));
            return me;
        }

        public static IEnumerable<T> ShouldBeEmpty<T>(this IEnumerable<T> me)
        {
            Assert.That(me, Is.Empty, "Contained " + me.Count() + " elements.");
            return me;
        }

        public static IEnumerable<T> ShouldNotBeEmpty<T>(this IEnumerable<T> me)
        {
            Assert.That(me, Is.Not.Empty);
            return me;
        }

        public static IEnumerable<T> ShouldHaveCount<T>(this IEnumerable<T> me, int expectedCount)
        {
            me.Count().ShouldBeEqualTo(expectedCount);
            return me;
        }

        public static ICollection ShouldBeEmpty(this ICollection collection)
        {
            Assert.IsEmpty(collection);
            return collection;
        }

        public static string ShouldBeEmpty(this string aString)
        {
            Assert.IsEmpty(aString);
            return aString;
        }

        public static string ShouldNotBeEmpty(this string aString)
        {
            Assert.IsNotEmpty(aString);
            return aString;
        }

        public static T ShouldBeInstanceOf<T>(this T me, Type expectedType)
        {
            Assert.That(me, Is.InstanceOf(expectedType));
            return me;
        }

        public static T ShouldNotBeInstanceOf<T>(this T me, Type expectedType)
        {
            Assert.That(me, Is.Not.InstanceOf(expectedType));
            return me;
        }

        public static T ShouldBe<T, TSampleInstance>(this T me, TSampleInstance sampleInstance) where TSampleInstance : class
        {
            Assert.That(me, Is.InstanceOf(sampleInstance.GetType()));
            return me;
        }

        public static T ShouldNotBe<T, TSampleInstance>(this T me, TSampleInstance sampleInstance) where TSampleInstance : class
        {
            Assert.That(me, Is.Not.InstanceOf(sampleInstance.GetType()));
            return me;
        }

        
        #endregion
    }
}