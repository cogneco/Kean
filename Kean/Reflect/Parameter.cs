// 
//  Parameter.cs
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
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.using System;

using Kean;
using Kean.Extension;
using Kean.Reflect.Extension;
using Kean.Collection.Extension;

namespace Kean.Reflect
{
	public class Parameter
	{
		public string Name { get { return this.information.Name; } }
		public Type Type { get { return this.information.ParameterType; } }
		System.Reflection.ParameterInfo information;
		internal Parameter(System.Reflection.ParameterInfo information)
		{
			this.information = information;
		}
		public T[] GetAttributes<T>()
			where T : System.Attribute
		{
			return System.Attribute.GetCustomAttributes(this.information, typeof(T), true).Map(attribute => attribute as T);
		}
	}
}
