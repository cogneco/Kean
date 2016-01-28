// 
//  List.cs
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
using Kean.Reflect.Extension;
using Collection = Kean.Collection;

namespace Kean.Serialize.Serializer
{
	public class List :
		Collection
	{
		public List()
		{ }
		System.Type GetInterface(Reflect.Type type)
		{
			return type.Name == "Kean.Collection.IList" ? (System.Type)type : ((System.Type)type).GetInterface(typeof(Kean.Collection.IList<>).Name);
		}
		protected override bool Found(Reflect.Type type, bool deserialize)
		{
			return this.GetInterface(type).NotNull();
		}
		protected override Reflect.Type GetElementType(Reflect.Type type)
		{
			return this.GetInterface(type).GetGenericArguments()[0];
		}
		protected override object Create(Reflect.Type type, Reflect.Type elementType, int count)
		{
			return System.Activator.CreateInstance(type);
		}
		protected override void Set(object collection, object value, int index)
		{
			this.GetInterface(collection.Type()).GetMethod("Add").Invoke(collection, new object[] { value });
		}
	}
}

