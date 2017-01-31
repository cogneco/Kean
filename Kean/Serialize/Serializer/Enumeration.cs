// 
//  Enumeration.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2011 Simon Mika
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

namespace Kean.Serialize.Serializer
{
	public class Enumeration :
		ISerializer
	{
		public Enumeration()
		{
		}
		#region ISerializer Members
		public ISerializer Find(Reflect.Type type, bool deserialize)
		{
			return type.Category == Reflect.TypeCategory.Enumeration ? this : null;
		}
		public Data.Node Serialize(IStorage storage, Reflect.Type type, object data, Uri.Locator locator)
		{
			return new Data.Enumeration(data, type);
		}
		public object Deserialize(IStorage storage, Data.Node data, object result)
		{
			return data is Data.Enumeration ? (data as Data.Enumeration).Value :
				data is Data.Binary ? this.Create((data as Data.Binary).Value, data.Type) :
				data is Data.String ? Enum.Parse(data.Type, (data as Data.String).Value.Replace('|', ',')) :
				Enum.ToObject(data.Type, 0);
		}
		object Create(byte[] value, Reflect.Type type)
		{
			object result = null;
			switch ((Reflect.Type)Enum.GetUnderlyingType(type))
			{
				case "sbyte": if (value.Length == 1) result = Enum.ToObject(type, (sbyte)value[0]); break;
				case "byte": if (value.Length == 1) result = Enum.ToObject(type, value[0]); break;
				case "short": if (value.Length == 2) result = Enum.ToObject(type, BitConverter.ToInt32(value, 0)); break;
				case "ushort": if (value.Length == 2) result = Enum.ToObject(type, BitConverter.ToUInt32(value, 0)); break;
				case "int": if (value.Length == 4) result = Enum.ToObject(type, BitConverter.ToInt32(value, 0)); break;
				case "uint": if (value.Length == 4) result = Enum.ToObject(type, BitConverter.ToUInt32(value, 0)); break;
				case "long": if (value.Length == 8) result = Enum.ToObject(type, BitConverter.ToInt64(value, 0)); break;
				case "ulong": if (value.Length == 8) result = Enum.ToObject(type, BitConverter.ToUInt64(value, 0)); break;
			}
			return result;
		}
		#endregion
	}
}

