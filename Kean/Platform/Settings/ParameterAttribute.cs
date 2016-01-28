// 
//  ParameterAttribute.cs
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

namespace Kean.Platform.Settings
{
	public class ParameterAttribute :
		Attribute
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public string Usage { get; set; }
		public string[] Values { get; set; }
		public ParameterAttribute(string name, string description)
		{
			this.Name = name;
			this.Description = description;
		}
		public ParameterAttribute(string name, string description, string usage)
		{
			this.Usage = usage;
		}
		public ParameterAttribute(string name, string description, params string[] values) :
			this(name, description, "[" + string.Join(" | ", values) + "]")
		{
			this.Values = values;
		}
	}
}

