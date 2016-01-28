// 
//  GdiExtension.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2010-2011 Simon Mika
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

using Kean.Extension;
using Buffer = Kean.Buffer;
using Geometry2D = Kean.Math.Geometry2D;

namespace Kean.Draw.Raster.Extension
{
	public static class GdiExtension
	{
		public static System.Drawing.Bitmap GdiBitmap(this Image me)
		{
			System.Drawing.Bitmap result = null;
			if (me is Bgr)
				result = new System.Drawing.Bitmap(me.Size.Width, me.Size.Height, me.Size.Width * 3 + ((me.Size.Width * 3) % 4 > 0 ? 4 - (me.Size.Width * 3) % 4 : 0), System.Drawing.Imaging.PixelFormat.Format24bppRgb, (me as Bgr).Pointer);
			else if (me is Bgra)
				result = new System.Drawing.Bitmap(me.Size.Width, me.Size.Height, me.Size.Width * 4, System.Drawing.Imaging.PixelFormat.Format32bppArgb, (me as Bgra).Pointer);
			return result;
		}
		public static System.Drawing.Imaging.ImageFormat ImageFormat(this Compression me)
		{
			System.Drawing.Imaging.ImageFormat result;
			switch (me)
			{
				default:
				case Compression.Jpeg:
					result = System.Drawing.Imaging.ImageFormat.Jpeg;
					break;
				case Compression.Png:
					result = System.Drawing.Imaging.ImageFormat.Png;
					break;
				case Compression.Bmp:
					result = System.Drawing.Imaging.ImageFormat.Bmp;
					break;
				case Compression.Gif:
					result = System.Drawing.Imaging.ImageFormat.Gif;
					break;
			}
			return result;
		}
		public static System.Drawing.RotateFlipType FlipType(this CoordinateSystem me)
		{
			System.Drawing.RotateFlipType result;
			switch (me)
			{
				default:
				case CoordinateSystem.Default:
					result = System.Drawing.RotateFlipType.RotateNoneFlipNone;
					break;
				case CoordinateSystem.XRightward | CoordinateSystem.YUpward:
					result = System.Drawing.RotateFlipType.RotateNoneFlipY;
					break;
				case CoordinateSystem.XLeftward | CoordinateSystem.YDownward:
					result = System.Drawing.RotateFlipType.RotateNoneFlipX;
					break;
				case CoordinateSystem.XLeftward | CoordinateSystem.YUpward:
					result = System.Drawing.RotateFlipType.RotateNoneFlipXY;
					break;
			}
			return result;
		}
		public static Image AsImage(this System.Drawing.Bitmap me)
		{
			Image result = null;
			if (me.NotNull())
			{
				if (me.PixelFormat != System.Drawing.Imaging.PixelFormat.Format24bppRgb &&
					me.PixelFormat != System.Drawing.Imaging.PixelFormat.Format32bppArgb)
				{ // Bitmap data that we don't support we draw upon a ARGB bitmap and use that instead.
					System.Drawing.Bitmap newBitmap = new System.Drawing.Bitmap(me.Width, me.Height);
					using (System.Drawing.Graphics canvas = System.Drawing.Graphics.FromImage(newBitmap))
						canvas.DrawImageUnscaled(me, 0, 0);
					me.Dispose();
					me = newBitmap;
				}
				System.Drawing.Imaging.BitmapData data = me.LockBits(new System.Drawing.Rectangle(0, 0, me.Width, me.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, me.PixelFormat);
				Buffer.Sized buffer = new Buffer.Sized(data.Scan0, data.Stride * data.Height, (pointer) =>
				{
					if (me.NotNull())
						me.Dispose();
				});
				switch (me.PixelFormat)
				{
					case System.Drawing.Imaging.PixelFormat.Format24bppRgb:
						result = new Bgr(buffer, new Geometry2D.Integer.Size(me.Width, me.Height));
						break;
					case System.Drawing.Imaging.PixelFormat.Format32bppArgb:
						result = new Bgra(buffer, new Geometry2D.Integer.Size(me.Width, me.Height));
						break;
				}
			}
			return result;
		}

	}
}
