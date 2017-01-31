// 
//  Integer.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2011-2014 Simon Mika
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
	public static class Integer
	{
		#region Constants
		public static int NegativeInfinity { get { return int.MinValue; } }
		public static int PositiveInfinity { get { return int.MaxValue; } }
		public static int Epsilon { get { return 1; } }
		public static int MinimumValue { get { return int.MinValue; } }
		public static int MaximumValue { get { return int.MaxValue; } }
		public static int Pi { get { return Integer.Convert(System.Math.PI); } }
		public static int E { get { return Integer.Convert(System.Math.E); } }
		#endregion
		#region Convert Functions
		public static int Convert(double value)
		{
			return System.Convert.ToInt32(value);
		}
		public static int Convert(float value)
		{
			return System.Convert.ToInt32(value);
		}
		/// <summary>
		/// Parses a string to a int
		/// </summary>
		/// <exception cref="System.FormatException">When string does not contain a int</exception>
		/// <param name="value"></param>
		/// <returns></returns>
		public static int Parse(string value)
		{
			return int.Parse(value, System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
		}
		public static int Parse(string value, int @default)
		{
			int result;
			if (!int.TryParse(value, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture.NumberFormat, out result))
				result = @default;
			return result;
		}
		public static string ToString(int value)
		{
			return value.ToString(System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
		}
		#endregion
		#region Utility Functions
		public static int Absolute(int value)
		{
			return Integer.Convert(System.Math.Abs(value));
		}
		public static int Sign(int value)
		{
			return System.Math.Sign(value);
		}
		public static int Clamp(int value, int floor, int ceiling)
		{
			if (value > ceiling)
				value = ceiling;
			else if (value < floor)
				value = floor;
			return value;
		}
		public static int Maximum(int first, int second)
		{
			return first > second ? first : second;
		}
		public static int Maximum(int value, params int[] values)
		{
			foreach (int v in values)
				if (value < v)
					value = v;
			return value;
		}
		public static int Minimum(int first, int second)
		{
			return first < second ? first : second;
		}
		public static int Minimum(int value, params int[] values)
		{
			foreach (int v in values)
				if (value > v)
					value = v;
			return value;
		}
		public static int Modulo(int dividend, int divisor)
		{
			if (dividend < 0)
				dividend += Integer.Ceiling(Integer.Absolute(dividend) / (float)divisor) * divisor;
			return dividend % divisor;
		}
		public static bool Odd(int value)
		{
			return Integer.Modulo(value, 2) == 1;
		}
		public static bool Even(int value)
		{
			return Integer.Modulo(value, 2) == 0;
		}
		#endregion
		#region Rounding Functions
		public static int Floor(float value)
		{
			return Integer.Convert(System.Math.Floor(value));
		}
		public static int Floor(double value)
		{
			return Integer.Convert(System.Math.Floor(value));
		}
		public static int Ceiling(float value)
		{
			return Integer.Convert(System.Math.Ceiling(value));
		}
		public static int Ceiling(double value)
		{
			return Integer.Convert(System.Math.Ceiling(value));
		}
		public static int Truncate(float value)
		{
			return Integer.Convert(System.Math.Truncate(value));
		}
		public static int Truncate(double value)
		{
			return Integer.Convert(System.Math.Truncate(value));
		}
		public static int Round(float value)
		{
			return Integer.Convert(Double.Clamp(System.Math.Round(value), int.MinValue, int.MaxValue));
		}
		public static int Round(double value)
		{
			return Integer.Convert(System.Math.Round(value));
		}
		public static int Round(float value, int digits)
		{
			return Integer.Convert(System.Math.Round(value, digits));
		}
		public static int Round(double value, int digits)
		{
			return Integer.Convert(System.Math.Round(value, digits));
		}
		#endregion
		#region Trigonometric Functions
		public static int ToRadians(int angle)
		{
			return Integer.Convert(Single.ToRadians(angle));
		}
		public static int ToDegrees(int angle)
		{
			return Integer.Convert(Single.ToDegrees(angle));
		}
		public static int ToDegrees(float angle)
		{
			return Integer.Convert(Single.ToDegrees(angle));
		}
		public static int ToDegrees(double angle)
		{
			return Integer.Convert(Double.ToDegrees(angle));
		}
		/// <summary>
		/// Convert arbitrary angle in radians to angle in interval [0, 2 * Pi] (i.e. calculate the remainder modulo 2 * Pi).  
		/// </summary> 
		/// <param name="value">Angle in radians.</param>
		/// <returns>Angle <paramref name="radians"/>converted to remainder.</returns>
		public static int ModuloTwoPi(int value)
		{
			return Integer.Modulo(value, 2 * Integer.Pi);
		}
		/// <summary>
		/// Convert angle in the interval [0, 2 * Pi] to the interval [- Pi, Pi].
		/// </summary>
		/// <param name="value">Angle in radians</param>
		/// <returns>Angle <paramref name="radians"/> converted to [- Pi, Pi].</returns>
		public static int MinusPiToPi(int value)
		{
			value = Integer.ModuloTwoPi(value);
			return (value <= Integer.Pi) ? value : (value - 2 * Integer.Pi);
		}
		public static int Sine(int value)
		{
			return Integer.Convert(System.Math.Sin(value));
		}
		public static int Cosine(int value)
		{
			return Integer.Convert(System.Math.Cos(value));
		}
		public static int Tangens(int value)
		{
			return Integer.Convert(System.Math.Tan(value));
		}
		public static int SinusHyperbolicus(int value)
		{
			return Integer.Convert(System.Math.Sinh(value));
		}
		public static int CosinusHyperbolicus(int value)
		{
			return Integer.Convert(System.Math.Cosh(value));
		}
		public static int TangensHyperbolicus(int value)
		{
			return Integer.Convert(System.Math.Tanh(value));
		}
		public static int ArcusSinus(int value)
		{
			return Integer.Convert(System.Math.Asin(value));
		}
		public static int ArcusCosinus(int value)
		{
			return Integer.Convert(System.Math.Acos(value));
		}
		public static int ArcusTangens(int value)
		{
			return Integer.Convert(System.Math.Atan(value));
		}
		public static int ArcusTangensExtended(int y, int x)
		{
			return Integer.Convert(System.Math.Atan2(y, x));
		}
		#endregion
		#region Transcendental and Power Functions
		public static int Exponential(int value)
		{
			return Integer.Convert(System.Math.Exp(value));
		}
		public static int Logarithm(int value)
		{
			return Integer.Convert(System.Math.Log(value));
		}
		public static int Logarithm(int value, int @base)
		{
			return Integer.Convert(System.Math.Log(value, @base));
		}
		public static int Power(int @base, int exponent)
		{
			return Integer.Convert(System.Math.Pow(@base, exponent));
		}
		public static int SquareRoot(int value)
		{
			return Integer.Convert(System.Math.Sqrt(value));
		}
		public static int Squared(int value)
		{
			return value * value;
		}
		#endregion
	}
}
