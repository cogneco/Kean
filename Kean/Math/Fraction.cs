// 
//  Fraction.cs
//  
//  Author:
//       Anders Frisk <andersfrisk77@gmail.com>
//  
//  Copyright (c) 2012 Anders Frisk
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
using Kean;
using Kean.Extension;

namespace Kean.Math
{
	public struct Fraction :
		IEquatable<Fraction>
	{
		#region Properties
		public int Nominator { get; private set; }
		int denominator;
		public int Denominator { get { return this.denominator != 0 ? this.denominator : 1; } }
		int greatestCommonDenominator;
		public int GreatestCommonDenominator { get { return this.greatestCommonDenominator != 0 ? this.greatestCommonDenominator : 1; } }
		int[] quotients;
		public string ContinousFraction
		{
			get
			{
				string result;
				int[] quotients = this.quotients;
				System.Text.StringBuilder builder = new System.Text.StringBuilder();
				builder.Append('[');
				builder.Append(Integer.ToString(quotients[0]));
				if (quotients.Length > 1)
				{
					builder.Append(';');
					for (int i = 1; i < quotients.Length - 1; i++)
					{
						builder.Append(Integer.ToString(quotients[i]));
						builder.Append(',');
					}
					builder.Append(Integer.ToString(quotients[quotients.Length - 1]));
				}
				builder.Append(']');
				result = builder.ToString();
				return result;
			}
		}
		#endregion
		#region Contructors
		public Fraction(int nominator, int denominator) :
			this()
		{
			this.Nominator = nominator;
			this.denominator = denominator;
			Tuple<int[], int> quotientsAndGreatestCommonDenominator = this.EuclidanAlgorithm();
			this.quotients = quotientsAndGreatestCommonDenominator.Item1;
			this.greatestCommonDenominator = quotientsAndGreatestCommonDenominator.Item2;

		}
		public Fraction(string value) :
			this()
		{
			if (value.NotEmpty())
			{
				if (value.Contains('[', ']'))
				{
					string[] splitted = value.Split(new char[] { '[', ']', ';', ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
					int[] nominatorDenominator = this.EvaluateQuotients(splitted.Map<string, int>(s => Integer.Parse(s)));
					this.Nominator = nominatorDenominator[0];
					this.denominator = nominatorDenominator[1];
				}
				else
				{
					if (value.Contains(':') || value.Contains('/'))
					{
						string[] splitted = value.Split(new char[] { ':', '/', ' ' }, StringSplitOptions.RemoveEmptyEntries);
						if (splitted.Length == 2)
						{
							this.Nominator = Integer.Parse(splitted[0], 0);
							this.denominator = Integer.Parse(splitted[1], 1);
						}
					}
					else
					{
						value = value.Replace(',', '.');
						if (!value.Contains('.'))
						{
							this.Nominator = Integer.Parse(value, 0);
							this.denominator = 1;
						}
						else
							this.DecimalToFraction(Kean.Math.Double.Parse(value, 0));
					}
				}
			}
			else
			{
				this.Nominator = 0;
				this.denominator = 1;
			}
			Tuple<int[], int> quotientsAndGreatestCommonDenominator = this.EuclidanAlgorithm();
			this.quotients = quotientsAndGreatestCommonDenominator.Item1;
			this.greatestCommonDenominator = quotientsAndGreatestCommonDenominator.Item2;
		}
		public Fraction(double value) :
			this()
		{
			this.DecimalToFraction(value);
		}
		#endregion
		#region Methods
		public Fraction Reduce()
		{
			int gcd = this.GreatestCommonDenominator;
			return new Fraction(this.Nominator / gcd, this.Denominator / gcd);
		}
		#endregion
		#region Private methods
		void DecimalToFraction(double value)
		{
			int[] quotients = this.ContinuousFraction(value, 10, 1e-3);
			int[] nominator = new int[quotients.Length + 2];
			int[] denominator = new int[quotients.Length + 2];
			nominator[1] = 1;
			denominator[0] = 1;
			for (int i = 2; i < quotients.Length + 2; i++)
			{
				nominator[i] = quotients[i - 2] * nominator[i - 1] + nominator[i - 2];
				denominator[i] = quotients[i - 2] * denominator[i - 1] + denominator[i - 2];
			}
			this.Nominator = nominator[nominator.Length - 1];
			this.denominator = denominator[denominator.Length - 1];
			Tuple<int[], int> quotientsAndGreatestCommonDenominator = this.EuclidanAlgorithm();
			this.quotients = quotientsAndGreatestCommonDenominator.Item1;
			this.greatestCommonDenominator = quotientsAndGreatestCommonDenominator.Item2;
		}
		int[] EvaluateQuotients(int[] quotients)
		{
			int[] result;
			int[] nominator = new int[quotients.Length + 2];
			int[] denominator = new int[quotients.Length + 2];
			nominator[1] = 1;
			denominator[0] = 1;
			for (int i = 2; i < quotients.Length + 2; i++)
			{
				nominator[i] = quotients[i - 2] * nominator[i - 1] + nominator[i - 2];
				denominator[i] = quotients[i - 2] * denominator[i - 1] + denominator[i - 2];
			}
			result = new int[] { nominator[nominator.Length - 1], denominator[denominator.Length - 1] };
			return result;
		}
		Tuple<int[], int> EuclidanAlgorithm()
		{
			Tuple<int[], int> result = null;
			int nominator = this.Nominator;
			int denominator = this.Denominator;
			nominator = Integer.Absolute(nominator);
			denominator = Integer.Absolute(denominator);
			if (nominator == denominator || nominator == 0)
				result = Tuple.Create(new int[] { 1 }, nominator != 0 ? nominator : 1);
			else
			{
				int a, b;
				if (nominator > denominator)
				{
					a = nominator;
					b = denominator;
				}
				else
				{
					b = nominator;
					a = denominator;
				}
				System.Collections.Generic.List<int> quotients = new System.Collections.Generic.List<int>();
				int r = 0, q = 1;
				do
				{
					q = a / b;
					r = a - b * q;
					a = b;
					b = r;
					quotients.Add(q);
				} while (r != 0);
				int gcd = a;
				if (quotients.Count > 0)
					quotients[0] *= Integer.Sign(this.Nominator) * Integer.Sign(this.Denominator);
				result = Tuple.Create(quotients.ToArray(), gcd);

			}
			return result;
		}
		int[] ContinuousFraction(double value, int length, double epsilon)
		{
			int[] result;
			int[] quotients = new int[length];
			int integerPart = Integer.Floor(value);
			int count = 0;
			while (count < length)
			{
				quotients[count++] = integerPart;
				double fraction = value - integerPart;
				if (fraction < epsilon)
					break;
				else
				{
					value = 1 / fraction;
					integerPart = Integer.Floor(value);
				}
			}
			if (count < 2 || quotients[count - 1] != 1)
			{
				result = new int[count];
				Array.Copy(quotients, 0, result, 0, count);
			}
			else
			{
				result = new int[count - 1];
				Array.Copy(quotients, 0, result, 0, count - 1);
				result[count - 2] += 1;
			}
			return result;
		}
		#endregion
		#region Arithmetic Operators
		public static Fraction operator +(Fraction left, Fraction right)
		{
			return new Fraction(left.Nominator * right.Denominator + left.Denominator * right.Nominator, left.Denominator * right.Denominator).Reduce();
		}
		public static Fraction operator -(Fraction left, Fraction right)
		{
			return new Fraction(left.Nominator * right.Denominator - left.Denominator * right.Nominator, left.Denominator * right.Denominator).Reduce();
		}
		public static Fraction operator *(Fraction left, Fraction right)
		{
			return new Fraction(left.Nominator * right.Nominator, left.Denominator * right.Denominator).Reduce();
		}
		public static Fraction operator /(Fraction left, Fraction right)
		{
			return new Fraction(left.Nominator * right.Denominator, left.Denominator * right.Nominator).Reduce();
		}
		public static Fraction operator *(int left, Fraction right)
		{
			return new Fraction(left * right.Nominator, right.Denominator).Reduce();
		}
		public static Fraction operator *(Fraction left, int right)
		{
			return (right * left).Reduce();
		}
		public static Fraction operator /(Fraction left, int right)
		{
			return new Fraction(left.Nominator, left.Denominator * right).Reduce();
		}

		public static Fraction operator -(Fraction value)
		{
			return (-1) * value;
		}
		#endregion
		#region Comparison Operators
		public static bool operator ==(Fraction left, Fraction right)
		{
			return object.ReferenceEquals(left, right) ||
				!object.ReferenceEquals(left, null) && !object.ReferenceEquals(right, null) &&
				left.Nominator * right.Denominator - right.Nominator * left.Denominator == 0;
		}
		public static bool operator !=(Fraction left, Fraction right)
		{
			return !(left == right);
		}
		#endregion
		#region Object Overrides
		public override bool Equals(object other)
		{
			return (other is Fraction) && this.Equals((Fraction)other);
		}
		public bool Equals(Fraction other)
		{
			return this == other;
		}
		public override int GetHashCode()
		{
			return (this.Nominator / (float)this.Denominator).GetHashCode();
		}
		public override string ToString()
		{
			return this;
		}
		#endregion
		#region Casts
		public static implicit operator string(Fraction value)
		{
			return value.NotNull() ? Integer.ToString(value.Nominator) + ":" + Integer.ToString(value.Denominator) : null;
		}
		public static implicit operator Fraction(string value)
		{
			return new Fraction(value);
		}
		public static explicit operator float(Fraction value)
		{
			return value.NotNull() ? value.Nominator / (float)value.Denominator : 0;
		}
		public static explicit operator Fraction(float value)
		{
			return new Fraction(value);
		}
		public static explicit operator double(Fraction value)
		{
			return value.NotNull() ? value.Nominator / (double)value.Denominator : 0;
		}
		public static explicit operator Fraction(double value)
		{
			return new Fraction(value);
		}
		#endregion

	}
}
