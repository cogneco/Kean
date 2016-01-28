//
//  Object.cs
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
//  You should have received a copy of the GNU Lesser General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using Kean;
using Kean.Extension;
using Kean.Collection;
using Uri = Kean.Uri;

namespace Kean.Json.Dom
{
	public class Object :
		Collection,
        IEquatable<Object>,
		Kean.Collection.IDictionary<Label, Item>
	{
		Kean.Collection.IDictionary<Label, Item> backend;
		public Object() :
			this((Uri.Region)null)
		{
		}
		public Object(Uri.Region region) :
			base(region)
		{
			Kean.Collection.Hooked.List<KeyValue<Label, Item>> list = new Kean.Collection.Hooked.List<KeyValue<Label,Item>>();
			list.Added += (index, item) =>
			{
				if (item.NotNull() && item.Value.NotNull())
					item.Value.Parent = this;
			};
			list.Removed += (index, item) =>
			{
				if (item.NotNull() && item.Value.NotNull())
					item.Value.Parent = null;
			};
			list.Replaced += (index, old, @new) =>
			{
				if (old.NotNull() && old.Value.NotNull())
					old.Value.Parent = null;
				if (@new.NotNull() && @new.Value.NotNull())
					@new.Value.Parent = this;
			};
			this.backend = new Kean.Collection.Wrap.ListDictionary<Label, Item>(list);
		}
		public Object(params KeyValue<string, Item>[] items) :
			this((Uri.Region)null)
		{
			foreach (KeyValue<string, Item> item in items)
				this[item.Key] = item.Value;
		}
		public Object(params KeyValue<Label, Item>[] items) :
			this((Uri.Region)null)
		{
			foreach (KeyValue<Label, Item> item in items)
				this[item.Key] = item.Value;
		}
		public Object Add(Label label, Item item)
		{
			this[label] = item;
			return this;
		}
		public Object Add(Label label, params Item[] items)
		{
			this[label] = items;
			return this;
		}
		public Object Add(params KeyValue<Label, Item>[] items)
		{
			foreach (KeyValue<Label, Item> item in items)
				this[item.Key] = item.Value;
			return this;
		}
		internal override Collection Add(string label, Uri.Region region, Item item)
		{
			this[new Label(label, region)] = item;
			return this;
		}
		#region IEnumerable implementation
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.backend.GetEnumerator();
		}
		#endregion
		#region IEnumerable implementation
		public System.Collections.Generic.IEnumerator<KeyValue<Label, Item>> GetEnumerator()
		{
			return this.backend.GetEnumerator();
		}
		#endregion
		#region IEquatable implementation
		public override int GetHashCode()
		{
			return this.backend.GetHashCode();
		}
		public override bool Equals(object other)
		{
			return this.Equals(other as Object);
		}
		public override bool Equals(Item other)
		{
			return other is Object && this.Equals(other as Object);
		}
		public bool Equals(Object other)
		{
			return other.NotNull() && this.Equals(other.backend as IDictionary<Label, Item>);
		}
		public bool Equals(IDictionary<Label, Item> other)
		{
			return this.backend.Equals(other);
		}
		#endregion
		#region IDictionary implementation
		public bool Contains(Label key)
		{
			return this.backend.Contains(key);
		}
		public bool Remove(Label key)
		{
			return this.backend.Remove(key);
		}
		public Item this [Label key]
		{
			get { return this.backend[key]; }
			set { this.backend[key] = value; }
		}
		#endregion
		#region Object overrides
		public override string ToString()
		{
			return base.ToString();
		}
		#endregion
		#region Casts
		public static explicit operator Object(string data)
		{
			return Object.Open(Sax.Parser.Open(new IO.Text.Reader(data))) as Object;
		}
		#endregion
	}
}

