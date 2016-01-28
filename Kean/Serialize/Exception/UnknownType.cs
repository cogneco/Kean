// 
//  CreateAbstract.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2009-2011 Simon Mika
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
	public class UnknownType :
		Abstract
	{
		internal UnknownType(string type, Uri.Region region) :
			this(null, type, region)
		{
		}
		internal UnknownType(System.Exception inner, string type, Uri.Region region) :
			base(inner, Error.Level.Notification, "Unknown type: \"{0}\".", "Attempted to create instance of unknown type \"{0}\" while deserializing {2} column {3} in file \"{1}\".", type, region.Resource, region.Start.Row, region.Start.Column)
		{
		}
	}
}
