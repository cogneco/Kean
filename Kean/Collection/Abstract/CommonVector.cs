// 
//  CommonVector.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2011-2012 Simon Mika
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
using Generic = System.Collections.Generic;

namespace Kean.Collection.Abstract
{
	public abstract class CommonVector<T> :
		System.Collections.Generic.IEnumerable<T>,
		System.IEquatable<IVector<T>>,
		System.IEquatable<IReadOnlyVector<T>>

	{
		public abstract int Count { get; }
		public CommonVector()
		{
		}
		protected abstract T Get(int index);
		#region IEnumerator<T>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return (this as System.Collections.Generic.IEnumerable<T>).GetEnumerator();
		}
		System.Collections.Generic.IEnumerator<T> System.Collections.Generic.IEnumerable<T>.GetEnumerator()
		{
			for (int i = 0; i < this.Count; i++)
				yield return this.Get(i);
		}
		#endregion
		#region Object override
		public override bool Equals(object other)
		{
			return other is IVector<T> ? this.Equals(other as IVector<T>) :	other is IReadOnlyVector<T> && this.Equals(other as IReadOnlyVector<T>);
		}
		public override int GetHashCode ()
		{
			int result = 0;
			foreach (T item in this as Generic.IEnumerable<T>)
				result = unchecked(result * 31 + item.Hash());
			return result;
		}
		#endregion
		#region IEquatable<IVector<T>>
		public bool Equals(IVector<T> other)
		{
			bool result = other.NotNull() && this.Count == other.Count;
			for (int i = 0; result && i < this.Count; i++)
				result = this.Get(i).Equals(other[i]);
			return result;
		}
		#endregion
		#region IEquatable<IReadOnlyVector<T>>
		public bool Equals(IReadOnlyVector<T> other)
		{
			bool result = other.NotNull() && this.Count == other.Count;
			for (int i = 0; result && i < this.Count; i++)
				result = this.Get(i).Equals(other[i]);
			return result;
		}
		#endregion
	}
}

