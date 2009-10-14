using System;
using System.Collections;
using System.Collections.Generic;

namespace Tools.GenericUtilities
{
    ///
    /// Downloaded from http://noocyte.wordpress.com/2008/02/18/double-key-dictionary/
    /// 
    public class DoubleKeyDictionary<TK1, TK2, TV> : IEnumerable<DoubleKeyPairValue<TK1, TK2, TV>>,
                                                IEquatable<DoubleKeyDictionary<TK1, TK2, TV>>
    {
        public DoubleKeyDictionary()
        {
            OuterDictionary = new Dictionary<TK1, Dictionary<TK2, TV>>();
        }

        private Dictionary<TK1, Dictionary<TK2, TV>> OuterDictionary { get; set; }

        public TV this[TK1 index1, TK2 index2]
        {
            get { return OuterDictionary[index1][index2]; }
            set { Add(index1, index2, value); }
        }

        #region IEnumerable<DoubleKeyPairValue<TK1,TK2,TV>> Members

        public IEnumerator<DoubleKeyPairValue<TK1, TK2, TV>> GetEnumerator()
        {
            foreach (var outer in OuterDictionary)
            {
                foreach (var inner in outer.Value)
                {
                    yield return new DoubleKeyPairValue<TK1, TK2, TV>(outer.Key, inner.Key, inner.Value);
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region IEquatable<DoubleKeyDictionary<TK1,TK2,TV>> Members

        public bool Equals(DoubleKeyDictionary<TK1, TK2, TV> other)
        {
            if (OuterDictionary.Keys.Count != other.OuterDictionary.Keys.Count)
            {
                return false;
            }

            bool isEqual = true;

            foreach (var innerItems in OuterDictionary)
            {
                if (!other.OuterDictionary.ContainsKey(innerItems.Key))
                {
                    isEqual = false;
                }

                if (!isEqual)
                {
                    break;
                }

                // here we can be sure that the key is in both lists, 
                // but we need to check the contents of the inner dictionary
                Dictionary<TK2, TV> otherInnerDictionary = other.OuterDictionary[innerItems.Key];
                foreach (var innerValue in innerItems.Value)
                {
                    if (!otherInnerDictionary.ContainsValue(innerValue.Value))
                    {
                        isEqual = false;
                    }
                    if (!otherInnerDictionary.ContainsKey(innerValue.Key))
                    {
                        isEqual = false;
                    }
                }

                if (!isEqual)
                {
                    break;
                }
            }

            return isEqual;
        }

        #endregion

        public void Add(TK1 key1, TK2 key2, TV value)
        {
            if (OuterDictionary.ContainsKey(key1))
            {
                if (OuterDictionary[key1].ContainsKey(key2))
                {
                    OuterDictionary[key1][key2] = value;
                }
                else
                {
                    Dictionary<TK2, TV> innerDictionary = OuterDictionary[key1];
                    innerDictionary.Add(key2, value);
                    OuterDictionary[key1] = innerDictionary;
                }
            }
            else
            {
                var innerDictionary = new Dictionary<TK2, TV>();
                innerDictionary[key2] = value;
                OuterDictionary.Add(key1, innerDictionary);
            }
        }

        public bool ContainsKey(TK1 key1, TK2 key2)
        {
            bool bReturn = false;
            if (OuterDictionary.ContainsKey(key1))
            {
                if (OuterDictionary[key1].ContainsKey(key2))
                {
                    bReturn = true;
                }
            }
            return bReturn;
        }
    }

    public class DoubleKeyPairValue<K, T, V>
    {
        public DoubleKeyPairValue(K key1, T key2, V value)
        {
            Key1 = key1;
            Key2 = key2;
            Value = value;
        }

        public K Key1 { get; set; }

        public T Key2 { get; set; }

        public V Value { get; set; }

        public override string ToString()
        {
            return Key1.ToString() + " - " + Key2.ToString() + " - " + Value.ToString();
        }
    }
}