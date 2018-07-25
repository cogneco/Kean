// Copyright (C) 2017	Simon Mika <simon@mika.se>
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
	public static class TaskExtension
	{
		public static void Forget<T>(this Tasks.Task<T> me)
		{
			me.ContinueWith(
				t => { ; },
				Tasks.TaskContinuationOptions.OnlyOnFaulted);
		}
		public async static Tasks.Task<TResult> Then<T, TResult>(this Tasks.Task<T> me, Func<T, TResult> map) {
			return map(await me);
		}
		public async static Tasks.Task<TResult> Then<T, TResult>(this Tasks.Task<T> me, Func<T, TResult> map, Func<Exception, TResult> onError) {
			TResult result;
			try
			{
				result = await me.Then(map);
			}
			catch (System.Exception e)
			{
				result = onError(e);
			}
			return result;
		}
		public async static Tasks.Task Then<T>(this Tasks.Task<T> me, Action<T> action) {
			action.Call(await me);
		}
		public async static Tasks.Task Then<T>(this Tasks.Task<T> me, Action<T> action, Action<Exception> onError) {
			try
			{
				await me.Then(action);
			}
			catch (System.Exception e)
			{
				onError.Call(e);
			}
		}
		public static T WaitFor<T>(this Tasks.Task<T> me, T @default = default(T)) {
			T result = @default;
			me.Then(r => result = r).Wait();
			return result;
		}
		public static T WaitFor<T>(this Tasks.Task<T> me, TimeSpan timeout, T @default = default(T)) {
			T result = @default;
			me.Then(r => result = r).Wait(timeout);
			return result;
		}
		public static async Tasks.Task<Tuple<T1, T2>> And<T1, T2>(this Tasks.Task<T1> me, Tasks.Task<T2> other) => Tuple.Create(await me, await other);
		public static async Tasks.Task<Tuple<T1, T2, T3>> And<T1, T2, T3>(this Tasks.Task<Tuple<T1, T2>> me, Tasks.Task<T3> other) {
			var t = await me;
			return Tuple.Create(t.Item1, t.Item2, await other);
		}
		public static async Tasks.Task<Tuple<T1, T2, T3, T4>> And<T1, T2, T3, T4>(this Tasks.Task<Tuple<T1, T2, T3>> me, Tasks.Task<T4> other) {
			var t = await me;
			return Tuple.Create(t.Item1, t.Item2, t.Item3, await other);
		}
		public static async Tasks.Task<Tuple<T1, T2, T3, T4, T5>> And<T1, T2, T3, T4, T5>(this Tasks.Task<Tuple<T1, T2, T3, T4>> me, Tasks.Task<T5> other) {
			var t = await me;
			return Tuple.Create(t.Item1, t.Item2, t.Item3, t.Item4, await other);
		}
		public static async Tasks.Task<Generic.IEnumerable<T>> WhenAll<T>(this Generic.IEnumerable<Tasks.Task<T>> me) {
			return await Tasks.Task.WhenAll(me);
		}
		static async Tasks.Task<T> FilterNext<T>(this Generic.IEnumerator<Tasks.Task<T>> me, Func<T, bool> predicate)
		{
				var result = await me.Next();
				if (!predicate(result))
					result = await me.FilterNext(predicate);
				return result;
		}
		public static Generic.IEnumerator<Tasks.Task<T>> FilterTasks<T>(this Generic.IEnumerator<Tasks.Task<T>> me, Func<T, bool> predicate) {
			return Enumerator.Create(() => me.FilterNext(predicate), me.Reset, me.Dispose);
		}
		public static async Tasks.Task<Generic.IEnumerator<T>> WhenAll<T>(this Generic.IEnumerator<Tasks.Task<T>> me) {
			var current = await me.Next();
			return current.IsNull() ? Enumerator.Create(current) : (await me.WhenAll()).Prepend(current);
		}
	}
}
