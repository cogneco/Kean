// 
//  Bgr.cs
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
using Geometry2D = Kean.Math.Geometry2D;
using Kean.Reflect.Extension;
using Reflect = Kean.Reflect;
using Kean.Extension;

namespace Kean.Draw.OpenGL
{
	public class Bgr :
		Packed
	{
		#region Constructors
		public Bgr(Raster.Image image) :
			base(OpenGL.Backend.Context.Current.CreateTexture(image), image.CoordinateSystem, image.Crop)
		{ }
		public Bgr(OpenGL.Yuv420 image) :
			this(image.Size, image.CoordinateSystem, image.Crop)
		{
			this.Canvas.Draw(Map.Yuv420ToBgr, image);
		}
		public Bgr(OpenGL.Monochrome image) :
			this(image.Size, image.CoordinateSystem, image.Crop)
		{
			this.Canvas.Draw(Map.MonochromeToBgr, image);
		}
		public Bgr(OpenGL.Bgra image) :
			this(image.Size, image.CoordinateSystem, image.Crop)
		{
			this.Canvas.Draw(image);
		}
		public Bgr(Geometry2D.Integer.Size size) :
			this(size, CoordinateSystem.Default, new Geometry2D.Integer.Shell())
		{ }
		public Bgr(Geometry2D.Integer.Size size, CoordinateSystem coordinateSystem, Geometry2D.Integer.Shell crop) :
			base(OpenGL.Backend.Context.Current.CreateTexture(OpenGL.Backend.TextureType.Rgb, size), coordinateSystem, crop)
		{ }
		protected Bgr(OpenGL.Backend.Texture image, CoordinateSystem coordinateSystem, Geometry2D.Integer.Shell crop) :
			base(image, coordinateSystem, crop)
		{ }
		#endregion
		#region Image Overrides
		public override Draw.Image Create(Geometry2D.Integer.Size size)
		{
			return new Bgr(size);
		}
		public override T Convert<T>()
		{
			T result = null;
			Reflect.Type type = typeof(T);
			if (type.Inherits<Raster.Image>())
				result = this.Backend.Read().Convert<T>() as T;
			else
			{
				if (type == typeof(Bgr))
					result = this.Copy() as T;
				else if (type == typeof(Bgra))
					result = new Bgra(this) as T;
				else if (type == typeof(Monochrome))
					result = new Monochrome(this) as T;
				else if (type == typeof(Yuv420))
					result = new Yuv420(this) as T;
			}
			return result;
		}
		public override Kean.Draw.Image ResizeTo(Geometry2D.Integer.Size size)
		{
			Bgr result = new Bgr(size);
			result.Canvas.Draw(this, new Geometry2D.Single.Box(0, 0, this.Size.Width, this.Size.Height), new Geometry2D.Single.Box(0, 0, size.Width, size.Height));
			return result.Canvas.Image;
		}
		public override Draw.Image Copy()
		{
			return new Bgr(this.Convert<Raster.Bgr>());
		}
		#endregion
	}
}
