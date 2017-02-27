// Copyright (C) 2017  Simon Mika <simon@mika.se>
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

namespace Kean.Extension
{
	public static class TaskExtension
	{
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
		public static Generic.IEnumerator<Tasks.Task<T>> Filter<T>(this Generic.IEnumerator<Tasks.Task<T>> me, Func<T, bool> predicate) {
			return Enumerator.Create(() => me.FilterNext(predicate), me.Reset, me.Dispose);
		}
	}
}
