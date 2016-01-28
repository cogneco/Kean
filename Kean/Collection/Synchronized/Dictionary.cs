//
//  Dictionary.cs
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

namespace Kean.Collection.Synchronized
{
	public class Dictionary<TKey, TValue> :
		Kean.Synchronized,
		IDictionary<TKey, TValue>
        where TKey : IEquatable<TKey>
	{
		IDictionary<TKey, TValue> data;

		#region Constructors
		public Dictionary() :
		    this(new Collection.Dictionary<TKey, TValue>())
		{
		}
		public Dictionary(IDictionary<TKey, TValue> data) :
			this(data, new object())
		{
		}
		public Dictionary(IDictionary<TKey, TValue> data, object @lock) :
			base(@lock)
		{
			this.data = data;
		}
		#endregion

		#region IDictionary<TKey,TValue> Members
		public TValue this [TKey key]
		{
			get
			{
				lock (this.Lock)
					return this.data[key];
			}
			set
			{
				lock (this.Lock)
					this.data[key] = value;
			}
		}
		public bool Contains(TKey key)
		{
			lock (this.Lock)
				return this.data.Contains(key);
			;
		}
		public bool Remove(TKey key)
		{
			lock (this.Lock)
				return this.data.Remove(key);
			;
		}
		#endregion

		#region IEnumerable<KeyValue<TKey,TValue>> Members
		public System.Collections.Generic.IEnumerator<KeyValue<TKey, TValue>> GetEnumerator()
		{
			lock (this.Lock)
				foreach (KeyValue<TKey, TValue> item in this.data)
					yield return item;
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
			lock (this.Lock)
				return this.data.Equals(other);
			;
		}
		#endregion

		#region Object Overrides
		public override bool Equals(object other)
		{
			lock (this.Lock)
				return this.data.Equals(other);
			;
		}
		public override int GetHashCode()
		{
			lock (this.Lock)
				return base.GetHashCode();
		}
		public override string ToString()
		{
			lock (this.Lock)
				return base.ToString();
		}
		#endregion

	}
}
