// 
//  VectorExtension.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2010 Simon Mika
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
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.using System;

using System;
using Kean.Extension;

namespace Kean.Collection.Extension
{
	public static class VectorExtension
	{
		public static IVector<T> Reverse<T>(this IVector<T> me)
		{
			int half = me.Count / 2;
			for (int i = 0; i < half; i++)
			{
				T t = me[i];
				me[i] = me[me.Count - i - 1];
				me[me.Count - i - 1] = t;
			}
			return me;
		}
		public static IVector<T> Apply<T>(this IVector<T> me, Action<T> function)
		{
			foreach (T element in me)
				function(element);
			return me;
		}
		public static IVector<T> Modify<T>(this IVector<T> me, Func<T, T> function)
		{
			for (int i = 0; i < me.Count; i++)
				me[i] = function(me[i]);
			return me;
		}
		public static void Map<T, S>(this IVector<T> me, IVector<S> output, Func<T, S> function)
		{
			int minimumLength = (me.Count > output.Count) ? output.Count : me.Count;
			for (int i = 0; i < minimumLength; i++)
				output[i] = function(me[i]);
		}
		public static IVector<S> Map<T, S>(this IVector<T> me, Func<T, S> function)
		{
			IVector<S> result = new Vector<S>(me.Count);
			for (int i = 0; i < me.Count; i++)
				result[i] = function(me[i]);
			return result;
		}
		public static int Index<T>(this IVector<T> me, Func<T, bool> function)
		{
			int result = -1;
			for (int i = 0; me.NotNull() && i < me.Count; i++)
				if (function(me[i]))
				{
					result = i;
					break;
				}
			return result;
		}
		public static T Find<T>(this IVector<T> me, Func<T, bool> function)
		{
			T result = default(T);
			foreach (T element in me)
				if (function(element))
				{
					result = element;
					break;
				}
			return result;
		}
		public static S Find<T, S>(this IVector<T> me, Func<T, S> function)
			where S : class
		{
			S result = default(S);
			foreach (T element in me)
				if ((result = function(element)) != null)
					break;
			return result;
		}
		public static bool Exists<T>(this IVector<T> me, Func<T, bool> function)
		{
			bool result = false;
			foreach (T element in me)
				if (function(element))
				{
					result = true;
					break;
				}
			return result;
		}
		public static bool All<T>(this IVector<T> me, Func<T, bool> function)
		{
			bool result = true;
			foreach (T element in me)
				if (!function(element))
				{
					result = false;
					break;
				}
			return result;
		}
		public static S Fold<T, S>(this IVector<T> me, Func<T, S, S> function, S initial)
		{
			foreach (T element in me)
				initial = function(element, initial);
			return initial;
		}
		public static T[] ToArray<T>(this IVector<T> me)
		{
			T[] result = new T[me.Count];
			for (int i = 0; i < result.Length; i++)
				result[i] = me[i];
			return result;
		}
		public static Slice<T> Slice<T>(this IVector<T> me, int offset)
		{
			return new Slice<T>(me, offset);
		}
		public static Slice<T> Slice<T>(this IVector<T> me, int offset, int count)
		{
			return new Slice<T>(me, offset, count);
		}
		public static IVector<T> Merge<T>(this IVector<T> me, IVector<T> other)
		{
			return me.IsNull() ? other :
				other.IsNull() ? me :
				new Merge<T>(me, other);
		}
		public static IVector<T> Merge<T>(this IVector<T> me, T[] other)
		{
			return me.IsNull() ? (Vector<T>)other :
				other.IsNull() ? me : 
				new Merge<T>(me, (Vector<T>)other);
		}
	}
}
