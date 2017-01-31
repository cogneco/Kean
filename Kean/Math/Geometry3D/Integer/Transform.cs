// 
// Transform.cs (generated by template)
// 
// Author:
//		Anders Frisk <andersfrisk77@gmail.com>
// 
// Copyright (c) 2011 Anders Frisk
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program. If not, see <http://www.gnu.org/licenses/>.

using System;
using Kean.Extension;
using Kean.Math.Exception;

namespace Kean.Math.Geometry3D.Integer
{
	/*
	 * The 3D transform is a 4x3 matrix where the last row in its representation of a 4x4 homogeneous coordinate matrix has been omitted.

	 * The element order is
	 * 
	 * A D G J
	 * B E H K
	 * C F I L
	 * 
	 * where A, E, and I are scale factors to X, Y, and Z, and where J, K, and L are translations in X, Y, and Z, respectively.
	*/

	public struct Transform :
		IEquatable<Transform>
	{
		public int A;
		public int B;
		public int C;
		public int D;
		public int E;
		public int F;
		public int G;
		public int H;
		public int I;
		public int J;
		public int K;
		public int L;
		public Transform(int a, int b, int c, int d, int e, int f, int g, int h, int i, int j, int k, int l) 
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
		public int this[int x, int y]
		{
			get
			{
				int result;
				switch (x)
				{
					case 0:
						switch (y)
						{
							case 0: result = this.A; break;
							case 1: result = this.B; break;
							case 2: result = this.C; break;
							case 3: result = 0; break;
							default: throw new IndexOutOfRange();
						}
						break;
					case 1:
						switch (y)
						{
							case 0: result = this.D; break;
							case 1: result = this.E; break;
							case 2: result = this.F; break;
							case 3: result = 0; break;
							default: throw new IndexOutOfRange();
						}
						break;
					case 2:
						switch (y)
						{
							case 0: result = this.G; break;
							case 1: result = this.H; break;
							case 2: result = this.I; break;
							case 3: result = 0; break;
							default: throw new IndexOutOfRange();
						}
						break;
					case 3:
						switch (y)
						{
							case 0: result = this.J; break;
							case 1: result = this.K; break;
							case 2: result = this.L; break;
							case 3: result = 1; break;
							default: throw new IndexOutOfRange();
						}
						break;
					default: throw new IndexOutOfRange();
				}
				return result;
			}
		}
		#region Transform Properties
		public int ScalingX { get { return Math.Integer.SquareRoot((Math.Integer.Squared(this.A) + Math.Integer.Squared(this.B) + Math.Integer.Squared(this.C))); } }
		public int ScalingY { get { return Math.Integer.SquareRoot((Math.Integer.Squared(this.D) + Math.Integer.Squared(this.E) + Math.Integer.Squared(this.F))); } }
		public int ScalingZ { get { return Math.Integer.SquareRoot((Math.Integer.Squared(this.G) + Math.Integer.Squared(this.H) + Math.Integer.Squared(this.I))); } }
		public int Scaling { get { return (this.ScalingX + this.ScalingY + this.ScalingZ) / 3; } }
		public Size Translation { get { return new Size(this.J, this.K, this.L); } }
		#endregion
		public Transform Inverse
		{
			get
			{
				int determinant = this.A * (this.E * this.I - this.F * this.H) + this.D * (this.H * this.C - this.I * this.B) + this.G * (this.B * this.F - this.E * this.C);
				Transform result = new Transform()
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
					J = new int(),
					K = new int(),
					L = new int()
				};
				Transform translation = result * Transform.CreateTranslation(this.J, this.K, this.L);
				result.J = -translation.J;
				result.K = -translation.K;
				result.L = -translation.L;
				return result;
			}
		}
		#region Manipulations
		public Transform Translate(int delta)
		{
			return this.Translate(delta, delta, delta);
		}
		public Transform Translate(Size delta)
		{
			return this.Translate(delta.Width, delta.Height, delta.Depth);
		}
		public Transform Translate(int xDelta, int yDelta, int zDelta)
		{
			return Transform.CreateTranslation(xDelta, yDelta, zDelta) * this;
		}
		public Transform Scale(int factor)
		{
			return this.Scale(factor, factor, factor);
		}
		public Transform Scale(Size factor)
		{
			return this.Scale(factor.Width, factor.Height, factor.Depth);
		}
		public Transform Scale(int xFactor, int yFactor, int zFactor)
		{
			return Transform.CreateScaling(xFactor, yFactor, zFactor) * this;
		}
		public Transform RotateX(int angle)
		{
			return Transform.CreateRotationX(angle) * this;
		}
		public Transform RotateY(int angle)
		{
			return Transform.CreateRotationY(angle) * this;
		}
		public Transform RotateZ(int angle)
		{
			return Transform.CreateRotationZ(angle) * this;
		}
		public Transform ReflectX()
		{
			return Transform.CreateReflectionX() * this;
		}
		public Transform ReflectY()
		{
			return Transform.CreateReflectionY() * this;
		}
		public Transform ReflectZ()
		{
			return Transform.CreateReflectionZ() * this;
		}
		#endregion
		#region Object Overrides
		public override bool Equals(object other)
		{
			return (other is Transform) && this.Equals((Transform)other);
		}
		public override int GetHashCode()
		{
			return (33* (33* (33* (33* (33 * (33 * (33 * (33 * this.A.GetHashCode() ^ this.B.GetHashCode()) ^ this.C.GetHashCode()) ^ this.D.GetHashCode()) ^ this.E.GetHashCode()) ^ this.F.GetHashCode()) ^ this.I.GetHashCode()) ^ this.J.GetHashCode()) ^ this.K.GetHashCode()) ^ this.F.GetHashCode() ;
		}
		public override string ToString()
		{
			return
				Kean.Math.Integer.ToString(this.A) + ", " +
				Kean.Math.Integer.ToString(this.B) + ", " +
				Kean.Math.Integer.ToString(this.C) + ", " +
				Kean.Math.Integer.ToString(this.D) + ", " +
				Kean.Math.Integer.ToString(this.E) + ", " +
				Kean.Math.Integer.ToString(this.F) + ", " +
				Kean.Math.Integer.ToString(this.G) + ", " +
				Kean.Math.Integer.ToString(this.H) + ", " +
				Kean.Math.Integer.ToString(this.I) + ", " +
				Kean.Math.Integer.ToString(this.J) + ", " +
				Kean.Math.Integer.ToString(this.K) + ", " +
				Kean.Math.Integer.ToString(this.L);
		}
		#endregion
		#region IEquatable<Transform> Members
		public bool Equals(Transform other)
		{
			return this.A == other.A && this.B == other.B && this.C == other.C && this.D == other.D && this.E == other.E && this.F == other.F && this.G == other.G && this.H == other.H && this.I == other.I && this.J == other.J && this.K == other.K && this.L == other.L;
		}
		#endregion
		#region Comparison Operators
		public static bool operator ==(Transform left, Transform right)
		{
			return left.Equals(right);
		}
		public static bool operator !=(Transform left, Transform right)
		{
			return !(left == right);
		}
		#endregion
		#region Static Creators
		public static Transform Identity
		{
			get
			{
				int zero = 0;
				int one = 1;
				return new Transform() { A = one, B = zero, C = zero, D = zero, E = one, F = zero, G = zero, H = zero, I = one, J = zero, K = zero, L = zero };
			}
		}
		public static Transform CreateTranslation(Size delta)
		{
			return Transform.CreateTranslation(delta.Width, delta.Height, delta.Depth);
		}
		public static Transform CreateTranslation(int xDelta, int yDelta, int zDelta)
		{
			int zero = 0;
			int one = 1;
			return new Transform() { A = one, B = zero, C = zero, D = zero, E = one, F = zero, G = zero, H = zero, I = one, J = xDelta, K = yDelta, L = zDelta };
		}
		public static Transform CreateScaling(int xFactor, int yFactor, int zFactor)
		{
			int zero = 0;
			return new Transform() { A = xFactor, B = zero, C = zero, D = zero, E = yFactor, F = zero, G = zero, H = zero, I = zFactor, J = zero, K = zero, L = zero };
		}
		public static Transform CreateRotationX(int angle)
		{
			int zero = 0;
			int one = 1;
			return new Transform() { A = one, B = zero, C = zero, D = zero, E = Math.Integer.Cosine(angle), F = Math.Integer.Sine(angle), G = zero, H = Math.Integer.Sine(- (angle)), I = Math.Integer.Cosine(angle), J = zero, K = zero, L = zero };
		}
		public static Transform CreateRotationY(int angle)
		{
			int zero = 0;
			int one = 1;
			return new Transform() { A = Math.Integer.Cosine(angle), B = zero, C = Math.Integer.Sine(angle), D = zero, E = one, F = zero, G = Math.Integer.Sine(-(angle)), H = zero, I = Math.Integer.Cosine(angle), J = zero, K = zero, L = zero };
		}
		public static Transform CreateRotationZ(int angle)
		{
			int zero = 0;
			int one = 1;
			return new Transform() { A = Math.Integer.Cosine(angle), B = Math.Integer.Sine(angle), C = zero, D = Math.Integer.Sine(-(angle)), E = Math.Integer.Cosine(angle), F = zero, G = zero, H = zero, I = one, J = zero, K = zero, L = zero };
		}
		public static Transform CreateRotation(Transform transform, Point pivot)
		{
			return Transform.CreateTranslation(pivot.X, pivot.Y, pivot.Z) *
				transform *
				Transform.CreateTranslation(-pivot.X, -pivot.Y, -pivot.Z);
		}
		public static Transform CreateReflectionX()
		{
			int zero = 0;
			int one = 1;
			return new Transform() { A = -one, B = zero, C = zero, D = zero, E = one, F = zero, G = zero, H = zero, I = one, J = zero, K = zero, L = zero };
		}
		public static Transform CreateReflectionY()
		{
			int zero = 0;
			int one = 1;
			return new Transform() { A = one, B = zero, C = zero, D = zero, E = -one, F = zero, G = zero, H = zero, I = one, J = zero, K = zero, L = zero };
		}
		public static Transform CreateReflectionZ()
		{
			int zero = 0;
			int one = 1;
			return new Transform() { A = one, B = zero, C = zero, D = zero, E = one, F = zero, G = zero, H = zero, I = -one, J = zero, K = zero, L = zero };
		}
		public static Transform Create(int a, int b, int c, int d, int e, int f, int g, int h, int i, int j, int k, int l)
		{
			return new Transform() { A = a, B = b, C = c, D = d, E = e, F = f, G = g, H = h, I = i, J = j, K = k, L = l };
		}
		#endregion
		#region Arithmetic Operators
		public static Transform operator *(Transform left, Transform right)
		{
			return new Transform()
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
		#region Casts
		public static implicit operator Transform(string value)
		{
			Transform result = new Transform();
			if (value.NotEmpty())
			{
				try
				{
					string[] values = value.Split(new char[] { ',', ' ', ';' }, StringSplitOptions.RemoveEmptyEntries);
					if (values.Length == 12)
						result = new Transform(
							Kean.Math.Integer.Parse(values[0]), Kean.Math.Integer.Parse(values[1]), Kean.Math.Integer.Parse(values[2]),
							Kean.Math.Integer.Parse(values[3]), Kean.Math.Integer.Parse(values[4]), Kean.Math.Integer.Parse(values[5]),
							Kean.Math.Integer.Parse(values[6]), Kean.Math.Integer.Parse(values[7]), Kean.Math.Integer.Parse(values[8]),
							Kean.Math.Integer.Parse(values[9]), Kean.Math.Integer.Parse(values[10]), Kean.Math.Integer.Parse(values[11]));
				}
				catch
				{
				}
			}
			return result;
		}
		public static explicit operator int[,](Transform value)
		{
			return new int[,] { 
						{ value[0, 0], value[0, 1], value[0, 2], value[0, 3] }, 
						{ value[1, 0], value[1, 1], value[1, 2], value[1, 3] }, 
						{ value[2, 0], value[2, 1], value[2, 2], value[2, 3] }, 
						{ value[3, 0], value[3, 1], value[3, 2], value[3, 3] }};
		}
		public static implicit operator string(Transform value)
		{
			return value.NotNull() ? value.ToString() : null;
		}
		public static explicit operator Transform(Geometry2D.Integer.Transform value)
		{
			int zero = 0;
			int one = 1;
			return new Transform(value.A, value.B, zero, value.C, value.D, zero, zero, zero, zero, value.E, value.F, zero);
		}
		public static explicit operator Geometry2D.Integer.Transform(Transform value)
		{
			return new Geometry2D.Integer.Transform(value.A, value.B, value.D, value.E, value.J, value.K);
		}
		#endregion
	}
}
