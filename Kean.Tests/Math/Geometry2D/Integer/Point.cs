// 
//  Point.cs
//  
//  Author:
//       Anders Frisk <andersfrisk77@gmail.com>
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
using NUnit.Framework;
using Target = Kean.Math.Geometry2D;
using Kean.Extension;

namespace Kean.Math.Geometry2D.Test.Integer
{
    [TestFixture]
    public class Point :
        Kean.Test.Fixture<Point>
    {
        string prefix = "Kean.Math.Geometry2D.Test.Integer.Point";
        float Precision { get { return 1e-4f; } }
        Target.Integer.Point CastFromString(string value)
        {
            return (Target.Integer.Point)value;
        }
        string CastToString(Target.Integer.Point value)
        {
            return value;
        }
        Target.Integer.Point vector0 = new Target.Integer.Point((int)22.221f, (int)-3.1f);
        Target.Integer.Point vector1 = new Target.Integer.Point((int)12.221f, (int)13.1f);
        Target.Integer.Point vector2 = new Target.Integer.Point((int)34.442f, (int)10.0f);
        Target.Integer.Point vector3 = new Target.Integer.Point((int)10, (int)20);

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
			Target.Integer.Point point = new Target.Integer.Point();
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
            Verify((this.vector0 + this.vector1).X, Is.EqualTo(this.vector2.X).Within(this.Precision));
            Verify((this.vector0 + this.vector1).Y, Is.EqualTo(this.vector2.Y).Within(this.Precision));
        }
        [Test]
        public void Subtraction()
        {
            Verify(this.vector0 - this.vector0, Is.EqualTo(new Target.Integer.Point()));
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
            Verify(this.vector0.X, Is.EqualTo((int)(22.221)).Within(this.Precision), this.prefix + "GetValues.0");
            Verify(this.vector0.Y, Is.EqualTo((int)(-3.1)).Within(this.Precision), this.prefix + "GetValues.1");
        }
        [Test]
        public void Swap()
        {
            Target.Integer.Point result = this.vector0.Swap();
            Verify(result.X, Is.EqualTo(this.vector0.Y), this.prefix + "Swap.0");
            Verify(result.Y, Is.EqualTo(this.vector0.X), this.prefix + "Swap.1");
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
            Target.Integer.Point point = new Target.Integer.Point();
            Verify(this.CastToString(point), Is.EqualTo(value), this.prefix + "CastingNull.0");
            Verify(this.CastFromString(value), Is.EqualTo(point), this.prefix + "CastingNull.1");
        }

        #region Polar Representation
        [Test]
        public void Polar0()
        {
            Target.Integer.Point point = new Target.Integer.Point();
            Verify(point.Norm, Is.EqualTo(0));
            Verify(point.Azimuth, Is.EqualTo(0));
        }
        [Test]
        public void Polar1()
        {
            Target.Integer.Point point = new Target.Integer.Point(1, 0);
            Verify(point.Norm, Is.EqualTo(1));
            Verify(point.Azimuth, Is.EqualTo(0));
        }
        [Test]
        public void Polar2()
        {
            Target.Integer.Point point = new Target.Integer.Point(0, 1);
            Verify(point.Norm, Is.EqualTo(1));
            Verify(point.Azimuth, Is.EqualTo(Kean.Math.Integer.ToRadians(90)));
        }
        [Test]
        public void Polar3()
        {
            Target.Integer.Point point = new Target.Integer.Point(0, -5);
            Verify(point.Norm, Is.EqualTo(5));
            Verify(point.Azimuth, Is.EqualTo(Kean.Math.Integer.ToRadians(-90)));
        }
        [Test]
        public void Polar4()
        {
            Target.Integer.Point point = new Target.Integer.Point(-1, 0);
            Verify(point.Norm, Is.EqualTo(1));
            Verify(point.Azimuth, Is.EqualTo(Kean.Math.Integer.ToRadians(180)));
        }
        [Test]
        public void Polar5()
        {
            Target.Integer.Point point = new Target.Integer.Point(-3, 0);
            int radius = point.Norm;
            int azimuth = point.Azimuth;
            Target.Integer.Point point2 = Target.Integer.Point.Polar(radius, azimuth);
            Verify(point.Distance(point2), Is.EqualTo(0).Within(this.Precision));
        }
        #endregion
        [Test]
        public void Angles()
        {
            Verify(Target.Single.Point.BasisX.Angle(Target.Single.Point.BasisX), Is.EqualTo(0).Within(this.Precision));
            Verify(Target.Single.Point.BasisX.Angle(Target.Single.Point.BasisY), Is.EqualTo(Kean.Math.Single.Pi / 2).Within(this.Precision));
            Verify(Target.Single.Point.BasisX.Angle(-Target.Single.Point.BasisY), Is.EqualTo(-Kean.Math.Single.Pi / 2).Within(this.Precision));
            Verify(Target.Single.Point.BasisX.Angle(-Target.Single.Point.BasisX), Is.EqualTo(Kean.Math.Single.Pi).Within(this.Precision));
        }
        [Test]
        public void Casts()
        { }
		[Test]
        public void StringCasts()
        {
            string textFromValue = new Target.Integer.Point(10, 20);
            Verify(textFromValue, Is.EqualTo("10, 20"));
            Target.Integer.Point @integerFromText = (Target.Integer.Point)"10, 20";
            Verify(@integerFromText.X, Is.EqualTo(10));
            Verify(@integerFromText.Y, Is.EqualTo(20));
        }
    }
}

