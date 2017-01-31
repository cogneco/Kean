using System;
using Geometry3D = Kean.Math.Geometry3D;
using Kean.Extension;

namespace Kean.Math.Geometry3D.Double
{
    public struct EuclidTransform :
        IEquatable<EuclidTransform>
    {
        public Geometry3D.Double.Rotation Rotation { get; private set; }
        public Geometry3D.Double.Size Translation { get; private set; }

        public EuclidTransform Inverse { get { return new EuclidTransform(this.Rotation.Inverse, -1*this.Translation); } }

        public EuclidTransform(Geometry3D.Double.Rotation rotation) :
            this(rotation, new Geometry3D.Double.Size(0, 0, 0))
        { }
        public EuclidTransform(Geometry3D.Double.Size translation) :
            this(new Geometry3D.Double.Rotation(0,0,0), translation)
        { }
        public EuclidTransform(Geometry3D.Double.Rotation rotation, Geometry3D.Double.Size translation) : 
            this()
        {
            this.Rotation = rotation;
            this.Translation = translation;
        }
        public EuclidTransform(double x, double y, double z, double rx, double ry, double rz) :
            this()
        {
            this.Rotation = new Geometry3D.Double.Rotation(rx, ry, rz);
            this.Translation = new Geometry3D.Double.Size(x, y, z);
        }
        public EuclidTransform Rotate(Rotation rotation)
        {
            return new EuclidTransform(rotation) * this;
        }
        public EuclidTransform Translate(Geometry3D.Double.Size translation)
        {
            return new EuclidTransform(translation) * this;
        }
        #region Object overrides
        public override bool Equals(object other)
        {
            return (other is EuclidTransform) ? this.Equals(other) : false;
        }
        public override int GetHashCode()
        {
            return (33 * this.Rotation.GetHashCode() ^ this.Translation.GetHashCode());
        }
        public override string ToString()
        {
            return this.Translation.ToString() +"; " + this.Rotation.ToString();
        }
        #endregion
        #region IEquatable<Rotation> Members
        public bool Equals(EuclidTransform other)
        {
            return other.NotNull() && this.Rotation == other.Rotation && this.Translation.Equals(other.Translation);
        }
        #endregion
        #region Binary Operators
        public static EuclidTransform operator +(EuclidTransform left, EuclidTransform right)
        {
            return new EuclidTransform(left.Rotation + right.Rotation, left.Translation + right.Translation);
        }
        public static EuclidTransform operator /(EuclidTransform left, double right)
        {
            return new EuclidTransform(left.Rotation / right, left.Translation / right);
        }
        public static Geometry3D.Double.Point operator *(EuclidTransform left, Geometry3D.Double.Point right)
        {
            return (left.NotNull() && right.NotNull()) ? left.Translation + (left.Rotation * right) : null;
        }
        public static EuclidTransform operator *(EuclidTransform left, EuclidTransform right)
        {
            return new EuclidTransform(left.Rotation * right.Rotation, left.Translation + right.Translation);
        }
        public static bool operator ==(EuclidTransform left, EuclidTransform right)
        {
            return left.NotNull() ? left.Equals(right) : right.IsNull();
        }
        public static bool operator !=(EuclidTransform left, EuclidTransform right)
        {
            return !(left == right);
        }
        #endregion
        #region Casts
        public static implicit operator Geometry3D.Double.Transform(EuclidTransform value)
        {
            return  Geometry3D.Double.Transform.CreateRotationX(value.Rotation.RotationX).RotateY(value.Rotation.RotationY).RotateZ(value.Rotation.RotationZ).Translate(value.Translation);
        }
        public static implicit operator string(EuclidTransform value)
        {
            return value.NotNull() ? value.ToString() : null;
        }
        #endregion
        #region Static
        public static EuclidTransform Identity { get { return new EuclidTransform(); } }
        #endregion

    }
}

