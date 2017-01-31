// 
//  Double.cs
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
	public static class Double
	{
		#region Constants
		public static double NegativeInfinity { get { return double.NegativeInfinity; } }
		public static double PositiveInfinity { get { return double.PositiveInfinity; } }
		public static double Epsilon { get { return double.Epsilon; } }
		public static double MinimumValue { get { return double.MinValue; } }
		public static double MaximumValue { get { return double.MaxValue; } }
		public static new double PI { get { return System.Math.PI; } }
		public static double E { get { return System.Math.E; } }
		#endregion
		#region Convert Functions
		public static double Convert(float value)
		{
			return (double)value;
		}
		public static double Convert(int value)
		{
			return (double)value;
		}
		/// <summary>
		/// Parses a string to a float
		/// </summary>
		/// <exception cref="System.FormatException">When string does not contain a float</exception>
		/// <param name="value"></param>
		/// <returns></returns>
		public static double Parse(string value)
		{
			return double.Parse(value, System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
		}
		public static double Parse(string value, double @default)
		{
			double result;
			if (!double.TryParse(value, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture.NumberFormat, out result))
				result = @default;
			return result;
		}
		public static string ToString(double value)
		{
			return value.ToString(System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
		}
		#endregion
		#region Utility Functions
		public static double Absolute(double value)
		{
			return System.Math.Abs(value);
		}
		public static int Sign(double value)
		{
			return System.Math.Sign(value);
		}
		public static double Clamp(double value, double floor, double ceiling)
		{
			if (value > ceiling)
				value = ceiling;
			else if (value < floor)
				value = floor;
			return value;
		}
		public static double Maximum(double first, double second)
		{
			return first > second ? first : second;
		}
		public static double Maximum(double value, params double[] values)
		{
			foreach (double v in values)
				if (value < v)
					value = v;
			return value;
		}
		public static double Minimum(double first, double second)
		{
			return first < second ? first : second;
		}
		public static double Minimum(double value, params double[] values)
		{
			foreach (double v in values)
				if (value > v)
					value = v;
			return value;
		}
		public static double Modulo(double dividend, double divisor)
		{
			if (dividend < 0)
				dividend += Double.Ceiling(Double.Absolute(dividend) / divisor) * divisor;
			return dividend % divisor;
		}
		#endregion
		#region Rounding Functions
		public static double Floor(double value)
		{
			return System.Math.Floor(value);
		}
		public static double Ceiling(double value)
		{
			return System.Math.Ceiling(value);
		}
		public static double Truncate(double value)
		{
			return System.Math.Truncate(value);
		}
		public static double Round(double value)
		{
			return System.Math.Round(value);
		}
		public static double Round(double value, int digits)
		{
			return System.Math.Round(value, digits);
		}
		#endregion
		#region Trigonometric Functions
		public static double ToRadians(double angle)
		{
			return Double.PI / 180 * angle;
		}
		public static double ToDegrees(double angle)
		{
			return 180 / Double.PI * angle;
		}
		/// <summary>
		/// Convert arbitrary angle in radians to angle in interval [0, 2 * Pi] (i.e. calculate the remainder modulo 2 * Pi).  
		/// </summary> 
		/// <param name="value">Angle in radians.</param>
		/// <returns>Angle <paramref name="radians"/>converted to remainder.</returns>
		public static double ModuloTwoPi(double value)
		{
			return Double.Modulo(value, 2 * Double.PI);
		}
		/// <summary>
		/// Convert angle in the interval [0, 2 * Pi] to the interval [- Pi, Pi].
		/// </summary>
		/// <param name="value">Angle in radians</param>
		/// <returns>Angle <paramref name="radians"/> converted to [- Pi, Pi].</returns>
		public static double MinusPiToPi(double value)
		{
			value = Double.ModuloTwoPi(value);
			return (value <= Double.PI) ? value : (value - 2 * Double.PI);
		}
		public static double Sine(double value)
		{
			return System.Math.Sin(value);
		}
		public static double Cosine(double value)
		{
			return System.Math.Cos(value);
		}
		public static double Tangens(double value)
		{
			return System.Math.Tan(value);
		}
		public static double SinusHyperbolicus(double value)
		{
			return System.Math.Sinh(value);
		}
		public static double CosinusHyperbolicus(double value)
		{
			return System.Math.Cosh(value);
		}
		public static double TangensHyperbolicus(double value)
		{
			return System.Math.Tanh(value);
		}
		public static double ArcusSinus(double value)
		{
			return System.Math.Asin(value);
		}
		public static double ArcusCosinus(double value)
		{
			return System.Math.Acos(value);
		}
		public static double ArcusTangens(double value)
		{
			return System.Math.Atan(value);
		}
		public static double ArcusSinusHyperbolicus(double value)
		{
			return Double.Logarithm(value + Double.SquareRoot(value * value + 1));
		}
		public static double ArcusCosinusHyperbolicus(double value)
		{
			return Double.Logarithm(value + Double.SquareRoot(value * value - 1));
		}
		public static double ArcusTangensHyperbolicus(double value)
		{
			return 0.5 * Double.Logarithm((1 + value) / (1 - value));
		}
		public static double ArcusTangensExtended(double y, double x)
		{
			return System.Math.Atan2(y, x);
		}
		#endregion
		#region Transcendental and Power Functions
		public static double Exponential(double value)
		{
			return System.Math.Exp(value);
		}
		public static double Logarithm(double value)
		{
			return System.Math.Log(value);
		}
		public static double Logarithm(double value, double @base)
		{
			return System.Math.Log(value, @base);
		}
		public static double Power(double @base, double exponent)
		{
			return System.Math.Pow(@base, exponent);
		}
		public static double SquareRoot(double value)
		{
			return System.Math.Sqrt(value);
		}
		public static double Squared(double value)
		{
			return value * value;
		}
		#endregion
	}
}
