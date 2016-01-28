// 
//  Collection.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2011-2012 Simon Mika
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
using Kean.Reflect.Extension;

namespace Kean.Serialize.Serializer
{
	public abstract class Collection :
		ISerializer
	{
		protected Collection()
		{
		}
		protected abstract bool Found(Reflect.Type type, bool deserialize);
		protected abstract Reflect.Type GetElementType(Reflect.Type type);
		protected abstract object Create(Reflect.Type type, Reflect.Type elementType, int count);
		protected abstract void Set(object collection, object value, int index);
		#region ISerializer Members
		public ISerializer Find(Reflect.Type type, bool deserialize)
		{
			return this.Found(type, deserialize) ? this : null;
		}
		public Data.Node Serialize(IStorage storage, Reflect.Type type, object data, Uri.Locator locator)
		{
			Data.Collection result = new Data.Collection(data, type);
			Reflect.Type elementType = this.GetElementType(type);
			int c = 0;
			foreach (object child in data as System.Collections.IEnumerable)
			{
				Uri.Locator l = null;
				if (locator.NotNull())
				{
					l = locator.Copy();
					l.Fragment = (l.Fragment.NotEmpty() ? l.Fragment + "/" : "") + (c++).ToString();
				}
				result.Nodes.Add(storage.Serialize(elementType, child, l));
			}
			return result;
		}
		public object Deserialize(IStorage storage, Data.Node data, object result)
		{
			Kean.Collection.IList<Data.Node> nodes;
			Reflect.Type type;
			Reflect.Type elementType;
			type = data.Type;
			elementType = this.GetElementType(type);
			if (data is Data.Collection)
				nodes = (data as Data.Collection).Nodes;
			else
			{ // only one element so it was impossible to know it was a collection
				nodes = new Kean.Collection.List<Data.Node>(data);
				data.Type = data.OriginalType ?? elementType;
				Uri.Locator locator = data.Locator.Copy();
				locator.Fragment += "0";
				data.Locator = locator;
			}

			if (result.IsNull())
				result = this.Create(type, elementType, nodes.Count);
			int i = 0;
			foreach (Data.Node child in nodes)
			{
				int c = i++; // ensure c is unique in every closure of lambda function below
				storage.Deserialize(child, elementType, d => this.Set(result, d, c));
			}
			return result;
		}
		#endregion
	}
}

