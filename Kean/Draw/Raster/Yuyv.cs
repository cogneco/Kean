// 
//  Yuyv.cs
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
using Kean.Extension;
using Integer = Kean.Math.Integer;
using Single = Kean.Math.Single;

namespace Kean.Draw.Raster
{
	/// <summary>
	/// Yuyv is a packed version of Yuv422 mentioned as YUY2 (see fourcc.org). Order yuyvyuyvyuyv
	/// </summary>
	[System.Runtime.InteropServices.ComVisible(true)]
	public class Yuyv :
		YuvPacked
	{
		protected override int BytesPerPixel { get { return 2; } }

		public override IColor this[int x, int y]
		{
			get
			{
				unsafe
				{
					byte* offset = (byte*)this.Buffer + y * this.Stride + x / 2 * 4;
					return new Color.Yuv(
						*(offset + (Integer.Even(x) ? 0 : 2)),
						*(offset + 1),
						*(offset + 3));
				}
			}
			set
			{
				unsafe
				{
					var yuv = value.Convert<Color.Yuv>(); 
					byte* offset = (byte*)this.Buffer + y * this.Stride + x / 2 * 4;
					*(offset + (Integer.Even(x) ? 0 : 2)) = (Color.Monochrome)yuv.Y;
					*(offset + 1) = (Color.Monochrome)yuv.U;
					*(offset + 3) = (Color.Monochrome)yuv.V;
				}
			}
		}

		public Yuyv(Geometry2D.Integer.Size size) :
			this(size, CoordinateSystem.Default, new Geometry2D.Integer.Shell()) { }
		public Yuyv(Geometry2D.Integer.Size size, CoordinateSystem coordinateSystem, Geometry2D.Integer.Shell crop = new Geometry2D.Integer.Shell()) :
			this(new Buffer.Vector<byte>(Packed.CalculateLength(size, 2)), size, coordinateSystem, crop) { }
		public Yuyv(byte[] data, Geometry2D.Integer.Size size) :
			this(new Buffer.Vector<byte>(data), size) { }
		public Yuyv(Buffer.Sized buffer, Geometry2D.Integer.Size size) :
			base(buffer, size, CoordinateSystem.Default, new Geometry2D.Integer.Shell()) { }
		public Yuyv(Buffer.Sized buffer, Geometry2D.Integer.Size size, CoordinateSystem coordinateSystem, Geometry2D.Integer.Shell crop = new Geometry2D.Integer.Shell()) :
			base(buffer, size, coordinateSystem, crop) { }
		protected Yuyv(Yuyv original) :
			base(original) { }
		internal Yuyv(Image original) :
			this(original.Size, original.CoordinateSystem, original.Crop)
		{
			unsafe
			{
				int y = 0;
				int x = 0;
				int width = this.Size.Width;

				byte* row = (byte*)this.Pointer;
				byte* yDestination = row;
				byte* uDestination = row + 1;
				byte* vDestination = row + 3;

				original.Apply(color =>
				{
					*yDestination = color.Y;
					yDestination += 2;
					if (x % 2 == 0)
					{
						*uDestination = color.U;
						*vDestination = color.V;
						uDestination += 4;
						vDestination += 4;
					}
					x++;
					if (x >= width)
					{
						x = 0;
						y++;

						row += this.Stride;
						yDestination = row;
						uDestination = row + 1;
						vDestination = row + 3;
					}
				});
			}
		}

		public override Draw.Image Copy()
		{
			return new Yuyv(this);
		}
		public override Draw.Image Create(Geometry2D.Integer.Size size)
		{
			return new Yuyv(size) { Crop = this.Crop, Wrap = this.Wrap };
		}
		public override void Apply(Action<Color.Bgr> action)
		{
			this.Apply(Color.Convert.FromYuv(action));
		}
		public override void Apply(Action<Color.Yuv> action)
		{
			unsafe
			{
				byte* row = (byte*)this.Pointer;
				byte* ySource = row;
				byte* uSource = row + 1;
				byte* vSource = row + 3;

				int width = this.Size.Width;
				int height = this.Size.Height;

				for (int y = 0; y < height; y++)
				{
					for (int x = 0; x < width; x++)
					{
						action(new Color.Yuv(*ySource, *uSource, *vSource));
						ySource += 2;
						if (x % 2 == 1)
						{
							uSource += 4;
							vSource += 4;
						}
					}
					row += this.Stride;
					ySource = row;
					uSource = row + 1;
					vSource = row + 3;
				}
			}
		}
		public override void Apply(Action<Color.Monochrome> action)
		{
			this.Apply(Color.Convert.FromYuv(action));
		}
		#region Static Open
		public static new Yuyv OpenResource(System.Reflection.Assembly assembly, string name)
		{
			return Image.OpenResource<Yuyv>(assembly, name);
		}
		public static new Yuyv OpenResource(string name)
		{
			return Image.OpenResource<Yuyv>(System.Reflection.Assembly.GetCallingAssembly(), name);
		}
		public static new Yuyv Open(string filename)
		{
			return Image.Open<Yuyv>(filename);
		}
		public static new Yuyv Open(System.IO.Stream stream)
		{
			return Image.Open<Yuyv>(stream);
		}
		#endregion
	}
}
