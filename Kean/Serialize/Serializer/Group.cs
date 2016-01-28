// 
//  Group.cs
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
	public class Group :
		ISerializer
	{
		ISerializer[] serializers;
		public Group(params ISerializer[] serializers)
		{
			this.serializers = serializers;
		}
		#region ISerializer Members
		public ISerializer Find(Reflect.Type type, bool deserialize)
		{
			ISerializer result = null;
			foreach (ISerializer serializer in this.serializers)
				if ((result = serializer.Find(type, deserialize)).NotNull())
					break;
			return result;
		}
		public Data.Node Serialize(IStorage storage, Reflect.Type type, object data, Uri.Locator locator)
		{
			ISerializer serializer = this.Find(data.Type(), false);
			return serializer.NotNull() ? serializer.Serialize(storage, type, data, locator) : null;
		}
		public object Deserialize(IStorage storage, Data.Node data, object result)
		{
			ISerializer serializer = this.Find(data.Type, true);
			return serializer.NotNull() ? serializer.Deserialize(storage, data, null) : null;
		}
		#endregion
	}
}

