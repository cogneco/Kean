// 
//  Leaf.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2010-2012 Simon Mika
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
using Reflect = Kean.Reflect;
using Kean.Reflect.Extension;

namespace Kean.Serialize.Data
{
	public abstract class Leaf :
		Node
	{
		public abstract string Text { get; }
		public abstract byte[] Raw { get; }
		protected Leaf()
		{ }
	}
	public abstract class Leaf<T> :
		Leaf
	{
		public T Value { get; private set; }
		protected Leaf(T value)
		{
			this.Value = value;
		}
		protected Leaf(T value, Reflect.Type type) :
			this(value)
		{
			this.Type = type;
		}
		Leaf(object value, Reflect.Type valueType, Reflect.Type type) :
			this((T)value, valueType != type ? valueType : null)
		{ }
		protected Leaf(object value, object data, Reflect.Type type) :
			this((T)value, data.Type(), type)
		{ }
		protected Leaf(object value, Reflect.Type type) :
			this(value, value, type)
		{ }
	}
}
