// 
//  Size.cs
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

namespace Kean.Math.Geometry2D.Test.Integer
{
	[TestFixture]
	public class Size :
		Kean.Test.Fixture<Size>
	{
		string prefix = "Kean.Math.Geometry2D.Test.Integer.Size";
		float Precision { get { return 1e-4f; } }
		Target.Integer.Size CastFromString(string value)
		{
			return (Target.Integer.Size)value;
		}
		string CastToString(Target.Integer.Size value)
		{
			return value;
		}
		Target.Integer.Size vector0 = new Target.Integer.Size((int)22.221f, (int)-3.1f);
		Target.Integer.Size vector1 = new Target.Integer.Size((int)12.221f, (int)13.1f);
		Target.Integer.Size vector2 = new Target.Integer.Size((int)34.442f, (int)10.0f);
		Target.Integer.Size vector3 = new Target.Integer.Size((int)10, (int)20);

		protected override void Run()
		{
			this.Run(
				this.Polar0,
				this.Polar1,
				this.Polar2,
				this.Polar3,
				this.Polar4,
				this.Polar5,
				this.Angles,
				this.Casts,
				this.StringCasts
				);
		}
		#region Equality
		[Test]
		public void Equality()
		{
#pragma warning disable 1718
			Target.Integer.Size point = new Target.Integer.Size();
			Verify(this.vector0, Is.EqualTo(this.vector0));
			Verify(this.vector0.Equals(this.vector0 as object), Is.True);
			Verify(this.vector0 == this.vector0, Is.True);
			Verify(this.vector0 != this.vector1, Is.True);
			Verify(this.vector0 == point, Is.False);
			Verify(point == point, Is.True);
			Verify(point == this.vector0, Is.False);
#pragma warning restore 1718
		}
		#endregion
		#region Arithmetic
		[Test]
		public void Addition()
		{
			Verify((this.vector0 + this.vector1).Width, Is.EqualTo(this.vector2.Width).Within(this.Precision));
			Verify((this.vector0 + this.vector1).Height, Is.EqualTo(this.vector2.Height).Within(this.Precision));
		}
		[Test]
		public void Subtraction()
		{
			Verify(this.vector0 - this.vector0, Is.EqualTo(new Target.Integer.Size()));
		}
		[Test]
		public void ScalarMultiplication()
		{
			Verify((-1) * this.vector0, Is.EqualTo(-this.vector0));
		}
		[Test]
		public void ScalarDivision()
		{
			Verify(this.vector0 / (-1), Is.EqualTo(-this.vector0));
		}
		#endregion
		#region Hash Code
		[Test]
		public void Hash()
		{
			Verify(this.vector0.Hash(), Is.Not.EqualTo(0));
		}
		#endregion

		[Test]
		public void GetValues()
		{
			Verify(this.vector0.Width, Is.EqualTo((int)(22.221)).Within(this.Precision), this.prefix + "GetValues.0");
			Verify(this.vector0.Height, Is.EqualTo((int)(-3.1)).Within(this.Precision), this.prefix + "GetValues.1");
		}
		[Test]
		public void Swap()
		{
			Target.Integer.Size result = this.vector0.Swap();
			Verify(result.Width, Is.EqualTo(this.vector0.Height), this.prefix + "Swap.0");
			Verify(result.Height, Is.EqualTo(this.vector0.Width), this.prefix + "Swap.1");
		}
		[Test]
		public void Casting()
		{
			string value = "10, 20";
			Verify(this.CastToString(this.vector3), Is.EqualTo(value), this.prefix + "Casting.0");
			Verify(this.CastFromString(value), Is.EqualTo(this.vector3), this.prefix + "Casting.1");
		}
		[Test]
		public void CastingNull()
		{
			string value = null;
			Target.Integer.Size point = new Target.Integer.Size();
			Verify(this.CastToString(point), Is.EqualTo(value), this.prefix + "CastingNull.0");
			Verify(this.CastFromString(value), Is.EqualTo(point), this.prefix + "CastingNull.1");
		}

		#region Polar Representation
		[Test]
		public void Polar0()
		{
			Target.Integer.Size point = new Target.Integer.Size();
			Verify(point.Norm, Is.EqualTo(0));
			Verify(point.Azimuth, Is.EqualTo(0));
		}
		[Test]
		public void Polar1()
		{
			Target.Integer.Size point = new Target.Integer.Size(1, 0);
			Verify(point.Norm, Is.EqualTo(1));
			Verify(point.Azimuth, Is.EqualTo(0));
		}
		[Test]
		public void Polar2()
		{
			Target.Integer.Size point = new Target.Integer.Size(0, 1);
			Verify(point.Norm, Is.EqualTo(1));
			Verify(point.Azimuth, Is.EqualTo(Kean.Math.Integer.ToRadians(90)));
		}
		[Test]
		public void Polar3()
		{
			Target.Integer.Size point = new Target.Integer.Size(0, -5);
			Verify(point.Norm, Is.EqualTo(5));
			Verify(point.Azimuth, Is.EqualTo(Kean.Math.Integer.ToRadians(-90)));
		}
		[Test]
		public void Polar4()
		{
			Target.Integer.Size point = new Target.Integer.Size(-1, 0);
			Verify(point.Norm, Is.EqualTo(1));
			Verify(point.Azimuth, Is.EqualTo(Kean.Math.Integer.ToRadians(180)));
		}
		[Test]
		public void Polar5()
		{
			Target.Integer.Size point = new Target.Integer.Size(-3, 0);
			int radius = point.Norm;
			int azimuth = point.Azimuth;
			Target.Integer.Size point2 = Target.Integer.Size.Polar(radius, azimuth);
			Verify(point.Distance(point2), Is.EqualTo(0).Within(this.Precision));
		}
		#endregion
		[Test]
		public void Angles()
		{
			Verify(Target.Single.Size.BasisX.Angle(Target.Single.Size.BasisX), Is.EqualTo(0).Within(this.Precision));
			Verify(Target.Single.Size.BasisX.Angle(Target.Single.Size.BasisY), Is.EqualTo(Kean.Math.Single.Pi / 2).Within(this.Precision));
			Verify(Target.Single.Size.BasisX.Angle(-Target.Single.Size.BasisY), Is.EqualTo(-Kean.Math.Single.Pi / 2).Within(this.Precision));
			Verify(Target.Single.Size.BasisX.Angle(-Target.Single.Size.BasisX), Is.EqualTo(Kean.Math.Single.Pi).Within(this.Precision));
		}
		[Test]
		public void Casts()
		{ }
		[Test]
		public void StringCasts()
		{
			string textFromValue = new Target.Integer.Size(10, 20);
			Verify(textFromValue, Is.EqualTo("10, 20"));
			Target.Integer.Size @integerFromText = (Target.Integer.Size)"10, 20";
			Verify(@integerFromText.Width, Is.EqualTo(10));
			Verify(@integerFromText.Height, Is.EqualTo(20));
		}
	}
}
