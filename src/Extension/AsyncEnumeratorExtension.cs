// Copyright (C) 2012, 2017	Simon Mika <simon@mika.se>
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
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.	See the
// GNU Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with Kean.	If not, see <http://www.gnu.org/licenses/>.
//

using System;
using Generic = System.Collections.Generic;
using Tasks = System.Threading.Tasks;

namespace Kean.Extension
{
	public static class AsyncEnumeratorExtension
	{
		public static async Tasks.Task<T> Next<T>(this IAsyncEnumerator<T> me, T @default = default(T))
		{
			return me.NotNull() && await me.MoveNext() ? me.Current : @default;
		}
		public static async Tasks.Task<T> First<T>(this IAsyncEnumerator<T> me)
		{
			T result;
			if (me.NotNull())
			{
				result = await me.MoveNext() ? me.Current : default(T);
				(me as IDisposable)?.Dispose();
			}
			else
				result = default(T);
			return result;
		}
		public static async Tasks.Task Apply<T>(this IAsyncEnumerator<T> me, Action<T> function)
		{
			while (await me.MoveNext())
				function(me.Current);
		}
		public static IAsyncEnumerator<S> Map<T, S>(this IAsyncEnumerator<T> me, Func<T, S> function)
		{
			return AsyncEnumerator.Create(async () => await me.MoveNext() ? function(me.Current) : default(S), me as IDisposable);
		}
		public static IAsyncEnumerator<T> Filter<T>(this IAsyncEnumerator<T> me, Func<T, bool> predicate)
		{
			async Tasks.Task<T> next() => !await me.MoveNext() ? default(T) :	predicate(me.Current) ? me.Current : await next();
			return AsyncEnumerator.Create(next, me as IDisposable);
		}
		public static async Tasks.Task<int> Index<T>(this IAsyncEnumerator<T> me, Func<T, bool> function)
		{
			int result = -1;
			int i = 0;
			while (await me.MoveNext())
				if (function(me.Current))
				{
					result = i;
					break;
				}
				else
					i++;
			return result;
		}
		public static Tasks.Task<int> Index<T>(this IAsyncEnumerator<T> me, T needle)
		{
			return me.Index(element => element.SameOrEquals(needle));
		}
		public static Tasks.Task<int> Index<T>(this IAsyncEnumerator<T> me, params T[] needles)
			where T : IEquatable<T>
		{
			return me.Index(element => needles.Contains(element));
		}
		public static async Tasks.Task<bool> Contains<T>(this IAsyncEnumerator<T> me, T needle)
			where T : IEquatable<T>
		{
			bool result = false;
			while (await me.MoveNext())
				if (needle.SameOrEquals(me.Current))
				{
					result = true;
					break;
				}
			return result;
		}
		public static async Tasks.Task<bool> Contains<T>(this IAsyncEnumerator<T> me, params T[] needles)
			where T : IEquatable<T>
		{
			bool result = false;
			while (await me.MoveNext())
				if (needles.Contains(me.Current))
				{
					result = true;
					break;
				}
			return result;
		}
		public static async Tasks.Task<T> Find<T>(this IAsyncEnumerator<T> me, Func<T, bool> function)
		{
			T result = default(T);
			while (await me.MoveNext())
				if (function(me.Current))
				{
					result = me.Current;
					break;
				}
			return result;
		}
		public static async Tasks.Task<S> Find<T, S>(this IAsyncEnumerator<T> me, Func<T, S> function)
		{
			S result = default(S);
			while (await me.MoveNext())
				if ((result = function(me.Current)) != null)
					break;
			return result;
		}
		public static async Tasks.Task<bool> Exists<T>(this IAsyncEnumerator<T> me, Func<T, bool> function)
		{
			bool result = false;
			while (await me.MoveNext())
				if (function(me.Current))
				{
					result = true;
					break;
				}
			return result;
		}
		public static async Tasks.Task<bool> All<T>(this IAsyncEnumerator<T> me, Func<T, bool> function)
		{
			bool result = true;
			while (await me.MoveNext())
				if (!function(me.Current))
				{
					result = false;
					break;
				}
			return result;
		}
		public static async Tasks.Task<S> Fold<T, S>(this IAsyncEnumerator<T> me, Func<T, S, S> function, S initial)
		{
			while (await me.MoveNext())
				initial = function(me.Current, initial);
			return initial;
		}
		#region While
		public static IAsyncEnumerator<T> While<T>(this IAsyncEnumerator<T> me, Func<T, bool> predicate) =>
			AsyncEnumerator.Create(async () => await me.MoveNext() && predicate(me.Current) ? me.Current : default(T), me as IDisposable);
		#endregion
		#region Skip
		/// <summary>
		/// Skip the next <paramref name="count"/> elements in <paramref name="me"/>.
		/// </summary>
		/// <param name="me">Enumerator to skip in.</param>
		/// <param name="count">Number of elements to skip.</param>
		/// <typeparam name="T">Any type.</typeparam>
		public static IAsyncEnumerator<T> Skip<T>(this IAsyncEnumerator<T> me, int count)
		{
			async Tasks.Task<T> next() => !await me.MoveNext() ?  default(T) :	unchecked(count-- > 0) ? await next() : me.Current;
			return AsyncEnumerator.Create(next, me as IDisposable);
		}
		/// <summary>
		/// Skip past the first occurance of separator.
		/// </summary>
		/// <param name="me">Enumerator to skip on.</param>
		/// <param name="separator">Separator to skip past. Shall not contain null.</param>
		/// <typeparam name="T">Any type implementing <c>IEquatable</c>.</typeparam>
		public static IAsyncEnumerator<T> Skip<T>(this IAsyncEnumerator<T> me, params T[] separator)
			where T : IEquatable<T>
		{
			int position = 0;
			async Tasks.Task<T> next()
			{
				while (await me.MoveNext())
				{
					if (!me.Current.Equals(separator[position++]))
						position = 0;
					else if (separator.Length == position)
						break;
				}
				return me.Current;
			}
			return AsyncEnumerator.Create(next, me as IDisposable);
		}
		#endregion
		#region Read
		/// <summary>
		/// Return new enumerator containing the next <paramref name="count"/> elements in <paramref name="me"/>.
		/// </summary>
		/// <param name="me">Enumerator to read from.</param>
		/// <param name="count">Number of elements read.</param>
		/// <typeparam name="T">Any type.</typeparam>
		public static IAsyncEnumerator<T> Read<T>(this IAsyncEnumerator<T> me, int count)
		{
			async Tasks.Task<T> next() => await me.MoveNext() && unchecked(count-- > 0) ?  me.Current : default(T);
			return AsyncEnumerator.Create(next, me as IDisposable);
		}
		#endregion
		public static async Tasks.Task<int> Count<T>(this IAsyncEnumerator<T> me)
		{
			int result = 0;
			while (await me.MoveNext())
				result++;
			return result;
		}
		public static async Tasks.Task<string> Join(this IAsyncEnumerator<string> me, string separator)
		{
			System.Text.StringBuilder result = new System.Text.StringBuilder();
			if (await me.MoveNext())
			{
				result.Append(me.Current);
				while (await me.MoveNext())
					result.Append(separator).Append(me.Current);
			}
			return result.ToString();
		}
		public static Tasks.Task<T[]> ToArray<T>(this IAsyncEnumerator<T> me)
		{
			async Tasks.Task<T[]> toArray(int count)
			{
				T[] result;
				if (await me.MoveNext())
				{
					var head = me.Current;
					result = await toArray(count + 1);
					result[count] = head;
				}
				else
					result = new T[count];
				return result;
			}
			return me.NotNull() ? toArray(0) : null;
		}
		#region Prepend, Append
		public static IAsyncEnumerator<T> Prepend<T>(this IAsyncEnumerator<T> me, params T[] other)
		{
			return me.Prepend(((Generic.IEnumerable<T>)other).GetEnumerator());
		}
		public static IAsyncEnumerator<T> Prepend<T>(this IAsyncEnumerator<T> me, Generic.IEnumerator<T> other)
		{
			return me.Prepend(AsyncEnumerator.Create(other));
		}
		public static IAsyncEnumerator<T> Prepend<T>(this Generic.IEnumerator<T> me, IAsyncEnumerator<T> other)
		{
			return AsyncEnumerator.Create(me).Prepend(other);
		}
		public static IAsyncEnumerator<T> Prepend<T>(this IAsyncEnumerator<T> me, IAsyncEnumerator<T> other)
		{
			return AsyncEnumerator.Create(
				async () =>
				await me.MoveNext() ? me.Current :
				await other.MoveNext() ? other.Current :
				default(T),
				() => {
					(me as IDisposable)?.Dispose();
					(other as IDisposable)?.Dispose();
			});
		}
		public static IAsyncEnumerator<T> Append<T>(this IAsyncEnumerator<T> me, params T[] other)
		{
			return me.Append(((Generic.IEnumerable<T>)other).GetEnumerator());
		}
		public static IAsyncEnumerator<T> Append<T>(this IAsyncEnumerator<T> me, Generic.IEnumerator<T> other)
		{
			return me.Append(AsyncEnumerator.Create(other));
		}
		public static IAsyncEnumerator<T> Append<T>(this Generic.IEnumerator<T> me, IAsyncEnumerator<T> other)
		{
			return AsyncEnumerator.Create(me).Append(other);
		}
		public static IAsyncEnumerator<T> Append<T>(this IAsyncEnumerator<T> me,IAsyncEnumerator<T> other)
		{
			return AsyncEnumerator.Create(
				async () =>
				await other.MoveNext() ? other.Current :
				await me.MoveNext() ? me.Current :
				default(T),
				() => {
					(other as IDisposable)?.Dispose();
					(me as IDisposable)?.Dispose();
			});
		}
		#endregion
	}
}
