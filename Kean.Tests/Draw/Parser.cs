using System;
using System.Collections.Generic;
using Geometry2D = Kean.Math.Geometry2D;
using NUnit.Framework;
using Target = Kean.Draw.Color;


namespace Kean.Draw.Test
{
	public class Parser:
		Kean.Test.Fixture<Parser>
	{
		protected override void Run()
		{
			this.Run(
				this.Close,
				this.CurveTo,
				this.LineTo,
				this.MoveTo,
				this.EllipticalArcTo,
				this.ComplexPath1,
				this.ComplexPath
				);
		}
		[Test]
		public void Close()
		{
			Draw.Path path = (Draw.Path)"Z";
			Verify((string)path, Is.EqualTo("Z "));
		}
		[Test]
		public void CurveTo()
		{
			Draw.Path path = (Draw.Path)"C100,200,50 100,200,100";
			Verify((string)path, Is.EqualTo("C100,200 50,100 200,100 "));
		}
		[Test]
		public void LineTo()
		{
			Draw.Path path = (Draw.Path)"L110, 110";
			Verify((string)path, Is.EqualTo("L110, 110 "));
		}
		[Test]
		public void MoveTo()
		{
			Draw.Path path = (Draw.Path)"M100.15, 100.0";
			Verify((string)path, Is.EqualTo("M100.15, 100 "));
		}
		[Test]
		public void EllipticalArcTo()
		{
			Draw.Path path = (Draw.Path)"A125, 75,100 0,1 100, 50";
			Verify((string)path, Is.EqualTo("A125, 75,100 0,1 100, 50 "));
		}
		[Test]
		public void ComplexPath1()
		{
			Draw.Path path = (Draw.Path)"M100, 100 L110, 110";
			Verify((string)path, Is.EqualTo("M100, 100 L110, 110 "));
		}
		[Test]
		public void ComplexPath()
		{
			Draw.Path path = (Draw.Path)"M100, 100A125, 75,100 0,1 100, 50L110, 110C100,200 50,100 200,100M100, 100C100,200 50,100 200,100Z";
			Verify((string)path, Is.EqualTo("M100, 100 A125, 75,100 0,1 100, 50 L110, 110 C100,200 50,100 200,100 M100, 100 C100,200 50,100 200,100 Z "));
		}

	}
}
