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

namespace Kean.Collection
{
	public class Dictionary<TKey, TValue> :
		IDictionary<TKey, TValue>
	{
		IDictionary<TKey, TValue> data;
		public Dictionary(IDictionary<TKey, TValue> data)
		{
			this.data = data;
		}
		public Dictionary() :
			this(new Hash.Dictionary<TKey, TValue>())
		{ }
		public Dictionary(int capacity) :
			this(new Hash.Dictionary<TKey, TValue>(capacity))
		{ }
		public Dictionary(System.Collections.Generic.IEnumerable<KeyValue<TKey, TValue>> data) :
			this()
		{
			foreach (KeyValue<TKey, TValue> item in data)
				if (item.Key.NotNull() && item.Value.NotNull())
					this[item.Key] = item.Value;
		}
		public Dictionary(params KeyValue<TKey, TValue>[] data) :
			this((System.Collections.Generic.IEnumerable<KeyValue<TKey, TValue>>)data)
		{ }

		#region IDictionary[TKey,TValue] implementation
		public bool Contains(TKey key)
		{
			return this.data.Contains(key);
		}

		public bool Remove(TKey key)
		{
			return this.data.Remove(key);
		}

		public TValue this[TKey key]
		{
			get { return this.data[key]; }
			set { this.data[key] = value; }
		}
		#endregion

		#region IEquatable[IDictionary[TKey,TValue]] implementation
		public bool Equals(IDictionary<TKey, TValue> other)
		{
			return this.data.Equals(other);
		}
		#endregion

		#region IEnumerable[KeyValue[TKey,TValue]] implementation
		System.Collections.Generic.IEnumerator<KeyValue<TKey, TValue>> System.Collections.Generic.IEnumerable<KeyValue<TKey, TValue>>.GetEnumerator()
		{
			return (this.data as System.Collections.Generic.IEnumerable<KeyValue<TKey, TValue>>).GetEnumerator();
		}
		#endregion

		#region IEnumerable implementation
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return (this.data as System.Collections.IEnumerable).GetEnumerator();
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