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
using Generic = System.Collections.Generic;
namespace Kean.Math
{
	public partial class Single :
		Abstract<Single, float>
	{
		#region Constants
		public static float NegativeInfinity { get { return float.NegativeInfinity; } }
		public static float PositiveInfinity { get { return float.PositiveInfinity; } }
		public static float Epsilon { get { return float.Epsilon; } }
		public static float MinimumValue { get { return float.MinValue; } }
		public static float MaximumValue { get { return float.MaxValue; } }
		public static float Pi { get { return Single.Convert(System.Math.PI); } }
		public static float E { get { return Single.Convert(System.Math.E); } }
		#endregion
		#region Convert Functions
		public static float Convert(double value)
		{
			return System.Convert.ToSingle(value);
		}
		public static float Convert(int value)
		{
			return (float)value;
		}
		/// <summary>
		/// Parses a string to a float
		/// </summary>
		/// <exception cref="System.FormatException">When string does not contain a float</exception>
		/// <param name="value"></param>
		/// <returns></returns>
		public static float Parse(string value)
		{
			return float.Parse(value, System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
		}
		public static float Parse(string value, float @default)
		{
			float result;
			if (!float.TryParse(value, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture.NumberFormat, out result))
				result = @default;
			return result;
		}
		public static string ToString(float value)
		{
			return value.ToString(System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
		}
		#endregion
		#region Utility Functions
		public static float Absolute(float value)
		{
			return Single.Convert(System.Math.Abs(value));
		}
		public static int Sign(float value)
		{
			return System.Math.Sign(value);
		}
		public static float Clamp(float value, float floor, float ceiling)
		{
			if (value > ceiling)
				value = ceiling;
			else if (value < floor)
				value = floor;
			return value;
		}
		public static float Maximum(float first, float second)
		{
			return first > second ? first : second;
		}
		public static float Maximum(float value, params float[] values)
		{
			return Single.Maximum(value, (Generic.IEnumerable<float>)values);
		}
		public static float Maximum(float value, Generic.IEnumerable<float> values)
		{
			foreach (float v in values)
				if (value < v)
					value = v;
			return value;
		}
		public static float Minimum(float first, float second)
		{
			return first < second ? first : second;
		}
		public static float Minimum(float value, params float[] values)
		{
			return Single.Minimum(value, (Generic.IEnumerable<float>)values);
		}
		public static float Minimum(float value, Generic.IEnumerable<float> values)
		{
			foreach (float v in values)
				if (value > v)
					value = v;
			return value;
		}
		public static float Modulo(float dividend, float divisor)
		{
			if (dividend < 0)
				dividend += Single.Ceiling(Single.Absolute(dividend) / divisor) * divisor;
			return dividend % divisor;
		}
		#endregion
		#region Rounding Functions
		public static float Floor(float value)
		{
			return Single.Convert(System.Math.Floor(value));
		}
		public static float Floor(double value)
		{
			return Single.Convert(System.Math.Floor(value));
		}
		public static float Ceiling(float value)
		{
			return Single.Convert(System.Math.Ceiling(value));
		}
		public static float Truncate(float value)
		{
			return Single.Convert(System.Math.Truncate(value));
		}
		public static float Round(float value)
		{
			return Single.Convert(System.Math.Round(value));
		}
		public static float Round(float value, int digits)
		{
			return Single.Convert(System.Math.Round(value, digits));
		}
		#endregion
		#region Trigonometric Functions
		public static float ToRadians(float angle)
		{
			return Single.Pi / 180 * angle;
		}
		public static float ToDegrees(float angle)
		{
			return 180 / Single.Pi * angle;
		}
		/// <summary>
		/// Convert arbitrary angle in radians to angle in interval [0, 2 * Pi] (i.e. calculate the remainder modulo 2 * Pi).  
		/// </summary> 
		/// <param name="value">Angle in radians.</param>
		/// <returns>Angle <paramref name="radians"/>converted to remainder.</returns>
		public static float ModuloTwoPi(float value)
		{
			return Single.Modulo(value, 2 * Single.Pi);
		}
		/// <summary>
		/// Convert angle in the interval [0, 2 * Pi] to the interval [- Pi, Pi].
		/// </summary>
		/// <param name="value">Angle in radians</param>
		/// <returns>Angle <paramref name="radians"/> converted to [- Pi, Pi].</returns>
		public static float MinusPiToPi(float value)
		{
			value = Single.ModuloTwoPi(value);
			return (value <= Single.Pi) ? value : (value - 2 * Single.Pi);
		}
		public static float Sinus(float value)
		{
			return Single.Convert(System.Math.Sin(value));
		}
		public static float Cosinus(float value)
		{
			return Single.Convert(System.Math.Cos(value));
		}
		public static float Tangens(float value)
		{
			return Single.Convert(System.Math.Tan(value));
		}
		public static float SinusHyperbolicus(float value)
		{
			return Single.Convert(System.Math.Sinh(value));
		}
		public static float CosinusHyperbolicus(float value)
		{
			return Single.Convert(System.Math.Cosh(value));
		}
		public static float TangensHyperbolicus(float value)
		{
			return Single.Convert(System.Math.Tanh(value));
		}
		public static float ArcusSinus(float value)
		{
			return Single.Convert(System.Math.Asin(value));
		}
		public static float ArcusCosinus(float value)
		{
			return Single.Convert(System.Math.Acos(value));
		}
		public static float ArcusTangens(float value)
		{
			return Single.Convert(System.Math.Atan(value));
		}
		public static float ArcusSinusHyperbolicus(float value)
		{
			return Single.Logarithm(value + Single.SquareRoot(value * value + 1));
		}
		public static float ArcusCosinusHyperbolicus(float value)
		{
			return Single.Logarithm(value + Single.SquareRoot(value * value - 1));
		}
		public static float ArcusTangensHyperbolicus(float value)
		{
			return 0.5f * Single.Logarithm((1 + value) / (1 - value));
		}
		public static float ArcusTangensExtended(float y, float x)
		{
			return Single.Convert(System.Math.Atan2(y, x));
		}
		#endregion
		#region Transcendental and Power Functions
		public static float Exponential(float value)
		{
			return Single.Convert(System.Math.Exp(value));
		}
		public static float Logarithm(float value)
		{
			return Single.Convert(System.Math.Log(value));
		}
		public static float Logarithm(float value, float @base)
		{
			return Single.Convert(System.Math.Log(value, @base));
		}
		public static float Power(float @base, float exponent)
		{
			return Single.Convert(System.Math.Pow(@base, exponent));
		}
		public static float SquareRoot(float value)
		{
			return Single.Convert(System.Math.Sqrt(value));
		}
		public static float Squared(float value)
		{
			return value * value;
		}
		#endregion
	}
}
