// 
//  DateTime.cs
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

namespace Kean.Serialize.Serializer
{
	public class DateTime :
		ISerializer
	{
		public DateTime()
		{ }
		#region ISerializer Members
		public ISerializer Find(Reflect.Type type, bool deserialize)
		{
			return type == "System.DateTime" ? this : null;
		}
		public Data.Node Serialize(IStorage storage, Reflect.Type type, object data, Uri.Locator locator)
		{
			return new Data.DateTime(data, type);
		}
		public object Deserialize(IStorage storage, Data.Node data, object result)
		{
			return data is Data.DateTime ? (data as Data.DateTime).Value :
				data is Data.Binary ? new System.DateTime(BitConverter.ToInt64((data as Data.Binary).Value, 0)) :
				data is Data.String ? System.DateTime.Parse((data as Data.String).Value) :
				new System.DateTime();
		}
		#endregion
	}
}

