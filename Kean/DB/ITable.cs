//
//  ITable.cs
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
//  You should have received data copy of the GNU Lesser General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using Kean;
using Kean.Extension;
using Collection = Kean.Collection;
using Kean.Collection.Extension;
using Uri = Kean.Uri;
using Serialize = Kean.Serialize;
using Reflect = Kean.Reflect;
using Kean.Reflect.Extension;
using IO = Kean.IO;
using Generic = System.Collections.Generic;
using Expressions = System.Linq.Expressions;

namespace Kean.DB
{
	public interface ITable<T> :
		IDisposable
		where T : Item<T>, new()
	{
		Database Database { get; }
		#region Filter, Sort, Limit, Offset
		ITable<T> Filter(Expressions.Expression<Func<T, bool>> predicate);
		ITable<T> Sort(Expressions.Expression<Func<T, object>> selector, bool descending);
		ITable<T> Limit(int limit, int offset);
		#endregion
		#region Count, Create, Read, Update, Delete
		int Count { get; }
		long Create(T item);
		Generic.IEnumerable<T> Read();
		bool Update(T item);
		int Update(params KeyValue<string, object>[] values);
		int Delete();
		#endregion
		bool Close();
	}
}

