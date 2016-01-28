//
//  Set.cs
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
	public class Set<T> :
		ISet<T>
		where T : System.IEquatable<T>
	{
		IList<T>[] data;
		public Set() :
			this(20)
		{ }
		public Set(int capacity)
		{
			this.data = new IList<T>[capacity];
		}
		int Index(T value)
		{
			int divided = value.GetHashCode();
			if (divided < 0)
				divided *= -1;
			return divided % this.data.Length;
		}
		public int Count
		{
			get
			{
				int result = 0;
				foreach (IList<T> list in this.data)
					if (list.NotNull())
						result += list.Count;
				return result;
			}
		}
		public bool Add(T value)
		{
			IList<T> list = this.data[this.Index(value)];
			if (list.IsNull())
				this.data[this.Index(value)] = list = new Linked.List<T>();
			int index = list.Index(entry => entry.Equals(value));
			if (index >= 0)
				list[index] = value;
			else
				list.Add(value);
			return index == -1;
		}
		public bool Contains(T value)
		{
			IList<T> list = this.data[this.Index(value)];
			return list.NotNull() && list.Count > 0 && list.Find(entry => entry.Equals(value)).NotNull();
		}
		public bool Remove(T value)
		{
			IList<T> list = this.data[this.Index(value)];
			return list.NotNull() && list.Remove(entry => entry.Equals(value));
		}
		#region Object Overrides
		public override bool Equals(object other)
		{
			return other is ISet<T> && this.Equals(other as ISet<T>);
		}
		public override int GetHashCode()
		{
			int result = 0;
			foreach (T value in this)
				result ^= value.Hash();
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
		public System.Collections.Generic.IEnumerator<T> GetEnumerator()
		{
			foreach (IList<T> list in this.data)
				if (list.NotNull())
					foreach (T value in list)
						yield return value;
		}
		#endregion
		#region IEquatable<ISet<T>> Members
		public bool Equals(ISet<T> other)
		{
			bool result = other.NotNull();
			int count = this.data.Fold((list, c) => c + (list.NotNull() ? list.Count : 0), 0);
			if (result)
				foreach (T value in other)
					if (!(result = count-- == 0 || this.Contains(value)))
						break;
			return result && count == 0;
		}
		#endregion
	}
}


