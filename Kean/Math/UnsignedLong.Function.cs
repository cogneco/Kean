// 
//  Integer.Function.cs
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
    public class UnsignedLong
    {
        #region Constants
        public static ulong NegativeInfinity { get { return ulong.MinValue; } }
        public static ulong PositiveInfinity { get { return ulong.MaxValue; } }
        public static ulong Epsilon { get { return 1; } }
        public static ulong MinimumValue { get { return ulong.MinValue; } }
        public static ulong MaximumValue { get { return ulong.MaxValue; } }
        public static ulong Pi { get { return UnsignedLong.Convert(System.Math.PI); } }
        public static ulong E { get { return UnsignedLong.Convert(System.Math.E); } }
        #endregion
        #region Convert Functions
        public static ulong Convert(double value)
        {
			return System.Convert.ToUInt64(value);
        }
        public static ulong Convert(float value)
        {
            return System.Convert.ToUInt64(value);
        }
        /// <summary>
        /// Parses a string to a ulong
        /// </summary>
        /// <exception cref="System.FormatException">When string does not contain a int</exception>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ulong Parse(string value)
        {
            return ulong.Parse(value, System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
        }
        public static string ToString(ulong value)
        {
            return value.ToString(System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
        }
        #endregion
        #region Utility Functions
        public static ulong Clamp(ulong value, ulong floor, ulong ceiling)
        {
            if (value > ceiling)
                value = ceiling;
            else if (value < floor)
                value = floor;
            return value;
        }
        public static ulong Maximum(ulong first, ulong second)
        {
            return first > second ? first : second;
        }
        public static ulong Maximum(ulong value, params ulong[] values)
        {
            foreach (ulong v in values)
                if (value < v)
                    value = v;
            return value;
        }
        public static ulong Minimum(ulong first, ulong second)
        {
            return first < second ? first : second;
        }
        public static ulong Minimum(ulong value, params ulong[] values)
        {
            foreach (ulong v in values)
                if (value > v)
                    value = v;
            return value;
        }
        public static ulong Modulo(ulong dividend, ulong divisor)
        {
            if (dividend < 0)
                dividend += UnsignedLong.Ceiling(dividend / (float)divisor) * divisor;
            return dividend % divisor;
        }
        #endregion
        #region Rounding Functions
        public static ulong Floor(float value)
        {
            return UnsignedLong.Convert(System.Math.Floor(value));
        }
        public static ulong Floor(double value)
        {
            return UnsignedLong.Convert(System.Math.Floor(value));
        }
        public static ulong Ceiling(float value)
        {
            return UnsignedLong.Convert(System.Math.Ceiling(value));
        }
        public static ulong Ceiling(double value)
        {
            return UnsignedLong.Convert(System.Math.Ceiling(value));
        }
        public static ulong Truncate(float value)
        {
            return UnsignedLong.Convert(System.Math.Truncate(value));
        }
        public static ulong Truncate(double value)
        {
            return UnsignedLong.Convert(System.Math.Truncate(value));
        }
        public static ulong Round(float value)
        {
            return UnsignedLong.Convert(System.Math.Round(value));
        }
        public static ulong Round(double value)
        {
            return UnsignedLong.Convert(System.Math.Round(value));
        }
        #endregion
        #region Transcendental and Power Functions
        public static ulong Exponential(ulong value)
        {
            return UnsignedLong.Convert(System.Math.Exp(value));
        }
        public static ulong Logarithm(ulong value)
        {
            return UnsignedLong.Convert(System.Math.Log(value));
        }
        public static ulong Logarithm(ulong value, ulong @base)
        {
            return UnsignedLong.Convert(System.Math.Log(value, @base));
        }
        public static ulong Power(ulong @base, ulong exponent)
        {
            return UnsignedLong.Convert(System.Math.Pow(@base, exponent));
        }
        public static ulong SquareRoot(ulong value)
        {
            return UnsignedLong.Convert(System.Math.Sqrt(value));
        }
        public static ulong Squared(ulong value)
        {
            return value * value;
        }
        #endregion
    }
}
