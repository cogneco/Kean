// Copyright (C) 2011, 2017  Simon Mika <simon@mika.se>
//
// This file is part of Kean.
//
// Kean is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Kean is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with Kean.  If not, see <http://www.gnu.org/licenses/>.
//

using System;
using Generic = System.Collections.Generic;
using Kean.Extension;

namespace Kean
{
	public static class Enumerable
	{
		public static Generic.IEnumerable<T> Empty<T>() { return System.Linq.Enumerable.Empty<T>(); }
		public static Generic.IEnumerable<T> Create<T>(Func<Generic.IEnumerator<T>> getEnumerator)
		{
			return new Enumerable<T>(getEnumerator);
		}
		public static Generic.IEnumerable<T> Create<T>(Generic.IEnumerable<T> enumerator)
		{
			return Enumerable.Create(enumerator.GetEnumerator);
		}

		public static Generic.IEnumerable<T> Create<T>(params T[] items)
		{
			return Enumerable.Create((Generic.IEnumerable<T>)items);
		}
	}
	public class Enumerable<T> :
		Generic.IEnumerable<T>
	{
		readonly Func<Generic.IEnumerator<T>> getEnumerator;
		#region Constructors
		internal Enumerable(Func<Generic.IEnumerator<T>> getEnumerator)
		{
			this.getEnumerator = getEnumerator;
		}
		#endregion
		#region IEnumerable<T> Members
		public Generic.IEnumerator<T> GetEnumerator()
		{
			return this.getEnumerator.NotNull() ? this.getEnumerator() : Enumerator.Empty<T>();
		}
		#endregion
		#region IEnumerable Members
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
		#endregion
	}
}
