// 
//  MethodAttribute.cs
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
using Kean;

namespace Kean.Platform.Settings
{
	[AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
	public class MethodAttribute :
		MemberAttribute
	{
		public string Default { get; set; }

		public MethodAttribute(string name) :
			this(name, null)
		{ }
		public MethodAttribute(string name, string description) :
			base(name, description, null)
		{ }
		public MethodAttribute(string name, string description, string usage) :
			base(name, description, usage)
		{ }
		public MethodAttribute(string name, string description, string usage, string example) :
			base(name, description, usage, example)
		{ }
		public MethodAttribute(string name, string description, string usage, string example, string @default) :
			base(name, description, usage, example)
		{
			this.Default = @default;
		}
	}
}

