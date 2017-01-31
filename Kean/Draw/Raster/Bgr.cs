// 
//  Bgr.cs
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

using System;
using Kean.Extension;
using Buffer = Kean.Buffer;
using Geometry2D = Kean.Math.Geometry2D;
using Collection = Kean.Collection;
using Integer = Kean.Math.Integer;
using Single = Kean.Math.Single;

namespace Kean.Draw.Raster
{
	[System.Runtime.InteropServices.ComVisible(true)]
	public class Bgr :
		Packed
	{
		public override IColor this[int x, int y]
		{
			get { unsafe { return this.IsValidIn(x, y) ? *((Color.Bgr*)((byte*)this.Buffer + y * this.Stride) + x) : new Color.Bgr(0, 0, 0); } }
			set { unsafe { *((Color.Bgr*)((byte*)this.Buffer + y * this.Stride) + x) = value.Convert<Color.Bgr>(); } }
		}

		protected override int BytesPerPixel { get { return 3; } }
		public Bgr(Geometry2D.Integer.Size size) :
			this(size, CoordinateSystem.Default, new Geometry2D.Integer.Shell()) { }
		public Bgr(Geometry2D.Integer.Size size, CoordinateSystem coordinateSystem, Geometry2D.Integer.Shell crop) :
			base(new Buffer.Vector<byte>(Packed.CalculateLength(size, 3)), size, coordinateSystem, crop) { }
		public Bgr(byte[] data, Geometry2D.Integer.Size size) :
			this(new Buffer.Vector<byte>(data), size) { }
		public Bgr(IntPtr pointer, Geometry2D.Integer.Size size) :
			this(new Buffer.Sized(pointer, Packed.CalculateLength(size, 3)), size) { }
		public Bgr(Buffer.Sized buffer, Geometry2D.Integer.Size size, CoordinateSystem coordinateSystem = CoordinateSystem.Default, Geometry2D.Integer.Shell crop = new Geometry2D.Integer.Shell()) :
			base(buffer, size, coordinateSystem, crop) { }
		protected Bgr(Bgr original) :
			base(original) { }
		internal Bgr(Image original) :
			this(original.Size, original.CoordinateSystem, original.Crop)
		{
			unsafe
			{
				byte* row = (byte*)this.Pointer;
				int rowLength = this.Size.Width;
				Color.Bgr* rowEnd = (Color.Bgr*)row + rowLength;
				Color.Bgr* destination = (Color.Bgr*)row;
				original.Apply((color) =>
				{
					*(destination++) = color;
					if (destination >= rowEnd)
					{
						row += this.Stride;
						destination = (Color.Bgr*)row;
						rowEnd = (Color.Bgr*)row + rowLength;
					}
				});
			}
		}
		public override Draw.Image Create(Geometry2D.Integer.Size size)
		{
			return new Bgr(size) { Crop = this.Crop, Wrap = this.Wrap };
		}
		public override Draw.Image Copy()
		{
			return new Bgr(this);
		}
		public override void Apply(Action<Color.Bgr> action)
		{
			unsafe
			{
				byte* end = (byte*)this.Pointer + this.Length;
				int rowLength = this.Size.Width;
				for (byte* row = (byte*)this.Pointer; row < end; row += this.Stride)
				{
					Color.Bgr* rowEnd = (Color.Bgr*)row + rowLength;
					for (Color.Bgr* source = (Color.Bgr*)row; source < rowEnd; source++)
						action(*source);
				}
			}
		}
		public override void Apply(Action<Color.Yuv> action)
		{
			this.Apply(Color.Convert.FromBgr(action));
		}
		public override void Apply(Action<Color.Monochrome> action)
		{
			this.Apply(Color.Convert.FromBgr(action));
		}
		public override float Distance(Draw.Image other)
		{
			float result = 0;
			if (other.IsNull())
				result = float.MaxValue;
			else if (!(other is Bgr))
				using (Bgr o = other.Convert<Bgr>())
					result = this.Distance(o);
			else if (this.Size != other.Size)
				using (Bgr o = other.ResizeTo(this.Size) as Bgr)
					result = this.Distance(o);
			else
			{
				for (int y = 0; y < this.Size.Height; y++)
					for (int x = 0; x < this.Size.Width; x++)
					{
						Color.Bgr c = (Color.Bgr)this[x, y];
						Color.Bgr o = (Color.Bgr)other[x, y];
						if (c.Distance(o) > 0)
						{
							Color.Bgr maximum = o;
							Color.Bgr minimum = o;
							for (int otherY = Integer.Maximum(0, y - this.DistanceRadius); otherY < Integer.Minimum(y + 1 + this.DistanceRadius, this.Size.Height); otherY++)
								for (int otherX = Integer.Maximum(0, x - this.DistanceRadius); otherX < Integer.Minimum(x + 1 + this.DistanceRadius, this.Size.Width); otherX++)
									if (otherX != x || otherY != y)
									{
										Color.Bgr pixel = (Color.Bgr)other[otherX, otherY];
										if (maximum.Blue < pixel.Blue)
											maximum.Blue = pixel.Blue;
										else if (minimum.Blue > pixel.Blue)
											minimum.Blue = pixel.Blue;
										if (maximum.Green < pixel.Green)
											maximum.Green = pixel.Green;
										else if (minimum.Green > pixel.Green)
											minimum.Green = pixel.Green;
										if (maximum.Red < pixel.Red)
											maximum.Red = pixel.Red;
										else if (minimum.Red > pixel.Red)
											minimum.Red = pixel.Red;
									}
							float distance = 0;
							if (c.Blue < minimum.Blue)
								distance += Single.Squared(minimum.Blue - c.Blue);
							else if (c.Blue > maximum.Blue)
								distance += Single.Squared(c.Blue - maximum.Blue);
							if (c.Green < minimum.Green)
								distance += Single.Squared(minimum.Green - c.Green);
							else if (c.Green > maximum.Green)
								distance += Single.Squared(c.Green - maximum.Green);
							if (c.Red < minimum.Red)
								distance += Single.Squared(minimum.Red - c.Red);
							else if (c.Red > maximum.Red)
								distance += Single.Squared(c.Red - maximum.Red);
							result += Single.SquareRoot(distance) / 3;
						}
					}
				result /= this.Size.Length;
			}
			return result;
		}
		#region Static Open
		public static new Bgr OpenResource(System.Reflection.Assembly assembly, string name)
		{
			return Image.OpenResource<Bgr>(assembly, name);
		}
		public static new Bgr OpenResource(string name)
		{
			return Image.OpenResource<Bgr>(System.Reflection.Assembly.GetCallingAssembly(), name);
		}
		public static new Bgr Open(string filename)
		{
			return Image.Open<Bgr>(filename);
		}
		public static new Bgr Open(System.IO.Stream stream)
		{
			return Image.Open<Bgr>(stream);
		}
		#endregion
	}
}
