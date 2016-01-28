// 
//  Unsortable.cs
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

namespace Kean.Collection.Exception
{
	public class Unsortable :
		Exception
	{
		internal Unsortable(Type type) :
			this(null, type)
		{ }
		internal Unsortable(System.Exception exception, Type type) :
			base(exception, Error.Level.Warning, "Unsortable Collection.", "It is not possible to sort a collection with items of type \"{0}\" since it does not implement one of \"Kean.IComparable<{0}>\", \"System.IComparable<{0}>\" or \"System.IComparable\". Either extend the type so that it implements one of the interfaces or provide a custom ordering function.", type.Name)
		{ }
	}
}
