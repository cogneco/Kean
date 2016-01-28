// 
//  Item.cs
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

namespace Kean.DB
{
	public abstract class Item<T>
		where T : Item<T>, new()
	{
		internal protected ITable<T> Table { get; internal set; }
		public long Key { get; internal protected set; }
		protected Item()
		{
		}
		protected Item(long key)
		{
			this.Key = key;
		}
		internal protected virtual Serialize.Data.Branch Serialize(Serialize.IStorage storage, Reflect.Type type, Uri.Locator resource)
		{
			return storage.Serialize(type, this, resource) as Serialize.Data.Branch;
		}
		internal protected virtual bool Deserialize(Serialize.IStorage storage, Serialize.Data.Branch node)
		{
			return storage.DeserializeContent(node, this);
		}
		public bool Save()
		{
			return this.Table.Update(this as T);
		}
		public bool Remove()
		{
			return this.Table.Delete(this.Key);
		}
	}
}

