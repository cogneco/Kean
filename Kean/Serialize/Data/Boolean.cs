// 
//  Boolean.cs
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
using Kean.Extension;
using Collection = Kean.Collection;
using Kean.Collection.Extension;

namespace Kean.Serialize.Data
{
	public class Boolean :
		Leaf<bool>
	{
		public override string Text { get { return this.Value ? "true" : "false"; } }
		public override byte[] Raw { get { return new byte[] { Convert.ToByte(this.Value) }; } }
		public Boolean(bool value) :
			base(value)
		{ }
		public Boolean(object value, Reflect.Type type) :
			base(value, type)
		{ }
	}
}
