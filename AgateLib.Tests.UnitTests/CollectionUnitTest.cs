using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AgateLib.UnitTests
{
	public static class CollectionUnitTest
	{
		/// <summary>
		/// Performs unit test on the dictionary contract.
		/// </summary>
		/// <typeparam name="TKey"></typeparam>
		/// <typeparam name="TValue"></typeparam>
		/// <param name="dictionary">The dictionary object.</param>
		/// <param name="keyAllocator">A function which returns a new key each time it is called. The key should not be equal
		/// to any previous value it returned.</param>
		/// <param name="valueAllocator">A function which returns a new value each time it is called. This value should not be equal
		/// to any previous value it returned.</param>
		/// <param name="equalityComparison">Function which compares values for equality.</param>
		public static void DictionaryContract<TKey, TValue>(IDictionary<TKey, TValue> dictionary,
			Func<TKey> keyAllocator,
			Func<TValue> valueAllocator,
			Func<TValue, TValue, bool> equalityComparison = null)
		{
			equalityComparison = equalityComparison ??
			           ((value1, value2) => ReferenceEquals(value1, value2));

			dictionary.Clear();
			Assert.AreEqual(0, dictionary.Count, "Clear method did not remove all items.");

			var firstKey = keyAllocator();
			dictionary.Add(firstKey, valueAllocator());
			Assert.AreEqual(1, dictionary.Count, "Add method did not add an item.");

			var v1 = valueAllocator();
			var v2 = valueAllocator();

			Assert.IsFalse(equalityComparison(v1, v2));

			var k1 = keyAllocator();
			var k2 = keyAllocator();

			dictionary[k1] = v1;
			dictionary[k2] = v2;

			Assert.IsTrue(equalityComparison(v1, dictionary[k1]), "Item added to dictionary was not equal to value received.");
			Assert.AreEqual(3, dictionary.Count, "Count mismatch.");

			dictionary.Remove(k1);
			Assert.IsTrue(dictionary.ContainsKey(firstKey), "First key was accidentally removed.");
			Assert.IsFalse(dictionary.ContainsKey(k1), "Remove did not remove the item.");
			Assert.IsTrue(dictionary.ContainsKey(k2), "Last key was accidentally removed.");

			TValue testValue;
			var success = dictionary.TryGetValue(k2, out testValue);

			Assert.IsTrue(success, "TryGetValue failed to get a value known to be in the dictionary.");
			Assert.IsTrue(equalityComparison(testValue, v2), "Value retrieved from TryGetValue did not match value entered.");

			var k3 = keyAllocator();
			var v3 = valueAllocator();
			dictionary.Add(new KeyValuePair<TKey, TValue>(k3, v3));
			Assert.IsTrue(equalityComparison(v3, dictionary[k3]));
			Assert.IsTrue(dictionary.Contains(new KeyValuePair<TKey, TValue>(k3, v3)));

			var array = new KeyValuePair<TKey, TValue>[dictionary.Count];
			dictionary.CopyTo(array, 0);

			CollectionAssert.AreEquivalent(dictionary.Keys.ToArray(), array.Select(x => x.Key).ToArray());
		}
	}
}
