// 
//  Convert.cs
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
using Kean.Extension;
using NUnit.Framework;

namespace Kean.Draw.OpenGL.Test
{
	public class Convert : 
		Fixture<Convert>
	{
		Raster.Bgra rasterBgra;
		Raster.Bgr rasterBgr;
		Raster.Monochrome rasterMonochrome;
		Raster.Yuv420 rasterYuv420;
		public override void Setup()
		{
			this.rasterBgr = Raster.Image.OpenResource("Draw.OpenGL.Input.Flower.jpg") as Raster.Bgr;
			this.rasterBgra = this.rasterBgr.Convert<Raster.Bgra>();
			this.rasterMonochrome = this.rasterBgr.Convert<Raster.Monochrome>();
			this.rasterYuv420 = this.rasterBgr.Convert<Raster.Yuv420>();
			base.Setup();
		}
		public override void TearDown()
		{
			this.rasterBgra.Dispose();
			this.rasterBgr.Dispose();
			this.rasterMonochrome.Dispose();
			this.rasterYuv420.Dispose();
			base.TearDown();
		}
		protected override void Run()
		{
			this.Run(
				//this.BgrToMonochrome, TODO: Monochrome surfaces don't work 
				this.BgrToBgra,
				//this.BgrToYuv420, TODO: Monochrome surfaces don't work 
				//this.BgraToMonochrome, TODO: Monochrome surfaces don't work 
				this.BgraToBgr,
				//this.BgraToYuv420, TODO: Monochrome surfaces don't work 
				//this.MonochromeToBgr, TODO: Monochrome surfaces don't work 
				//this.MonochromeToBgra, TODO: Monochrome surfaces don't work 
				//this.MonochromeToYuv420, TODO: Monochrome surfaces don't work 
				this.Yuv420ToMonochrome,
				this.Yuv420ToBgr,
				this.Yuv420ToBgra
				);
		}
		[Test]
		public void BgrToMonochrome()
		{
			using (OpenGL.Image source = OpenGL.Image.Create(this.rasterBgr))
			using (OpenGL.Image destination = source.Convert<OpenGL.Monochrome>())
				Verify(destination, "Draw.OpenGL.Correct.Convert.BgrToMonochrome.png");
		}
		[Test]
		public void BgrToBgra()
		{
			using (OpenGL.Image source = OpenGL.Image.Create(this.rasterBgr))
			using (OpenGL.Image destination = source.Convert<OpenGL.Bgra>())
				Verify(destination, "Draw.OpenGL.Correct.Convert.BgrToBgra.png");
		}
		[Test]
		public void BgrToYuv420()
		{
			using (OpenGL.Image source = OpenGL.Image.Create(this.rasterBgr))
			using (OpenGL.Image destination = source.Convert<OpenGL.Yuv420>())
				Verify(destination, "Draw.OpenGL.Correct.Convert.BgrToYuv420.png");
		}
		[Test]
		public void BgraToMonochrome()
		{
			using (OpenGL.Image source = OpenGL.Image.Create(this.rasterBgra))
			using (OpenGL.Image destination = source.Convert<OpenGL.Monochrome>())
				Verify(destination, "Draw.OpenGL.Correct.Convert.BgraToMonochrome.png");
		}
		[Test]
		public void BgraToBgr()
		{
			using (OpenGL.Image source = OpenGL.Image.Create(this.rasterBgra))
			using (OpenGL.Image destination = source.Convert<OpenGL.Bgr>())
				Verify(destination, "Draw.OpenGL.Correct.Convert.BgraToBgr.png");
		}

		[Test]
		public void BgraToYuv420()
		{
			using (OpenGL.Image source = OpenGL.Image.Create(this.rasterBgra))
			using (OpenGL.Image destination = source.Convert<OpenGL.Yuv420>())
				Verify(destination, "Draw.OpenGL.Correct.Convert.BgraToYuv420.png");
		}
		[Test]
		public void MonochromeToBgr()
		{
			using (OpenGL.Image source = OpenGL.Image.Create(this.rasterMonochrome))
			using (OpenGL.Image destination = source.Convert<OpenGL.Bgr>())
				Verify(destination, "Draw.OpenGL.Correct.Convert.MonochromeToBgr.png");
		}
		[Test]
		public void MonochromeToBgra()
		{
			using (OpenGL.Image source = OpenGL.Image.Create(this.rasterMonochrome))
			using (OpenGL.Bgra destination = source.Convert<OpenGL.Bgra>())
				Verify(destination, "Draw.OpenGL.Correct.Convert.MonochromeToBgra.png");
		}
		[Test]
		public void MonochromeToYuv420()
		{
			using (OpenGL.Image source = OpenGL.Image.Create(this.rasterMonochrome))
			using (OpenGL.Image destination = source.Convert<OpenGL.Yuv420>())
				Verify(destination, "Draw.OpenGL.Correct.Convert.MonochromeToYuv420.png");
		}
		[Test]
		public void Yuv420ToBgr()
		{
			using (OpenGL.Image source = OpenGL.Image.Create(this.rasterYuv420))
			using (OpenGL.Bgr destination = source.Convert<OpenGL.Bgr>())
				Verify(destination, "Draw.OpenGL.Correct.Convert.Yuv420ToBgr.png");
		}
		[Test]
		public void Yuv420ToBgra()
		{
			using (OpenGL.Image source = OpenGL.Image.Create(this.rasterYuv420))
			using (OpenGL.Image destination = source.Convert<OpenGL.Bgra>())
				Verify(destination, "Draw.OpenGL.Correct.Convert.Yuv420ToBgra.png");
		}
		[Test]
		public void Yuv420ToMonochrome()
		{
			using (OpenGL.Image source = OpenGL.Image.Create(this.rasterYuv420))
			using (OpenGL.Image destination = source.Convert<OpenGL.Monochrome>())
				Verify(destination, "Draw.OpenGL.Correct.Convert.Yuv420ToMonochrome.png");
		}
	}
}
