//
//  Storer.cs
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
using Kean.Extension;
using Collection = Kean.Collection;
using Kean.Collection.Extension;
using Uri = Kean.Uri;
using Kean.Reflect.Extension;

namespace Kean.Serialize
{
	public class Storer :
	IStorage
	{
		ISerializer serializer;
		public Resolver Resolver { get; private set; }
		public Casing Casing { get; set; }
		public bool NoTypes { get; set; }
		public Storer() :
			this(null)
		{
		}
		protected Storer(Resolver resolver, params ISerializer[] serializers)
		{
			this.Resolver = resolver ?? new Resolver();
			this.serializer = new Serializer.Cache(serializers.NotEmpty() ? new Serializer.Group(serializers) : new Serializer.Default());
		}
		public Data.Node Serialize(Reflect.Type type, object data, Uri.Locator locator)
		{
			return this.serializer.Serialize(this, this.NoTypes ? data.Type() : type, data, locator);
		}
		protected object Deserialize(object result, Serialize.Data.Node node)
		{
			return node.NotNull() ? (this.Resolver[node.Locator] = this.serializer.Deserialize(this, node, result)) : null;
		}
		public bool DeserializeContent(Serialize.Data.Node node, object result)
		{
			return node.NotNull() && result.NotNull() && this.Deserialize(result, node).NotNull();
		}
		public void Deserialize(Serialize.Data.Node node, Reflect.Type type, Action<object> set)
		{
			node = node.DefaultType(type);
			if (node is Data.Link)
				this.Resolver.Resolve((node as Data.Link).Target, d =>
				{
					this.Resolver[node.Locator] = d;
					set.Call(d);
				});
			else
				set.Call(this.Deserialize(null, node));
		}
		static Storer storer = new Storer() { Casing = Casing.Camel };
		public static Data.Node Store(Reflect.Type type, object data)
		{
			return Storer.storer.Serialize(type, data, null);
		}
		public static Data.Node Store<T>(T data)
		{
			return Storer.storer.Serialize(typeof(T), data, "stream:///");
		}
		public static object Load(Serialize.Data.Node node)
		{
			return Storer.storer.Deserialize(null, node);
		}
		public static T Load<T>(Serialize.Data.Node node)
		{
			return (T)Storer.storer.Deserialize(null, node);
		}
		public static bool Load<T>(Serialize.Data.Node node, T result)
		{
			return Storer.storer.Deserialize(result, node).NotNull();
		}
	}
}