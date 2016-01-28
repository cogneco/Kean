// 
//  Binary.cs
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
using Collection = Kean.Collection;
using Kean.Collection.Extension;

namespace Kean.Serialize.Data
{
	public class Binary :
		Leaf<byte[]>
	{
		public override string Text { get { return Convert.ToBase64String(this.Value); } }
		public override byte[] Raw { get { return this.Value; } }
		public Binary(byte[] value) :
			base(value)
		{ }
		public Binary(object value, Reflect.Type type) :
			base(value, type)
		{ }
		public static Binary Create(string value)
		{
			return new Binary(Convert.FromBase64String(value));
		}
		public static Binary Create(byte[] value)
		{
			return new Binary(value);
		}
	}
}
