// Copyright (C) 2011, 2012, 2017  Simon Mika <simon@mika.se>
//
// This file is part of Kean.
//
// Kean is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Kean is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with Kean.  If not, see <http://www.gnu.org/licenses/>.
//

using System;
using Kean.Extension;

namespace Kean.Text
{
	public struct Fragment :
		IEquatable<Fragment>
	{
		public string Content { get; }
		public Position Start { get; }
		public Position End { get; }
		public Uri.Locator Resource { get; }
		public Fragment(string content, Position start, Position end, Uri.Locator resource)
		{
			this.Content = content;
			this.Start = start;
			this.End = end;
			this.Resource = resource;
		}
		#region Object Overrides
		public override bool Equals(object other)
		{
			return other is Fragment && this.Equals((Fragment)other);
		}
		public override int GetHashCode()
		{
			return this.Content.Hash(this.Resource, this.Start, this.End);
		}
		public override string ToString()
		{
			return string.Format("{3} ({1} - {2}) [{0}]", this.Content, this.Start, this.End, this.Resource);
		}
		#endregion<
		#region IEquatable<Fragment> Members
		public bool Equals(Fragment other)
		{
			return this.Content == other.Content && this.Start == other.Start && this.End == other.End && this.Resource == other.Resource;
		}
		#endregion
		#region Operators
		public static bool operator ==(Fragment left, Fragment right)
		{
			return left.Equals(right);
		}
		public static bool operator !=(Fragment left, Fragment right)
		{
			return !(left == right);
		}
		public static Fragment operator +(Fragment left, Fragment right)
		{
			return new Fragment(left.Content + right.Content, left.Start < right.Start ? left.Start : right.Start, left.End > right.End ? left.End : right.End, left.Resource);
		}
		#endregion
	}
}