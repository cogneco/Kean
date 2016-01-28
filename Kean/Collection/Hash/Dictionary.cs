//
//  Dictionary.cs
//
//  Author:
//       Simon Mika <smika@hx.se>
//
//  Copyright (c) 2011 Simon Mika
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
//
//  You should have received a copy of the GNU Lesser General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using Kean;
using Kean.Extension;
using Kean.Collection.Extension;

namespace Kean.Collection.Hash
{
	public class Dictionary<TKey, TValue> :
		IDictionary<TKey, TValue>
	{
		IList<KeyValue<TKey, TValue>>[] data;
		public TValue this[TKey key]
		{
			get
			{
				TValue result = default(TValue);
				IList<KeyValue<TKey, TValue>> list = this.data[this.Index(key)];
				if (list.NotNull() && list.Count > 0)
				{
					KeyValue<TKey, TValue> entry = list.Find(e => e.Key.Equals(key));
					if (entry.NotNull())
						result = entry.Value;
				}
				return result;
			}
			set
			{
				IList<KeyValue<TKey, TValue>> list = this.data[this.Index(key)];
				if (list.IsNull())
					this.data[this.Index(key)] = list = new Linked.List<KeyValue<TKey, TValue>>();
				int index = list.Index(entry => entry.Key.Equals(key));
				if (index >= 0)
					list[index] = KeyValue.Create(key, value);
				else
					list.Add(KeyValue.Create(key, value));
			}
		}
		public Dictionary() :
			this(20)
		{ }
		public Dictionary(int capacity)
		{
			this.data = new IList<KeyValue<TKey, TValue>>[capacity];
		}
		int Index(TKey key)
		{
			int divided = key.NotNull() ? key.GetHashCode() : 0;
			if (divided < 0)
				divided *= -1;
			return divided % this.data.Length;
		}
		public bool Contains(TKey key)
		{
			IList<KeyValue<TKey, TValue>> list = this.data[this.Index(key)];
			return list.NotNull() && list.Count > 0 && list.Find(entry => entry.Key.Equals(key)).Key.NotNull();
		}
		public bool Remove(TKey key)
		{
			IList<KeyValue<TKey, TValue>> list = this.data[this.Index(key)];
			return list.NotNull() && list.Remove(entry => entry.Key.Equals(key));
		}
		#region Object Overrides
		public override bool Equals(object other)
		{
			return other is IDictionary<TKey, TValue> && this.Equals(other as IDictionary<TKey, TValue>);
		}
		public override int GetHashCode()
		{
			int result = 0;
			foreach (KeyValue<TKey, TValue> pair in this)
				result ^= pair.Hash();
			return result;
		}
		#endregion
		#region IEnumerable Members
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
		#endregion
		#region IEnumerable<KeyValue<TKey,TValue>> Members
		public System.Collections.Generic.IEnumerator<KeyValue<TKey, TValue>> GetEnumerator()
		{
			foreach (IList<KeyValue<TKey, TValue>> list in this.data)
				if (list.NotNull())
					foreach (KeyValue<TKey, TValue> pair in list)
						yield return pair;
		}
		#endregion
		#region IEquatable<IDictionary<TKey,TValue>> Members
		public bool Equals(IDictionary<TKey, TValue> other)
		{
			bool result = other.NotNull();
			int count = this.data.Fold((list, c) => c + (list.NotNull() ? list.Count : 0), 0);
			if (result)
				foreach (KeyValue<TKey, TValue> pair in other)
					if (!(result = count-- == 0 || this[pair.Key].Equals(pair.Value)))
						break;

			return result && count == 0;
		}
		#endregion
		#region Comparison Operators
		public static bool operator ==(Dictionary<TKey, TValue> left, IDictionary<TKey, TValue> right)
		{
			return left.SameOrEquals(right);
		}
		public static bool operator !=(Dictionary<TKey, TValue> left, IDictionary<TKey, TValue> right)
		{
			return !(left == right);
		}
		#endregion
	}
}


