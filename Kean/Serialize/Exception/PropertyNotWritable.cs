// 
//  PropertyNotWriteable.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2012 Simon Mika
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
namespace Kean.Serialize.Exception
{
	/// <summary>
	/// Exception thrown when trying to deserialize a type and the reader is not positioned at an element.
	/// </summary>
	public class PropertyNotWriteable :
		Abstract
	{
		internal PropertyNotWriteable(string type, string node, Uri.Region region) :
			base(Error.Level.Notification, "Property Not Writable.", "Property in \"{0}\" that corresponds to node \"{1}\"{2} can't be set.", type, node, PropertyNotWriteable.Location(", ", region, ","))
		{
		}
	}
}
