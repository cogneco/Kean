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
	public class Canvas :
		Draw.Canvas
	{
		Backend.Renderer renderer;
		IDisposable backend;
		Canvas(Image image, Backend.Renderer renderer, IDisposable backend) :
			base(new Surface(renderer), image)
		{
			this.renderer = renderer;
			this.backend = backend;
		}
		Canvas(Image image, Backend.Composition backend) :
			this(image, backend.Renderer, backend)
		{ }
		internal Canvas(Packed image) :
			this(image, image.Backend.Composition)
		{ }
		internal Canvas(Planar image, params Packed[] channels) :
			this(image, channels[0].Backend.Composition)
		{ }
		public Draw.Image Read()
		{
			return this.Read(new Geometry2D.Integer.Box(new Geometry2D.Integer.Point(), this.Size));
		}
		public Raster.Image Read(Geometry2D.Integer.Box region)
		{
			Raster.Packed result;
			switch (this.renderer.Type)
			{
				case OpenGL.Backend.TextureType.Rgb:
					result = new Raster.Bgr(this.Size, this.Image.CoordinateSystem, this.Image.Crop);
					break;
				case OpenGL.Backend.TextureType.Rgba:
					result = new Raster.Bgra(this.Size, this.Image.CoordinateSystem, this.Image.Crop);
					break;
				case OpenGL.Backend.TextureType.Monochrome:
					result = new Raster.Monochrome(this.Size, this.Image.CoordinateSystem, this.Image.Crop);
					break;
				default:
					result = null;
					break;
			}
			if (result.NotNull())
				this.renderer.Read(result.Pointer, region);
			return result;
		}
		#region Create
		public override Draw.Canvas CreateSubcanvas(Geometry2D.Single.Box bounds)
		{
			return null;
		}
		#endregion
		public override void Dispose()
		{
			if (this.backend.NotNull())
			{
				this.backend.Dispose();
				this.backend = null;
			}
			base.Dispose();
		}
	}
}
