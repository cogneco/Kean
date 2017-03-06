// Copyright (C) 2012	Simon Mika <simon@mika.se>
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
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.	See the
// GNU Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with Kean.	If not, see <http://www.gnu.org/licenses/>.
//

using System;
using Kean.Extension;
using Kean.Collection.Extension;
using Generic = System.Collections.Generic;

namespace Kean.Text
{
	public class Builder :
	Generic.IEnumerable<char>,
	IEquatable<Generic.IEnumerable<char>>,
	IEquatable<string>,
	IEquatable<char[]>,
	IEquatable<Builder>
	{
		Collection.IList<Generic.IEnumerable<char>> data;
		public int Length { get { return this.data.Fold((item, result) => result + item.Count(), 0); } }
		public Builder()
		{
			this.data = new Collection.List<Generic.IEnumerable<char>>();
		}
		public Builder(string value) :
			this((Generic.IEnumerable<char>)value)
		{ }
		public Builder(Generic.IEnumerable<char> value) :
			this()
		{
			this.Append(value);
		}
		public Builder(Generic.IEnumerator<char> value) :
			this()
		{
			this.Append(value);
		}
		protected Builder(Builder original) :
			this()
		{
			original.data.Apply(item => this.Append(item));
		}
		public Builder Copy()
		{
			return new Builder(this);
		}
		#region Append
		public Builder Append(Builder value)
		{
			this.data.Add(value.data);
			return this;
		}
		public Builder Append(char value)
		{
			return this.Append(Enumerable.Create(value));
		}
		public Builder Append(params char[] value)
		{
			return this.Append(new string(value));
		}
		public Builder Append(string format, params object[] arguments)
		{
			return this.Append(string.Format(format, arguments));
		}
		public Builder Append(string value)
		{
			return this.Append((Generic.IEnumerable<char>)value);
		}
		public Builder Append(Generic.IEnumerable<char> value)
		{
			this.data.Add(value);
			return this;
		}
		public Builder Append(Generic.IEnumerator<char> value)
		{
			this.data.Add(value.ToArray());
			return this;
		}
		#endregion
		#region Prepend
		public Builder Prepend(Builder value)
		{
			value.data.Apply(item => this.Prepend(item));
			return this;
		}
		public Builder Prepend(char value)
		{
			return this.Prepend(Enumerable.Create(value));
		}
		public Builder Prepend(string format, params object[] arguments)
		{
			return this.Prepend(string.Format(format, arguments));
		}
		public Builder Prepend(string value)
		{
			return this.Prepend((Generic.IEnumerable<char>)value);
		}
		public Builder Prepend(Generic.IEnumerable<char> value)
		{
			this.data.Insert(0, value);
			return this;
		}
		public Builder Prepend(Generic.IEnumerator<char> value)
		{
			this.data.Add(value.ToArray());
			return this;
		}
		#endregion
		#region Casts
		#region string
		public static implicit operator string(Builder builder)
		{
			string result;
			if (builder.NotNull())
			{
				result = builder.Join();
				builder.data = new Collection.List<Generic.IEnumerable<char>>();
				builder.Append(result);
			}
			else
				result = null;
			return result;
		}
		public static implicit operator Builder(string value)
		{
			return new Builder(value);
		}
		#endregion
		#region char
		public static implicit operator Builder(char value)
		{
			return new Builder(new string(value, 1));
		}
		#endregion
		#region char[]
		public static explicit operator char[](Builder value)
		{
			return value.ToArray();
		}
		public static implicit operator Builder(char[] value)
		{
			return new Builder(new string(value));
		}
		#endregion
		#endregion
		#region Binary operators
		#region Builder
		public static Builder operator +(Builder left, Builder right)
		{
			return left.NotNull() ? left.Copy().Append(right) : new Builder().Append(right);
		}
		#endregion
		#region string
		public static Builder operator +(Builder left, string right)
		{
			return left.NotNull() ? left.Copy().Append(right) : new Builder(right);
		}
		public static Builder operator +(string left, Builder right)
		{
			return right.NotNull() ? right.Copy().Prepend(left) : new Builder(left);
		}
		#endregion
		#region char
		public static Builder operator +(Builder left, char right)
		{
			return left.NotNull() ? left.Copy().Append(right) : new Builder(right);
		}
		public static Builder operator +(char left, Builder right)
		{
			return right.NotNull() ? right.Copy().Prepend(left) : new Builder(left);
		}
		#endregion
		#region char[]
		public static Builder operator +(Builder left, char[] right)
		{
			return left.NotNull() ? left.Copy().Append(right) : new Builder(right);
		}
		public static Builder operator +(char[] left, Builder right)
		{
			return right.NotNull() ? right.Copy().Prepend(left) : new Builder(left);
		}
		#endregion
		#region IEnumerable<char>
		public static Builder operator +(Builder left, Generic.IEnumerable<char> right)
		{
			return left.NotNull() ? left.Copy().Append(right) : new Builder(right);
		}
		public static Builder operator +(Generic.IEnumerable<char> left, Builder right)
		{
			return right.NotNull() ? right.Copy().Prepend(left) : new Builder(left);
		}
		#endregion
		#region IEnumerator<char>
		public static Builder operator +(Builder left, Generic.IEnumerator<char> right)
		{
			return left.NotNull() ? left.Copy().Append(right) : new Builder(right);
		}
		public static Builder operator +(Generic.IEnumerator<char> left, Builder right)
		{
			return right.NotNull() ? right.Copy().Prepend(left) : new Builder(left);
		}
		#endregion
		#endregion
		#region Object Overrides
		public override int GetHashCode()
		{
			return ((string)this).GetHashCode();
		}
		public override bool Equals(object other)
		{
			return ((string)this).Equals(other);
		}
		public override string ToString()
		{
			return this;
		}
		#endregion
		#region IEquatable implementation
		public bool Equals(Generic.IEnumerable<char> other)
		{
			return ((Generic.IEnumerable<char>)this).SameOrEquals(other);
		}
		public bool Equals(Builder other)
		{
			return this.Equals(other as Generic.IEnumerable<char>);
		}
		public bool Equals(string other)
		{
			return this.Equals(other as Generic.IEnumerable<char>);
		}
		public bool Equals(char[] other)
		{
			return this.Equals(other as Generic.IEnumerable<char>);
		}
		#endregion
		#region IEnumerable implementation
		public Generic.IEnumerator<char> GetEnumerator()
		{
			foreach (var part in this.data)
				if (part.NotNull())
					foreach (var c in part)
						yield return c;
		}
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
		#endregion
	}
}
