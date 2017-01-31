// 
//  Image.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2012 Simon Mika
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
using Reflect = Kean.Reflect;
using Kean.Reflect.Extension;

namespace Kean.Draw.Vector
{
	public class Image :
		Draw.Image
	{
		Canvas canvas;
		public override Draw.Canvas Canvas { get { return this.canvas; } }
		public override IColor this[int x, int y]
		{
			get { throw new NotImplementedException(); }
			set { }
		}

		public Image(Geometry2D.Integer.Size size) :
			base(size, CoordinateSystem.Default, new Geometry2D.Integer.Shell())
		{
			this.canvas = new Canvas(this);
		}
		Image(Image original) :
			base(original)
		{
			this.canvas = original.canvas.Copy(this);
		}
		public override T Convert<T>()
		{
			T result = null;
			if (((Reflect.Type)typeof(Raster.Bgra)).Inherits<T>())
				result = new Raster.Bgra(this.Size) as T;
			if (result.NotNull() && result.Canvas.NotNull())
				this.canvas.Render(result.Canvas);
			else
				result = null;
			return result;
		}

		public override Draw.Image ResizeTo(Geometry2D.Integer.Size size)
		{
			throw new System.NotImplementedException();
		}

		public override Draw.Image Create(Geometry2D.Integer.Size size)
		{
			throw new System.NotImplementedException();
		}

		public override Draw.Image Copy()
		{
			return new Image(this);
		}

		public override Draw.Image Copy(Geometry2D.Integer.Size size, Geometry2D.Single.Transform transform)
		{
			throw new System.NotImplementedException();
		}
		public override Draw.Image Shift(Geometry2D.Integer.Size offset)
		{
			throw new System.NotImplementedException();
		}
		
		public override float Distance(Draw.Image other)
		{
			using (Raster.Image raster = this.Convert<Raster.Bgra>())
				return raster.Distance(other);
		}
	}
}
