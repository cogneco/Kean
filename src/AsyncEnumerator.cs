// Copyright (C) 2012, 2016, 2017  Simon Mika <simon@mika.se>
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
using Tasks = System.Threading.Tasks;
using Kean.Extension;
using System.Collections;

namespace Kean
{
	public static class AsyncEnumerator
	{
		public static IAsyncEnumerator<T> Empty<T>() => AsyncEnumerator.Create<T>(Enumerator.Empty<T>());
		public static IAsyncEnumerator<T> Create<T>(Func<Tasks.Task<T>> next, Action dispose = null)
		{
			return next.NotNull() ? new AsyncEnumerator<T>(next, dispose) : AsyncEnumerator.Empty<T>();
		}
		public static IAsyncEnumerator<T> Create<T>(Func<Tasks.Task<T>> next, IDisposable disposable = null)
		{
			return AsyncEnumerator.Create(next, disposable is IDisposable ? (Action)((IDisposable)disposable).Dispose : null);
		}
		public static IAsyncEnumerator<T> Create<T>(Generic.IEnumerator<T> enumerator)
		{
			return enumerator.NotNull() ? AsyncEnumerator.Create(() => Tasks.Task.FromResult(enumerator.Next()), enumerator) : AsyncEnumerator.Empty<T>();
		}
		public static IAsyncEnumerator<T> Create<T>(Generic.IEnumerable<T> enumerable) => AsyncEnumerator.Create(enumerable?.GetEnumerator());
		public static IAsyncEnumerator<T> Create<T>(params T[] items) => AsyncEnumerator.Create(items as Generic.IEnumerable<T>);
	}
	public class AsyncEnumerator<T> :
		IAsyncEnumerator<T>,
		Generic.IEnumerator<T>,
		IDisposable
	{
		readonly Func<Tasks.Task<T>> next;
		readonly Action dispose;
		T current;
		public T Current => this.current;
		object IEnumerator.Current => this.current;
		internal AsyncEnumerator(Func<Tasks.Task<T>> next, Action dispose = null)
		{
			this.next = next;
			this.dispose = dispose;
		}
		public async Tasks.Task<bool> MoveNext()
		{
			return (this.current = await this.next()).NotNull();
		}
		bool IEnumerator.MoveNext() => this.MoveNext().WaitFor();
		void IEnumerator.Reset() => throw new NotImplementedException();
		public void Dispose() => this.dispose.Call();
	}
}
