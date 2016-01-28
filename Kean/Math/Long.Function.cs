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
    public class Long
    {
        #region Constants
        public static long NegativeInfinity { get { return long.MinValue; } }
        public static long PositiveInfinity { get { return long.MaxValue; } }
        public static long Epsilon { get { return 1; } }
        public static long MinimumValue { get { return long.MinValue; } }
        public static long MaximumValue { get { return long.MaxValue; } }
        public static long Pi { get { return Long.Convert(System.Math.PI); } }
        public static long E { get { return Long.Convert(System.Math.E); } }
        #endregion
        #region Convert Functions
        public static long Convert(double value)
        {
            return System.Convert.ToInt64(value);
        }
        public static long Convert(float value)
        {
            return System.Convert.ToInt64(value);
        }
        /// <summary>
        /// Parses a string to a long
        /// </summary>
        /// <exception cref="System.FormatException">When string does not contain a int</exception>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long Parse(string value)
        {
            return long.Parse(value, System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
        }
        public static string ToString(long value)
        {
            return value.ToString(System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
        }
        #endregion
        #region Utility Functions
        public static long Clamp(long value, long floor, long ceiling)
        {
            if (value > ceiling)
                value = ceiling;
            else if (value < floor)
                value = floor;
            return value;
        }
        public static long Maximum(long first, long second)
        {
            return first > second ? first : second;
        }
        public static long Maximum(long value, params long[] values)
        {
            foreach (long v in values)
                if (value < v)
                    value = v;
            return value;
        }
        public static long Minimum(long first, long second)
        {
            return first < second ? first : second;
        }
        public static long Minimum(long value, params long[] values)
        {
            foreach (long v in values)
                if (value > v)
                    value = v;
            return value;
        }
        public static long Modulo(long dividend, long divisor)
        {
            if (dividend < 0)
                dividend += Long.Ceiling(dividend / (float)divisor) * divisor;
            return dividend % divisor;
        }
        #endregion
        #region Rounding Functions
        public static long Floor(float value)
        {
            return Long.Convert(System.Math.Floor(value));
        }
        public static long Floor(double value)
        {
            return Long.Convert(System.Math.Floor(value));
        }
        public static long Ceiling(float value)
        {
            return Long.Convert(System.Math.Ceiling(value));
        }
        public static long Ceiling(double value)
        {
            return Long.Convert(System.Math.Ceiling(value));
        }
        public static long Truncate(float value)
        {
            return Long.Convert(System.Math.Truncate(value));
        }
        public static long Truncate(double value)
        {
            return Long.Convert(System.Math.Truncate(value));
        }
        public static long Round(float value)
        {
            return Long.Convert(System.Math.Round(value));
        }
        public static long Round(double value)
        {
            return Long.Convert(System.Math.Round(value));
        }
        #endregion
        #region Transcendental and Power Functions
        public static long Exponential(long value)
        {
            return Long.Convert(System.Math.Exp(value));
        }
        public static long Logarithm(long value)
        {
            return Long.Convert(System.Math.Log(value));
        }
        public static long Logarithm(long value, long @base)
        {
            return Long.Convert(System.Math.Log(value, @base));
        }
        public static long Power(long @base, long exponent)
        {
            return Long.Convert(System.Math.Pow(@base, exponent));
        }
        public static long SquareRoot(long value)
        {
            return Long.Convert(System.Math.Sqrt(value));
        }
        public static long Squared(long value)
        {
            return value * value;
        }
        #endregion
    }
}
