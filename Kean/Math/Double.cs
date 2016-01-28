// 
//  Double.cs
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
    public partial class Double :
        Abstract<Double, double>
    {
        #region Abtract Properties
        protected override Double EpsilonHelper { get { return Double.Epsilon; } }
        protected override Double PiHelper { get { return Double.PI; } }
        #endregion
        #region Constructors
        public Double() :
            base(0)
        { }
        public Double(double value) :
            base(value)
        { }
        #endregion
        public override Double CreateConstant(int value)
        {
            return new Double(value);
        }
        #region Functions
        #region Arithmetic Functions
        public override Double Add(double value)
        {
            return new Double(this.Value + value);
        }
        public override Double Substract(double value)
        {
            return new Double(this.Value - value);
        }
        public override Double Multiply(double value)
        {
            return new Double(this.Value * value);
        }
        public override Double Divide(double value)
        {
            return new Double(this.Value / value);
        }
        public override Double Negate()
        {
            return new Double(-this.Value);
        }
        public override Double Invert()
        {
            return new Double(1 / this.Value);
        }
        #endregion
        #region Trigonometric Helpers
        public override Double ToRadians()
        {
            return Double.PI / 180 * this;
        }
        public override Double ToDegrees()
        {
            return 180 / Double.PI * this;
        }
        #endregion
		#region Utility Functions
		public override Double Round()
		{
			return Double.Round(this);
		}
		public override Double Ceiling()
		{
			return Double.Ceiling(this);
		}
		public override Double Floor()
		{
			return Double.Floor(this);
		}
		#endregion
		#region Trigonometric Functions
        public override Double Sinus()
        {
            return Double.Sinus(this.Value);
        }
        public override Double Cosinus()
        {
            return Double.Cosinus(this.Value);
        }
        public override Double Tangens()
        {
            return Double.Tangens(this.Value);
        }
        #endregion
        #region Inverse Trigonometric Functions
        public override Double ArcusSinus()
        {
            return Double.ArcusSinus(this.Value);
        }
        public override Double ArcusCosinus()
        {
            return Double.ArcusCosinus(this.Value);
        }
        public override Double ArcusTangens()
        {
            return Double.ArcusTangens(this.Value);
        }
        public override Double ArcusTangensExtended(Double x)
        {
            return Double.ArcusTangensExtended(this.Value, x);
        }
        #endregion
        #region Transcendental Functions
        public override Double Exponential()
        {
            return Double.Exponential(this.Value);
        }
        public override Double Logarithm()
        {
            return Double.Logarithm(this.Value);
        }
        public override Double Logarithm(Double @base)
        {
            return Double.Logarithm(this.Value, @base);
        }
        #endregion
        #region Power Function
        public override Double Power(Double exponent)
        {
            return Double.Power(this.Value, exponent);
        }
        public override Double SquareRoot()
        {
            return Double.SquareRoot(this.Value);
        }
        public override Double Squared()
        {
            return this.Value * this.Value;
        }
        #endregion
        #region Comparison Functions
        public override bool LessThan(Double other)
        {
            return this.Value < other.Value;
        }
        public override bool GreaterThan(Double other)
        {
            return this.Value > other.Value;
        }
        #endregion
        #endregion
        #region Cast Operators
        public static implicit operator double(Double value)
        {
            return value.IsNull() ? 0 : value.Value;
        }
        public static implicit operator Double(double value)
        {
            return new Double(value);
        }
        public static implicit operator Double(Single value)
        {
            return new Double(value.Value);
        }
        public static implicit operator Double(Integer value)
        {
            return new Double(value.Value);
        }
        public static explicit operator Single(Double value)
        {
            return new Single(System.Convert.ToSingle(value.Value));
        }
        public static implicit operator Integer(Double value)
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

