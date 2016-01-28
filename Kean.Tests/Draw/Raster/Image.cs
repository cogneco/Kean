//
//  Image.cs
//
//  Author:
//      Simon Mika <smika@hx.se
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
using Geometry2D = Kean.Math.Geometry2D;

namespace Kean.Draw.Raster.Test
{
	public abstract class Image<T> :
		Fixture<T>
		where T : Image<T>, new()
	{
		string correctPath;
		protected Image(string correct, float tolerance) :
			base(tolerance)
		{
			this.correctPath = "Draw.Raster.Correct." + correct + ".";
		}
		protected override void Run()
		{
			this.Run(
				this.Rectangle,
				this.RectangleFill,
				this.RectangleFillOnly,
				this.RoundedRectangle,
				this.RoundedRectangleStroke,
				this.Triangle,
				this.Circle,
				this.CircleFill,
				this.Ellipse,
				this.EllipseStroke,
				this.Bitmap,
				this.BitmapRectangle,
				this.BitmapText,
				this.Text,
				this.TextFill,
				this.TextStroke,
				this.CurveTo,
				this.EllipticalArcTo,
				this.MoveToLineTo
			);
		}
		protected abstract Draw.Image CreateImage(Geometry2D.Integer.Size size);
		[Test]
		public void Rectangle()
		{
			using (Draw.Image image = this.CreateImage(new Geometry2D.Integer.Size(128, 256)))
			{
				image.Canvas.Draw(Colors.Blue, new Stroke(Colors.Red, 2), Draw.Path.Rectangle(new Geometry2D.Single.Box(10, 10, 100, 100)));
				Verify(image, this.correctPath + "Rectangle.png");
			}
		}
		[Test]
		public void RectangleFill()
		{
			using (Draw.Image image = this.CreateImage(new Geometry2D.Integer.Size(128, 256)))
			{
				image.Canvas.Draw(Colors.Blue, Draw.Path.Rectangle(new Geometry2D.Single.Box(10, 10, 100, 100)));
				Verify(image, this.correctPath + "RectangleFill.png");
			}
		}
		[Test]
		public void RectangleFillOnly()
		{
			using (Draw.Image image = this.CreateImage(new Geometry2D.Integer.Size(128, 256)))
			{
				image.Canvas.Draw(Colors.Blue);
				Verify(image, this.correctPath + "RectangleFillOnly.png");
			}
		}
		[Test]
		public void RoundedRectangle()
		{
			using (Draw.Image image = this.CreateImage(new Geometry2D.Integer.Size(128, 256)))
			{
				image.Canvas.Draw(Colors.Blue, new Stroke(Colors.Red, 2), Draw.Path.Rectangle(new Kean.Math.Geometry2D.Single.Box(10, 10, 100, 100), new Math.Geometry2D.Single.Size(10, 20)));
				Verify(image, this.correctPath + "RoundedRectangle.png");
			}
		}
		[Test]
		public void RoundedRectangleStroke()
		{
			using (Draw.Image image = this.CreateImage(new Geometry2D.Integer.Size(128, 256)))
			{
				image.Canvas.Draw(new Stroke(Colors.Red, 2), Draw.Path.Rectangle(new Kean.Math.Geometry2D.Single.Box(10, 10, 100, 100), new Math.Geometry2D.Single.Size(10, 20)));
				Verify(image, this.correctPath + "RoundedRectangleStroke.png");
			}
		}
		[Test]
		public void Circle()
		{
			using (Draw.Image image = this.CreateImage(new Geometry2D.Integer.Size(128, 256)))
			{
				image.Canvas.Draw(Colors.Blue, new Stroke(Colors.Red, 4), Draw.Path.Circle(new Geometry2D.Single.Point(64, 128), 48));
				Verify(image, this.correctPath + "Circle.png");
			}
		}
		[Test]
		public void CircleFill()
		{
			using (Draw.Image image = this.CreateImage(new Geometry2D.Integer.Size(128, 256)))
			{
				image.Canvas.Draw(Colors.Cyan, Draw.Path.Circle(new Geometry2D.Single.Point(64, 128), 48));
				Verify(image, this.correctPath + "CircleFill.png");
			}
		}
		[Test]
		public void Ellipse()
		{
			using (Draw.Image image = this.CreateImage(new Geometry2D.Integer.Size(128, 256)))
			{
				image.Canvas.Draw(Colors.Blue, new Stroke(Colors.Red, 8), Draw.Path.Ellipse(new Geometry2D.Single.Point(64, 128), new Geometry2D.Single.Size(48, 72)));
				Verify(image, this.correctPath + "Ellipse.png");
			}
		}
		[Test]
		public void EllipseStroke()
		{
			using (Draw.Image image = this.CreateImage(new Geometry2D.Integer.Size(128, 256)))
			{
				image.Canvas.Draw(Colors.Blue, Draw.Path.Ellipse(new Geometry2D.Single.Point(64, 128), new Geometry2D.Single.Size(48, 72)));
				Verify(image, this.correctPath + "EllipseStroke.png");
			}
		}
		[Test]
		public void Bitmap()
		{
			using (Draw.Image image = this.CreateImage(new Geometry2D.Integer.Size(256, 512)))
			using (Image ellipse = Image.OpenResource(this.correctPath + "Ellipse.png"))
			{
				image.Canvas.Draw(ellipse, new Geometry2D.Single.Point(128, 256));
				Verify(image, this.correctPath + "Bitmap.png");
			}
		}
		[Test]
		public void BitmapRectangle()
		{
			using (Draw.Image image = this.CreateImage(new Geometry2D.Integer.Size(256, 512)))
			using (Image rectangle = Image.OpenResource(this.correctPath + "Rectangle.png"))
			{
				image.Canvas.Draw(rectangle, new Geometry2D.Single.Point(128, 256));
				Verify(image, this.correctPath + "BitmapRectangle.png");
			}
		}
		[Test]
		public void BitmapText()
		{
			if (Kean.Environment.IsWindows)
			{
				using (Draw.Image image = this.CreateImage(new Geometry2D.Integer.Size(256, 512)))
				using (Image Text = Image.OpenResource(this.correctPath + "Text.png"))
				{
					image.Canvas.Draw(Text, new Geometry2D.Single.Point(128, 256));
					Verify(image, this.correctPath + "BitmapText.png");
				}
			}
		}
		[Test]
		public void Text()
		{
			if (Kean.Environment.IsWindows)
			{
				using (Draw.Image image = this.CreateImage(new Geometry2D.Integer.Size(128, 256)))
				{
					image.Canvas.Draw(Colors.Blue, new Stroke(Colors.Red, 1), new Text()
					{
						Markup = "Kean Toolkit",
						Font = new Font("Verdana", 12, FontWeight.Normal, FontSlant.Normal)
					}, new Geometry2D.Single.Point(10, 20));
					Verify(image, this.correctPath + "Text.png");
				}
			}
		}
		[Test]
		public void TextFill()
		{
			if (Kean.Environment.IsWindows)
			{

				using (Draw.Image image = this.CreateImage(new Geometry2D.Integer.Size(128, 256)))
				{
					image.Canvas.Draw(Colors.Blue, new Text()
					{
						Markup = "Kean Toolkit",
						Font = new Font("Verdana", 12, FontWeight.Normal, FontSlant.Normal)
					}, new Geometry2D.Single.Point(10, 20));
					Verify(image, this.correctPath + "TextFill.png");
				}
			}
		}
		[Test]
		public void TextStroke()
		{
			if (Kean.Environment.IsWindows)
			{

				using (Draw.Image image = this.CreateImage(new Geometry2D.Integer.Size(128, 256)))
				{
					image.Canvas.Draw(new Stroke(Colors.Red, 1), new Text()
					{
						Markup = "Kean Toolkit",
						Font = new Font("Verdana", 12, FontWeight.Normal, FontSlant.Normal)
					}, new Geometry2D.Single.Point(10, 20));
					Verify(image, this.correctPath + "TextStroke.png");
				}
			}
		}
		[Test]
		public void Triangle()
		{
			using (Draw.Image image = this.CreateImage(new Geometry2D.Integer.Size(128, 256)))
			{
				image.Canvas.Draw(
					Colors.Blue, 
					new Stroke(Colors.Red, 1), 
					new Path().MoveTo(10, 10).LineTo(10, 110).LineTo(110, 110).Close());
				Verify(image, this.correctPath + "Triangle.png");
			}
		}
		[Test]
		public void CurveTo()
		{
			using (Draw.Image image = this.CreateImage(new Geometry2D.Integer.Size(128, 256)))
			{
				image.Canvas.Draw(Colors.Blue, new Stroke(Colors.Red, 1), new Path().CurveTo(100, 200, 50, 100, 200, 100));
				Verify(image, this.correctPath + "CurveTo.png");
			}
		}
		[Test]
		public void EllipticalArcTo()
		{
			using (Draw.Image image = this.CreateImage(new Geometry2D.Integer.Size(128, 256)))
			{
				image.Canvas.Draw(Colors.Blue, new Stroke(Colors.Red, 1), new Path().EllipticalArcTo(125, 75, 100, false, true, 100, 50));
				Verify(image, this.correctPath + "EllipticalArcTo.png");
			}
		}
		[Test]
		public void MoveToLineTo()
		{
			using (Draw.Image image = this.CreateImage(new Geometry2D.Integer.Size(128, 256)))
			{
				image.Canvas.Draw(Colors.Blue, new Stroke(Colors.Red, 1), new Path().MoveTo(10, 10).LineTo(110, 110).Close());
				Verify(image, this.correctPath + "MoveToLineTo.png");
			}
		}
	}
}
