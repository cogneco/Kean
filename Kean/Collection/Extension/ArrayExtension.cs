// 
//  ArrayExtension.cs
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
namespace Kean.Collection.Extension
{
	public static class ArrayExtension
	{
		public static Slice<T> Slice<T>(this T[] me, int offset, int count)
		{
			return new Slice<T>(me, offset, count);
		}
		public static Merge<T> Merge<T>(this T[] me, IVector<T> other)
		{
			return new Merge<T>((Vector<T>)me, other);
		}
		public static Merge<T> Merge<T>(this T[] me, T[] other)
		{
			return new Merge<T>(me, other);
		}
	}
}
