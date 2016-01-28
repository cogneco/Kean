using System;
using System.Collections.Generic;
using Geometry2D = Kean.Math.Geometry2D;
using NUnit.Framework;
using Target = Kean.Draw.Color;

namespace Kean.Draw.Test
{
	public class Path:
		Kean.Test.Fixture<Path>
	{
		protected override void Run()
		{
			this.Run(
				this.Close,
				this.CurveTo,
				this.EllipticalArcTo,
				this.LineTo,
				this.MoveTo,
				this.ComplexPath 
				);
		}
		[Test]
		public void Close()
		{
			Draw.Path path = new Draw.Path().Close();
			Verify(path.ToString(), Is.EqualTo("Z "));
		}
		[Test]
		public void CurveTo()
		{
			Draw.Path path = new Draw.Path().CurveTo(100, 200, 50, 100, 200, 100);
			Verify(path.ToString(), Is.EqualTo("C100,200 50,100 200,100 "));
		}
		[Test]
		public void EllipticalArcTo()
		{
			Draw.Path path = new Draw.Path().EllipticalArcTo(125, 75, 100, false, true, 100, 50);
			Verify(path.ToString(), Is.EqualTo("A125, 75,100 0,1 100, 50 "));
		}
		[Test]
		public void LineTo()
		{
			Draw.Path path = new Draw.Path().LineTo(110, 110);
			Verify(path.ToString(), Is.EqualTo("L110, 110 "));
		}
		[Test]
		public void MoveTo()
		{
			Draw.Path path = new Draw.Path().MoveTo(100, 100);
			Verify(path.ToString(), Is.EqualTo("M100, 100 "));
		}
		[Test]
		public void ComplexPath()
		{
			Draw.Path path = new Draw.Path().MoveTo(100, 100).CurveTo(100, 200, 50, 100, 200, 100).EllipticalArcTo(125, 75, 100, false, true, 100, 50);
			Verify(path.ToString(), Is.EqualTo("M100, 100 C100,200 50,100 200,100 A125, 75,100 0,1 100, 50 "));
		}
	}
}
