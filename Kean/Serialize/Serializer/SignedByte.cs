// 
//  SignedByte.cs
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
	public class SignedByte :
		ISerializer
	{
		public SignedByte()
		{ }
		#region ISerializer Members
		public ISerializer Find(Reflect.Type type, bool deserialize)
		{
			return type == "sbyte" ? this : null;
		}
		public Data.Node Serialize(IStorage storage, Reflect.Type type, object data, Uri.Locator locator)
		{
			return new Data.SignedByte(data, type);
		}
		public object Deserialize(IStorage storage, Data.Node data, object result)
		{
			return data is Data.SignedByte ? (data as Data.SignedByte).Value :
				data is Data.Binary ? unchecked((sbyte)(data as Data.Binary).Value[0]) :
				data is Data.String ? sbyte.Parse((data as Data.String).Value, System.Globalization.CultureInfo.InvariantCulture.NumberFormat) :
				(sbyte)0;
		}
		#endregion
	}
}

