// 
//  ReadOnlyDictionary.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2012-2013 Simon Mika
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

namespace Kean.Collection.Abstract
{
	public abstract class ReadOnlyDictionary<TKey, TValue> :
		IReadOnlyDictionary<TKey, TValue>
	{
		protected ReadOnlyDictionary()
		{ }
		#region IReadOnlyDictionary<TKey,TValue> Members
		public abstract TValue this[TKey key] { get; }
		public abstract bool Contains(TKey key);
		public IReadOnlyDictionary<TKey, TValue> Update(TKey key, TValue value)
		{
			Collection.Dictionary<TKey, TValue> result = new Collection.Dictionary<TKey, TValue>(this);
			result[key] = value;
			return new Collection.ReadOnlyDictionary<TKey, TValue>(result);
		}
		public IReadOnlyDictionary<TKey, TValue> Remove(TKey key)
		{
			Collection.Dictionary<TKey, TValue> result = new Collection.Dictionary<TKey, TValue>(this);
			return result.Remove(key) ? (IReadOnlyDictionary<TKey, TValue>)new Collection.ReadOnlyDictionary<TKey, TValue>(result) : this;
		}
		#endregion
		#region IEnumerable<KeyValue<TKey,TValue>> Members
		public abstract System.Collections.Generic.IEnumerator<KeyValue<TKey, TValue>> GetEnumerator();
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
			return this.Equals(other as ReadOnlyDictionary<TKey, TValue>);
		}
		#endregion
		#region Object Overrides
		public override bool Equals(object other)
		{
			return (other is IDictionary<TKey, TValue>) && this.Equals(other as IDictionary<TKey, TValue>);
		}
		public bool Equals(ReadOnlyDictionary<TKey, TValue> other)
		{
			bool result;
			if (result = other.NotNull())
			{
				int count = other.Fold((item, c) => c + 1, 0);
				foreach (KeyValue<TKey, TValue> pair in this)
					if (!(result &= other[pair.Key].SameOrEquals(pair.Value) && count-- > 0))
						break;
			}
			return result;
		}
		public override int GetHashCode()
		{
			int result = 0;
			foreach (KeyValue<TKey, TValue> pair in this)
				result ^= pair.Hash();
			return result;
		}
		#endregion
		#region Comparison Operators
		public static bool operator ==(ReadOnlyDictionary<TKey, TValue> left, ReadOnlyDictionary<TKey, TValue> right)
		{
			return left.Same(right) || left.NotNull() && right.NotNull() && left.Equals(right);
		}
		public static bool operator !=(ReadOnlyDictionary<TKey, TValue> left, ReadOnlyDictionary<TKey, TValue> right)
		{
			return !(left == right);
		}
		#endregion
	}
}