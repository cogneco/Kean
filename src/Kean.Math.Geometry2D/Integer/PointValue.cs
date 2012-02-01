﻿// 
//  PointValue.cs
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
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.using System;
using System;
using Kean.Core.Extension;

namespace Kean.Math.Geometry2D.Integer
{
	public struct PointValue :
		Abstract.IPoint<int>, Abstract.IVector<int>
	{
        public int X;
        public int Y;
        #region IPoint<int> Members
        int Kean.Math.Geometry2D.Abstract.IPoint<int>.X { get { return this.X; } }
        int Kean.Math.Geometry2D.Abstract.IPoint<int>.Y { get { return this.Y; } }
        #endregion
        #region IVector<int> Members
        int Kean.Math.Geometry2D.Abstract.IVector<int>.X { get { return this.X; } }
        int Kean.Math.Geometry2D.Abstract.IVector<int>.Y { get { return this.Y; } }
        #endregion
        public int Length { get { return Kean.Math.Integer.SquareRoot(Kean.Math.Integer.Squared(this.X) + Kean.Math.Integer.Squared(this.Y)); } }
        public PointValue(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
        public void Clear()
        {
            this.X = this.Y = 0;
        }
        public void Set(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
        public int Norm(int p)
        {
            int result;
            if (p == int.MaxValue)
                result = Kean.Math.Integer.Maximum(Kean.Math.Integer.Absolute(this.X), Kean.Math.Integer.Absolute(this.Y));
            else if (p == 1)
                result = Kean.Math.Integer.Absolute(this.X) + Kean.Math.Integer.Absolute(this.Y);
            else
                result = Kean.Math.Integer.Round(Kean.Math.Single.Power(Kean.Math.Single.Power(Kean.Math.Single.Absolute(this.X), p) + Kean.Math.Single.Power(Kean.Math.Single.Absolute(this.Y), p), 1f / p));
            return result;
        }
        #region Arithmetic Vector - Vector Operators
        public static PointValue operator +(PointValue left, PointValue right)
        {
            return new PointValue(left.X + right.X, left.Y + right.Y);
        }
        public static PointValue operator -(PointValue left, PointValue right)
        {
            return new PointValue(left.X - right.X, left.Y - right.Y);
        }
        public static PointValue operator -(PointValue vector)
        {
            return new PointValue(-vector.X, -vector.Y);
        }
        public static int operator *(PointValue left, PointValue right)
        {
            return left.X * right.X + left.Y * right.Y;
        }
        public void Add(int x, int y)
        {
            this.X += x;
            this.Y += y;
        }
        public void Add(PointValue other)
        {
            this.X += other.X;
            this.Y += other.Y;
        }
        #endregion
        #region Arithmetic Vector and Scalar
        public void Multiply(int scalar)
        {
            this.X *= scalar;
            this.Y *= scalar;
        }
        public static PointValue operator *(PointValue left, int right)
        {
            return new PointValue(left.X * right, left.Y * right);
        }
        public static PointValue operator *(int left, PointValue right)
        {
            return right * left;
        }
        #endregion
        #region Arithmetic Transform and Vector
        public static PointValue operator *(TransformValue left, PointValue right)
        {
            return new PointValue(left.A * right.X + left.C * right.Y + left.E, left.B * right.X + left.D * right.Y + left.F);
        }
        #endregion
        #region Static Operators
        public static PointValue Floor(Geometry2D.Single.PointValue other)
        {
            return new PointValue(Kean.Math.Integer.Floor(other.X), Kean.Math.Integer.Floor(other.Y));
        }
        public static PointValue Ceiling(Geometry2D.Single.PointValue other)
        {
            return new PointValue(Kean.Math.Integer.Ceiling(other.X), Kean.Math.Integer.Ceiling(other.Y));
        }
        public static PointValue Floor(Geometry2D.Double.PointValue other)
        {
            return new PointValue(Kean.Math.Integer.Floor(other.X), Kean.Math.Integer.Floor(other.Y));
        }
        public static PointValue Ceiling(Geometry2D.Double.PointValue other)
        {
            return new PointValue(Kean.Math.Integer.Ceiling(other.X), Kean.Math.Integer.Ceiling(other.Y));
        }
        public static PointValue Maximum(PointValue left, PointValue right)
        {
            return new PointValue(Kean.Math.Integer.Maximum(left.X, right.X), Kean.Math.Integer.Maximum(left.Y, right.Y));
        }
        public static PointValue Minimum(PointValue left, PointValue right)
        {
            return new PointValue(Kean.Math.Integer.Minimum(left.X, right.X), Kean.Math.Integer.Minimum(left.Y, right.Y));
        }
        #endregion
        #region Comparison Operators
        /// <summary>
        /// Defines equality.
        /// </summary>
        /// <param name="Left">Point Left of operator.</param>
        /// <param name="Right">Point Right of operator.</param>
        /// <returns>True if <paramref name="Left"/> equals <paramref name="Right"/> else false.</returns>
        public static bool operator ==(PointValue left, PointValue right)
        {
            return left.X == right.X && left.Y == right.Y;
        }
        /// <summary>
        /// Defines inequality.
        /// </summary>
        /// <param name="Left">Point Left of operator.</param>
        /// <param name="Right">Point Right of operator.</param>
        /// <returns>False if <paramref name="Left"/> equals <paramref name="Right"/> else true.</returns>
        public static bool operator !=(PointValue left, PointValue right)
        {
            return !(left == right);
        }
        public static bool operator <(PointValue left, PointValue right)
        {
            return left.X < right.X && left.Y < right.Y;
        }
        public static bool operator >(PointValue left, PointValue right)
        {
            return left.X > right.X && left.Y > right.Y;
        }
        public static bool operator <=(PointValue left, PointValue right)
        {
            return left.X <= right.X && left.Y <= right.Y;
        }
        public static bool operator >=(PointValue left, PointValue right)
        {
            return left.X >= right.X && left.Y >= right.Y;
        }
        #endregion
        #region Casts
        public static implicit operator string(PointValue value)
        {
            return value.NotNull() ? value.ToString() : null;
        }
        public static implicit operator PointValue(string value)
        {
            PointValue result = new PointValue();
            if (value.NotEmpty())
            {
                try
                {
					result = (PointValue)(Point)value;
				}
                catch
                {
                }
            }
            return result;
        }
        #endregion
        #region Object Overrides
        public override int GetHashCode()
        {
            return 33 * this.X.GetHashCode() ^ this.Y.GetHashCode();
        }
		public override string ToString()
		{
			return ((Point)this).ToString();
		}
		public string ToString(string format)
		{
			return ((Point)this).ToString(format);
		}
		#endregion
    }
}
