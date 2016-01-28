// 
//  String.cs
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
	public class String :
		ISerializer
	{
		public String()
		{
		}
		#region ISerializer Members
		public ISerializer Find(Reflect.Type type, bool deserialize)
		{
			return type == "string" ? this : null;
		}
		public Data.Node Serialize(IStorage storage, Reflect.Type type, object data, Uri.Locator locator)
		{
			return new Data.String(data, type);
		}
		public object Deserialize(IStorage storage, Data.Node data, object result)
		{
			return data is Data.String ? (data as Data.String).Value :
				data is Data.Binary ? System.Text.Encoding.UTF8.GetString((data as Data.Binary).Value) :
				data is Data.Null ? null :
				"";
		}
		#endregion
	}
}

