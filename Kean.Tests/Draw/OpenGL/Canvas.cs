// 
//  Bgra.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2011-2012 Simon Mika
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
	public class Canvas :
		Fixture<Canvas>
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
				this.CreateFromRaster,
				this.DrawColor,
				this.DrawColorRegion,
				this.Clear,
				this.ClearArea,
				this.DrawImageOnPosition,
				this.DrawImageOnRegion,
				this.DrawImageUsingMapping,
				this.DrawImageUsingMappingTwice,
				this.Blend,
				this.DrawColorRegionWithClipping,
				this.DrawImageOnRegionWithClipping,
				this.BlendWithClipping,
				this.DrawColorRegionWithTransformAndClipping
				);
		}
		[Test]
		public void Create()
		{
			using (OpenGL.Bgra image = new OpenGL.Bgra(new Geometry2D.Integer.Size(256, 128)))
			{
				Verify(image.Canvas, Is.Not.Null);
				Verify(image.Canvas.Size, Is.EqualTo(new Geometry2D.Integer.Size(256, 128)));
			}
		}
		[Test]
		public void CreateFromRaster()
		{
			using (Raster.Bgra raster = Raster.Bgra.OpenResource("Draw.OpenGL.Input.Flower.jpg"))
				Verify(this.image.Canvas.Image, raster);
		}
		[Test]
		public void Copy()
		{
			using (Raster.Bgra raster = Raster.Bgra.OpenResource("Draw.OpenGL.Input.Flower.jpg"))
			using (Draw.Image copy = this.image.Copy())
				Verify(copy, raster);
		}
		[Test]
		public void DrawColor()
		{
			using (Draw.Image image = this.image.Copy())
			{
				image.Canvas.Draw(new Kean.Draw.Color.Bgra(255, 0, 0, 255));
				Verify(image, "Draw.OpenGL.Correct.Bgra.DrawColor.png");
			}
		}
		[Test]
		public void DrawColorRegion()
		{
			using (Draw.Image image = this.image.Copy())
			{
				image.Canvas.Draw(new Kean.Draw.Color.Bgra(255, 0, 0, 255), new Geometry2D.Single.Box(100, 100, 200, 200));
				Verify(image, "Draw.OpenGL.Correct.Bgra.DrawColorRegion.png");
			}
		}
		[Test]
		public void Clear()
		{
			using (Draw.Image image = this.image.Copy())
			{
				image.Canvas.Clear();
				Verify(image, "Draw.OpenGL.Correct.Bgra.Clear.png");
			}
		}
		[Test]
		public void ClearArea()
		{
			using (Draw.Image image = this.image.Copy())
			{
				image.Canvas.Clear(new Geometry2D.Single.Box(200, 300, 200, 100));
				Verify(image, "Draw.OpenGL.Correct.Bgra.ClearArea.png");
			}
		}
		[Test]
		public void DrawImageOnPosition()
		{
			using (Draw.Image image = this.image.Copy())
			using (Draw.Image part = Raster.Bgra.OpenResource("Draw.OpenGL.Input.Flower.jpg"))
			using (Draw.Image resized = part.ResizeWithin(new Geometry2D.Integer.Size(300, 300)))
			{
				image.Canvas.Draw(resized, new Geometry2D.Single.Point(300, 200));
				Verify(image, "Draw.OpenGL.Correct.Bgra.DrawImageOnPosition.png");
			}
		}
		[Test]
		public void DrawImageOnRegion()
		{
			using (Draw.Image image = this.image.Copy())
			using (Draw.Image part = Raster.Bgra.OpenResource("Draw.OpenGL.Input.Flower.jpg"))
			using (Draw.Image resized = part.ResizeWithin(new Geometry2D.Integer.Size(300, 300)))
			{
				image.Canvas.Draw(resized, new Geometry2D.Single.Box(0, 0, 150, 150), new Geometry2D.Single.Box(200, 200, 100, 100));
				Verify(image, "Draw.OpenGL.Correct.Bgra.DrawImageOnRegion.png");
			}
		}
		[Test]
		public void DrawImageUsingMapping()
		{
			using (Draw.Image image = this.image.Copy())
			using (Draw.Image part = Raster.Bgra.OpenResource("Draw.OpenGL.Input.Flower.jpg"))
			using (Draw.Image resized = part.ResizeWithin(new Geometry2D.Integer.Size(300, 300)))
			using (Draw.Map map = OpenGL.Map.Create(@"uniform sampler2D texture; void main() { vec4 value = texture2D(texture, gl_TexCoord[0].xy); gl_FragColor = vec4(1.0 - value.z, 1.0 - value.y, 1.0 - value.x, 1.0); }"))
			{
				image.Canvas.Draw(map, resized, new Geometry2D.Single.Point(300, 200));
				Verify(image, "Draw.OpenGL.Correct.Bgra.DrawImageUsingMapping.png");
			}
		}
		[Test]
		public void DrawImageUsingMappingTwice()
		{
			using (Draw.Image part = Raster.Bgra.OpenResource("Draw.OpenGL.Input.Flower.jpg"))
			using (Draw.Image resized = part.ResizeWithin(new Geometry2D.Integer.Size(300, 300)))
			using (Draw.Map map = OpenGL.Map.Create(@"uniform sampler2D texture; void main() { vec4 value = texture2D(texture, gl_TexCoord[0].xy); gl_FragColor = vec4(1.0 - value.z, 1.0 - value.y, 1.0 - value.x, 1.0); }"))
			{
				using (Draw.Image image = this.image.Copy())
				{
					image.Canvas.Draw(map, resized, new Geometry2D.Single.Point(300, 200));
					Verify(image, "Draw.OpenGL.Correct.Bgra.DrawImageUsingMapping.png");
				}
				using (Draw.Image image = this.image.Copy())
				{
					image.Canvas.Draw(resized, new Geometry2D.Single.Point(300, 200));
					Verify(image, "Draw.OpenGL.Correct.Bgra.DrawImageOnPosition.png");
				}
				using (Draw.Image image = this.image.Copy())
				{
					image.Canvas.Draw(map, resized, new Geometry2D.Single.Point(300, 200));
					Verify(image, "Draw.OpenGL.Correct.Bgra.DrawImageUsingMapping.png");
				}
			}
		}
		[Test]
		public void Blend()
		{
			using (Draw.Image image = this.image.Copy())
			{
				image.Canvas.Blend(0.5f);
				Verify(image, "Draw.OpenGL.Correct.Bgra.Blend.png");
			}
		}
		[Test]
		public void DrawColorRegionWithClipping()
		{
			using (Draw.Image image = this.image.Copy())
			{
				image.Canvas.Draw(new Kean.Draw.Color.Bgra(0, 0, 255, 255), new Geometry2D.Single.Box(50, 100, 400, 100));
				image.Canvas.Push(new Geometry2D.Single.Box(100, 50, 320, 200));
				image.Canvas.Draw(new Kean.Draw.Color.Bgra(255, 0, 0, 255), new Geometry2D.Single.Box(50, 100, 400, 100));
				image.Canvas.Pop();
				Verify(image, "Draw.OpenGL.Correct.Bgra.DrawColorRegionWithClipping.png");
			}
		}
		[Test]
		public void DrawImageOnRegionWithClipping()
		{
			using (Draw.Image image = this.image.Copy())
			using (Draw.Image part = Raster.Bgra.OpenResource("Draw.OpenGL.Input.Flower.jpg"))
			using (Draw.Image resized = part.ResizeTo(new Geometry2D.Integer.Size(100, 100)).Convert<Raster.Bgra>())
			{
				image.Canvas.Push(new Geometry2D.Single.Box(100, 50, 320, 200));
				image.Canvas.Draw(part, new Geometry2D.Single.Box(0, 0, 50, 100), new Geometry2D.Single.Box(200, 200, 100, 100));
				image.Canvas.Pop();
				Verify(image, "Draw.OpenGL.Correct.Bgra.DrawImageOnRegionWithClipping.png");
			}
		}
		[Test]
		public void BlendWithClipping()
		{
			using (Draw.Image image = this.image.Copy())
			{
				image.Canvas.Blend(0.25f);
				image.Canvas.Push(new Geometry2D.Single.Box(100, 50, 320, 200));
				image.Canvas.Blend(0.5f);
				image.Canvas.Pop();
				Verify(image, "Draw.OpenGL.Correct.Bgra.BlendWithClipping.png");
			}
		}
		//TODO Need a way to set both clip and transform independently.
		[Test]
		public void DrawColorRegionWithTransformAndClipping()
		{
			using (Draw.Image image = this.image.Copy())
			{
				image.Canvas.Draw(new Color.Bgra(0, 0, 255, 255), new Geometry2D.Single.Box(50, image.Size.Height / 2 - 50, image.Size.Width - 100, 100));
				image.Canvas.Push(new Geometry2D.Single.Box(100, 100, image.Size.Width / 2 - 100, image.Size.Height), Geometry2D.Single.Transform.CreateTranslation(image.Size / 2) * Geometry2D.Single.Transform.CreateRotation(Math.Single.ToRadians(11.25f)) * Geometry2D.Single.Transform.CreateTranslation(-image.Size / 2));
				image.Canvas.Draw(new Color.Bgra(255, 0, 0, 128), new Geometry2D.Single.Box(image.Size.Width / 2 - 50, image.Size.Height / 2 - 50, 100, 100));
				image.Canvas.Draw(new Color.Bgra(0, 255, 0, 128), new Geometry2D.Single.Box(100, 100, 100, 100));
				image.Canvas.Pop();
				Verify(image, "Draw.OpenGL.Correct.Bgra.DrawColorRegionWithTransformAndClipping.png");
			}
		}		
	}
}
