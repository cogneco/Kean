// 
//  ListDictionary.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2012 Simon Mika
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
using Kean.Extension;
using Kean.Collection.Extension;

namespace Kean.Collection.Wrap
{
	public class ListDictionary<TKey, TValue> :
		IDictionary<TKey, TValue>
	{
		IList<KeyValue<TKey, TValue>> data;

		public ListDictionary(IList<KeyValue<TKey, TValue>> data)
		{
			this.data = data;
		}
		public ListDictionary(int capacity) :
			this(new Collection.List<KeyValue<TKey, TValue>>(capacity))
		{ }
		public ListDictionary() :
			this(10)
		{ }
		public ListDictionary(params KeyValue<TKey, TValue>[] items) :
			this(new Collection.List<KeyValue<TKey, TValue>>(items))
		{ }

		#region IDictionary<TKey,TValue> Members
		public TValue this[TKey key]
		{
			get { return this.data.Find(entry => entry.Key.Equals(key)).Value; }
			set
			{
				this.Remove(key);
				this.data.Add(KeyValue.Create(key, value));
			}
		}
		public bool Contains(TKey key)
		{
			return this.data.Exists(entry => entry.Key.Equals(key));
		}
		public bool Remove(TKey key)
		{
			return this.data.Remove(entry => entry.Key.Equals(key));
		}
		#endregion

		#region IEnumerable<KeyValue<TKey,TValue>> Members
		public System.Collections.Generic.IEnumerator<KeyValue<TKey, TValue>> GetEnumerator()
		{
			return this.data.GetEnumerator();
		}
		#endregion
		#region IEnumerable Members
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
		#endregion

		#region IEquatable<IDictionary<TKey,TValue>> Members
		public bool Equals(IDictionary<TKey, TValue> other)
		{
			bool result = other.NotNull();
			int count = this.data.Count;
			if (result)
				foreach (KeyValue<TKey, TValue> pair in other)
					if (!(result = count-- == 0 || this[pair.Key].Equals(pair.Value)))
						break;

			return result && count == 0;
		}
		#endregion
		#region Object Overrides
		public override bool Equals(object other)
		{
			return (other is IDictionary<TKey, TValue>) && this.Equals(other as IDictionary<TKey, TValue>);
		}
		public override string ToString()
		{
			return this.data.ToString();
		}
		public override int GetHashCode()
		{
			return this.data.GetHashCode();
		}
		#endregion
		#region Comparison Operators
		public static bool operator ==(ListDictionary<TKey, TValue> left, IDictionary<TKey, TValue> right)
		{
			return left.SameOrEquals(right);
		}
		public static bool operator !=(ListDictionary<TKey, TValue> left, IDictionary<TKey, TValue> right)
		{
			return !(left == right);
		}
		#endregion
	}
}
