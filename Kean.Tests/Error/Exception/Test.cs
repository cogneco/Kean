// 
//  Test.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2010 Simon Mika
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
using Target = Kean.Error;
namespace Kean.Error.Test.Exception
{
	public class Test :
		Abstract
	{
		internal Test (Target.Level level, string title, string message, params object[] arguments) :
			this(null, level, title, message, arguments)
		{ }
		internal Test (Abstract exception, Target.Level level, string title, string message, params object[] arguments) :
			base(exception, level, title, message, arguments)
		{ }
		internal static void Check(Target.Level level, string title, string message, params object[] arguments)
		{
			new Exception.Test(level, title, message, arguments).Throw();
		}
	}
}

