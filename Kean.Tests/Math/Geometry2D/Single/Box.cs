// 
//  Box.cs
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

namespace Kean.Math.Geometry2D.Test.Single
{
	[TestFixture]
	public class Box : 
		Kean.Test.Fixture<Box>
	{
		string prefix = "Kean.Math.Geometry2D.Test.Single.Box.";
		protected override void Run()
		{
			this.Run(
				this.Equality,
				this.LeftTop,
				this.Size,
				this.Addition,
				this.Subtraction,
				this.ScalarMultitplication,
				this.Hash,
				this.Casts,
				this.StringCasts);
		}
	   Target.Single.Box box0 = new Target.Single.Box(1, 2, 3, 4);
	   Target.Single.Box box1 = new Target.Single.Box(4, 3, 2, 1);
	   Target.Single.Box box2 = new Target.Single.Box(2, 1, 4, 3);
	   #region Equality
	   [Test]
	   public void Equality()
	   {
#pragma warning disable 1718
		   Target.Single.Box box = new  Target.Single.Box();
		   Verify(this.box0, Is.EqualTo(this.box0));
		   Verify(this.box0.Equals(this.box0 as object), Is.True);
		   Verify(this.box0 == this.box0, Is.True);
		   Verify(this.box0 != this.box1, Is.True);
		   Verify(this.box0 == box, Is.False);
		   Verify(box == box, Is.True);
		   Verify(box == this.box0, Is.False);
#pragma warning restore 1718
	   }
	   #endregion
	   [Test]
	   public void LeftTop()
	   {
		   Target.Single.Point leftTop = this.box0.LeftTop;
		   Verify(leftTop.X, Is.EqualTo(1), this.prefix + "LeftTop.0");
		   Verify(leftTop.Y, Is.EqualTo(2), this.prefix + "LeftTop.1");
	   }
	   [Test]
	   public void Size()
	   {
		   Target.Single.Size size = this.box0.Size;
		   Verify(size.Width, Is.EqualTo(3), this.prefix + "Size.0");
		   Verify(size.Height, Is.EqualTo(4), this.prefix + "Size.1");
	   }
	   #region Arithmetic
	   [Test]
	   public void Addition()
	   {
	   }
	   [Test]
	   public void Subtraction()
	   {
	   }
	   [Test]
	   public void ScalarMultitplication()
	   {
	   }
	   #endregion
	   #region Hash Code
	   [Test]
	   public void Hash()
	   {
		   Verify(this.box0.Hash(), Is.Not.EqualTo(0));
	   }
	   #endregion
	   [Test]
	   public void Casts()
	   {
		   // integer - single
		   {
			   Target.Integer.Box integer = new Target.Integer.Box(10, 20, 30, 40);
			   Target.Single.Box @single = integer;
			   Verify(@single.Left, Is.EqualTo(10));
			   Verify(@single.Top, Is.EqualTo(20));
			   Verify(@single.Width, Is.EqualTo(30));
			   Verify(@single.Height, Is.EqualTo(40));
			   Verify((Target.Integer.Box)@single, Is.EqualTo(integer));
		   }
	   }
	   [Test]
	   public void StringCasts()
	   {
		   string textFromValue = new Target.Single.Box(10, 20, 30, 40);
		   Verify(textFromValue, Is.EqualTo("10, 20, 30, 40"));
		   Target.Single.Box @singleFromText = (Target.Single.Box)"10 20 30 40";
		   Verify(@singleFromText.Left, Is.EqualTo(10));
		   Verify(@singleFromText.Top, Is.EqualTo(20));
		   Verify(@singleFromText.Width, Is.EqualTo(30));
		   Verify(@singleFromText.Height, Is.EqualTo(40));
	   }
	}
}

