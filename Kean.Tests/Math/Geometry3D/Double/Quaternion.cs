//using System;
//using NUnit.Framework;


//using Kean.Extension;
//using Target = Kean.Math.Geometry3D.Double;
//namespace Kean.Math.Geometry3D.Test.Double
//{

//	[TestFixture]
//	public class Quaternion :
//		Kean.Test.Fixture<Quaternion>
//	{
//		Target.Quaternion q0;
//		Target.Quaternion q1;
//		Target.Quaternion q2;
//		Target.Quaternion q3;
//		Target.Point p0;
//		Target.Point p1;
//		public double Precision { get { return 1e-5; } }
//		[TestFixtureSetUp]
//		public override void Setup()
//		{
//			this.q0 = new Target.Quaternion(33, 10, -12, 54.5f);
//			this.q1 = new Target.Quaternion(10, 17, -10, 14.5f);
//			this.q2 = new Target.Quaternion(43, 27, -22, 69);
//			this.q3 = new Target.Quaternion(-750.25f, 1032, 331.5f, 1127.5f);
//			this.p0 = new Target.Point(22.221f, -3.1f, 10);
//			this.p1 = new Target.Point(12.221f, 13.1f, 20);
//		}
//		protected override void Run()
//		{
//			this.Run(
//				this.RollPitchYaw,
//				this.Action,
//				this.Action2,
//				this.ActionOnVector,
//				this.Addition,
//				this.Casting,
//				this.CastingNull,
//				this.CastToTransform,
//				this.CastToTransformMultiplication,
//				this.Equality,
//				this.GetValues,
//				this.InverseMatrix,
//				this.LogarithmExponential,
//				this.Multitplication,
//				this.Norm,
//				this.Pitch,
//				this.Roll,
//				this.RotationDirectionRepresentation1,
//				this.RotationDirectionRepresentation2,
//				this.RotationDirectionRepresentation3,
//				this.RotationDirectionRepresentation4,
//				this.Yaw,
//				this.SplitOutRotation
//				);
//		}

//		double Cast(double value)
//		{
//			return (double)value;
//		}
//		Target.Quaternion CastFromString(string value)
//		{
//			return (Target.Quaternion)value;
//		}
//		string CastToString(Target.Quaternion value)
//		{
//			return (string)value;
//		}
//		#region Equality
//		[Test]
//		public void Equality()
//		{
//			Target.Quaternion quaternion = null;
//			Verify(this.q0, Is.EqualTo(this.q0));
//			Verify(this.q0.Copy(), Is.EqualTo(this.q0));
//			Verify(this.q0.Copy().Equals(this.q0), Is.True);
//			Verify(this.q0.Equals(this.q0 as object), Is.True);
//			Verify(this.q0 == this.q0, Is.True);
//			Verify(this.q0 != this.q1, Is.True);
//			Verify(this.q0 == quaternion, Is.False);
//			Verify(quaternion == quaternion, Is.True);
//			Verify(quaternion == this.q0, Is.False);
//		}
//		#endregion

//		#region Arithmetic
//		[Test]
//		public void Addition()
//		{
//			Verify(this.q0 + this.q1, Is.EqualTo(this.q2));
//		}
//		[Test]
//		public void Subtraction()
//		{
//			Verify(this.q0 - this.q0, Is.EqualTo(new Target.Quaternion()));
//		}
//		[Test]
//		public void ScalarMultitplication()
//		{
//			Verify((-1) * this.q0, Is.EqualTo(-this.q0));
//		}
//		[Test]
//		public void Multitplication()
//		{
//			Verify(this.q0 * this.q1, Is.EqualTo(this.q3));
//		}

//		#endregion
//		[Test]
//		public void GetValues()
//		{
//			double[] values = (double[])(this.q0);
//			Verify(values[0], Is.EqualTo(33).Within(this.Precision));
//			Verify(values[1], Is.EqualTo(10).Within(this.Precision));
//			Verify(values[2], Is.EqualTo(-12).Within(this.Precision));
//			Verify(values[3], Is.EqualTo(54.5).Within(this.Precision));
//		}
//		[Test]
//		public void LogarithmExponential()
//		{
//			double roll = Kean.Math.Double.ToRadians(20);
//			double pitch = Kean.Math.Double.ToRadians(-30);
//			double yaw = Kean.Math.Double.ToRadians(45);
//			Target.Quaternion quaternion = Target.Quaternion.CreateRotationZ(yaw) * Target.Quaternion.CreateRotationY(pitch) * Target.Quaternion.CreateRotationX(roll);
//			Verify(quaternion.Exponential().Logarithm().Real, Is.EqualTo(quaternion.Real).Within(this.Precision));
//			Verify(quaternion.Logarithm().Exponential().Real, Is.EqualTo(quaternion.Real).Within(this.Precision));
//			Verify(quaternion.Exponential().Logarithm().Imaginary.Distance(quaternion.Imaginary).Value, Is.EqualTo(0).Within(this.Precision));
//			Verify(quaternion.Logarithm().Exponential().Imaginary.Distance(quaternion.Imaginary).Value, Is.EqualTo(0).Within(this.Precision));
//		}
//		[Test]
//		public void Norm()
//		{
//			Verify(this.q0.Norm, Is.EqualTo(this.Cast(65.5991592)).Within(this.Precision));
//		}
//		[Test]
//		public void ActionOnVector()
//		{
//			Target.Point direction = Target.Point.Create(1, 1, 1);
//			Target.Quaternion quaternion = Target.Quaternion.CreateRotation(Kean.Math.Double.ToRadians(120), direction);
//			Target.Point point = Target.Point.Create(5, 6, 7);
//			Target.Point point2 = Target.Point.Create(7, 5, 6);
//			Verify((quaternion * point).Distance(point2).Value, Is.EqualTo(0).Within(this.Precision));
//		}
//		[Test]
//		public void Casting()
//		{
//			string value = "1, 2, 3, 4";
//			Target.Quaternion quaternion = (Target.Quaternion)(1.0) + Target.Quaternion.BasisImaginaryX * 2 + Target.Quaternion.BasisImaginaryY * 3 + Target.Quaternion.BasisImaginaryZ * 4;
//			Verify(this.CastToString(quaternion), Is.EqualTo(value));
//			Verify(this.CastFromString(value), Is.EqualTo(quaternion));
//		}
//		[Test]
//		public void CastingNull()
//		{
//			string value = null;
//			Target.Quaternion quaternion = null;
//			Verify(this.CastToString(quaternion), Is.EqualTo(value));
//			Verify(this.CastFromString(value), Is.EqualTo(quaternion));
//		}
//		[Test]
//		public void InverseMatrix()
//		{
//			Target.Quaternion q = Target.Quaternion.CreateRotationX(Kean.Math.Double.ToRadians(30)) * Target.Quaternion.CreateRotationY(Kean.Math.Double.ToRadians(-20)) * Target.Quaternion.CreateRotationZ(Kean.Math.Double.ToRadians(50));
//			Kean.Math.Matrix.Double matrix = (Kean.Math.Matrix.Double)(double[,])q;
//			Kean.Math.Matrix.Double matrixInverse = (Kean.Math.Matrix.Double)(double[,])(q.Inverse);
//			Kean.Math.Matrix.Double matrixInverse2 = matrix.Inverse();
//			Verify(matrixInverse.Distance(matrixInverse2), Is.LessThan(0.000001));
//		}
//		[Test]
//		public void Action()
//		{
//			double roll = Kean.Math.Double.ToRadians(30);
//			double pitch = Kean.Math.Double.ToRadians(20);
//			double yaw = Kean.Math.Double.ToRadians(-45);
//			Target.Quaternion quaternion = Target.Quaternion.CreateRotationZ(yaw) * Target.Quaternion.CreateRotationY(pitch) * Target.Quaternion.CreateRotationX(roll);
//			Verify(quaternion.RotationX, Is.EqualTo(roll).Within(this.Precision));
//			Verify(quaternion.RotationY, Is.EqualTo(pitch).Within(this.Precision));
//			Verify(quaternion.RotationZ, Is.EqualTo(yaw).Within(this.Precision));
//		}
//		[Test]
//		public void RotationDirectionRepresentation1()
//		{
//			double angle = Kean.Math.Double.ToRadians(30);
//			Target.Point direction = new Target.Point(1, 4, 7);
//			direction /= direction.Norm;
//			Target.Quaternion q = Target.Quaternion.CreateRotation(angle, direction);
//			Verify(q.Rotation, Is.EqualTo(angle).Within(this.Precision));
//			Verify(q.Direction.Distance(direction).Value, Is.EqualTo(0).Within(this.Precision));
//		}
//		[Test]
//		public void RotationDirectionRepresentation2()
//		{
//			double angle = Kean.Math.Double.ToRadians(110);
//			Target.Point direction = new Target.Point(1, 4, 7);
//			direction /= direction.Norm;
//			Target.Quaternion q = Target.Quaternion.CreateRotation(angle, direction);
//			Verify(q.Rotation, Is.EqualTo(angle).Within(this.Precision));
//			Verify(q.Direction.Distance(direction).Value, Is.EqualTo(0).Within(this.Precision));
//		}
//		[Test]
//		public void RotationDirectionRepresentation3()
//		{
//			double angle = Kean.Math.Double.ToRadians(110);
//			Target.Point direction = new Target.Point(0, 0, 5);
//			direction /= direction.Norm;
//			Target.Quaternion q = Target.Quaternion.CreateRotation(angle, direction);
//			Verify(q.Rotation, Is.EqualTo(angle).Within(this.Precision));
//			Verify(q.Direction.Distance(direction).Value, Is.EqualTo(0).Within(this.Precision));
//		}
//		[Test]
//		public void RotationDirectionRepresentation4()
//		{
//			double angle = Kean.Math.Double.ToRadians(110);
//			Target.Point direction = new Target.Point(0, 0, -5);
//			direction /= direction.Norm;
//			Target.Quaternion q = Target.Quaternion.CreateRotation(angle, direction);
//			Verify(q.Rotation, Is.EqualTo(angle).Within(this.Precision));
//			Verify(q.Direction.Distance(direction).Value, Is.EqualTo(0).Within(this.Precision));
//		}

//		[Test]
//		public void Roll()
//		{
//			double angle = Kean.Math.Double.ToRadians(20);
//			Target.Quaternion quaternion = Target.Quaternion.CreateRotationX(angle);
//			Target.Transform rotation = (Target.Transform)(quaternion);
//			Target.Transform rotation2 = Target.Transform.CreateRotationX(angle);
//			Verify(((Kean.Math.Matrix.Double)(double[,])(rotation)).Distance((Kean.Math.Matrix.Double)(double[,])rotation2), Is.EqualTo(0).Within(this.Precision));
//			Verify(quaternion.RotationX, Is.EqualTo(angle).Within(this.Precision));
//			Verify(quaternion.Rotation, Is.EqualTo(angle).Within(this.Precision));
//		}
//		[Test]
//		public void Pitch()
//		{
//			double angle = Kean.Math.Double.ToRadians(20);
//			Target.Quaternion quaternion = Target.Quaternion.CreateRotationY(angle);
//			Target.Transform rotation = (Target.Transform)(quaternion);
//			Target.Transform rotation2 = Target.Transform.CreateRotationY(-angle);
//			Verify(((Kean.Math.Matrix.Double)(double[,])(rotation)).Distance((Kean.Math.Matrix.Double)(double[,])rotation2), Is.EqualTo(0).Within(this.Precision));
//			Verify(quaternion.RotationY, Is.EqualTo(angle).Within(this.Precision));
//			Verify(quaternion.Rotation, Is.EqualTo(angle).Within(this.Precision));
//		}
//		[Test]
//		public void Yaw()
//		{
//			double angle = Kean.Math.Double.ToRadians(20);
//			Target.Quaternion quaternion = Target.Quaternion.CreateRotationZ(angle);
//			Target.Transform rotation = (Target.Transform)(quaternion);
//			Target.Transform rotation2 = Target.Transform.CreateRotationZ(angle);
//			Verify(((Kean.Math.Matrix.Double)(double[,])(rotation)).Distance((Kean.Math.Matrix.Double)(double[,])rotation2), Is.EqualTo(0).Within(this.Precision));
//			Verify(quaternion.RotationZ, Is.EqualTo(angle).Within(this.Precision));
//			Verify(quaternion.Rotation, Is.EqualTo(angle).Within(this.Precision));
//		}
//		[Test]
//		public void Action2()
//		{
//			double roll = Kean.Math.Double.ToRadians(20);
//			double pitch = Kean.Math.Double.ToRadians(-30);
//			double yaw = Kean.Math.Double.ToRadians(45);
//			Target.Quaternion quaternion = Target.Quaternion.CreateRotationZ(yaw) * Target.Quaternion.CreateRotationY(pitch) * Target.Quaternion.CreateRotationX(roll);
//			Target.Point rotatedPointQuaternion = quaternion * this.p0;
//			Kean.Math.Matrix.Double rotation = (Kean.Math.Matrix.Double)(double[,])(quaternion);
//			Kean.Math.Matrix.Double rotatedPoint = rotation * (Kean.Math.Matrix.Double)((double[])(this.p0));
//			Target.Point rotatedPointRotation = (Target.Point)(double[])(rotatedPoint);
//			Verify(rotatedPointQuaternion.Distance(rotatedPointRotation).Value, Is.EqualTo(0).Within(this.Precision));
//			Verify(quaternion.RotationX, Is.EqualTo(roll).Within(this.Precision));
//			Verify(quaternion.RotationY, Is.EqualTo(pitch).Within(this.Precision));
//			Verify(quaternion.RotationZ, Is.EqualTo(yaw).Within(this.Precision));
//		}
//		[Test]
//		public void CastToTransform()
//		{
//			double angle = Kean.Math.Double.ToRadians(20);
//			Target.Quaternion quaternion = Target.Quaternion.CreateRotationX(angle);
//			Target.Transform transform0 = (Target.Transform)(quaternion);
//			Target.Transform transform1 = Target.Transform.CreateRotationX(angle);
//			Kean.Math.Matrix.Double matrix0 = (Kean.Math.Matrix.Double)(double[,])(Target.Transform)(transform0);
//			Kean.Math.Matrix.Double matrix1 = (Kean.Math.Matrix.Double)(double[,])(Target.Transform)(transform1);
//			Verify(matrix0.Distance(matrix1), Is.EqualTo(0).Within(this.Precision));
//		}
//		[Test]
//		public void CastToTransformMultiplication()
//		{
//			double roll = Kean.Math.Double.ToRadians(20);
//			double pitch = Kean.Math.Double.ToRadians(-30);
//			double yaw = Kean.Math.Double.ToRadians(45);
//			Target.Quaternion quaternion1 = Target.Quaternion.CreateRotationZ(yaw) * Target.Quaternion.CreateRotationY(pitch) * Target.Quaternion.CreateRotationX(roll);
//			double roll2 = Kean.Math.Double.ToRadians(40);
//			double pitch2 = Kean.Math.Double.ToRadians(37);
//			double yaw2 = Kean.Math.Double.ToRadians(-15);
//			Target.Quaternion quaternion2 = Target.Quaternion.CreateRotationZ(yaw2) * Target.Quaternion.CreateRotationY(pitch2) * Target.Quaternion.CreateRotationX(roll2);
//			Target.Transform transform1 = (Target.Transform)(Target.Quaternion)(quaternion1);
//			Target.Transform transform2 = (Target.Transform)(Target.Quaternion)(quaternion2);
//			Target.Transform transform = (Target.Transform)(quaternion1 * quaternion2);
//			Kean.Math.Matrix.Double matrix12 = (Kean.Math.Matrix.Double)(double[,])(Target.Transform)(transform1 * transform2);
//			Kean.Math.Matrix.Double matrix = (Kean.Math.Matrix.Double)(double[,])(Target.Transform)(transform);
//			Verify(matrix12.Distance(matrix), Is.EqualTo(0).Within(this.Precision));
//		}
//		[Test]
//		public void RollPitchYaw()
//		{
//			for (double r = -180; r < 180; r += 30)
//				for (double p = -90; p <= 90; p += 30)
//					for (double y = -180; y < 180; y += 30)
//					{
//						if (p != 90 && p != -90)
//						{
//							double roll = Kean.Math.Double.ToRadians(r);
//							double pitch = Kean.Math.Double.ToRadians(p);
//							double yaw = Kean.Math.Double.ToRadians(y);
//							Target.Quaternion quaternion = Target.Quaternion.CreateRotationZ(yaw) * Target.Quaternion.CreateRotationY(pitch) * Target.Quaternion.CreateRotationX(roll);
//							Target.Quaternion quaternion2 = Target.Quaternion.CreateRotationZ(quaternion.RotationZ) * Target.Quaternion.CreateRotationY(quaternion.RotationY) * Target.Quaternion.CreateRotationX(quaternion.RotationX);
//							Verify(this.AngleDistance(quaternion.RotationX, quaternion2.RotationX), Is.EqualTo(0).Within(this.Precision));
//							Verify(this.AngleDistance(quaternion.RotationY, quaternion2.RotationY), Is.EqualTo(0).Within(this.Precision));
//							Verify(this.AngleDistance(quaternion.RotationZ, quaternion2.RotationZ), Is.EqualTo(0).Within(this.Precision));
//							Verify(this.AngleDistance(quaternion.RotationX, roll), Is.EqualTo(0).Within(this.Precision));
//							Verify(this.AngleDistance(quaternion.RotationY, pitch), Is.EqualTo(0).Within(this.Precision));
//							Verify(this.AngleDistance(quaternion.RotationZ, yaw), Is.EqualTo(0).Within(this.Precision));
//						}
//					}
//		}
//		[Test]
//		public void SplitOutRotation()
//		{
//			for (double r = -180; r < 180; r += 45)
//				for (double p = -90; p <= 90; p += 45)
//					for (double y = -180; y < 180; y += 45)
//						for (double pp = -90; pp <= 90; pp += 45)
//							for (double yy = -180; yy < 180; yy += 45)
//								if (p != 90 && p != -90 && pp != 90 && pp != -45)
//								{
//									Target.Quaternion quaternion = Target.Quaternion.CreateRotationZ(Kean.Math.Double.ToRadians(y)) * Target.Quaternion.CreateRotationY(Kean.Math.Double.ToRadians(p)) * Target.Quaternion.CreateRotationX(Kean.Math.Double.ToRadians(r));
//									Target.Quaternion noRotation = Target.Quaternion.CreateRotationZ(Kean.Math.Double.ToRadians(yy)) * Target.Quaternion.CreateRotationY(Kean.Math.Double.ToRadians(pp)) * Target.Quaternion.CreateRotationX(0);
//									Target.Quaternion temporary = quaternion.Inverse * noRotation;
//									Target.Quaternion left = Target.Quaternion.CreateRotationZ(Kean.Math.Double.ToRadians(yy)) * Target.Quaternion.CreateRotationY(Kean.Math.Double.ToRadians(pp)) * Target.Quaternion.CreateRotationX(-temporary.RotationX);
//									Target.Quaternion right = Target.Quaternion.CreateRotationZ(temporary.RotationZ) * Target.Quaternion.CreateRotationY(temporary.RotationY) * Target.Quaternion.CreateRotationX(0);
//									//Verify(left.Distance(quaternion * right), Is.EqualTo(0).Within(this.Precision));
//									//Verify(noRotation.Distance(quaternion * temporary), Is.EqualTo(0).Within(this.Precision));
//									right = quaternion * right;
//									Verify(this.AngleDistance(left.RotationX, right.RotationX), Is.EqualTo(0).Within(this.Precision));
//									Verify(this.AngleDistance(left.RotationY, right.RotationY), Is.EqualTo(0).Within(this.Precision));
//									Verify(this.AngleDistance(left.RotationZ, right.RotationZ), Is.EqualTo(0).Within(this.Precision));
                                
//								}
//		}
//		double AngleDistance(double a, double b)
//		{
//			return (Kean.Math.Geometry2D.Double.Point.Polar(1, a) - Kean.Math.Geometry2D.Double.Point.Polar(1, b)).Norm;
//		}
//	}
//}
