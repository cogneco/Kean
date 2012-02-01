﻿// 
//  ITransform.cs
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
using Kean.Core.Extension;

namespace Kean.Math.Geometry3D.Abstract
{
    public abstract class Transform<TransformType, TransformValue, SizeType, SizeValue, R, V> :
        ITransform<V>,
        IEquatable<TransformType>
        where TransformType : Transform<TransformType, TransformValue, SizeType, SizeValue, R, V>, ITransform<V>, new()
        where TransformValue : struct, ITransform<V>
        where SizeType : Size<TransformType, TransformValue, SizeType, SizeValue, R, V>, IVector<V>, new()
        where SizeValue : struct, ISize<V>, IVector<V>
        where R : Kean.Math.Abstract<R, V>, new()
        where V : struct
    {
        public R A { get; private set; }
        public R B { get; private set; }
        public R C { get; private set; }
        public R D { get; private set; }
        public R E { get; private set; }
        public R F { get; private set; }
        public R G { get; private set; }
        public R H { get; private set; }
        public R I { get; private set; }
        public R J { get; private set; }
        public R K { get; private set; }
        public R L { get; private set; }

        public R this[int x, int y]
        {
            get
            {
                R result;
                switch (x)
                {
                    case 0:
                        switch (y)
                        {
                            case 0: result = this.A; break;
                            case 1: result = this.B; break;
                            case 2: result = this.C; break;
                            case 3: result = Kean.Math.Abstract<R, V>.Zero; break;
                            default: throw new System.Exception(); // TODO: create new exception
                        }
                        break;
                    case 1:
                        switch (y)
                        {
                            case 0: result = this.D; break;
                            case 1: result = this.E; break;
                            case 2: result = this.F; break;
                            case 3: result = Kean.Math.Abstract<R, V>.Zero; break;
                            default: throw new System.Exception(); // TODO: create new exception
                        }
                        break;
                    case 2:
                        switch (y)
                        {
                            case 0: result = this.G; break;
                            case 1: result = this.H; break;
                            case 2: result = this.I; break;
                            case 3: result = Kean.Math.Abstract<R, V>.Zero; break;
                            default: throw new System.Exception(); // TODO: create new exception
                        }
                        break;
                    case 3:
                        switch (y)
                        {
                            case 0: result = this.J; break;
                            case 1: result = this.K; break;
                            case 2: result = this.L; break;
                            case 3: result = Kean.Math.Abstract<R, V>.One; break;
                            default: throw new System.Exception(); // TODO: create new exception
                        }
                        break;
                    default: throw new System.Exception(); // TODO: create new exception
                }
                return result;
            }
        }
        public abstract TransformValue Value { get; }
        public TransformType Inverse
        {
            get
            {
                R determinant = this.A * (this.E *  this.I - this.F * this.H) + this.D * (this.H* this.C - this.I * this.B) + this.G * (this.B * this.F - this.E * this.C);
                TransformType result = new TransformType()
                {
                    A = (this.E * this.I - this.H * this.F) / determinant,
                    B = (this.H * this.C - this.I * this.B) / determinant,
                    C = (this.B * this.F - this.E * this.C) / determinant,
                    D = (this.G * this.F - this.I * this.D) / determinant,
                    E = (this.A * this.I - this.G * this.C) / determinant,
                    F = (this.D * this.C - this.F * this.A) / determinant,
                    G = (this.D * this.H - this.E * this.G) / determinant,
                    H = (this.G * this.B - this.A * this.H) / determinant,
                    I = (this.A * this.E - this.D * this.B) / determinant,
                    J = new R(),
                    K = new R(),
                    L = new R()
                };
                TransformType translation = result * Kean.Math.Geometry3D.Abstract.Transform<TransformType, TransformValue, SizeType, SizeValue, R, V>.CreateTranslation(this.J, this.K, this.L);
                result.J = -translation.J;
                result.K = -translation.K;
                result.L = -translation.L;
                return result;
            }
        }
        #region Transform Properties
        public V ScalingX { get { return (this.A.Squared() + this.B.Squared() + this.C.Squared()).SquareRoot(); } }
        public V ScalingY { get { return (this.D.Squared() + this.E.Squared() + this.F.Squared()).SquareRoot(); } }
        public V ScalingZ { get { return (this.G.Squared() + this.H.Squared() + this.I.Squared()).SquareRoot(); } }
        public V Scaling { get { return ((R)this.ScalingX + (R)this.ScalingY + (R)this.ScalingZ) / new R().CreateConstant(3); } }
        public SizeType Translation { get { return Size<TransformType, TransformValue, SizeType, SizeValue, R, V>.Create(this.J,this.K, this.L); } }
        #endregion
	
        #region Constructors
        protected Transform() : this(new R(), new R(), new R(), new R(), new R(), new R(), new R(), new R(), new R(), new R(), new R(), new R()) { }
        protected Transform(R a, R b, R c, R d, R e, R f, R g, R h, R i, R j, R k, R l)
        {
            this.A = a;
            this.B = b;
            this.C = c;
            this.D = d;
            this.E = e;
            this.F = f;
            this.G = g;
            this.H = h;
            this.I = i;
            this.J = j;
            this.K = k;
            this.L = l;



        }
        #endregion
        #region Manipulations
        public TransformType Translate(V delta)
        {
            return this.Translate(delta, delta, delta);
        }
        public TransformType Translate(IVector<V> delta)
        {
            return this.Translate(delta.X, delta.Y, delta.Z);
        }
        public TransformType Translate(V xDelta, V yDelta, V zDelta)
        {
            return Transform<TransformType, TransformValue, SizeType, SizeValue, R, V>.CreateTranslation(xDelta, yDelta, zDelta) * this;
        }
        public TransformType Scale(V factor)
        {
            return this.Scale(factor, factor, factor);
        }
        public TransformType Scale(IVector<V> factor)
        {
            return this.Scale(factor.X, factor.Y, factor.Z);
        }
        public TransformType Scale(V xFactor, V yFactor, V zFactor)
        {
            return Transform<TransformType, TransformValue, SizeType, SizeValue, R, V>.CreateScaling(xFactor, yFactor, zFactor) * this;
        }
        public TransformType RotateX(V angle)
        {
            return Transform<TransformType, TransformValue, SizeType, SizeValue, R, V>.CreateRotationX(angle) * this;
        }
        public TransformType RotateY(V angle)
        {
            return Transform<TransformType, TransformValue, SizeType, SizeValue, R, V>.CreateRotationY(angle) * this;
        }
        public TransformType RotateZ(V angle)
        {
            return Transform<TransformType, TransformValue, SizeType, SizeValue, R, V>.CreateRotationZ(angle) * this;
        }
        public TransformType ReflectX()
        {
            return Transform<TransformType, TransformValue, SizeType, SizeValue, R, V>.CreateReflectionX() * this;
        }
        public TransformType ReflectY()
        {
            return Transform<TransformType, TransformValue, SizeType, SizeValue, R, V>.CreateReflectionY() * this;
        }
        public TransformType ReflectZ()
        {
            return Transform<TransformType, TransformValue, SizeType, SizeValue, R, V>.CreateReflectionZ() * this;
        }
        #endregion
        #region Static Creators
        public static TransformType Identity
        {
            get
            {
                R zero = Kean.Math.Abstract<R, V>.Zero;
                R one = Kean.Math.Abstract<R, V>.One;
                return new TransformType() { A = one, B = zero, C = zero, D = zero, E = one, F = zero, G = zero, H = zero, I = one, J = zero, K = zero, L = zero};
            }
        }
        public static TransformType CreateTranslation(IVector<V> delta)
        {
            return Kean.Math.Geometry3D.Abstract.Transform<TransformType, TransformValue, SizeType, SizeValue, R, V>.CreateTranslation(delta.X, delta.Y, delta.Z);
        }
        public static TransformType CreateTranslation(V xDelta, V yDelta, V zDelta)
        {
            R zero = Kean.Math.Abstract<R, V>.Zero;
            R one = Kean.Math.Abstract<R, V>.One;
            return new TransformType() { A = one, B = zero, C = zero, D = zero, E = one, F = zero, G = zero, H = zero, I = one, J = (R)xDelta, K = (R)yDelta, L = (R)zDelta };
        }
        public static TransformType CreateScaling(V xFactor, V yFactor, V zFactor)
        {
            R zero = Kean.Math.Abstract<R, V>.Zero;
            return new TransformType() { A = (R)xFactor, B = zero, C = zero, D = zero, E = (R)yFactor, F = zero, G = zero, H = zero, I = (R)zFactor, J = zero, K = zero, L = zero };
        }
        public static TransformType CreateRotationX(V angle)
        {
            R zero = Kean.Math.Abstract<R, V>.Zero;
            R one = Kean.Math.Abstract<R, V>.One;
            return new TransformType() { A = one, B = zero, C = zero, D = zero,  E = ((R)angle).Cosinus(), F = ((R)angle).Sinus(), G = zero, H = -((R)angle).Sinus(), I = ((R)angle).Cosinus(), J = zero, K = zero, L = zero };
        }
        public static TransformType CreateRotationY(V angle)
        {
            R zero = Kean.Math.Abstract<R, V>.Zero;
            R one = Kean.Math.Abstract<R, V>.One;
            return new TransformType() { A = ((R)angle).Cosinus(), B = zero, C = ((R)angle).Sinus(), D = zero, E = one, F = zero, G = -((R)angle).Sinus(), H = zero, I = ((R)angle).Cosinus(), J = zero, K = zero, L = zero };
        }
        public static TransformType CreateRotationZ(V angle)
        {
            R zero = Kean.Math.Abstract<R, V>.Zero;
            R one = Kean.Math.Abstract<R, V>.One;
            return new TransformType() { A = ((R)angle).Cosinus(), B = ((R)angle).Sinus(), C = zero, D = -((R)angle).Sinus(), E = ((R)angle).Cosinus(), F = zero, G = zero, H = zero, I = one, J = zero, K = zero, L = zero };
        }
        public static TransformType CreateReflectionX()
        {
            R zero = Kean.Math.Abstract<R, V>.Zero;
            R one = Kean.Math.Abstract<R, V>.One;
            return new TransformType() { A = -one, B = zero, C = zero, D = zero, E = one, F = zero, G = zero, H = zero, I = one, J = zero, K = zero, L = zero };
        }
        public static TransformType CreateReflectionY()
        {
            R zero = Kean.Math.Abstract<R, V>.Zero;
            R one = Kean.Math.Abstract<R, V>.One;
            return new TransformType() { A = one, B = zero, C = zero, D = zero, E = -one, F = zero, G = zero, H = zero, I = one, J = zero, K = zero, L = zero };
        }
        public static TransformType CreateReflectionZ()
        {
            R zero = Kean.Math.Abstract<R, V>.Zero;
            R one = Kean.Math.Abstract<R, V>.One;
            return new TransformType() { A = one, B = zero, C = zero, D = zero, E = one, F = zero, G = zero, H = zero, I = -one, J = zero, K = zero, L = zero };
        }
        public static TransformType Create(V a, V b, V c, V d, V e, V f, V g, V h, V i, V j, V k, V l)
        {
            return new TransformType() { A = (R)a, B = (R)b, C = (R)c, D = (R)d, E = (R)e, F = (R)f, G = (R)g, H = (R)h, I = (R)i, J = (R)j, K = (R)k, L = (R)l};
        }
        #endregion
        #region Arithmetic Operators
        public static TransformType operator *(Transform<TransformType, TransformValue, SizeType, SizeValue, R, V> left, ITransform<V> right)
        {
            return new TransformType()
            {
                A = left.A * right.A + left.D * right.B + left.G * right.C,
                B = left.B * right.A + left.E * right.B + left.H * right.C,
                C = left.C * right.A + left.F * right.B + left.I * right.C,
                D = left.A * right.D + left.D * right.E + left.G * right.F,
                E = left.B * right.D + left.E * right.E + left.H * right.F,
                F = left.C * right.D + left.F * right.E + left.I * right.F,
                G = left.A * right.G + left.D * right.H + left.G * right.I,
                H = left.B * right.G + left.E * right.H + left.H * right.I,
                I = left.C * right.G + left.F * right.H + left.I * right.I,
                J = left.A * right.J + left.D * right.K + left.G * right.L + left.J,
                K = left.B * right.J + left.E * right.K + left.H * right.L + left.K,
                L = left.C * right.J + left.F * right.K + left.I * right.L + left.L,
            };
        }
        #endregion
        #region ITransform<V> Members
        V ITransform<V>.A { get { return this.A; } }
        V ITransform<V>.B { get { return this.B; } }
        V ITransform<V>.C { get { return this.C; } }
        V ITransform<V>.D { get { return this.D; } }
        V ITransform<V>.E { get { return this.E; } }
        V ITransform<V>.F { get { return this.F; } }
        V ITransform<V>.G { get { return this.G; } }
        V ITransform<V>.H { get { return this.H; } }
        V ITransform<V>.I { get { return this.I; } }
        V ITransform<V>.J { get { return this.J; } }
        V ITransform<V>.K { get { return this.K; } }
        V ITransform<V>.L { get { return this.L; } }
        #endregion
        #region Comparison Operators
        public static bool operator ==(Transform<TransformType, TransformValue, SizeType, SizeValue, R, V> left, TransformType right)
        {
            return object.ReferenceEquals(left, right) ||
                !object.ReferenceEquals(left, null) && !object.ReferenceEquals(right, null) &&
                left.A == right.A && left.B == right.B && left.C == right.C &&
                left.D == right.D && left.E == right.E && left.F == right.F &&
                left.G == right.G && left.H == right.H && left.I == right.I &&
                left.J == right.J && left.K == right.K && left.L == right.L;
        }
        public static bool operator !=(Transform<TransformType, TransformValue, SizeType, SizeValue, R, V> left, TransformType right)
        {
            return !(left == right);
        }
        #endregion
        #region IEquatable<TransformType> Members
        public bool Equals(TransformType other)
        {
            return this == other; 
                
        }
        #endregion
        #region Object Overrides
        /// <summary>
        /// Return true if this object and <paramref name="other">other</paramref> are equal.
        /// </summary>
        /// <param name="other">Object to compare with</param>
        /// <returns>True if this object and <paramref name="other">other</paramref> are equal else false.</returns>
        public override bool Equals(object other)
        {
            return (other is TransformType) && this.Equals(other as TransformType);
        }
        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>Hash code for this instance.</returns>
        public override int GetHashCode()
        {
            return 33 * (33 * (33 * (33 * (33 * (33 * (33 * (33 * (33 * (33 * (33 * this.A.GetHashCode() ^ this.B.GetHashCode()) ^ this.C.GetHashCode()) ^ this.D.GetHashCode()) ^ this.E.GetHashCode()) ^ this.F.GetHashCode()) ^ this.G.GetHashCode()) ^ this.H.GetHashCode()) ^ this.I.GetHashCode()) ^ this.J.GetHashCode())^this.K.GetHashCode()) ^ this.L.GetHashCode();
        }
		public string ToString(bool matlab)
		{
			string result;
			if (matlab)
				result = ((R)this.A).ToString() + ", " + ((R)this.D).ToString() + ", " + ((R)this.G).ToString() + ", " + ((R)this.J).ToString() + "; " +
				((R)this.B).ToString() + ", " + ((R)this.E).ToString() + ", " + ((R)this.H).ToString() + ", " + ((R)this.K).ToString() + "; " +
				((R)this.C).ToString() + ", " + ((R)this.F).ToString() + ", " + ((R)this.I).ToString() + ", " + ((R)this.L).ToString() + "; " +
				new R().ToString() + ", " + new R().ToString() + ", " + new R().ToString() + ", " + Kean.Math.Abstract<R, V>.One.ToString();
			else
				result = ((R)this.A).ToString() + ", " + ((R)this.B).ToString() + ", " + ((R)this.C).ToString() + ", " + ((R)this.D).ToString() + ", " +
				((R)this.E).ToString() + ", " + ((R)this.F).ToString() + ", " + ((R)this.G).ToString() + ", " + ((R)this.H).ToString() + ", " +
				((R)this.I).ToString() + ", " + ((R)this.J).ToString() + ", " + ((R)this.K).ToString() + ", " + ((R)this.L).ToString();
			return result;
		}
		public override string ToString()
		{
			return this.ToString("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}");
		}
		public string ToMatlabString()
		{
			return this.ToString("{0}, {3}, {6}, {9}; {1}, {4}, {7}, {10}; {2}, {5}, {8}, {11}; 0, 0, 0, 1");
		}
		public string ToString(string format)
		{
			return string.Format(format, ((R)this.A).ToString(), ((R)this.B).ToString(), ((R)this.C).ToString(), ((R)this.D).ToString(), ((R)this.E).ToString(), ((R)this.F).ToString(), ((R)this.G).ToString(), ((R)this.H).ToString(), ((R)this.I).ToString(), ((R)this.J).ToString(), ((R)this.K).ToString(), ((R)this.L).ToString());
		}

        #endregion
        #region Casts
        public static explicit operator V[,](Transform<TransformType, TransformValue, SizeType, SizeValue, R, V> value)
        {
            V[,] result = new V[4, 4];
            for (int x = 0; x < 4; x++)
                for (int y = 0; y < 4; y++)
                    result[x, y] = value[x, y];
            return result;
        }
        #endregion
    }
}
