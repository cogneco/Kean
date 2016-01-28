// 
//  Character.cs
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
using Kean.Extension;

namespace Kean.Serialize.Serializer
{
	public class Character :
		ISerializer
	{
		public Character()
		{ }
		#region ISerializer Members
		public ISerializer Find(Reflect.Type type, bool deserialize)
		{
			return type == "char" ? this : null;
		}
		public Data.Node Serialize(IStorage storage, Reflect.Type type, object data, Uri.Locator locator)
		{
			return new Data.Character(data, type);
		}
		public object Deserialize(IStorage storage, Data.Node data, object result)
		{
			return data is Data.Character ? (data as Data.Character).Value :
				data is Data.Binary ? this.Convert((data as Data.Binary).Value) :
				data is Data.String ? char.Parse((data as Data.String).Value) :
				'\0';
		}
		char Convert(byte[] value)
		{
			string result = System.Text.Encoding.UTF8.GetString(value);
			return result.NotEmpty() && result.Length == 1 ? result[0] : '\0';
		}
		#endregion
	}
}

