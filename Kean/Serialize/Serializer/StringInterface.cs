// 
//  StringInterface.cs
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
	public class StringInterface :
		ISerializer
	{
		public StringInterface()
		{ }
		#region ISerializer Members
		public ISerializer Find(Reflect.Type type, bool deserialize)
		{
			return type.Implements<IString>() ? this : null;
		}
		public Data.Node Serialize(IStorage storage, Reflect.Type type, object data, Uri.Locator locator)
		{
			return new Data.String((data as IString).String, data, type);
		}
		public object Deserialize(IStorage storage, Data.Node data, object result)
		{
			if (data is Data.String)
			{
				if (!(result is IString))
                    result = data.Type.Create();
				(result as IString).String = (data as Data.String).Value;
			}
			else
				result = null;
			return result;
		}
		#endregion
	}
}

