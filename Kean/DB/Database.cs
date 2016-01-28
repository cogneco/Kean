//
//  Database.cs
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
using Generic = System.Collections.Generic;
using Reflect = Kean.Reflect;

namespace Kean.DB
{
	public abstract class Database :
        Serialize.IStorage,
		IDisposable
	{
		Collection.IDictionary<Reflect.Type, IDisposable> tables = new Collection.Dictionary<Reflect.Type, IDisposable>();
		Serialize.Resolver resolver;
		Serialize.ISerializer serializer;
		Serialize.IRebuilder rebuilder;

		public Uri.Locator Locator { get; private set; }

		public string Prefix { get; private set; }

		public Serialize.Casing Casing { get { return Serialize.Casing.Camel; } }

		protected Database(Uri.Locator locator, string prefix, Serialize.Resolver resolver, Serialize.IRebuilder rebuilder, params Serialize.ISerializer[] serializers)
		{
			this.Locator = locator;
			this.Prefix = prefix ?? "";
			this.resolver = resolver ?? new Serialize.Resolver();
			this.rebuilder = rebuilder ?? new Serialize.Rebuilder.Identity();
			this.serializer = new Serialize.Serializer.Cache(serializers.NotEmpty() ? new Serialize.Serializer.Group(serializers) : new Serialize.Serializer.Default());
		}

		#region IStorage implementation

		Serialize.Resolver Serialize.IStorage.Resolver { get { return this.resolver; } }

		Serialize.Data.Node Serialize.IStorage.Serialize (Reflect.Type type, object data, Uri.Locator locator)
		{
			return this.serializer.Serialize(this, type, data, locator);
		}

		bool Serialize.IStorage.DeserializeContent (Serialize.Data.Node node, object result)
		{
			return node.NotNull() && result.NotNull() && this.Deserialize(result, node).NotNull();
		}

		void Serialize.IStorage.Deserialize (Serialize.Data.Node node, Reflect.Type type, Action<object> set)
		{
			node = node.DefaultType(type);
			if (node is Serialize.Data.Link)
				this.resolver.Resolve((node as Serialize.Data.Link).Target, d =>
				{
					this.resolver[node.Locator] = d;
					set.Call(d);
				});
			else
				set.Call(this.Deserialize(null, node));
		}

		object Deserialize (object result, Serialize.Data.Node node)
		{
			return node.NotNull() ? (this.resolver[node.Locator] = this.serializer.Deserialize(this, node, result)) : null;
		}

		#endregion

		#region Add & Create Table

		protected abstract Table<T> New<T> (string name) where T : Item<T>, new();

		public Table<T> Get<T> () where T : Item<T>, new()
		{
			lock (this.tables)
			{
				Table<T> result = this.tables[typeof(T)] as Table<T>;
				if (result.IsNull())
				{
					this.tables[typeof(T)] = result = this.New<T>(this.GetName(typeof(T)));
					result.Database = this;
				}
				return result;
			}
		}

		public Table<T> Create<T> () where T : Item<T>, new()
		{
			lock (this.tables)
			{
				Table<T> result = this.Get<T>();
				result.Create();
				return result;
			}
		}

		protected virtual string GetName (Reflect.Type type)
		{
			TableAttribute attribute = type.GetAttributes<TableAttribute>().First();
			return this.Prefix + (attribute.NotNull() ? attribute.Name : null) ?? type.ShortName.FirstToLower();
		}

		public abstract Database ChangePrefix (string prefix);

		#endregion

		public virtual bool Close ()
		{
			bool result;
			if (result = this.tables.NotNull())
			{
				this.tables.Apply(table => table.Value.Dispose());
				this.tables = null;
			}
			return result;
		}

		#region IDisposable implementation

		~Database ()
		{
			(this as IDisposable).Dispose();
		}

		void IDisposable.Dispose ()
		{
			this.Close();
		}

		#endregion

	}
}

