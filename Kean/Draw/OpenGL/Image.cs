// 
//  Image.cs
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
using Kean;
using Kean.Extension;
using Geometry2D = Kean.Math.Geometry2D;
using Geometry3D = Kean.Math.Geometry3D;

namespace Kean.Draw.OpenGL
{
	public abstract class Image :
		Draw.Image
	{
		Geometry2D.Single.Point LeftTop { get; set; }
		Geometry2D.Single.Point RightTop { get; set; }
		Geometry2D.Single.Point LeftBottom { get; set; }
		Geometry2D.Single.Point RightBottom { get; set; }
		public override IColor this[int x, int y]
		{
			get { throw new NotImplementedException(); }
			set { }
		}
		#region Constructors
		protected Image(Image original) :
			base(original) 
		{
			this.LeftTop = original.LeftTop;
			this.RightTop = original.RightTop;
			this.LeftBottom = original.LeftBottom;
			this.RightBottom = original.RightBottom;
		}
		protected Image(Geometry2D.Integer.Size size, CoordinateSystem coordinateSystem, Geometry2D.Integer.Shell crop) :
			base(size, coordinateSystem, crop)
		{
			this.LeftTop = new Geometry2D.Single.Point(0, 0);
			this.RightTop = new Geometry2D.Single.Point(1, 0);
			this.LeftBottom = new Geometry2D.Single.Point(0, 1);
			this.RightBottom = new Geometry2D.Single.Point(1, 1);
		}
		#endregion
		internal void Render(Map map, Geometry2D.Single.Box source, Geometry2D.Single.Box destination)
		{
			if (this.Crop.NotZero)
			{
				source = (source).Intersection(this.Crop.Decrease(this.Size));
				destination = ((Geometry2D.Single.Box)destination).Intersection(this.Crop.Decrease(this.Size));
			}
			Geometry2D.Single.Point leftTop = new Geometry2D.Single.Point(source.Left / this.Size.Width, source.Top / this.Size.Height);
			Geometry2D.Single.Point rightTop = new Geometry2D.Single.Point(source.Right / this.Size.Width, source.Top / this.Size.Height);
			Geometry2D.Single.Point leftBottom = new Geometry2D.Single.Point(source.Left / this.Size.Width, source.Bottom / this.Size.Height);
			Geometry2D.Single.Point rightBottom = new Geometry2D.Single.Point(source.Right / this.Size.Width, source.Bottom / this.Size.Height);
			leftTop.X = this.LeftTop.X * (1 - leftTop.X) + this.RightTop.X * leftTop.X;
			rightTop.X = this.LeftTop.X * (1 - rightTop.X) + this.RightTop.X * rightTop.X;
			leftBottom.X = this.LeftBottom.X * (1 - leftBottom.X) + this.RightBottom.X * leftBottom.X;
			rightBottom.X = this.LeftBottom.X * (1 - rightBottom.X) + this.RightBottom.X * rightBottom.X;
			leftTop.Y = this.LeftTop.Y * (1 - leftTop.Y) + this.LeftBottom.Y * leftTop.Y;
			leftBottom.Y = this.LeftTop.Y * (1 - leftBottom.Y) + this.LeftBottom.Y * leftBottom.Y;
			rightTop.Y = this.RightTop.Y * (1 - rightTop.Y) + this.RightBottom.Y * rightTop.Y;
			rightBottom.Y = this.RightTop.Y * (1 - rightBottom.Y) + this.RightBottom.Y * rightBottom.Y;
			this.Render(map, leftTop, rightTop, leftBottom, rightBottom, destination);
		}
		internal abstract void Render(Map map, Geometry2D.Single.Point leftTop, Geometry2D.Single.Point rightTop, Geometry2D.Single.Point leftBottom, Geometry2D.Single.Point rightBottom, Geometry2D.Single.Box destination);
		#region Draw.Image Overrides
		protected override void Project(Draw.Image source, Geometry3D.Single.Transform pointTransform, Geometry3D.Single.Point cam)
		{
			using (var map = new Map(Backend.Programs.Projection))
			{
				source.Wrap = false;
				map.Add("cam", cam);
				//pointTransform = Geometry3D.Single.Transform.Identity;
				map.Add("transform", pointTransform);
				map.Add("sourceSize", source.Size);
				map.Add("destinationSize", this.Size);
				this.Canvas.Draw(map, source, new Geometry2D.Single.Box(source.Size), new Geometry2D.Single.Box(this.Size));
			}
		}
		#endregion
		#region Static Creators
		public static Image Create(Draw.Image image)
		{
			Image result = null;
			if (image is Raster.Image)
			{
				if (image is Raster.Bgra)
					result = new Bgra(image as Raster.Bgra);
				else if (image is Raster.Bgr)
					result = new Bgr(image as Raster.Bgr);
				else if (image is Raster.Monochrome)
					result = new Monochrome(image as Raster.Monochrome);
				else if (image is Raster.Yuv420)
					result = new Yuv420(image as Raster.Yuv420);
				else
					result = new Bgra(image.Convert<Raster.Bgra>());
			}
			else if (image is Image)
				result = image.Copy() as Image;
			else if (image.NotNull())
				result = new Bgra(image.Convert<Raster.Bgra>());
			return result;
		}
		#endregion
	}
}
