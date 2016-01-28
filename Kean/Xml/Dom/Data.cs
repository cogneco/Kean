// 
//  Data.cs
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
	public class Data :
		Node
	{
		public string Value { get; set; }
		public Data() { }
		public Data(string value)
		{
			this.Value = value;
		}
		#region Object Overrides
		public override bool Equals(object other)
		{
			return this.Equals(other as Data);
		}
		public override int GetHashCode()
		{
			return this.Value.Hash();
		}
		public override string ToString()
		{
			return base.ToString();
		}
		#endregion
		#region IEquatable<Data> Members
		public bool Equals(Data other)
		{
			return other.NotNull() &&
				this.Value == other.Value;
		}
		#endregion
		#region Operators
		public static bool operator ==(Data left, Data right)
		{
			return left.Same(right) || left.NotNull() && left.Equals(right);
		}
		public static bool operator !=(Data left, Data right)
		{
			return !(left == right);
		}
		#endregion
	}
}
