//
//  Convert.cs
//
//  Author:
//      Anders Frisk <andersfrisk77@gmail.com>
//      Simon Mika <smika@hx.se>
//
//  Copyright (c) 2013 Simon Mika, 2011 Anders Frisk
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
using Geometry2D = Kean.Math.Geometry2D;

namespace Kean.Draw.Raster.Test
{
	[TestFixture]
	public class Convert :
		Fixture<Convert>
	{
		public Convert() :
			base(30f)
		{
		}
		protected override void Run()
		{
			this.Run(
				this.Open,
				this.Equality,
				this.Inequality,
				this.Save,
				this.Copy,
				this.Monochrome,
				this.Bgr,
				this.Bgra,
				this.Yuv420,
				this.Yvu420,
				this.Yuv422,
				this.Yuv444,
				this.Yuyv,
				this.Uyvy
			);
		}

		#region Basic Operations
		[Test]
		public void Open()
		{
			using (Raster.Image bitmap = Raster.Image.OpenResource("Draw/Raster/Correct/Convert/original.png"))
			{
				Verify(bitmap.Size, Is.EqualTo(new Geometry2D.Integer.Size(800, 348)));
				Verify(bitmap.Length, Is.EqualTo(new Geometry2D.Integer.Size(800, 348).Area * 3));
			}
			using (Raster.Image bitmap = Raster.Image.OpenResource("Draw/Raster/Correct/Convert/monochrome.png"))
			{
				Verify(bitmap.Size, Is.EqualTo(new Geometry2D.Integer.Size(800, 348)));
				Verify(bitmap.Length, Is.EqualTo(new Geometry2D.Integer.Size(800, 348).Area * 4));
			}

		}
		[Test]
		public void Equality()
		{
			using (Raster.Image first = Raster.Image.OpenResource("Draw/Raster/Correct/Convert/original.png"))
				Verify(first, "Draw/Raster/Correct/Convert/original.png");
		}
		[Test]
		public void Inequality()
		{
			using (Raster.Image first = Raster.Image.OpenResource("Draw/Raster/Correct/Convert/original.png"))
				Verify(first, "Draw/Raster/Correct/Convert/monochrome.png", Is.GreaterThan(this.Tolerance));
		}
		[Test]
		public void Save()
		{
			if (Kean.Environment.IsWindows)
			{
				using (Raster.Image original = Raster.Image.OpenResource("Draw/Raster/Correct/Convert/original.png"))
				{
					original.Save(Uri.Locator.FromRelativePlatformPath("original.png"));
					original.Save(Uri.Locator.FromRelativePlatformPath("original.bmp"));
					using (Raster.Image opened = Raster.Image.Open("original.png"))
						Verify(opened, original);
					using (Raster.Image opened = Raster.Image.Open("original.bmp"))
						Verify(opened, original);
				}
			}
		}
		[Test]
		public void Copy()
		{
			using (Raster.Image original = Raster.Image.OpenResource("Draw/Raster/Correct/Convert/original.png"))
			using (Raster.Image copy = original.Copy() as Raster.Image)
			{
				Verify(original.Pointer, Is.Not.EqualTo(copy.Pointer));
				Verify(copy, original);
			}
		}
		#endregion

		#region Convert
		[Test]
		public void Monochrome()
		{
			using (Raster.Monochrome original = Raster.Monochrome.OpenResource("Draw/Raster/Correct/Convert/original.png"))
			{
				Verify(original.Convert<Raster.Monochrome>(), "Draw/Raster/Correct/Convert/monochrome.png");
				Verify(original.Convert<Raster.Bgr>(), "Draw/Raster/Correct/Convert/monochrome.png");
				Verify(original.Convert<Raster.Bgra>(), "Draw/Raster/Correct/Convert/monochrome.png");
				if (Kean.Environment.IsWindows)
				{
					Verify(original.Convert<Raster.Yuv420>(), "Draw/Raster/Correct/Convert/monochrome.png");
					Verify(original.Convert<Raster.Yvu420>(), "Draw/Raster/Correct/Convert/monochrome.png");
					Verify(original.Convert<Raster.Yuv422>(), "Draw/Raster/Correct/Convert/monochrome.png");
					Verify(original.Convert<Raster.Yuv444>(), "Draw/Raster/Correct/Convert/monochrome.png");
				}
				Verify(original.Convert<Raster.Yuyv>(), "Draw/Raster/Correct/Convert/monochrome.png");
				Verify(original.Convert<Raster.Uyvy>(), "Draw/Raster/Correct/Convert/monochrome.png");
			}
		}
		[Test]
		public void Bgr()
		{
			using (Raster.Bgr original = Raster.Bgr.OpenResource("Draw/Raster/Correct/Convert/original.png"))
			{
				Verify(original.Convert<Raster.Monochrome>(), "Draw/Raster/Correct/Convert/monochrome.png");
				Verify(original.Convert<Raster.Bgr>(), "Draw/Raster/Correct/Convert/original.png");
				Verify(original.Convert<Raster.Bgra>(), "Draw/Raster/Correct/Convert/original.png");
				if (Kean.Environment.IsWindows)
				{
					Verify(original.Convert<Raster.Yuv420>(), "Draw/Raster/Correct/Convert/original.png");
					Verify(original.Convert<Raster.Yvu420>(), "Draw/Raster/Correct/Convert/original.png");
					Verify(original.Convert<Raster.Yuv422>(), "Draw/Raster/Correct/Convert/original.png");
					Verify(original.Convert<Raster.Yuv444>(), "Draw/Raster/Correct/Convert/original.png");
				}
				Verify(original.Convert<Raster.Yuyv>(), "Draw/Raster/Correct/Convert/original.png");
				Verify(original.Convert<Raster.Uyvy>(), "Draw/Raster/Correct/Convert/original.png");
			}
		}
		[Test]
		public void Bgra()
		{
			using (Raster.Bgra original = Raster.Bgra.OpenResource("Draw/Raster/Correct/Convert/original.png"))
			{
				Verify(original.Convert<Raster.Monochrome>(), "Draw/Raster/Correct/Convert/monochrome.png");
				Verify(original.Convert<Raster.Bgr>(), "Draw/Raster/Correct/Convert/original.png");
				Verify(original.Convert<Raster.Bgra>(), "Draw/Raster/Correct/Convert/original.png");
				if (Kean.Environment.IsWindows)
				{
					Verify(original.Convert<Raster.Yuv420>(), "Draw/Raster/Correct/Convert/original.png");
					Verify(original.Convert<Raster.Yvu420>(), "Draw/Raster/Correct/Convert/original.png");
					Verify(original.Convert<Raster.Yuv422>(), "Draw/Raster/Correct/Convert/original.png");
					Verify(original.Convert<Raster.Yuv444>(), "Draw/Raster/Correct/Convert/original.png");
				}
				Verify(original.Convert<Raster.Yuyv>(), "Draw/Raster/Correct/Convert/original.png");
				Verify(original.Convert<Raster.Uyvy>(), "Draw/Raster/Correct/Convert/original.png");
			}
		}
		[Test]
		public void Yuv420()
		{
			if (Kean.Environment.IsWindows)
				using (Raster.Yuv420 original = Raster.Yuv420.OpenResource("Draw/Raster/Correct/Convert/original.png"))
				{
					Verify(original.Convert<Raster.Monochrome>(), "Draw/Raster/Correct/Convert/monochrome.png");
					Verify(original.Convert<Raster.Bgr>(), "Draw/Raster/Correct/Convert/original.png");
					Verify(original.Convert<Raster.Bgra>(), "Draw/Raster/Correct/Convert/original.png");
					Verify(original.Convert<Raster.Yuv420>(), "Draw/Raster/Correct/Convert/original.png");
					Verify(original.Convert<Raster.Yvu420>(), "Draw/Raster/Correct/Convert/original.png");
					Verify(original.Convert<Raster.Yuv422>(), "Draw/Raster/Correct/Convert/original.png");
					Verify(original.Convert<Raster.Yuv444>(), "Draw/Raster/Correct/Convert/original.png");
					Verify(original.Convert<Raster.Yuyv>(), "Draw/Raster/Correct/Convert/original.png");
					Verify(original.Convert<Raster.Uyvy>(), "Draw/Raster/Correct/Convert/original.png");
				}
		}
		[Test]
		public void Yvu420()
		{
			if (Kean.Environment.IsWindows)
				using (Raster.Yvu420 original = Raster.Yvu420.OpenResource("Draw/Raster/Correct/Convert/original.png"))
				{
					Verify(original.Convert<Raster.Monochrome>(), "Draw/Raster/Correct/Convert/monochrome.png");
					Verify(original.Convert<Raster.Bgr>(), "Draw/Raster/Correct/Convert/original.png");
					Verify(original.Convert<Raster.Bgra>(), "Draw/Raster/Correct/Convert/original.png");
					Verify(original.Convert<Raster.Yuv420>(), "Draw/Raster/Correct/Convert/original.png");
					Verify(original.Convert<Raster.Yvu420>(), "Draw/Raster/Correct/Convert/original.png");
					Verify(original.Convert<Raster.Yuv422>(), "Draw/Raster/Correct/Convert/original.png");
					Verify(original.Convert<Raster.Yuv444>(), "Draw/Raster/Correct/Convert/original.png");
					Verify(original.Convert<Raster.Yuyv>(), "Draw/Raster/Correct/Convert/original.png");
					Verify(original.Convert<Raster.Uyvy>(), "Draw/Raster/Correct/Convert/original.png");
				}
		}
		[Test]
		public void Yuv422()
		{
			if (Kean.Environment.IsWindows)
				using (Raster.Yuv422 original = Raster.Yuv422.OpenResource("Draw/Raster/Correct/Convert/original.png"))
				{
					Verify(original.Convert<Raster.Monochrome>(), "Draw/Raster/Correct/Convert/monochrome.png");
					Verify(original.Convert<Raster.Bgr>(), "Draw/Raster/Correct/Convert/original.png");
					Verify(original.Convert<Raster.Bgra>(), "Draw/Raster/Correct/Convert/original.png");
					Verify(original.Convert<Raster.Yuv420>(), "Draw/Raster/Correct/Convert/original.png");
					Verify(original.Convert<Raster.Yvu420>(), "Draw/Raster/Correct/Convert/original.png");
					Verify(original.Convert<Raster.Yuv422>(), "Draw/Raster/Correct/Convert/original.png");
					Verify(original.Convert<Raster.Yuv444>(), "Draw/Raster/Correct/Convert/original.png");
					Verify(original.Convert<Raster.Yuyv>(), "Draw/Raster/Correct/Convert/original.png");
					Verify(original.Convert<Raster.Uyvy>(), "Draw/Raster/Correct/Convert/original.png");
				}
		}
		[Test]
		public void Yuv444()
		{
			if (Kean.Environment.IsWindows)
				using (Raster.Yuv444 original = Raster.Yuv444.OpenResource("Draw/Raster/Correct/Convert/original.png"))
				{
					Verify(original.Convert<Raster.Monochrome>(), "Draw/Raster/Correct/Convert/monochrome.png");
					Verify(original.Convert<Raster.Bgr>(), "Draw/Raster/Correct/Convert/original.png");
					Verify(original.Convert<Raster.Bgra>(), "Draw/Raster/Correct/Convert/original.png");
					Verify(original.Convert<Raster.Yuv420>(), "Draw/Raster/Correct/Convert/original.png");
					Verify(original.Convert<Raster.Yvu420>(), "Draw/Raster/Correct/Convert/original.png");
					Verify(original.Convert<Raster.Yuv422>(), "Draw/Raster/Correct/Convert/original.png");
					Verify(original.Convert<Raster.Yuv444>(), "Draw/Raster/Correct/Convert/original.png");
					Verify(original.Convert<Raster.Yuyv>(), "Draw/Raster/Correct/Convert/original.png");
					Verify(original.Convert<Raster.Uyvy>(), "Draw/Raster/Correct/Convert/original.png");
				}
		}
		[Test]
		public void Yuyv()
		{
			using (Raster.Yuyv original = Raster.Yuyv.OpenResource("Draw/Raster/Correct/Convert/original.png"))
			{
				Verify(original.Convert<Raster.Monochrome>(), "Draw/Raster/Correct/Convert/monochrome.png");
				Verify(original.Convert<Raster.Bgr>(), "Draw/Raster/Correct/Convert/original.png");
				Verify(original.Convert<Raster.Bgra>(), "Draw/Raster/Correct/Convert/original.png");
				if (Kean.Environment.IsWindows)
				{
					Verify(original.Convert<Raster.Yuv420>(), "Draw/Raster/Correct/Convert/original.png");
					Verify(original.Convert<Raster.Yvu420>(), "Draw/Raster/Correct/Convert/original.png");
					Verify(original.Convert<Raster.Yuv422>(), "Draw/Raster/Correct/Convert/original.png");
					Verify(original.Convert<Raster.Yuv444>(), "Draw/Raster/Correct/Convert/original.png");
				}
				Verify(original.Convert<Raster.Yuyv>(), "Draw/Raster/Correct/Convert/original.png");
				Verify(original.Convert<Raster.Uyvy>(), "Draw/Raster/Correct/Convert/original.png");
			}
		}
		[Test]
		public void Uyvy()
		{
			using (Raster.Uyvy original = Raster.Uyvy.OpenResource("Draw/Raster/Correct/Convert/original.png"))
			{
				Verify(original.Convert<Raster.Monochrome>(), "Draw/Raster/Correct/Convert/monochrome.png");
				Verify(original.Convert<Raster.Bgr>(), "Draw/Raster/Correct/Convert/original.png");
				Verify(original.Convert<Raster.Bgra>(), "Draw/Raster/Correct/Convert/original.png");
				if (Kean.Environment.IsWindows)
				{
					Verify(original.Convert<Raster.Yuv420>(), "Draw/Raster/Correct/Convert/original.png");
					Verify(original.Convert<Raster.Yvu420>(), "Draw/Raster/Correct/Convert/original.png");
					Verify(original.Convert<Raster.Yuv422>(), "Draw/Raster/Correct/Convert/original.png");
					Verify(original.Convert<Raster.Yuv444>(), "Draw/Raster/Correct/Convert/original.png");
				}
				Verify(original.Convert<Raster.Yuyv>(), "Draw/Raster/Correct/Convert/original.png");
				Verify(original.Convert<Raster.Uyvy>(), "Draw/Raster/Correct/Convert/original.png");
			}
		}
		#endregion

	}
}
