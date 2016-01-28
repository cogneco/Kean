// 
//  Items.cs
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
using Kean.DB.Extension;
using Expressions = System.Linq.Expressions;

namespace Kean.DB
{
	public abstract class Items<T>
		where T : Item<T>, new()
	{
		ITable<T> table;
		int? count;

		public int Count
		{
			get
			{
				if (!this.count.HasValue)
					this.count = this.table.Count;
				return this.count.GetValueOrDefault();
			}
		}

		protected Items(ITable<T> table)
		{
			this.table = table;
		}

		public T Open (long key)
		{
			return this.OpenFirst(item => item.Key == key);
		}

		protected T OpenFirst (Expressions.Expression<Func<T, bool>> predicate)
		{
			T result = this.table.Filter(predicate).ReadFirst();
			if (result.NotNull())
				result.Table = this.table;
			return result;
		}

		public Generic.IEnumerable<T> Open (int limit, int offset)
		{
			return this.SetTable(this.table.Limit(limit, offset).Read());
		}

		protected Generic.IEnumerable<T> Open (Expressions.Expression<Func<T, bool>> predicate)
		{
			return this.SetTable(this.table.Filter(predicate).Read());
		}

		Generic.IEnumerable<T> SetTable (Generic.IEnumerable<T> items)
		{
			foreach (T item in items)
			{
				if (item.NotNull())
					item.Table = this.table;
				yield return item;
			}
		}

		public virtual long Create (T item)
		{
			item.Table = this.table;
			return this.table.Create(item);
		}
	}
}

