// 
//  Region.cs
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

namespace Kean.Uri
{
	public class Region :
		IEquatable<Region>
	{
		public Locator Resource { get; private set; }
		public Position Start { get; private set; }
		public Position End { get; private set; }
		public Region(Locator resource, Position start, Position end)
		{
			this.Resource = resource;
			this.Start = start;
			this.End = end;
		}
		#region Object Overrides
		public override bool Equals(object other)
		{
			return this.Equals(other as Region);
		}
		public override int GetHashCode()
		{
			return this.Resource.Hash() ^ this.Start.Hash() ^ this.End.Hash();
		}
		public override string ToString()
		{
			return string.Format("{0} ({1} - {2})", this.Resource, this.Start, this.End);
		}
		#endregion
		#region IEquatable<Region> Members
		public bool Equals(Region other)
		{
			return other.NotNull() && this.Resource == other.Resource && this.Start == other.Start && this.End == other.End;
		}
		#endregion
		#region Operators
		public static bool operator ==(Region left, Region right)
		{
			return left.Same(right) || left.NotNull() && left.Equals(right);
		}
		public static bool operator !=(Region left, Region right)
		{
			return !(left == right);
		}
		public static Region operator +(Region left, Region right)
		{
			return left.NotNull() && right.NotNull() && left.Resource == right.Resource ? new Region(left.Resource, left.Start < right.Start ? left.Start : right.Start, left.End > right.End ? left.End : right.End) : null;
		}
		#endregion
	}
}
