//
//  TableExtension.cs
//
//  Author:
//       Simon Mika <smika@hx.se>
//
//  Copyright (c) 2013 Simon Mika
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
using Kean;
using Kean.Extension;
using Expressions = System.Linq.Expressions;

namespace Kean.DB.Extension
{
	public static class TableExtension
	{
		public static T ReadFirst<T>(this ITable<T> me) where T : Item<T>, new()
		{
			return me.Limit(1, 0).Read().First();
		}
		public static T Read<T>(this ITable<T> me, long key) where T : Item<T>, new()
		{
			return me.Filter(item => item.Key == key).Read().First();
		}
		public static bool Delete<T>(this ITable<T> me, long key) where T : Item<T>, new()
		{
			return me.Filter(item => item.Key == key).Delete() == 1;
		}
	}
}