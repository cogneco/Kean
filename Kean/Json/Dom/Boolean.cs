//
//  Boolean.cs
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
	public class Boolean :
		Primitive<bool>,
		IEquatable<Boolean>
	{
		public Boolean (bool value) :
			base(value)
		{
		}
		public Boolean (bool value, Uri.Region region) :
			base(value, region)
		{
		}
		#region IEquatable implementation
		public override int GetHashCode ()
		{
			return this.Value.GetHashCode ();
		}
		public override bool Equals (object other)
		{
			return other is Boolean && this.Equals (other as Boolean);
		}
		public override bool Equals (Item other)
		{
			return this.Equals (other as Boolean);
		}
		public bool Equals (Boolean other)
		{
			return other.NotNull () && this.Value == other.Value;
		}
		public static bool operator == (Boolean left, Boolean right)
		{
			return left.Same (right) || left.NotNull () && left.Equals (right);
		}
		public static bool operator != (Boolean left, Boolean right)
		{
			return !left.Same (right) || left.IsNull () || !left.Equals (right);
		}
		#endregion
		#region Casts
		public static implicit operator bool (Boolean item)
		{
			return item.NotNull () ? item.Value : false;
		}
		public static implicit operator Boolean (bool value)
		{
			return new Boolean (value);
		}
		#endregion
	}
}


