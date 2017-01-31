//
//  Abstract.cs
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

namespace Kean.Math
{
	public abstract class Abstract<R, V> :
		IEquatable<R>,
		System.IComparable<R>
		where R : Abstract<R, V>, new()
		where V : struct
	{
		public V Value { get; private set; }

		#region Abstract Properties
		protected R ZeroHelper { get { return this.CreateConstant(0); } }
		protected R OneHelper { get { return this.CreateConstant(1); } }
		protected R TwoHelper { get { return this.CreateConstant(2); } }
		protected abstract R PiHelper { get; }
		protected abstract R EpsilonHelper { get; }
		#endregion

		#region Static Constants
		public static R Zero { get { return new R().ZeroHelper; } }
		public static R One { get { return new R().OneHelper; } }
		public static R Two { get { return new R().TwoHelper; } }
		public static R PI { get { return new R().PiHelper; } }
		public static R Precision { get { return new R().EpsilonHelper; } }
		#endregion

		#region Constructors
		protected Abstract(V value)
		{
			this.Value = value;
		}
		#endregion

		public abstract R CreateConstant(int value);
		public R Copy()
		{
			return new R() { Value = this.Value };
		}

		#region Artihmetic Operators
		public static R operator +(Kean.Math.Abstract<R, V> left, Kean.Math.Abstract<R, V> right)
		{
			return left.Add(right);
		}
		public static R operator -(Kean.Math.Abstract<R, V> left, Kean.Math.Abstract<R, V> right)
		{
			return left.Substract(right);
		}
		public static R operator -(Kean.Math.Abstract<R, V> value)
		{
			return value.Negate();
		}
		public static R operator *(Kean.Math.Abstract<R, V> left, Kean.Math.Abstract<R, V> right)
		{
			return left.Multiply(right);
		}
		public static R operator /(Kean.Math.Abstract<R, V> left, Kean.Math.Abstract<R, V> right)
		{
			return left.Divide(right);
		}
		#endregion

		#region Static Functions
		public static R Absolute(R value)
		{
			return value.LessThan(Kean.Math.Abstract<R, V>.Zero) ? -value : value;
		}
		public static R Maximum(R value, params R[] values)
		{
			foreach (R v in values)
				if (value < v)
					value = v;
			return value;
		}
		public static R Minimum(R value, params R[] values)
		{
			foreach (R v in values)
				if (value > v)
					value = v;
			return value;
		}
		#endregion

		#region Functions
		#region Arithmetic Functions
		public abstract R Add(V value);
		public abstract R Substract(V value);
		public abstract R Multiply(V value);
		public abstract R Divide(V value);
		public abstract R Negate();
		public abstract R Invert();
		#endregion

		#region Trigonometric Helpers
		public abstract R ToRadians();
		public abstract R ToDegrees();
		#endregion

		#region Utility Functions
		public R Clamp(R floor, R ceiling)
		{
			R result = null;
			if (this > ceiling)
				result = ceiling;
			else if (this < floor)
				result = floor;
			else
				result = (R)this;
			return result;
		}
		public abstract R Round();
		public abstract R Ceiling();
		public abstract R Floor();
		#endregion

		#region Trigonometric Functions
		public abstract R Sine();
		public abstract R Cosine();
		public abstract R Tangens();
		#endregion

		#region Inverse Trigonometric Functions
		public abstract R ArcusSinus();
		public abstract R ArcusCosinus();
		public abstract R ArcusTangens();
		public abstract R ArcusTangensExtended(R x);
		#endregion

		#region Transcendental Functions
		public abstract R Exponential();
		public abstract R Logarithm();
		public abstract R Logarithm(R @base);
		#endregion

		#region Power Function
		public abstract R Power(R value);
		public abstract R SquareRoot();
		public abstract R Squared();
		#endregion

		#region Object overides and IEquatable<R>
		public override bool Equals(object other)
		{
			return (other is R) && this.Equals(other as R);
		}
		// other is not null here.
		public bool Equals(R other)
		{
			return this.Value.Equals(other.Value);
		}
		public override int GetHashCode()
		{
			return this.Value.GetHashCode();
		}
		#endregion

		#region Comparison Functions and IComparable<R>
		public static bool operator ==(Kean.Math.Abstract<R, V> left, Kean.Math.Abstract<R, V> right)
		{
			return
				object.ReferenceEquals(left, right) || (!object.ReferenceEquals(left, null) && !object.ReferenceEquals(right, null)) &&
				left.Equals(right);
		}
		public static bool operator !=(Kean.Math.Abstract<R, V> left, Kean.Math.Abstract<R, V> right)
		{
			return !(left == right);
		}
		public static bool operator <(Abstract<R, V> left, R right)
		{
			return left.LessThan(right);
		}
		public static bool operator <=(Abstract<R, V> left, R right)
		{
			return left < right || left == right;
		}
		public static bool operator >(Abstract<R, V> left, R right)
		{
			return left.GreaterThan(right);
		}
		public static bool operator >=(Abstract<R, V> left, R right)
		{
			return left > right || left == right;
		}
		public abstract bool LessThan(R other);
		public bool LessOrEqualThan(R other)
		{
			return this.LessThan(other) || this == other;
		}
		public abstract bool GreaterThan(R other);
		public bool GreaterOrEqualThan(R other)
		{
			return this.GreaterThan(other) || this == other;
		}
		public int CompareTo(R other)
		{
			return this.LessThan(other) ? -1 : (this.GreaterThan(other) ? 1 : 0);
		}
		#endregion

		#endregion

		#region SetValue between class and struct.
		public static implicit operator V(Abstract<R, V> value)
		{
			return value.Value;
		}
		public static implicit operator Abstract<R, V>(V value)
		{
			return new R() { Value = value };
		}
		#endregion

	}
}

