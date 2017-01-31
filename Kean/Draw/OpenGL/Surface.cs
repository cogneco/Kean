// 
//  Canvas.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2013 Simon Mika
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
using Kean.Extension;
using Geometry2D = Kean.Math.Geometry2D;

namespace Kean.Draw.OpenGL
{
	public class Surface :
		Draw.Surface
	{
		Backend.Renderer renderer;
		Geometry2D.Single.Box clip;
		Geometry2D.Single.Transform transform = Geometry2D.Single.Transform.Identity;
		internal Surface(Backend.Renderer renderer) :
			base(renderer.Size, CoordinateSystem.Default)
		{
			this.renderer = renderer;
		}
		protected internal override void Use()
		{
			this.renderer.Use();
			this.renderer.SetClip(this.clip);
			this.renderer.SetTransform(this.transform);
		}
		protected internal override void Unuse()
		{
			this.renderer.UnSetClip();
			this.renderer.Unuse();
		}
		public Raster.Image Read()
		{
			return this.Read(new Geometry2D.Integer.Box(new Geometry2D.Integer.Point(), this.Size));
		}
		public Raster.Image Read(Geometry2D.Integer.Box region)
		{
			Raster.Packed result;
			switch (this.renderer.Type)
			{
				case OpenGL.Backend.TextureType.Rgb:
					result = new Raster.Bgr(this.Size) { CoordinateSystem = CoordinateSystem.YUpward };
					break;
				case OpenGL.Backend.TextureType.Rgba:
					result = new Raster.Bgra(this.Size) { CoordinateSystem = CoordinateSystem.YUpward };
					break;
				case OpenGL.Backend.TextureType.Monochrome:
					result = new Raster.Monochrome(this.Size) { CoordinateSystem = CoordinateSystem.YUpward };
					break;
				default:
					result = null;
					break;
			}
			if (result.NotNull())
				this.renderer.Read(result.Pointer, region);
			return result;
		}
		public override Draw.Image Convert(Draw.Image image)
		{
			return image is Image ? null : Image.Create(image);
		}
		#region Draw.Canvas Overrides
		#region Clip, Transform, Push & Pop
		protected override Geometry2D.Single.Box OnClipChange(Geometry2D.Single.Box clip)
		{
			this.renderer.SetClip(clip);
			return base.OnClipChange(this.clip = clip);
		}
		protected override Geometry2D.Single.Transform OnTransformChange(Geometry2D.Single.Transform transform)
		{
			this.renderer.SetTransform(transform);
			return base.OnTransformChange(this.transform = transform);
		}
		#endregion
		#region Draw, Blend, Clear
		#region Draw Image
		void Draw(Map map, Image image, Geometry2D.Single.Box source, Geometry2D.Single.Box destination)
		{
			image.Render(map, source, destination);
		}
		public override void Draw(Draw.Map map, Draw.Image image, Geometry2D.Single.Box source, Geometry2D.Single.Box destination)
		{
			if (!(image is Image))
				using (image = OpenGL.Image.Create(image))
					this.Draw(map as Map, image as Image, source, destination);
			else
				this.Draw(map as Map, image as Image, source, destination);
		}
		#endregion
		#region Draw Box
		public override void Draw(IColor color)
		{
			this.Draw(color, new Geometry2D.Single.Box(this.Size));
		}
		public override void Draw(IColor color, Geometry2D.Single.Box region)
		{
			this.renderer.Draw(color, region);
		}
		#endregion
		#region Draw Path
		public override void Draw(IPaint fill, Stroke stroke, Path path)
		{
			throw new NotImplementedException();
		}
		#endregion
		#region Draw Text
		public override void Draw(IPaint fill, Stroke stroke, Text text, Geometry2D.Single.Point position)
		{
			throw new NotImplementedException();
		}
		#endregion
		#region Blend
		public override void Blend(float factor)
		{
			this.renderer.Blend(factor);
		}
		#endregion
		#region Clear
		public override void Clear()
		{
			this.renderer.UnSetClip();
			this.renderer.Clear();
		}
		public override void Clear(Geometry2D.Single.Box region)
		{
			this.renderer.Clear(region);
		}
		#endregion
		#endregion
		#region Flush, Finish
		public override void Flush()
		{
			this.renderer.Flush();
			base.Flush();
		}
		public override bool Finish()
		{
			return this.renderer.Finish() && base.Finish();
		}
		#endregion
		#region Read
		public override T Read<T>()
		{
			// TODO: add more effective when we need a OpenGL image.
			return this.Read().Convert<T>();
		}
		#endregion
		#endregion
	}
}
