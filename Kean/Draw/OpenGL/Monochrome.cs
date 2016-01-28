// 
//  Monochrome.cs
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

namespace Kean.Draw.OpenGL
{
	public class Monochrome :
		Packed
	{
		#region Constructors
		public Monochrome(Raster.Monochrome image) :
			base(OpenGL.Backend.Context.Current.CreateTexture(image), image.CoordinateSystem, image.Crop)
		{ }
		public Monochrome(Yuv420 image) :
			this(image.Y.Backend, image.CoordinateSystem, image.Crop)
		{ }
		public Monochrome(Bgr image) :
			this(image.Size, image.CoordinateSystem)
		{
			this.Canvas.Draw(Map.BgrToMonochrome, image);
		}
		public Monochrome(Bgra image) :
			this(image.Size, image.CoordinateSystem)
		{
			this.Canvas.Draw(Map.BgrToMonochrome, image);
		}
		public Monochrome(Geometry2D.Integer.Size size) :
			this(size, CoordinateSystem.Default)
		{ }
		public Monochrome(Geometry2D.Integer.Size size, CoordinateSystem coordinateSystem) :
			base(OpenGL.Backend.Context.Current.CreateTexture(OpenGL.Backend.TextureType.Monochrome, size), coordinateSystem, new Geometry2D.Integer.Shell())
		{ }
		protected internal Monochrome(OpenGL.Backend.Texture image, CoordinateSystem coordinateSystem, Geometry2D.Integer.Shell crop) :
			base(image, coordinateSystem, crop)
		{ }
		#endregion
		#region Image Overrides
		public override Draw.Image Create(Geometry2D.Integer.Size size)
		{
			return new Monochrome(size);
		}
		public override T Convert<T>()
		{
			T result = null;
			if (this is T)
				result = this.Copy() as T;
			else if (typeof(T).IsSubclassOf(typeof(Raster.Image)) || typeof(T) == typeof(Raster.Image))
				result = this.Backend.Read().Convert<T>() as T; 
			else
			{
				if (typeof(T) == typeof(Monochrome))
					result = this.Copy() as T;
				else if (typeof(T) == typeof(Bgr))
					result = new Bgr(this) as T;
				else if (typeof(T) == typeof(Bgra))
					result = new Bgra(this) as T;
				else if (typeof(T) == typeof(Yuv420))
					result = new Yuv420(this) as T;
			}
			return result;
		}
		internal override void Render(Map map, Geometry2D.Single.Point leftTop, Geometry2D.Single.Point rightTop, Geometry2D.Single.Point leftBottom, Geometry2D.Single.Point rightBottom, Geometry2D.Single.Box destination)
		{
			base.Render(map ?? Map.MonochromeToBgr, leftTop, rightTop, leftBottom, rightBottom, destination);
		}
		// TODO:  Resize Monochrome using GPU
		public override Draw.Image ResizeTo(Kean.Math.Geometry2D.Integer.Size size)
		{
			Monochrome result = new Monochrome(size);
			result.Canvas.Draw(this, new Geometry2D.Single.Box(0, 0, this.Size.Width, this.Size.Height), new Geometry2D.Single.Box(0, 0, size.Width, size.Height));
			return result.Canvas.Image;
		}
		public override Draw.Image Copy()
		{
			return new Monochrome(this.Convert<Raster.Monochrome>());
		}		
		#endregion
	}
}
