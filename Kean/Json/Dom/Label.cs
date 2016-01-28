//
//  Label.cs
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
	public class Label :
		IEquatable<Label>
	{
		public string Value { get; private set; }
		public Uri.Region Region { get; private set; }
		public Label (string label)
		{
			this.Value = label;
		}
		public Label (string label, Uri.Region region) :
			this(label)
		{
			this.Region = region;
		}
		#region Object Overrides
		public override string ToString()
		{
			return this.Value;
		}
		#endregion
		#region Equals implementation
		public override bool Equals(object other)
		{
			return this.Equals(other is string ? (Label)(string)other : other as Label);
		}
		public override int GetHashCode()
		{
			return this.Value.Hash();
		}
		public bool Equals (Label other)
		{
			return other.NotNull() && this.Value ==  other.Value;
		}
		public static bool operator ==(Label left, Label right)
		{
			return left.Same(right) || left.NotNull() && left.Equals(right);
		}
		public static bool operator !=(Label left, Label right)
		{
			return !left.Same(right) && (left.IsNull() || !left.Equals(right));
		}
		#endregion
		public static implicit operator string(Label label)
		{
			return label.Value;
		}
		public static implicit operator Label(string label)
		{
			return new Label(label);
		}
	}
}

