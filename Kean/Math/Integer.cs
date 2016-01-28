// 
//  Integer.cs
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
namespace Kean.Math
{
	public partial class Integer :
		Abstract<Integer, int>
	{
		#region Abtract Properties
		protected override Integer EpsilonHelper { get { return Integer.Epsilon; } }
		protected override Integer PiHelper { get { return Integer.Pi; } }
		#endregion
		#region Constructors
		public Integer() :
			base(0)
		{ }
		public Integer(int value) :
			base(value)
		{ }
		#endregion
		public override Integer CreateConstant(int value)
		{
			return new Integer(value);
		}
		#region Functions
		#region Arithmetic Functions
		public override Integer Add(int value)
		{
			return new Integer(this.Value + value);
		}
		public override Integer Substract(int value)
		{
			return new Integer(this.Value - value);
		}
		public override Integer Multiply(int value)
		{
			return new Integer(this.Value * value);
		}
		public override Integer Divide(int value)
		{
			return new Integer(this.Value / value);
		}
		public override Integer Negate()
		{
			return new Integer(-this.Value);
		}
		public override Integer Invert()
		{
			throw new Exception.NotAllowed();
		}
		#endregion
		#region Trigonometric Helpers
		public override Integer ToRadians()
		{
			return Integer.Pi / 180 * this;
		}
		public override Integer ToDegrees()
		{
			return 180 / Integer.Pi * this;
		}
		#endregion
		#region Utility Functions
		public override Integer Round()
		{
			return Integer.Round(this);
		}
		public override Integer Ceiling()
		{
			return Integer.Ceiling(this);
		}
		public override Integer Floor()
		{
			return Integer.Floor(this);
		}
		#endregion
		#region Trigonometric Functions
		public override Integer Sinus()
		{
			return Integer.Sinus(this.Value);
		}
		public override Integer Cosinus()
		{
			return Integer.Cosinus(this.Value);
		}
		public override Integer Tangens()
		{
			return Integer.Tangens(this.Value);
		}
		#endregion
		#region Inverse Trigonometric Functions
		public override Integer ArcusSinus()
		{
			return Integer.ArcusSinus(this.Value);
		}
		public override Integer ArcusCosinus()
		{
			return Integer.ArcusCosinus(this.Value);
		}
		public override Integer ArcusTangens()
		{
			return Integer.ArcusTangens(this.Value);
		}
		public override Integer ArcusTangensExtended(Integer x)
		{
			return Integer.ArcusTangensExtended(this.Value, x);
		}
		#endregion
		#region Transcendental Functions
		public override Integer Exponential()
		{
			return Integer.Exponential(this.Value);
		}
		public override Integer Logarithm()
		{
			return Integer.Logarithm(this.Value);
		}
		public override Integer Logarithm(Integer @base)
		{
			return Integer.Logarithm(this.Value, @base);
		}
		#endregion
		#region Power Function
		public override Integer Power(Integer exponent)
		{
			return Integer.Power(this.Value, exponent);
		}
		public override Integer SquareRoot()
		{
			return Integer.SquareRoot(this.Value);
		}
		public override Integer Squared()
		{
			return this.Value * this.Value;
		}
		#endregion
		#region Comparison Functions
		public override bool LessThan(Integer other)
		{
			return this.Value < other.Value;
		}
		public override bool GreaterThan(Integer other)
		{
			return this.Value > other.Value;
		}
		#endregion
		#endregion
		#region Cast Operators
		public static implicit operator int(Integer value)
		{
			return value.IsNull() ? 0 : value.Value;
		}
		public static implicit operator Integer(int value)
		{
			return new Integer(value);
		}
		public static implicit operator Single(Integer value)
		{
			return new Single(value.Value);
		}
		public static explicit operator Integer(Single value)
		{
			return new Integer(System.Convert.ToInt32(value.Value));
		}
		#endregion
		#region Object overides
		public override string ToString()
		{
			return this.Value.ToString(System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
		}
		#endregion
	}
}


