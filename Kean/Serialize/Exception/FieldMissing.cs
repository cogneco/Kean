// 
//  FieldMissing.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2013 Simon Mika
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
	/// Exception thrown when trying to deserialize a node that does not correspond to field in the current type.
	/// </summary>
	public class FieldMissing :
		Abstract
	{
		internal FieldMissing(string type, string node, Uri.Region region) :
			base(Error.Level.Notification, "Field Missing.", "No field exists in \"{0}\" that corresponds to node \"{1}\"{2}.", type, node, PropertyMissing.Location(" ", region))
		{
		}
	}
}
