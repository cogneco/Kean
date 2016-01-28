// 
//  Uyvy.cs
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
	/// Uyvy is a packed version of Yuv422 mentioned as YUY2 (see fourcc.org). Order uyvyuyvyuyvy
	/// </summary>
	[System.Runtime.InteropServices.ComVisible(true)]
	public class Uyvy :
		YuvPacked
	{
		protected override int BytesPerPixel { get { return 2; } }
		public override Color.Yuv this[int x, int y]
		{
			get
			{
				unsafe
				{
					byte* offset = (byte*)this.Buffer + y * this.Stride + x / 2 * 4;
					return new Color.Yuv(
						*(offset + (Integer.Even(x) ? 1 : 3)),
						*offset,
						*(offset + 2));
				}
			}
			set 
			{ 
				unsafe 
				{
					byte* offset = (byte*)this.Buffer + y * this.Stride + x / 2 * 4;
					*(offset + (Integer.Even(x) ? 1 : 3)) = value.Y;
					*offset = value.U;
					*(offset + 2) = value.V;
				}
			}
		}

		public Uyvy(Geometry2D.Integer.Size size) :
			this(size, CoordinateSystem.Default, new Geometry2D.Integer.Shell()) { }
		public Uyvy(Geometry2D.Integer.Size size, CoordinateSystem coordinateSystem, Geometry2D.Integer.Shell crop) :
			this(new Buffer.Vector<byte>(Packed.CalculateLength(size, 2)), size, coordinateSystem, crop) { }
		public Uyvy(byte[] data, Geometry2D.Integer.Size size) :
			this(new Buffer.Vector<byte>(data), size) { }
		public Uyvy(Buffer.Sized buffer, Geometry2D.Integer.Size size) :
			base(buffer, size, CoordinateSystem.Default, new Geometry2D.Integer.Shell()) { }
		public Uyvy(Buffer.Sized buffer, Geometry2D.Integer.Size size, CoordinateSystem coordinateSystem, Geometry2D.Integer.Shell crop) :
			base(buffer, size, coordinateSystem, crop) { }
		protected Uyvy(Uyvy original) :
			base(original) { }
		internal Uyvy(Image original) :
			this(original.Size, original.CoordinateSystem, original.Crop)
		{
			unsafe
			{
				int y = 0;
				int x = 0;
				int width = this.Size.Width;

				byte* row = (byte*)this.Pointer;
				byte* uDestination = row;
				byte* yDestination = row + 1;
				byte* vDestination = row + 2;

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
						uDestination = row;
						yDestination = row + 1;
						vDestination = row + 2;
					}
				});
			}
		}

		public override Draw.Image Copy()
		{
			return new Uyvy(this);
		}
		public override Draw.Image Create(Geometry2D.Integer.Size size)
		{
			return new Uyvy(size) { Crop = this.Crop, Wrap = this.Wrap };
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
				byte* uSource = row;
				byte* ySource = row + 1;
				byte* vSource = row + 2;

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
					uSource = row;
					ySource = row + 1;
					vSource = row + 2;
				}
			}
		}
		public override void Apply(Action<Color.Monochrome> action)
		{
			this.Apply(Color.Convert.FromYuv(action));
		}
		#region Static Open
		public static new Uyvy OpenResource(System.Reflection.Assembly assembly, string name)
		{
			return Image.OpenResource<Uyvy>(assembly, name);
		}
		public static new Uyvy OpenResource(string name)
		{
			return Image.OpenResource<Uyvy>(System.Reflection.Assembly.GetCallingAssembly(), name);
		}
		public static new Uyvy Open(string filename)
		{
			return Image.Open<Uyvy>(filename);
		}
		public static new Uyvy Open(System.IO.Stream stream)
		{
			return Image.Open<Uyvy>(stream);
		}
		#endregion
	}
}
