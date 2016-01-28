// 
//  VectorParallellExtension.cs
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
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.using System;
using System;
using Kean.Extension;
namespace Kean.Collection.Extension
{
	public static class VectorParallelExtension
	{
		public static void ParallelApply<T>(this IVector<T> data, Action<T> function)
		{
			new Action<int>(index => function(data[index])).For(data.Count);
		}
		public static void ParallelModify<T>(this IVector<T> data, Func<T, T> function)
		{
			new Action<int>(index => data[index] = function(data[index])).For(data.Count);
		}
		public static void ParallelMap<T, S>(this IVector<T> input, IVector<S> output, Func<T, S> function)
		{
			int minimumCount = (input.Count > output.Count) ? output.Count : input.Count;
			new Action<int>(index => output[index] = function(input[index])).For(minimumCount);
		}
		public static IVector<S> ParallelMap<T, S>(this IVector<T> input, Func<T, S> function)
		{
			IVector<S> result = new Vector<S>(input.Count);
			new Action<int>(index => result[index] = function(input[index])).For(input.Count);
			return result;
		}
	}
}
