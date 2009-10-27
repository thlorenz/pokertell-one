namespace NUnit.Tests
{
    using Framework;

    public class ThatNUnit
    {
        [Test]
        public void IsNotEqualTo_ClientsNotEqual_Passes()
        {
            var client1 = new DerrivedClient();
            var client2 = new DerrivedClient();

            client1.Name = "player1";
            client1.SomeGenericProperty = client1.Name;
            client2.Name = "player2";
            client2.SomeGenericProperty = client2.Name;

            Assert.That(client1.Equals(client2), Is.False);
            Assert.That(client1, Is.Not.EqualTo(client2));
        }

        [Test]
        public void IsNotEqualTo_ClientsAreEqual_DoesntPassAnymore_ProblemFixed_ReturnsFalse()
        {
            var client1 = new DerrivedClient();
            var client2 = new DerrivedClient();

            client1.Name = "player1";
            client1.SomeGenericProperty = client1.Name;
            client2.Name = client1.Name;
            client2.SomeGenericProperty = client1.Name;

            Assert.That(client1.Equals(client2), Is.True);
           
            Assert.That(client1, Is.Not.Not.EqualTo(client2));
        }
    }

    public class DerrivedClient : Client<string>
    {
    }

    public class Client<T>
    {
        public string Name { get; set; }

        public T SomeGenericProperty { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            // This caused the problem:
            // if (obj.GetType() != typeof(Client<T>))
            // When changed like below, problem is solved
            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((Client<T>)obj);
        }

        public bool Equals(Client<T> other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return Equals(other.Name, Name) && Equals(other.SomeGenericProperty, SomeGenericProperty);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Name != null ? Name.GetHashCode() : 0) * 397) ^ SomeGenericProperty.GetHashCode();
            }
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}", Name, SomeGenericProperty);
        }
    }
}