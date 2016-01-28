// 
//  Enumeration.cs
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
	public class Enumeration :
		Leaf<object>
	{
		public override string Text { get { return this.Value.ToString(); } }
		public override byte[] Raw 
		{ 
			get 
			{
				byte[] result;
				switch ((Reflect.Type)Enum.GetUnderlyingType(this.Type))
				{
					case "sbyte": result = new byte[] { unchecked((byte)(sbyte)this.Value) }; break;
					case "byte": result = new byte[] { (byte)this.Value }; break;
					case "short": result = BitConverter.GetBytes((short) this.Value); break;
					case "ushort": result = BitConverter.GetBytes((ushort)this.Value); break;
					case "int": result = BitConverter.GetBytes((int)this.Value); break;
					case "uint": result = BitConverter.GetBytes((uint)this.Value); break;
					case "long": result = BitConverter.GetBytes((long)this.Value); break;
					case "ulong": result = BitConverter.GetBytes((ulong)this.Value); break;
					default: result = new byte[0]; break;
				}
				return result;
			} 
		}
		public Enumeration(object value, Reflect.Type type) :
			base(value, type)
		{ }
	}
}
