// 
//  Decimal.cs
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
	public class Decimal :
		ISerializer
	{
		public Decimal()
		{ }
		#region ISerializer Members
		public ISerializer Find(Reflect.Type type, bool deserialize)
		{
			return type == "decimal" ? this : null;
		}
		public Data.Node Serialize(IStorage storage, Reflect.Type type, object data, Uri.Locator locator)
		{
			return new Data.Decimal(data, type);
		}
		public object Deserialize(IStorage storage, Data.Node data, object result)
		{
			return data is Data.Decimal ? (data as Data.Decimal).Value :
				data is Data.Binary ? new decimal(new int[] { BitConverter.ToInt32((data as Data.Binary).Value, 0), BitConverter.ToInt32((data as Data.Binary).Value, 4), BitConverter.ToInt32((data as Data.Binary).Value, 8), BitConverter.ToInt32((data as Data.Binary).Value, 12) }) :
				data is Data.String ? decimal.Parse((data as Data.String).Value, System.Globalization.CultureInfo.InvariantCulture.NumberFormat) :
				0;
		}
		#endregion
	}
}

