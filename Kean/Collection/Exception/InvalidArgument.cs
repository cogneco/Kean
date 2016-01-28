// 
//  InvalidArgument.cs
//  
//  Author:
//       smika <${AuthorEmail}>
//  
//  Copyright (c) 2010 smika
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
	public class InvalidArgument :
		Exception
	{
		internal InvalidArgument() :
			this(null as System.Exception)
		{ }
		internal InvalidArgument(string message) :
			this(null, message)
		{ }
		internal InvalidArgument(string message, params object[] arguments) :
			this(null, message, arguments)
		{ }
		internal InvalidArgument(System.Exception exception) :
			this(exception, "Argument has an invalid value.")
		{ }
		internal InvalidArgument(System.Exception exception, string message, params object[] arguments) :
			base(exception, Error.Level.Warning, "Invalid Argument.", message, arguments)
		{ }
	}
}
