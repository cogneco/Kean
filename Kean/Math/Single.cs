// 
//  Single.cs
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
    public partial class Single :
        Abstract<Single, float>
    {
        #region Abtract Properties
        protected override Single EpsilonHelper { get { return Single.Epsilon; } }
        protected override Single PiHelper { get { return Single.Pi; } }
        #endregion
        #region Constructors
        public Single() :
            base(0)
        { }
        public Single(float value) :
            base(value)
        { }
        #endregion
        public override Single CreateConstant(int value)
        {
            return new Single(value);
        }
        #region Functions
        #region Arithmetic Functions
        public override Single Add(float value)
        {
            return new Single(this.Value + value);
        }
        public override Single Substract(float value)
        {
            return new Single(this.Value - value);
        }
        public override Single Multiply(float value)
        {
            return new Single(this.Value * value);
        }
        public override Single Divide(float value)
        {
            return new Single(this.Value / value);
        }
        public override Single Negate()
        {
            return new Single(-this.Value);
        }
        public override Single Invert()
        {
            return new Single(1 / this.Value);
        }
        #endregion
        #region Trigonometric Helpers
        public override Single ToRadians()
        {
            return Single.Pi / 180 * this;
        }
        public override Single ToDegrees()
        {
            return 180 / Single.Pi * this;
        }
        #endregion
		#region Utility Functions
		public override Single Round()
		{
			return Single.Round(this);
		}
		public override Single Ceiling()
		{
			return Single.Ceiling(this);
		}
		public override Single Floor()
		{
			return Single.Floor(this);
		}
		#endregion
		#region Trigonometric Functions
        public override Single Sinus()
        {
            return Single.Sinus(this.Value);
        }
        public override Single Cosinus()
        {
            return Single.Cosinus(this.Value);
        }
        public override Single Tangens()
        {
            return Single.Tangens(this.Value);
        }
        #endregion
        #region Inverse Trigonometric Functions
        public override Single ArcusSinus()
        {
            return Single.ArcusSinus(this.Value);
        }
        public override Single ArcusCosinus()
        {
            return Single.ArcusCosinus(this.Value);
        }
        public override Single ArcusTangens()
        {
            return Single.ArcusTangens(this.Value);
        }
        public override Single ArcusTangensExtended(Single x)
        {
            return Single.ArcusTangensExtended(this.Value, x);
        }
        #endregion
        #region Transcendental Functions
        public override Single Exponential()
        {
            return Single.Exponential(this.Value);
        }
        public override Single Logarithm()
        {
            return Single.Logarithm(this.Value);
        }
        public override Single Logarithm(Single @base)
        {
            return Single.Logarithm(this.Value, @base);
        }
        #endregion
        #region Power Function
        public override Single Power(Single exponent)
        {
            return Single.Power(this.Value, exponent);
        }
        public override Single SquareRoot()
        {
            return Single.SquareRoot(this.Value);
        }
        public override Single Squared()
        {
            return this.Value * this.Value;
        }
        #endregion
        #region Comparison Functions
        public override bool LessThan(Single other)
        {
            return this.Value < other.Value;
        }
        public override bool GreaterThan(Single other)
        {
            return this.Value > other.Value;
        }
        #endregion
        #endregion
        #region Cast Operators
        public static implicit operator float(Single value)
        {
            return value.IsNull() ? 0 : value.Value;
        }
        public static implicit operator Single(float value)
        {
            return new Single(value);
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

