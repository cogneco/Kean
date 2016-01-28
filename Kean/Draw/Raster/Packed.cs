// 
//  Packed.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
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
using Buffer = Kean.Buffer;
using Geometry2D = Kean.Math.Geometry2D;

namespace Kean.Draw.Raster
{
	[System.Runtime.InteropServices.ComVisible(true)]
	public abstract class Packed :
		Image
	{
		protected abstract int BytesPerPixel { get; }

		public int Stride { get; private set; }

		protected Packed(Buffer.Sized buffer, Geometry2D.Integer.Size size, CoordinateSystem coordinateSystem, Geometry2D.Integer.Shell crop) :
			base(buffer, size, coordinateSystem, crop)
		{
			this.Stride = Packed.CalculateStride(size, this.BytesPerPixel);
		}
		protected Packed(Packed original) :
			base(original)
		{
			this.Stride = original.Stride;
		}
		public override Draw.Image Shift(Geometry2D.Integer.Size offset)
		{
			Raster.Image result = null;
			if (this is Monochrome)
				result = new Monochrome(this.Size);
			else if (this is Bgr)
				result = new Bgr(this.Size);
			else if (this is Bgra)
				result = new Bgra(this.Size);
			else if (this is Yuyv)
				result = new Yuyv(this.Size);
			int offsetX = Kean.Math.Integer.Modulo(this is Yuyv && Kean.Math.Integer.Modulo(offset.Width, 2) != 0 ? offset.Width + 1 : offset.Width, this.Size.Width);
			int length = (this.Size.Width - offsetX) * this.BytesPerPixel;
			int line = this.Size.Width * this.BytesPerPixel;
			for (int y = 0; y < this.Size.Height; y++)
				{
					int destination = Kean.Math.Integer.Modulo(y + offset.Height, this.Size.Height) * this.Stride;
					result.Buffer.CopyFrom(this.Buffer, this.Stride * y, destination + offsetX * this.BytesPerPixel, length);
					result.Buffer.CopyFrom(this.Buffer, this.Stride * y + length, destination, line - length);
				}
			return result;
		}
		public override bool Equals(Draw.Image other)
		{
			return other is Packed &&
				this.BytesPerPixel == (other as Packed).BytesPerPixel &&
				base.Equals(other);
		}
		public override int GetHashCode()
		{
			return this.BytesPerPixel.GetHashCode() ^ base.GetHashCode();
		}
		public override float Distance(Draw.Image other)
		{
			return other is Packed && this.BytesPerPixel == (other as Packed).BytesPerPixel ? base.Distance(other) : float.MaxValue;
		}

		internal static int CalculateStride(Geometry2D.Integer.Size size, int bytesPerPixel)
		{
			return size.Width * bytesPerPixel + (4 - (size.Width * bytesPerPixel) % 4) % 4;
		}
		internal static int CalculateLength(Geometry2D.Integer.Size size, int bytesPerPixel)
		{
			return Packed.CalculateStride(size, bytesPerPixel) * size.Height;
		}
	}
}
