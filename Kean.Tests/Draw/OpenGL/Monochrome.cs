// 
//  Monochrome.cs
//  
//  Author:
//       Anders Frisk <andersfrisk77@gmail.com>
//  
//  Copyright (c) 2011 Anders Frisk
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
	public class Monochrome :
		Fixture<Monochrome>
	{
		OpenGL.Image image;
		public Monochrome() :
			base(0.1f)
		{ }
		public override void  Setup()
		{
			base.Setup();
			using (Raster.Image flower = Raster.Image.OpenResource("Draw.OpenGL.Input.Flower.jpg"))
			using (Raster.Monochrome monochrome = flower.Convert<Raster.Monochrome>())
				this.image = OpenGL.Image.Create(monochrome);
		}
		public override void TearDown()
		{
			this.image.Dispose();
			base.TearDown();
		}
		protected override void Run()
		{
			this.Run(
				this.Create,
				this.Equality,
//				this.ConvertToRaster,
				this.CreateFromRaster,
				this.Copy,
				this.ResizeTo,
				this.ResizeWithin,
				this.DrawColor,
				this.DrawColorRegion,
				this.ClearArea,
				this.DrawImageOnRegion,
				this.DrawColorRegionWithoutBackground
				//this.DrawImageOnBgra
				);
		}
		[Test]
		public void Create()
		{
			using (OpenGL.Monochrome image = new OpenGL.Monochrome(new Geometry2D.Integer.Size(128, 256)))
			{
				Verify(image, Is.Not.Null);
				Verify(image.Size, Is.EqualTo(new Geometry2D.Integer.Size(128, 256)));
			}
		}
		[Test]
		public void Equality()
		{
			using (OpenGL.Monochrome a = new OpenGL.Monochrome(new Geometry2D.Integer.Size(128, 256)))
			using (OpenGL.Monochrome b = new OpenGL.Monochrome(new Geometry2D.Integer.Size(256, 256)))
			{
				Verify(a, Is.EqualTo(a));
				Verify(a, Is.Not.EqualTo(b));
			}
		}
		//[Test]
		//public void ConvertToRaster()
		//{
		//	using (OpenGL.Image image = new OpenGL.Monochrome(new Geometry2D.Integer.Size(128, 256)))
		//	using (Raster.Monochrome raster = image.Convert<Raster.Monochrome>())
		//		Verify(raster, "Draw.OpenGL.Correct.Monochrome.ConvertToRaster.png");
		//}
		[Test]
		public void CreateFromRaster()
		{
			Verify(this.image, "Draw.OpenGL.Correct.Monochrome.CreateFromRaster.png");
		}
		[Test]
		public void Copy()
		{
			using (Draw.Image image = this.image.Copy())
				Verify(image, "Draw.OpenGL.Correct.Monochrome.CreateFromRaster.png");
		}
		[Test]
		public void ResizeTo()
		{
			using (Draw.Image resized = this.image.ResizeTo(new Geometry2D.Integer.Size(100, 100)))
				Verify(resized, "Draw.OpenGL.Correct.Monochrome.ResizeTo.png");
		}
		[Test]
		public void ResizeWithin()
		{
			using (Draw.Image resized = this.image.ResizeWithin(new Geometry2D.Integer.Size(100, 100)))
				Verify(resized, "Draw.OpenGL.Correct.Monochrome.ResizeWithin.png");
		}
		#region Canvas
		[Test]
		public void ClearArea()
		{
			using (Draw.Image image = this.image.Copy())
			{
				image.Canvas.Clear(new Geometry2D.Single.Box(200, 300, 200, 100));
				Verify(image, "Draw.OpenGL.Correct.Monochrome.ClearArea.png");
			}
		}
		[Test]
		public void DrawColor()
		{
			using (Draw.Image image = this.image.Copy())
			{
				image.Canvas.Draw(new Color.Bgra(50, 100, 200, 255));
				Verify(image, "Draw.OpenGL.Correct.Monochrome.DrawColor.png");
			}
		}
		[Test]
		public void DrawColorRegion()
		{
			using (Draw.Image image = this.image.Copy())
			{
				image.Canvas.Draw(new Color.Bgra(0, 0, 255, 255), new Geometry2D.Single.Box(100, 100, 200, 200));
				Verify(image, "Draw.OpenGL.Correct.Monochrome.DrawColorRegion.png");
			}
		}
		[Test]
		public void DrawColorRegionWithoutBackground()
		{
			using (Draw.Image image = new OpenGL.Monochrome(this.image.Size))
			{
				image.Canvas.Draw(new Color.Bgra(64, 64, 64, 255));
				image.Canvas.Draw(new Color.Bgra(128, 128, 128, 255), new Geometry2D.Single.Box(100, 100, 200, 200));
				Verify(image, "Draw.OpenGL.Correct.Monochrome.DrawColorRegionWithoutBackground.png");
			}
		}
		[Test]
		public void DrawImageOnRegion()
		{
			using (Draw.Image image = this.image.Copy())
			using (Draw.Image flower = Raster.Image.OpenResource("Draw.OpenGL.Input.Flower.jpg"))
			using (Draw.Image part = flower.ResizeTo(new Geometry2D.Integer.Size(100, 100)))
			using (Draw.Image monochrome = part.Convert<Raster.Monochrome>())
			using (Draw.Image bgra = monochrome.Convert<Raster.Bgra>())
			{
				image.Canvas.Draw(bgra, new Geometry2D.Single.Box(0, 0, 50, 100), new Geometry2D.Single.Box(200, 200, 100, 100));
				Verify(image, "Draw.OpenGL.Correct.Monochrome.DrawImageOnRegion.png");
			}
		}
		[Test]
		public void DrawImageOnBgra()
		{
			using (Draw.Image bgr = Raster.Image.OpenResource("Draw.OpenGL.Input.ElephantSeal.jpg"))
			using (Draw.Image bgra = bgr.Convert<Raster.Bgra>())
			using (Draw.OpenGL.Image destination = OpenGL.Image.Create(bgra))
			using (Draw.Image resized = this.image.ResizeWithin(new Geometry2D.Integer.Size(100, 100)))
			{
				destination.Canvas.Draw(resized);
				destination.Canvas.Finish();
				Verify(destination, "Draw.OpenGL.Correct.Monochrome.DrawImageOnBgra.png");
			}
		}
		#endregion
	}
}
