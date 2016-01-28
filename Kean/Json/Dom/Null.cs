//
//  Null.cs
//
//  Author:
//       Simon Mika <smika@hx.se>
//
//  Copyright (c) 2012 Simon Mika
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
using Uri = Kean.Uri;

namespace Kean.Json.Dom
{
	public class Null :
		Primitive<object>,
		IEquatable<Null>
	{
		public Null () :
			base(null)
		{ }
		public Null (Uri.Region region) :
			base(null, region)
		{ }
		#region IEquatable implementation
		public override int GetHashCode()
		{
			return 0;
		}
		public override bool Equals(object other)
		{
			return other is Null && this.Equals(other as Null);
		}
        public override bool Equals(Item other)
        {
            return this.Equals(other as Null);
        }
		public bool Equals (Null other)
		{
			return other.NotNull();
		}
		public static bool operator ==(Null left, Null right)
		{
			return left.Same(right) || left.NotNull() && left.Equals(right);
		}
		public static bool operator !=(Null left, Null right)
		{
			return !left.Same(right) || left.IsNull() || !left.Equals(right);
		}
		#endregion
	}
}

