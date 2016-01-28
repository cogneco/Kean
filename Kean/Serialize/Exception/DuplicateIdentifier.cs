// 
//  DuplicateIdentifier.cs
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
	public class DuplicateIdentifier :
		Abstract
	{
		public DuplicateIdentifier(string identifier, string resource, int line, int column) :
			base(Error.Level.Notification, "Identifier already defined.", "An identifier with the name \"{0}\" is already defined previously. Error located at line {2} column {3} in file \"{1}\".", identifier, resource, line.ToString(), column.ToString())
		{
		}
		public DuplicateIdentifier(DuplicateIdentifier inner, Uri.Locator resource, int row, int column) :
			base(inner, inner.Level, inner.Title, inner.Message + "Error located at line {1} column {2} in file \"{0}\".", resource, row.ToString(), column.ToString())
		{
		}
		public DuplicateIdentifier(string identifier) :
			base(Error.Level.Notification, "Identifier already defined.", "An identifier with the name \"{0}\" is already defined previously.", identifier)
		{
		}
	}
}
