// 
//  Transform.cs
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
using NUnit.Framework;
using Target = Kean.Math.Geometry2D;
using Kean.Extension;

namespace Kean.Math.Geometry2D.Test.Double
{
	[TestFixture]
	public class Transform :
		Kean.Test.Fixture<Transform>
	{
		string prefix = "Kean.Math.Geometry2D.Test.Double.Transform";
		float Precision { get { return 1e-4f; } }
		Target.Double.Transform transform0;
		Target.Double.Transform transform1;
		Target.Double.Transform transform2;
		Target.Double.Transform transform3;
		Target.Double.Transform transform4;
		Target.Double.Point point0;
		Target.Double.Point point1;
		Target.Double.Size size0;

		[TestFixtureSetUp]
		public override void Setup()
		{
			this.transform0 = new Target.Double.Transform(3, 1, 2, 1, 5, 7);
			this.transform1 = new Target.Double.Transform(7, 4, 2, 5, 7, 6);
			this.transform2 = new Target.Double.Transform(29, 11, 16, 7, 38, 20);
			this.transform3 = new Target.Double.Transform(1, -1, -2, 3, 9, -16);
			this.transform4 = new Target.Double.Transform(10, 20, 30, 40, 50, 60);
			this.point0 = new Target.Double.Point(-7, 3);
			this.point1 = new Target.Double.Point(-10,3);
			this.size0 = new Target.Double.Size(10, 10);
		}
		protected override void Run()
		{
			this.Run(
				 this.Equality,
				this.CreateZeroTransform,
				this.CreateIdentity,
				this.CreateRotation,
				this.CreateScale,
				this.CreateTranslation,
				this.Rotatate,
				this.Scale,
				this.Translatate,
				this.InverseTransform,
				this.MultiplicationTransformTransform,
				this.MultiplicationTransformPoint,
				this.GetValueValues,
				this.CastToArray,
				this.GetTranslation,
				this.GetScalingX,
				this.GetScalingY,
				this.GetScaling,
				this.Casting,
				this.Hash,
				this.Casts,
				this.StringCasts
				);
		}
		double Cast(double value)
		{
			return (double)value;
		}
		Target.Double.Transform CastFromString(string value)
		{
			return (Target.Double.Transform)value;
		}
		string CastToString(Target.Double.Transform value)
		{
			return (string)value;
		}
		#region Equality
		[Test]
		public void Equality()
		{
#pragma warning disable 1718
			Target.Double.Transform transform = new Target.Double.Transform();
			Verify(this.transform0, Is.EqualTo(this.transform0));
			Verify(this.transform0.Equals(this.transform0 as object), Is.True);
			Verify(this.transform0 == this.transform0, Is.True);
			Verify(this.transform0 != this.transform1, Is.True);
			Verify(this.transform0 == transform, Is.False);
			Verify(transform == transform, Is.True);
			Verify(transform == this.transform0, Is.False);
#pragma warning restore 1718
		}
		#endregion
		#region Arithmetic
		[Test]
		public void InverseTransform()
		{
			Verify(this.transform0.Inverse, Is.EqualTo(this.transform3));
		}
		[Test]
		public void MultiplicationTransformTransform()
		{
			Verify(this.transform0 * this.transform1, Is.EqualTo(this.transform2));
		}
		[Test]
		public void MultiplicationTransformPoint()
		{
			Verify(this.transform0 * this.point0, Is.EqualTo(this.point1));
		}
		#endregion
		[Test]
		public void CreateZeroTransform()
		{
			Target.Double.Transform transform = new Target.Double.Transform();
			Verify(transform.A, Is.EqualTo(this.Cast(0)).Within(this.Precision), this.prefix + "CreateZeroTransform.0");
			Verify(transform.B, Is.EqualTo(this.Cast(0)).Within(this.Precision), this.prefix + "CreateZeroTransform.1");
			Verify(transform.D, Is.EqualTo(this.Cast(0)).Within(this.Precision), this.prefix + "CreateZeroTransform.2");
			Verify(transform.E, Is.EqualTo(this.Cast(0)).Within(this.Precision), this.prefix + "CreateZeroTransform.3");
			Verify(transform.G, Is.EqualTo(this.Cast(0)).Within(this.Precision), this.prefix + "CreateZeroTransform.4");
			Verify(transform.H, Is.EqualTo(this.Cast(0)).Within(this.Precision), this.prefix + "CreateZeroTransform.5");
		}
		[Test]
		public void CreateIdentity()
		{
			Target.Double.Transform transform = Target.Double.Transform.Identity;
			Verify(transform.A, Is.EqualTo(this.Cast(1)).Within(this.Precision), this.prefix + "CreateIdentity.0");
			Verify(transform.B, Is.EqualTo(this.Cast(0)).Within(this.Precision), this.prefix + "CreateIdentity.1");
			Verify(transform.D, Is.EqualTo(this.Cast(0)).Within(this.Precision), this.prefix + "CreateIdentity.2");
			Verify(transform.E, Is.EqualTo(this.Cast(1)).Within(this.Precision), this.prefix + "CreateIdentity.3");
			Verify(transform.G, Is.EqualTo(this.Cast(0)).Within(this.Precision), this.prefix + "CreateIdentity.4");
			Verify(transform.H, Is.EqualTo(this.Cast(0)).Within(this.Precision), this.prefix + "CreateIdentity.5");
		}
		[Test]
		public void Rotatate()
		{
			Target.Double.Transform identity = Target.Double.Transform.Identity;
			double angle = Kean.Math.Double.ToRadians(20);
			Target.Double.Transform transform = Target.Double.Transform.CreateZRotation(angle);
			transform = transform.Rotate(-angle);
			Verify(transform.A, Is.EqualTo(this.Cast(1)).Within(this.Precision), this.prefix + "Rotatate.0");
			Verify(transform.B, Is.EqualTo(this.Cast(0)).Within(this.Precision), this.prefix + "Rotatate.1");
			Verify(transform.D, Is.EqualTo(this.Cast(0)).Within(this.Precision), this.prefix + "Rotatate.2");
			Verify(transform.E, Is.EqualTo(this.Cast(1)).Within(this.Precision), this.prefix + "Rotatate.3");
			Verify(transform.G, Is.EqualTo(this.Cast(0)).Within(this.Precision), this.prefix + "Rotatate.4");
			Verify(transform.H, Is.EqualTo(this.Cast(0)).Within(this.Precision), this.prefix + "Rotatate.5");
		}
		[Test]
		public void Scale()
		{
			Target.Double.Transform identity = new Target.Double.Transform(20, 0, 0, 20, 0, 0);
			double scale = 20;
			Target.Double.Transform transform = Target.Double.Transform.CreateScaling(scale, scale);
			transform = transform.Scale(5, 5);
			Verify(transform.A, Is.EqualTo(this.Cast(100)).Within(this.Precision), this.prefix + "Scale.0");
			Verify(transform.B, Is.EqualTo(this.Cast(0)).Within(this.Precision), this.prefix + "Scale.1");
			Verify(transform.D, Is.EqualTo(this.Cast(0)).Within(this.Precision), this.prefix + "Scale.2");
			Verify(transform.E, Is.EqualTo(this.Cast(100)).Within(this.Precision), this.prefix + "Scale.3");
			Verify(transform.G, Is.EqualTo(this.Cast(0)).Within(this.Precision), this.prefix + "Scale.4");
			Verify(transform.H, Is.EqualTo(this.Cast(0)).Within(this.Precision), this.prefix + "Scale.5");
		}
		[Test]
		public void Translatate()
		{
			double xDelta = 40;
			double yDelta = -40;
			Target.Double.Transform transform = Target.Double.Transform.CreateTranslation(xDelta, yDelta);
			transform = transform.Translate(-xDelta, -yDelta);
			Verify(transform.A, Is.EqualTo(this.Cast(1)).Within(this.Precision), this.prefix + "Translatate.0");
			Verify(transform.B, Is.EqualTo(this.Cast(0)).Within(this.Precision), this.prefix + "Translatate.1");
			Verify(transform.D, Is.EqualTo(this.Cast(0)).Within(this.Precision), this.prefix + "Translatate.2");
			Verify(transform.E, Is.EqualTo(this.Cast(1)).Within(this.Precision), this.prefix + "Translatate.3");
			Verify(transform.G, Is.EqualTo(this.Cast(0)).Within(this.Precision), this.prefix + "Translatate.4");
			Verify(transform.H, Is.EqualTo(this.Cast(0)).Within(this.Precision), this.prefix + "Translatate.5");
		}
		[Test]
		public void CreateRotation()
		{
			double angle = Kean.Math.Double.ToRadians(20);
			Target.Double.Transform transform = Target.Double.Transform.CreateZRotation(angle);
			Verify(transform.A, Is.EqualTo(Kean.Math.Double.Cosine(angle)).Within(this.Precision), this.prefix + "CreateRotation.0");
			Verify(transform.B, Is.EqualTo(Kean.Math.Double.Sine(angle)).Within(this.Precision), this.prefix + "CreateRotation.1");
			Verify(transform.D, Is.EqualTo(-Kean.Math.Double.Sine(angle)).Within(this.Precision), this.prefix + "CreateRotation.2");
			Verify(transform.E, Is.EqualTo(Kean.Math.Double.Cosine(angle)).Within(this.Precision), this.prefix + "CreateRotation.3");
			Verify(transform.G, Is.EqualTo(0).Within(this.Precision), this.prefix + "CreateRotation.4");
			Verify(transform.H, Is.EqualTo(0).Within(this.Precision), this.prefix + "CreateRotation.5");
		}
		[Test]
		public void CreateScale()
		{
			double scale = 20;
			Target.Double.Transform transform = Target.Double.Transform.CreateScaling(scale, scale);
			Verify(transform.A, Is.EqualTo(scale).Within(this.Precision), this.prefix + "CreateScale.0");
			Verify(transform.B, Is.EqualTo(0).Within(this.Precision), this.prefix + "CreateScale.1");
			Verify(transform.D, Is.EqualTo(0).Within(this.Precision), this.prefix + "CreateScale.2");
			Verify(transform.E, Is.EqualTo(scale).Within(this.Precision), this.prefix + "CreateScale.3");
			Verify(transform.G, Is.EqualTo(0).Within(this.Precision), this.prefix + "CreateScale.4");
			Verify(transform.H, Is.EqualTo(0).Within(this.Precision), this.prefix + "CreateScale.5");
		}
		[Test]
		public void CreateTranslation()
		{
			double xDelta = 40;
			double yDelta = -40;
			Target.Double.Transform transform = Target.Double.Transform.CreateTranslation(xDelta, yDelta);
			Verify(transform.A, Is.EqualTo(1).Within(this.Precision), this.prefix + "CreateTranslation.0");
			Verify(transform.B, Is.EqualTo(0).Within(this.Precision), this.prefix + "CreateTranslation.1");
			Verify(transform.D, Is.EqualTo(0).Within(this.Precision), this.prefix + "CreateTranslation.2");
			Verify(transform.E, Is.EqualTo(1).Within(this.Precision), this.prefix + "CreateTranslation.3");
			Verify(transform.G, Is.EqualTo(xDelta).Within(this.Precision), this.prefix + "CreateTranslation.4");
			Verify(transform.H, Is.EqualTo(yDelta).Within(this.Precision), this.prefix + "CreateTranslation.5");
		}
		[Test]
		public void GetValueValues()
		{
			Target.Double.Transform transform = this.transform0;
			Verify(transform.A, Is.EqualTo(this.Cast(3)).Within(this.Precision), this.prefix + "GetValueValues.0");
			Verify(transform.B, Is.EqualTo(this.Cast(1)).Within(this.Precision), this.prefix + "GetValueValues.1");
			Verify(transform.D, Is.EqualTo(this.Cast(2)).Within(this.Precision), this.prefix + "GetValueValues.2");
			Verify(transform.E, Is.EqualTo(this.Cast(1)).Within(this.Precision), this.prefix + "GetValueValues.3");
			Verify(transform.G, Is.EqualTo(this.Cast(5)).Within(this.Precision), this.prefix + "GetValueValues.4");
			Verify(transform.H, Is.EqualTo(this.Cast(7)).Within(this.Precision), this.prefix + "GetValueValues.5");
		}
		[Test]
		public void GetScalingX()
		{
			double scale = this.transform0.ScalingX;
			Verify(scale, Is.EqualTo(this.Cast(3.162277f)).Within(this.Precision), this.prefix + "GetScalingX.0");
		}
		[Test]
		public void GetScalingY()
		{
			double scale = this.transform0.ScalingY;
			Verify(scale, Is.EqualTo(this.Cast( 2.23606801f)).Within(this.Precision), this.prefix + "GetScalingY.0");
		}
		[Test]
		public void GetScaling()
		{
			double scale = this.transform0.Scaling;
			Verify(scale, Is.EqualTo(this.Cast(2.69917297f)).Within(this.Precision), this.prefix + "GetScaling.0");
		}
		
		[Test]
		public void GetTranslation()
		{
			Target.Double.Size translation = this.transform0.Translation;
			Verify(translation.Width, Is.EqualTo(this.Cast(5)).Within(this.Precision), this.prefix + "GetTranslation.0");
			Verify(translation.Height, Is.EqualTo(this.Cast(7)).Within(this.Precision), this.prefix + "GetTranslation.1");
		}
		[Test]
		public void CastToArray()
		{
			double[,] values = (double[,])(Target.Double.Transform.Identity);
			for (int x = 0; x < 3; x++)
				for (int y = 0; y < 3; y++)
					Verify(values[x, y], Is.EqualTo(this.Cast(x == y ? 1 : 0)).Within(this.Precision), this.prefix + "CastToArray.0");
		}
		[Test]
		public void Casting()
		{
			string value = "10, 20, 30, 40, 50, 60";
			Verify(this.CastToString(this.transform4), Is.EqualTo(value), this.prefix + "Casting.0");
			Verify(this.CastFromString(value), Is.EqualTo(this.transform4), this.prefix + "Casting.1");
		}
		#region Hash Code
		[Test]
		public void Hash()
		{
			Verify(this.transform0.Hash(), Is.Not.EqualTo(0));
		}
		#endregion

		[Test]
		public void Casts()
		{
			// integer - double
			{
				Target.Integer.Transform integer = new Target.Integer.Transform(10, 20, 30, 40, 50, 60);
				Target.Double.Transform @double = integer;
				Verify(@double.A, Is.EqualTo(10));
				Verify(@double.B, Is.EqualTo(20));
				Verify(@double.D, Is.EqualTo(30));
				Verify(@double.E, Is.EqualTo(40));
				Verify(@double.G, Is.EqualTo(50));
				Verify(@double.H, Is.EqualTo(60));
				Verify((Target.Integer.Transform)@double, Is.EqualTo(integer));
			}
			// single - double
			{
				Target.Single.Transform integer = new Target.Single.Transform(10, 20, 30, 40, 50, 60);
				Target.Double.Transform @double = integer;
				Verify(@double.A, Is.EqualTo(10));
				Verify(@double.B, Is.EqualTo(20));
				Verify(@double.D, Is.EqualTo(30));
				Verify(@double.E, Is.EqualTo(40));
				Verify(@double.G, Is.EqualTo(50));
				Verify(@double.H, Is.EqualTo(60));
				Verify((Target.Single.Transform)@double, Is.EqualTo(integer));
			}
		}
		[Test]
		public void StringCasts()
		{
			string textFromValue = new Target.Double.Transform(10, 20, 30, 40, 50, 60);
			Verify(textFromValue, Is.EqualTo("10, 20, 30, 40, 50, 60"));
			Target.Double.Transform @integerFromText = (Target.Integer.Transform)"10 20 30 40 50 60";
			Verify(@integerFromText.A, Is.EqualTo(10));
			Verify(@integerFromText.B, Is.EqualTo(20));
			Verify(@integerFromText.D, Is.EqualTo(30));
			Verify(@integerFromText.E, Is.EqualTo(40));
			Verify(@integerFromText.G, Is.EqualTo(50));
			Verify(@integerFromText.H, Is.EqualTo(60));
		}

	}
}

