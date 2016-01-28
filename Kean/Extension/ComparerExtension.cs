// 
//  ComparerExtension.cs
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
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;

namespace Kean.Extension
{
	public static class ComparerExtension
	{
		public static Comparison<T> AsComparison<T>(this Comparer<T> me)
		{
			return (left, right) =>
			{
				int result;
				switch (me(left, right))
				{
					default:
					case Order.GreaterThan: result = 1; break;
					case Order.Equal: result = 0; break;
					case Order.LessThan: result = -1; break;
				}
				return result;
			};
		}
		public static Comparer<T> Then<T>(this Comparer<T> me, Comparer<T> secondary)
		{
			return (left, right) =>
			{
				Order result = me(left, right);
				if (result == Order.Equal)
					result = secondary(left, right);
				return result;
			};
		}
	}
}
