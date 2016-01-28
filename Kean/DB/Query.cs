//
//  Query.cs
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
using Collection = Kean.Collection;
using Kean.Collection.Extension;
using Uri = Kean.Uri;
using Serialize = Kean.Serialize;
using Reflect = Kean.Reflect;
using Kean.Reflect.Extension;
using IO = Kean.IO;
using Generic = System.Collections.Generic;
using Expressions = System.Linq.Expressions;
using Integer = Kean.Math.Integer;

namespace Kean.DB
{
	public class Query<T> :
		ITable<T>
		where T: Item<T>, new()
	{
		public Database Database { get { return this.backend.Database; } }
		Table<T> backend;
		Collection.List<Expressions.Expression<Func<T, bool>>> filters = new Collection.List<Expressions.Expression<Func<T, bool>>>();
		Sorting<T> sorting;
		int limit;
		int offset;
		Query(Table<T> backend)
		{
			this.backend = backend;
		}
		internal Query(Table<T> backend, Expressions.Expression<Func<T, bool>> filter) :
			this(backend)
		{
			this.filters.Add(filter);
		}
		internal Query(Table<T> backend, Expressions.Expression<Func<T, object>> selector, bool descending) :
			this(backend)
		{
			this.sorting = new Sorting<T>(null, selector, descending);
		}
		internal Query(Table<T> backend, int limit, int offset) :
			this(backend)
		{
			this.limit = limit;
			this.offset = offset;
		}
		#region ITable implementation
		public int Count { get { return this.backend.GetCount(this.filters, this.sorting, this.limit, this.offset); } }
		#region Filter, Sort, Limit
		public ITable<T> Filter(Expressions.Expression<Func<T, bool>> predicate)
		{
			Query<T> result = new Query<T>(this.backend)
			{
				sorting = this.sorting,
				limit = this.limit,
				offset = this.offset,
			};
			result.filters.Add(this.filters);
			result.filters.Add(predicate);
			return result;
		}
		public ITable<T> Sort(Expressions.Expression<Func<T, object>> selector, bool descending)
		{
			return new Query<T>(this.backend)
			{
				filters = this.filters,
				sorting = new Sorting<T>(this.sorting, selector, descending),
				limit = this.limit,
				offset = this.offset,
			};
		}
		public ITable<T> Limit(int limit, int offset)
		{
			return new Query<T>(this.backend)
			{
				filters = this.filters,
				sorting = this.sorting,
				limit = limit > 0 ? this.limit > 0 ? Integer.Minimum(limit, this.limit) : limit : this.limit,
				offset = this.offset + offset,
			};
		}
		#endregion
		#region CRUD - Create, Read, Update, Delete
		public long Create(T item)
		{
			return this.backend.Create(item);
		}
		public Generic.IEnumerable<T> Read()
		{
			return this.backend.Read(this.filters, this.sorting, this.limit, this.offset);
		}
		public bool Update(T item)
		{
			return this.backend.Update(this.filters, this.sorting, this.limit, this.offset, item);
		}
		public int Update(params KeyValue<string, object>[] values)
		{
			return this.backend.Update(this.filters, this.sorting, this.limit, this.offset, values);
		}
		public int Delete()
		{
			return this.backend.Delete(this.filters, this.sorting, this.limit, this.offset);
		}
		#endregion
		public bool Close()
		{
			bool result;
			if (result = this.backend.NotNull())
				this.backend = null;
			return result;
		}
		#endregion
		#region IDisposable implementation
		public void Dispose()
		{
			this.Close();
		}
		#endregion
	}
}

