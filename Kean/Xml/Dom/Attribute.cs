// 
//  Attribute.cs
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
using Kean.Extension;

namespace Kean.Xml.Dom
{
	public class Attribute :
		Object
	{
		public string Name { get; set; }
		public string Value { get; set; }
		public Attribute() { }
		public Attribute(string name) :
			this()
		{
			this.Name = name;
		}
		public Attribute(string name, string value) :
			this(name)
		{
			this.Value = value;
		}
		#region Object Overrides
		public override bool Equals(object other)
		{
			return this.Equals(other as Attribute);
		}
		public override int GetHashCode()
		{
			return this.Name.Hash() ^ this.Value.Hash();
		}
		public override string ToString()
		{
			return base.ToString();
		}
		#endregion
		#region IEquatable<Attribute> Members
		public bool Equals(Attribute other)
		{
			return other.NotNull() &&
				this.Name == other.Name &&
				this.Value == other.Value;
		}
		#endregion
		#region Operators
		public static bool operator ==(Attribute left, Attribute right)
		{
			return left.Same(right) || left.NotNull() && left.Equals(right);
		}
		public static bool operator !=(Attribute left, Attribute right)
		{
			return !(left == right);
		}
		#endregion
	}
}
