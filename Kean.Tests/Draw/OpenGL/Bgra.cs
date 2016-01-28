// 
//  Bgra.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
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
using Geometry2D = Kean.Math.Geometry2D;
using NUnit.Framework;

namespace Kean.Draw.OpenGL.Test
{
	public class Bgra :
		Fixture<Bgra>
	{
		OpenGL.Image image;
		public override void Setup()
		{
			base.Setup();
			using (Raster.Bgra raster = Raster.Bgra.OpenResource("Draw.OpenGL.Input.Flower.jpg"))
				this.image = OpenGL.Image.Create(raster);
		}
		public override void TearDown()
		{
			base.TearDown();
			this.image.Dispose();
		}
		protected override void Run()
		{
			this.Run(
				this.Create,
				this.Equality,
				this.ConvertToRaster,
				this.CreateFromRaster,
				this.Copy,
				this.ResizeTo,
				this.ResizeWithin
				);
		}
		[Test]
		public void Create()
		{
			using (OpenGL.Bgra image = new OpenGL.Bgra(new Geometry2D.Integer.Size(128, 256)))
			{
				Verify(image, Is.Not.Null);
				Verify(image.Size, Is.EqualTo(new Geometry2D.Integer.Size(128, 256)));
			}
		}
		[Test]
		public void Equality()
		{
			using (OpenGL.Bgra a = new OpenGL.Bgra(new Geometry2D.Integer.Size(128, 256)))
			using (OpenGL.Bgra b = new OpenGL.Bgra(new Geometry2D.Integer.Size(256, 256)))
			{
				Verify(a, Is.EqualTo(a));
				Verify(a, Is.Not.EqualTo(b));
			}
		}
		[Test]
		public void ConvertToRaster()
		{
			using (OpenGL.Bgra image = new OpenGL.Bgra(new Geometry2D.Integer.Size(128, 256)))
				Expect(image.Convert<Raster.Bgra>(), Is.EqualTo(Raster.Bgra.OpenResource("Draw.OpenGL.Correct.Bgra.ConvertToRaster.png")));
		}
		[Test]
		public void CreateFromRaster()
		{
			Verify(this.image, "Draw.OpenGL.Correct.Bgra.CreateFromRaster.png");
		}
		[Test]
		public void Copy()
		{
			using (Draw.Image copy = this.image.Copy())
				Verify(copy, "Draw.OpenGL.Correct.Bgra.CreateFromRaster.png");
		}
		[Test]
		public void ResizeTo()
		{
			using (Draw.Image resized = this.image.ResizeTo(new Geometry2D.Integer.Size(100, 100)))
				Verify(resized, "Draw.OpenGL.Correct.Bgra.ResizeTo.png");
		}
		[Test]
		public void ResizeWithin()
		{
			using (Draw.Image resized = this.image.ResizeWithin(new Geometry2D.Integer.Size(100, 100)))
				Verify(resized, "Draw.OpenGL.Correct.Bgra.ResizeWithin.png");
		}
	}
}
