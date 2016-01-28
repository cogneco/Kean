// 
//  Boolean.cs
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
using Kean;
using Kean.Extension;
using Reflect = Kean.Reflect;

namespace Kean.Platform.Settings.Parameter
{
	class Boolean :
		Abstract
	{
		internal Boolean(Reflect.Type type) :
			base(type)
		{
		}
		public override string AsString(object value)
		{
			return (bool)value ? "true" : "false";
		}
		public override object FromString(string value)
		{
			return value.Trim().ToLower() == "true";
		}
		public override string Complete(string incomplete)
		{
			string result = "";
			if (incomplete.NotEmpty())
				switch (char.ToLower(incomplete[0]))
				{
					case 't': result = "true "; break;
					case 'f': result = "false "; break;
				}
			return result;
		}
		public override string Help(string incomplete)
		{
			return "true\nfalse\n";
		}
	}
}
