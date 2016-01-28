//
//  Table.cs
//
//  Author:
//       Simon Mika <smika@hx.se>
//
//  Copyright (c) 2012 Simon Mika
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
using Kean.Serialize.Extension;

namespace Kean.DB
{
	public abstract class Table<T> :
		ITable<T>
		where T : Item<T>, new()
	{
		public string Name { get; private set; }
		public Uri.Locator Locator { get; private set; }
		protected Reflect.Type Type { get; private set; }
		Database database;
		public Database Database
		{ 
			get { return this.database; } 
			internal set
			{
				this.database = value;
				this.Locator = this.database.Locator + this.Name;
			}
		}
		Generic.IEnumerable<KeyValue<string, Reflect.Type>> columns;
		protected Generic.IEnumerable<KeyValue<string, Reflect.Type>> Columns
		{
			get
			{
				if (this.columns.IsNull())
					this.columns = this.GetColumns();
				return this.columns;
			}
		}
		protected Table(string name)
		{
			this.Name = name;
			this.Type = typeof(T);
		}
		Generic.IEnumerable<KeyValue<string, Reflect.Type>> GetColumns()
		{
			yield return KeyValue.Create("key", (Reflect.Type)typeof(string));
			switch (this.Type.Category)
			{
				case Reflect.TypeCategory.Class:
					foreach (Reflect.PropertyInformation property in this.Type.Properties)
					{
						IndexAttribute attribute = property.GetAttributes<IndexAttribute>().First();
						if (attribute.NotNull())
							yield return KeyValue.Create(attribute.Name ?? property.Name.Convert(Kean.Serialize.Casing.Pascal, this.Database.Casing), property.Type);
					}
					break;
				case Reflect.TypeCategory.Structure:
					foreach (Reflect.FieldInformation field in this.Type.Fields)
					{
						IndexAttribute attribute = field.GetAttributes<IndexAttribute>().First();
						if (attribute.NotNull())
							yield return KeyValue.Create(attribute.Name ?? field.Name.Convert(Kean.Serialize.Casing.Camel, this.Database.Casing), field.Type);
					}
					break;
			}
			yield return KeyValue.Create("_data", (Reflect.Type)typeof(string));
			yield return KeyValue.Create("_type", (Reflect.Type)typeof(string));
		}
		#region Create Table
		public bool Create()
		{
			return this.Create(this.Columns);
		}
		#endregion
		#region Create, Read, Update, Delete
		internal Generic.IEnumerable<T> Read(Generic.IEnumerable<Expressions.Expression<Func<T, bool>>> filters, Sorting<T> sorting, int limit, int offset)
		{
			return this.Deserialize(this.FromFields(this.ReadFields(filters, sorting, limit, offset)));
		}
		internal bool Update(Generic.IEnumerable<Expressions.Expression<Func<T, bool>>> filters, Sorting<T> sorting, int limit, int offset, T item)
		{		
			long key = item.Key; // TODO: fix this
			filters = ((Generic.IEnumerable<Expressions.Expression<Func<T, bool>>>)new Expressions.Expression<Func<T, bool>>[] { i => i.Key == key }).Append(filters);
			return this.Update(filters, sorting, limit, offset, this.ToFields(this.Serialize(item))) == 1;
		}
		internal int Update(Generic.IEnumerable<Expressions.Expression<Func<T, bool>>> filters, Sorting<T> sorting, int limit, int offset, params KeyValue<string, object>[] values)
		{
			return this.Update(filters, sorting, limit, offset, values.Map(value => this.Serialize(value.Value).UpdateName(value.Key) as Serialize.Data.Leaf));
		}
		internal protected abstract int Delete(Generic.IEnumerable<Expressions.Expression<Func<T, bool>>> filters, Sorting<T> sorting, int limit, int offset);
		#endregion
		#region ITable implementation
		public int Count { get { return this.GetCount(null, null, 0, 0); } }
		#region CRUD - Create, Read, Update, Delete
		public long Create(T item)
		{
			return item.Key = this.Create(this.ToFields(this.Serialize(item)));
		}
		public Generic.IEnumerable<T> Read()
		{
			return this.Read(null, null, 0, 0);
		}
		public bool Update(T item)
		{
			return this.Update(null, null, 0, 0, item);
		}
		public int Update(params KeyValue<string, object>[] values)
		{
			return this.Update(null, null, 0, 0, values);
		}
		public int Delete()
		{
			return this.Delete(null, null, 0, 0);
		}
		#endregion
		#region Filter, Sort, Limit, Offset
		public ITable<T> Filter(Expressions.Expression<Func<T, bool>> predicate)
		{
			return new Query<T>(this, predicate);
		}
		public ITable<T> Sort(Expressions.Expression<Func<T, object>> selector, bool descending)
		{
			return new Query<T>(this, selector, descending);
		}
		public ITable<T> Limit(int limit, int offset)
		{
			return new Query<T>(this, limit, offset);
		}
		#endregion
		#endregion
		#region Implementors interface
		internal protected abstract int GetCount(Generic.IEnumerable<Expressions.Expression<Func<T, bool>>> filters, Sorting<T> sorting, int limit, int offset);
		protected abstract bool Create(Generic.IEnumerable<KeyValue<string, Reflect.Type>> columns);
		protected abstract long Create(Generic.IEnumerable<Serialize.Data.Leaf> fields);
		protected abstract Generic.IEnumerable<Generic.IEnumerable<Serialize.Data.Leaf>> ReadFields(Generic.IEnumerable<Expressions.Expression<Func<T, bool>>> filters, Sorting<T> sorting, int limit, int offset);
		protected abstract int Update(Generic.IEnumerable<Expressions.Expression<Func<T, bool>>> filters, Sorting<T> sorting, int limit, int offset, Generic.IEnumerable<Serialize.Data.Leaf> fields);
		protected Serialize.Data.Leaf Serialize(object data)
		{
			return (this.Database as Serialize.IStorage).Serialize(data.Type(), data, null) as Serialize.Data.Leaf;
		}
		protected Generic.IEnumerable<Serialize.Data.Leaf> ToFields(Serialize.Data.Branch data)
		{
			Json.Dom.Object nonIndexData = new Json.Dom.Object();
			foreach (Serialize.Data.Node node in data.Nodes)
			{
				if (node is Serialize.Data.Leaf)
				{
					if (node.Attribute is IndexAttribute)
						yield return node as Serialize.Data.Leaf;
					else
						nonIndexData.Add(node.Name.FirstToLower(), Json.Serialize.Storage.Convert(node as Serialize.Data.Leaf));
				}
			}
			yield return new Serialize.Data.String((string)nonIndexData) { Name = "_data" };
			if (data.Type.NotNull())
				yield return new Serialize.Data.String(data.Type) { Name = "_type" };
		}
		protected Generic.IEnumerable<Serialize.Data.Branch> FromFields(Generic.IEnumerable<Generic.IEnumerable<Serialize.Data.Leaf>> items)
		{
			return items.Map(this.FromFields);
		}
		protected Serialize.Data.Branch FromFields(Generic.IEnumerable<Serialize.Data.Leaf> fields)
		{
			Serialize.Data.Branch result = new Serialize.Data.Branch();
			foreach (Serialize.Data.Leaf field in fields)
				switch (field.Name)
				{
					default:
						result.Nodes.Add(field);
						break;
					case "_data":
						result.Merge(Json.Serialize.Storage.Convert((Json.Dom.Object)field.Text));
						break;
					case "_type":
						result.Type = field.Text;
						break;
				}
			return result;
		}
		protected Serialize.Data.Branch Serialize(T item)
		{
			return item.Serialize(this.Database as Serialize.IStorage, typeof(T), this.Locator + item.Key);
		}
		protected T Deserialize(Serialize.Data.Branch item)
		{
			item.DefaultType(typeof(T));
			T result = item.Type.Create<T>();
			return result.Deserialize(this.Database as Serialize.IStorage, item) ? result : null;
		}
		protected Generic.IEnumerable<T> Deserialize(Generic.IEnumerable<Serialize.Data.Branch> items)
		{
			return items.Map(this.Deserialize);
		}
		#endregion
		#region Object overrides
		public override string ToString()
		{
			return string.Format("[Table: Name={0}]", this.Name);
		}
		#endregion
		#region IDisposable implementation
		public virtual bool Close()
		{
			return true;
		}
		~Table ()
		{
			this.Close();
		}
		void IDisposable.Dispose()
		{
			this.Close();
		}
		#endregion
	}
}

