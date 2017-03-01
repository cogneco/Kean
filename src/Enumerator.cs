// Copyright (C) 2012, 2016  Simon Mika <simon@mika.se>
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
	public static class Enumerator
	{
		public static Generic.IEnumerator<T> Empty<T>() { return Enumerable.Empty<T>().GetEnumerator(); }
		public static Generic.IEnumerator<T> Create<T>(Func<T> next, Action reset = null, Action dispose = null)
		{
			return next.NotNull() ? new Enumerator<T>(next, reset, dispose) : Enumerator.Empty<T>();
		}
		public static Generic.IEnumerator<T> Create<T>(Generic.IEnumerator<T> enumerator)
		{
			return enumerator.NotNull() ? Enumerator.Create(() => enumerator.Next(), enumerator.Reset, enumerator.Dispose) : Enumerator.Empty<T>();
		}
		public static Generic.IEnumerator<T> Create<T>(params T[] items)
		{
			return Enumerator.Create(items as Generic.IEnumerator<T>);
		}
	}
	public class Enumerator<T> :
		Generic.IEnumerator<T>
	{
		readonly Func<T> next;
		readonly Action reset;
		readonly Action dispose;
		T current;
		public T Current { get { return this.current; } }
		object System.Collections.IEnumerator.Current { get { return this.current; } }
		internal Enumerator(Generic.IEnumerator<T> enumerator):
			this(() => enumerator.Next(), enumerator.Reset, enumerator.Dispose)
		{}
		internal Enumerator(Func<T> next, Action reset = null, Action dispose = null)
		{
			this.next = next;
			this.reset = reset;
			this.dispose = dispose;
		}
		public bool MoveNext()
		{
			return (this.current = this.next()).NotNull();
		}
		public void Reset()
		{
			this.reset.Call();
		}
		public void Dispose()
		{
			this.dispose.Call();
		}
	}
}
