﻿// 
//  TransformValue.cs
//  
//  Author:
//       Anders Frisk <andersfrisk77@gmail.com>
//  
//  Copyright (c) 2011 Anders Frisk
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
using Kean.Core.Extension;

namespace Kean.Math.Geometry2D.Single
{
    public struct TransformValue :
        Abstract.ITransform<float>
    {
        float a;
        float b;
        float c;
        float d;
        float e;
        float f;
        public float A
        {
            get { return this.a; }
            set { this.a = value; }
        }
        public float B
        {
            get { return this.b; }
            set { this.b = value; }
        }
        public float C
        {
            get { return this.c; }
            set { this.c = value; }
        }
        public float D
        {
            get { return this.d; }
            set { this.d = value; }
        }
        public float E
        {
            get { return this.e; }
            set { this.e = value; }
        }
        public float F
        {
            get { return this.f; }
            set { this.f = value; }
        }
        public TransformValue(float a, float b, float c, float d, float e, float f)
        {
            this.a = a;
            this.b = b;
            this.c = c;
            this.d = d;
            this.e = e;
            this.f = f;
        }
        #region Comparison Operators
        /// <summary>
        /// Defines equality.
        /// </summary>
        /// <param name="Left">Point Left of operator.</param>
        /// <param name="Right">Point Right of operator.</param>
        /// <returns>True if <paramref name="Left"/> equals <paramref name="Right"/> else false.</returns>
        public static bool operator ==(TransformValue left, TransformValue right)
        {
            return left.A == right.A && left.B == right.B && left.C == right.C && left.D == right.D && left.E == right.E && left.F == right.F;
        }
        /// <summary>
        /// Defines inequality.
        /// </summary>
        /// <param name="Left">Point Left of operator.</param>
        /// <param name="Right">Point Right of operator.</param>
        /// <returns>False if <paramref name="Left"/> equals <paramref name="Right"/> else true.</returns>
        public static bool operator !=(TransformValue left, TransformValue right)
        {
            return !(left == right);
        }
        #endregion
        #region Casts
        public static implicit operator TransformValue(Integer.TransformValue value)
        {
            return new TransformValue(value.A, value.B, value.C, value.D, value.E, value.F);
        }
        public static explicit operator Integer.TransformValue(TransformValue value)
        {
            return new Integer.TransformValue((Kean.Math.Integer)(value.A), (Kean.Math.Integer)(value.B), (Kean.Math.Integer)(value.C), (Kean.Math.Integer)(value.D), (Kean.Math.Integer)(value.E), (Kean.Math.Integer)(value.F));
        }
        public static implicit operator string(TransformValue value)
        {
            return value.NotNull() ? value.ToString() : null;
        }
        public static implicit operator TransformValue(string value)
        {
            TransformValue result = new TransformValue();
            if (value.NotEmpty())
            {
                try
                {
					result = (TransformValue)(Transform)(value);
				}
                catch
                {
                }
            }
            return result;
        }
        #endregion
        #region Object Overrides
        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>Hash code for this instance.</returns>
        public override int GetHashCode()
        {
            return (33 * (33 * (33 * (33 * this.A.GetHashCode() ^ this.B.GetHashCode()) ^ this.C.GetHashCode()) ^ this.D.GetHashCode()) ^ this.E.GetHashCode()) ^ this.F.GetHashCode();
        }
		public override string ToString()
		{
			return ((Transform)this).ToString();
		}
		public string ToString(string format)
		{
			return ((Transform)this).ToString(format);
		}
		#endregion
   }
}