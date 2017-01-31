//
//  Quaternion.cs
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
using Kean.Extension;

namespace Kean.Math.Geometry3D.Single
{
    public struct Quaternion :
        IEquatable<Quaternion>
    {
        public float Real { get; private set; }
        public Point Imaginary { get; private set; }
        public float X { get { return this.Real; } }
        public float Y { get { return this.Imaginary.X; } }
        public float Z { get { return this.Imaginary.Y; } }
        public float W { get { return this.Imaginary.Z; } }

        public float Norm { get { return Kean.Math.Single.SquareRoot(Kean.Math.Single.Squared(this.Real) + Kean.Math.Single.Squared(this.Imaginary.Norm)); } }
        /// <summary>
        /// Clockwise rotation around the direction vector of the corresponding rotation.
        /// </summary>
        public float Rotation { get { return 2 * (this.Logarithm().Imaginary).Norm; } }
        /// <summary>
        /// Direction vector of corresponding rotation. 
        /// </summary>
        public Point Direction { get { return this.Logarithm().Imaginary / this.Logarithm().Imaginary.Norm; } }
        public Quaternion Inverse { get { return this.Conjugate / Kean.Math.Single.Squared(this.Norm); } }
        public Quaternion Conjugate { get { return new Quaternion() { Real = this.Real, Imaginary = -this.Imaginary }; } }
        #region Representations
        /// <summary>
        /// Roll (bank)
        /// </summary>
        public float RotationX
        {
            get
            {
                float result;
                float value = this.X * this.Z - this.W * this.Y;
                if (Kean.Math.Single.Absolute(Kean.Math.Single.Absolute(value) - (float)0.5) < 1e-5)
                    result = 0;
                else
                    result = Kean.Math.Single.ArcusTangensExtended(2 * (this.X * this.Y + this.Z * this.W), 1 - 2 * (Kean.Math.Single.Squared(this.Y) + Kean.Math.Single.Squared(this.Z)));
                return result;
            }
        }

        /// <summary>
        /// Pitch 
        /// </summary>
        public float RotationY
        {
            get
            {
                float result = 0;
                float value = this.X * this.Z - this.W * this.Y;
                if (Kean.Math.Single.Absolute(Kean.Math.Single.Absolute(value) - (float)0.5) > 1e-5)
                    result = Kean.Math.Single.ArcusSinus(Kean.Math.Single.Clamp(2 * (this.X * this.Z - this.W * this.Y), -1, 1));
                else
                    result = Kean.Math.Single.Sign(value) * (float)Kean.Math.Double.PI / 2;
                return result;

            }
        }
        /// <summary>
        /// Yaw (Heading)
        /// </summary>
        public float RotationZ
        {
            get
            {
                float result;
                float value = this.X * this.Z - this.W * this.Y;
                if (Kean.Math.Single.Absolute(Kean.Math.Single.Absolute(value) - (float)0.5) < 1e-5)
                    result = 2 * Kean.Math.Single.ArcusTangensExtended(this.W, this.X);
                else
                    result = Kean.Math.Single.ArcusTangensExtended(2 * (this.X * this.W + this.Y * this.Z), 1 - 2 * (Kean.Math.Single.Squared(this.Z) + Kean.Math.Single.Squared(this.W)));
                return result;
            }
        }
        #endregion
        #region Static Constants
        public static Quaternion BasisReal { get { return new Quaternion() { Real = 1, Imaginary = new Point() }; } }
        public static Quaternion BasisImaginaryX { get { return new Quaternion() { Real = 0, Imaginary = Point.BasisX }; } }
        public static Quaternion BasisImaginaryY { get { return new Quaternion() { Real = 0, Imaginary = Point.BasisY }; } }
        public static Quaternion BasisImaginaryZ { get { return new Quaternion() { Real = 0, Imaginary = Point.BasisZ }; } }
        #endregion
        #region Constructors
        public Quaternion(float x, float y, float z, float w) :
            this(x, new Point(y, z, w)) { }
        public Quaternion(float real, Point imaginary) :
            this()
        {
            this.Real = real;
            this.Imaginary = imaginary;
        }
        #endregion
        #region Methods
        public float Distance(Quaternion other)
        {
            return (this - other).Norm;
        }
        public Quaternion Copy()
        {
            return new Quaternion() { Real = this.Real, Imaginary = this.Imaginary };
        }
        #region Transcendental Functions
        public Quaternion Exponential()
        {
            Quaternion result = new Quaternion();
            float norm = this.Imaginary.Norm;
            float exponentialReal = Kean.Math.Single.Exponential(this.Real);
            if (norm != 0)
            {
                result.Real = exponentialReal * Kean.Math.Single.Cosine(norm);
                result.Imaginary = exponentialReal * (this.Imaginary / norm) * Kean.Math.Single.Sine(norm);
            }
            else
                result = (Quaternion)(exponentialReal);
            return result;
        }
        public Quaternion Logarithm()
        {
            Quaternion result = new Quaternion();
            float norm = this.Imaginary.Norm;
            if (norm != 0)
            {
                result.Real = Kean.Math.Single.Logarithm(this.Norm);
                result.Imaginary = (this.Imaginary / norm) * Kean.Math.Single.ArcusCosinus(this.Real / this.Norm);
            }
            else
                result = (Quaternion)(this.Norm);
            return result;
        }
        #endregion
        #endregion

        #region Arithmetic Point - Point Operators
        public static Point operator *(Quaternion left, Point right)
        {
            return (left * new Quaternion() { Real = 0, Imaginary = right } * left.Inverse).Imaginary;
        }
        public static Quaternion operator *(Quaternion left, Quaternion right)
        {
            Quaternion result = new Quaternion()
            {
                Real = left.Real * right.Real - left.Imaginary.ScalarProduct(right.Imaginary),
                Imaginary = left.Real * right.Imaginary + left.Imaginary * right.Real + left.Imaginary.VectorProduct(right.Imaginary)
            };
            return result;
        }

        public static Quaternion operator +(Quaternion left, Quaternion right)
        {
            Quaternion result = new Quaternion()
            {
                Real = left.Real + right.Real,
                Imaginary = left.Imaginary + right.Imaginary,
            };
            return result;
        }
        public static Quaternion operator -(Quaternion quaternion)
        {
            Quaternion result = new Quaternion()
            {
                Real = -quaternion.Real,
                Imaginary = -quaternion.Imaginary,
            };
            return result;
        }
        public static Quaternion operator -(Quaternion left, Quaternion right)
        {
            return left + (-right);
        }
        #endregion
        #region Arithmetic Point and Scalar
        public static Quaternion operator *(Quaternion left, float right)
        {
            Quaternion result = new Quaternion()
            {
                Real = left.Real * right,
                Imaginary = left.Imaginary * right,
            };
            return result;
        }
        public static Quaternion operator *(float left, Quaternion right)
        {
            return right * left;
        }
        public static Quaternion operator /(Quaternion left, float right)
        {
            Quaternion result = new Quaternion()
            {
                Real = left.Real / right,
                Imaginary = left.Imaginary / right,
            };
            return result;
        }
        #endregion
        #region Static Functions
        /// <summary>
        /// Rotation around the real-axis
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static Quaternion CreateRotationX(float angle)
        {
            return Quaternion.CreateRotation(angle, Point.BasisX);
        }
        /// <summary>
        /// Rotation around the imaginary-axis
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static Quaternion CreateRotationY(float angle)
        {
            return Quaternion.CreateRotation(angle, Point.BasisY);
        }
        /// <summary>
        /// Rotation around the z-axis
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static Quaternion CreateRotationZ(float angle)
        {
            return Quaternion.CreateRotation(angle, Point.BasisZ);
        }
        /// <summary>
        /// Rotation around the given axis vector 
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static Quaternion CreateRotation(float angle, Point direction)
        {
            float halfAngle = angle / 2;
            float norm = direction.Norm;
            if (norm != 0)
                direction = direction / direction.Norm;
            return ((Quaternion)(halfAngle * direction)).Exponential();
        }
        #endregion
        #region Comparison Operators
        /// <summary>
        /// Defines equality.
        /// </summary>
        /// <param name="left">Point left of operator.</param>
        /// <param name="right">Point right of operator.</param>
        /// <returns>True if <paramref name="left"/> equals <paramref name="right"/> else false.</returns>
        public static bool operator ==(Quaternion left, Quaternion right)
        {
            return object.ReferenceEquals(left, right) ||
                !object.ReferenceEquals(left, null) && !object.ReferenceEquals(right, null) && left.Real == right.Real && left.Imaginary == right.Imaginary;
        }
        /// <summary>
        /// Defines inequality.
        /// </summary>
        /// <param name="left">Point left of operator.</param>
        /// <param name="right">Point right of operator.</param>
        /// <returns>False if <paramref name="left"/> equals <paramref name="right"/> else true.</returns>
        public static bool operator !=(Quaternion left, Quaternion right)
        {
            return !(left == right);
        }
        #endregion
        #region Object overides and IEquatable<Quaternion>
        public override bool Equals(object other)
        {
            return (other is Quaternion) ? this.Equals(other) : false;
        }
        // other is not null here.
        public bool Equals(Quaternion other)
        {
            return this == other;
        }
        public override int GetHashCode()
        {
            return 33 * this.Real.GetHashCode() ^ this.Imaginary.GetHashCode();
        }
        public override string ToString()
        {
            return this.Real.ToString() + ", " + this.Imaginary.ToString();
        }
        #endregion
        #region Casts
        /// <summary>
        /// Cast from Real to a quaternion.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static explicit operator Quaternion(float value)
        {
            return new Quaternion() { Real = value, Imaginary = new Point() };
        }
        /// <summary>
        /// Cast from Vector to a quaternion.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static explicit operator Quaternion(Point value)
        {
            return new Quaternion() { Real = 0, Imaginary = value };
        }
        public static explicit operator float[](Quaternion value)
        {
            return new float[] { value.Real, value.Imaginary.X, value.Imaginary.Y, value.Imaginary.Z };
        }
        public static explicit operator float[,](Quaternion value)
        {
            float[,] result = new float[3, 3];
            Quaternion normalized = value / value.Norm;
            float q0 = normalized.Real;
            float q1 = normalized.Imaginary.X;
            float q2 = normalized.Imaginary.Y;
            float q3 = normalized.Imaginary.Z;
            result[0, 0] = Kean.Math.Single.Squared(q0) + Kean.Math.Single.Squared(q1) - Kean.Math.Single.Squared(q2) - Kean.Math.Single.Squared(q3);
            result[1, 0] = 2 * (q1 * q2 - q0 * q3);
            result[2, 0] = 2 * (q0 * q2 + q1 * q3);
            result[0, 1] = 2 * (q1 * q2 + q0 * q3);
            result[1, 1] = Kean.Math.Single.Squared(q0) - Kean.Math.Single.Squared(q1) + Kean.Math.Single.Squared(q2) - Kean.Math.Single.Squared(q3);
            result[2, 1] = 2 * (q2 * q3 - q0 * q1);
            result[0, 2] = 2 * (q1 * q3 - q0 * q2);
            result[1, 2] = 2 * (q0 * q1 + q2 * q3);
            result[2, 2] = Kean.Math.Single.Squared(q0) - Kean.Math.Single.Squared(q1) - Kean.Math.Single.Squared(q2) + Kean.Math.Single.Squared(q3);
            return result;
        }
        public static explicit operator Transform(Quaternion value)
        {
            float[,] values = (float[,])value;
            return Transform.Create(values[0, 0], values[0, 1], values[0, 2], values[1, 0], values[1, 1], values[1, 2], values[2, 0], values[2, 1], values[2, 2], 0, 0, 0);
        }
        public static implicit operator string(Quaternion value)
        {
            return value.NotNull() ? value.ToString() : null;
        }
        public static implicit operator Quaternion(string value)
        {
            Quaternion result = null;
            if (value.NotEmpty())
            {

                try
                {
                    string[] values = value.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (values.Length == 4)
                        result = new Quaternion(Kean.Math.Single.Parse(values[0]), Kean.Math.Single.Parse(values[1]), Kean.Math.Single.Parse(values[2]), Kean.Math.Single.Parse(values[3]));
                }
                catch
                {
                    result = null;
                }
            }
            return result;
        }
        #endregion
    }
}
