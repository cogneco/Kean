//
//  Storage.cs
//
//  Author:
//       Simon Mika <smika@hx.se>
//
//  Copyright (c) 2011-2013 Simon Mika
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
using IO = Kean.IO;
using Uri = Kean.Uri;

namespace Kean.Json.Serialize
{
	public class Storage : 
		Kean.Serialize.Storage
	{
		public override string ContentType { get { return "application/json"; } }
		public Storage() :
			base(null, null, null)
		{
			this.Casing = Kean.Serialize.Casing.Camel;
		}
		protected override Kean.Serialize.Data.Node LoadImplementation(IO.IByteInDevice device)
		{
			Dom.Item root = Dom.Item.Open(device);
			return root.NotNull() ? Storage.Convert(root) : null;
		}
		protected override bool StoreImplementation(Kean.Serialize.Data.Node value, IO.IByteOutDevice device)
		{
			Dom.Item item = Storage.Convert(value);
			if (!(item is Dom.Collection))
				item = new Dom.Object(KeyValue.Create(value.Name, item));
			return item.Save(device);
		}
		#region Static Convert
		#region From Dom
		public static Kean.Serialize.Data.Node Convert(Dom.Item item)
		{
			Kean.Serialize.Data.Node result;
			if (item is Dom.Object)
				result = Storage.Convert(item as Dom.Object);
			else if (item is Dom.Primitive)
			{
				if (item is Dom.String)
					result = new Kean.Serialize.Data.String((item as Dom.String).Value);
				else if (item is Dom.Number)
					result = new Kean.Serialize.Data.Decimal((item as Dom.Number).Value);
				else if (item is Dom.Boolean)
					result = new Kean.Serialize.Data.Boolean((item as Dom.Boolean).Value);
				else if (item is Dom.Null)
					result = new Kean.Serialize.Data.Null();
				else
					result = null;
			}
			else if (item is Dom.Array)
				result = Storage.Convert(item as Dom.Array);
			else
				result = null;
			return result;
		}
		static Kean.Serialize.Data.Branch Convert(Dom.Object item)
		{
			Kean.Serialize.Data.Branch result = new Kean.Serialize.Data.Branch() { Region = item.Region };
			foreach (KeyValue<Json.Dom.Label, Dom.Item> child in item)
			{
				Kean.Serialize.Data.Node c = Storage.Convert(child.Value);
				c.Name = child.Key;
				result.Nodes.Add(c);
			}
			return result;
		}
		static Kean.Serialize.Data.Collection Convert(Dom.Array item)
		{
			Kean.Serialize.Data.Collection result = new Kean.Serialize.Data.Collection() { Region = item.Region };
			foreach (Dom.Item child in item)
				result.Nodes.Add(Storage.Convert(child));
			return result;
		}
		#endregion
		#region To Dom
		public static Dom.Item Convert(Kean.Serialize.Data.Node item)
		{
			Dom.Item result;
			if (item is Kean.Serialize.Data.Collection)
				result = Storage.Convert(item as Kean.Serialize.Data.Collection);
			else if (item is Kean.Serialize.Data.Branch)
				result = Storage.Convert(item as Kean.Serialize.Data.Branch);
			else if (item is Kean.Serialize.Data.Leaf)
				result = Storage.Convert(item as Kean.Serialize.Data.Leaf);
			else if (item is Kean.Serialize.Data.Link)
				result = Storage.Convert(item as Kean.Serialize.Data.Link);
			else
				result = new Kean.Json.Dom.Null(item.Region);
			return result;
		}
		static Dom.Object Convert(Kean.Serialize.Data.Branch item)
		{
			Dom.Object result = new Dom.Object(item.Region);
			if (item.Type.NotNull())
				result.Add("$type", (string)item.Type);
			item.Nodes.Apply(e => result.Add(e.Name, Storage.Convert(e)));
			return result;
		}
		static Dom.Primitive Convert(Kean.Serialize.Data.Leaf item)
		{
			Dom.Primitive result;
			if (item is Kean.Serialize.Data.Binary)
				result = new Dom.String((item as Kean.Serialize.Data.Binary).Text);
			else if (item is Kean.Serialize.Data.Boolean)
				result = new Dom.Boolean((item as Kean.Serialize.Data.Boolean).Value);
			else if (item is Kean.Serialize.Data.Byte)
				result = new Dom.Number((item as Kean.Serialize.Data.Byte).Value);
			else if (item is Kean.Serialize.Data.Character)
				result = new Dom.String((item as Kean.Serialize.Data.Character).Text);
			else if (item is Kean.Serialize.Data.DateTime)
				result = new Dom.String((item as Kean.Serialize.Data.DateTime).Text);
			else if (item is Kean.Serialize.Data.DateTimeOffset)
				result = new Dom.String((item as Kean.Serialize.Data.DateTimeOffset).Text);
			else if (item is Kean.Serialize.Data.Decimal)
				result = new Dom.Number((item as Kean.Serialize.Data.Decimal).Value);
			else if (item is Kean.Serialize.Data.Double)
				result = new Dom.Number((Decimal)(item as Kean.Serialize.Data.Double).Value);
			else if (item is Kean.Serialize.Data.Enumeration)
				result = new Dom.String((item as Kean.Serialize.Data.Enumeration).Text);
			else if (item is Kean.Serialize.Data.Integer)
				result = new Dom.Number((item as Kean.Serialize.Data.Integer).Value);
			else if (item is Kean.Serialize.Data.Long)
				result = new Dom.Number((item as Kean.Serialize.Data.Long).Value);
			else if (item is Kean.Serialize.Data.Short)
				result = new Dom.Number((item as Kean.Serialize.Data.Short).Value);
			else if (item is Kean.Serialize.Data.SignedByte)
				result = new Dom.Number((item as Kean.Serialize.Data.SignedByte).Value);
			else if (item is Kean.Serialize.Data.Single)
				result = new Dom.Number((Decimal)(item as Kean.Serialize.Data.Single).Value);
			else if (item is Kean.Serialize.Data.String)
				result = new Dom.String((item as Kean.Serialize.Data.String).Value);
			else if (item is Kean.Serialize.Data.TimeSpan)
				result = new Dom.String((item as Kean.Serialize.Data.TimeSpan).Text);
			else if (item is Kean.Serialize.Data.UnsignedInteger)
				result = new Dom.Number((item as Kean.Serialize.Data.UnsignedInteger).Value);
			else if (item is Kean.Serialize.Data.UnsignedLong)
				result = new Dom.Number((item as Kean.Serialize.Data.UnsignedLong).Value);
			else if (item is Kean.Serialize.Data.UnsignedShort)
				result = new Dom.Number((item as Kean.Serialize.Data.UnsignedShort).Value);
			else
				result = new Kean.Json.Dom.Null();
			return result;
		}
		static Dom.Array Convert(Kean.Serialize.Data.Collection item)
		{
			Dom.Array result = new Dom.Array(item.Region);
			item.Nodes.Apply(e => result.Add(Storage.Convert(e)));
			return result;
		}
		static Dom.Object Convert(Kean.Serialize.Data.Link item)
		{
			Dom.Object result = new Dom.Object(item.Region);
			result.Add("$ref", new Dom.String(item.Relative, item.Region));
			return result;
		}
		#endregion
		#endregion
		public static T Open<T>(Uri.Locator resource)
		{
			return new Storage().Load<T>(resource);
		}
		public static bool Save<T>(T value, Uri.Locator resource, string name = null)
		{
			return new Storage().Store<T>(value, resource, name);
		}
		public static T Open<T>(IO.IByteInDevice device)
		{
			return new Storage().Load<T>(device);
		}
		public static bool Save<T>(T value, IO.IByteOutDevice device, string name = null)
		{
			return new Storage().Store<T>(value, device, name);
		}
	}
}
