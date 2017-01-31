using System;
using Geometry3D = Kean.Math.Geometry3D;
using Kean.Extension;

namespace Kean.Math.Geometry3D.Single
{
	public struct Rotation :
		IEquatable<Rotation>
	{
        public float RotationX { get; private set; }
        public float RotationY { get; private set; }
        public float RotationZ { get; private set; }
		public Rotation Inverse { get { return new Rotation(this.quaternion.Inverse); } }
		Geometry3D.Single.Quaternion quaternion;
		public Rotation(float rotationX, float rotationY, float rotationZ) :
			this(rotationX, rotationY, rotationZ,  Geometry3D.Single.Quaternion.CreateRotationZ(rotationZ) * Geometry3D.Single.Quaternion.CreateRotationY(rotationY) * Geometry3D.Single.Quaternion.CreateRotationX(rotationX))
		{ }
		Rotation(Geometry3D.Single.Quaternion value) :
			this(value.RotationX, value.RotationY, value.RotationZ, value)
		{ }
        Rotation(float rotationX, float rotationY, float rotationZ, Geometry3D.Single.Quaternion value) :
            this()
		{
			this.RotationX = rotationX;
			this.RotationY = rotationY;
			this.RotationZ = rotationZ;
			this.quaternion = value;
		}

		public Rotation Rotate(float rotationX, float rotationY, float rotationZ)
		{
			return new Rotation(rotationX, rotationY, rotationZ) * this;
		}
		#region Object overrides
		public override bool Equals(object other)
		{
            return (other is Rotation) ? this.Equals(other) : false;
		}
		public override int GetHashCode()
		{
			return this.quaternion.GetHashCode();
		}
		public override string ToString()
		{
			return this.RotationX + " " + this.RotationY + " " + this.RotationZ;
		}
		#endregion
		#region IEquatable<Rotation> Members
		public bool Equals(Rotation other)
		{
			return other.NotNull() && this.quaternion == other.quaternion;
		}
		#endregion
		#region Binary Operators
        public static Geometry3D.Single.Rotation operator +(Rotation left, Rotation right)
		{
            return new Rotation(left.RotationX + right.RotationX, left.RotationY + right.RotationY, left.RotationZ + right.RotationZ);
		}
		public static Geometry3D.Single.Rotation operator /(Rotation left, float right)
		{
            return new Rotation(left.RotationX / right, left.RotationY / right, left.RotationZ / right);
		}
		public static Geometry3D.Single.Point operator *(Rotation left, Geometry3D.Single.Point right)
		{
			return (left.NotNull() && right.NotNull()) ? left.quaternion * right : null;
		}
		public static Rotation operator *(Rotation left, Rotation right)
		{
            return new Rotation(left.quaternion * right.quaternion);
		}
		public static bool operator ==(Rotation left, Rotation right)
		{
			return left.NotNull() ? left.Equals(right) : right.IsNull();
		}
		public static bool operator !=(Rotation left, Rotation right)
		{
			return !(left == right);
		}
		#endregion
		#region Casts
		public static implicit operator string(Rotation value)
		{
			return value.NotNull() ? value.ToString() : null;
		}
		public static explicit operator Geometry3D.Single.Quaternion(Rotation value)
		{
			return value.NotNull() ? value.quaternion : null;
		}
		public static explicit operator Rotation(Geometry3D.Single.Quaternion value)
		{
			return new Rotation(value = Geometry3D.Single.Quaternion.BasisReal);
		}
		#endregion
		#region Static
		public static Rotation Identity { get { return new Rotation(); } }
		#endregion


	}
}
