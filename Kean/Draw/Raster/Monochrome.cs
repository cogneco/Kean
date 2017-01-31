// 
//  Monochrome.cs
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
	[System.Runtime.InteropServices.ComVisible(true)]
	public class Monochrome :
		Packed
	{
		public override IColor this[int x, int y]
		{
			get { unsafe { return *((Color.Monochrome*)((byte*)this.Buffer + y * this.Stride) + x); } }
			set { unsafe { *((Color.Monochrome*)((byte*)this.Buffer + y * this.Stride) + x) = value.Convert<Color.Monochrome>(); } }
		}

		protected override int BytesPerPixel { get { return 1; } }

		public Monochrome(Geometry2D.Integer.Size size) :
			this(size, CoordinateSystem.Default, new Geometry2D.Integer.Shell()) { }
		public Monochrome(Geometry2D.Integer.Size size, CoordinateSystem coordinateSystem, Geometry2D.Integer.Shell crop) :
			base(new Buffer.Vector<byte>(Packed.CalculateLength(size, 1)), size, coordinateSystem, crop) { }
		public Monochrome(byte[] data, Geometry2D.Integer.Size size) :
			this(new Buffer.Vector<byte>(data), size) { }
		public Monochrome(IntPtr pointer, Geometry2D.Integer.Size size) :
			this(new Buffer.Sized(pointer, Packed.CalculateLength(size, 1)), size) { }
		public Monochrome(Buffer.Sized buffer, Geometry2D.Integer.Size size) :
			base(buffer, size, CoordinateSystem.Default, new Geometry2D.Integer.Shell()) { }
		public Monochrome(Buffer.Sized buffer, Geometry2D.Integer.Size size, CoordinateSystem coordinateSystem, Geometry2D.Integer.Shell crop) :
			base(buffer, size, coordinateSystem, crop) { }
		protected Monochrome(Monochrome original) :
			base(original) { }
		internal Monochrome(Image original) :
			this(original.Size, original.CoordinateSystem, original.Crop)
		{
			unsafe
			{
				byte* row = (byte*)this.Pointer;
				int rowLength = this.Size.Width;
				byte* rowEnd = row + rowLength;
				byte* destination = row;
				original.Apply((color) =>
				{
					*(destination++) = color;
					if (destination >= rowEnd)
					{
						row += this.Stride;
						destination = row;
						rowEnd = row + rowLength;
					}
				});
			}
		}
		public override Draw.Image Create(Geometry2D.Integer.Size size)
		{
			return new Monochrome(size) { Crop = this.Crop, Wrap = this.Wrap };
		}
		public override Draw.Image Copy()
		{
			return new Monochrome(this);
		}
		public override void Apply(Action<Color.Bgr> action)
		{
			this.Apply(Color.Convert.FromY(action));
		}
		public override void Apply(Action<Color.Yuv> action)
		{
			this.Apply(Color.Convert.FromY(action));
		}
		public override void Apply(Action<Color.Monochrome> action)
		{
			unsafe
			{
				byte* end = (byte*)this.Pointer + this.Length;
				int rowLength = this.Size.Width;
				for (byte* row = (byte*)this.Pointer; row < end; row += this.Stride)
				{
					byte* rowEnd = row + rowLength;
					for (byte* source = row; source < rowEnd; source++)
						action(*source);
				}
			}
		}
		public override float Distance(Draw.Image other)
		{
			float result = 0;
			if (other.IsNull())
				result = float.MaxValue;
			else if (!(other is Monochrome))
				using (Monochrome o = other.Convert<Monochrome>())
					result = this.Distance(o);
			else if (this.Size != other.Size)
				using (Monochrome o = other.ResizeTo(this.Size) as Monochrome)
					result = this.Distance(o);
			else
			{
				for (int y = 0; y < this.Size.Height; y++)
					for (int x = 0; x < this.Size.Width; x++)
					{
						Color.Monochrome c = (Color.Monochrome)this[x, y];
						Color.Monochrome o = (Color.Monochrome)other[x, y];
						if (c.Distance(o) > 0)
						{
							Color.Monochrome maximum = o;
							Color.Monochrome minimum = o;
							for (int otherY = Integer.Maximum(0, y - 2); otherY < Integer.Minimum(y + 3, this.Size.Height); otherY++)
								for (int otherX = Integer.Maximum(0, x - 2); otherX < Integer.Minimum(x + 3, this.Size.Width); otherX++)
									if (otherX != x || otherY != y)
									{
										Color.Monochrome pixel = (Color.Monochrome)other[otherX, otherY];
										if (maximum.Y < pixel.Y)
											maximum.Y = pixel.Y;
										else if (minimum.Y > pixel.Y)
											minimum.Y = pixel.Y;
									}
							float distance = 0;
							if (c.Y < minimum.Y)
								distance += Single.Squared(minimum.Y - c.Y);
							else if (c.Y > maximum.Y)
								distance += Single.Squared(c.Y - maximum.Y);
							result += Single.SquareRoot(distance);
						}
					}
				result /= this.Size.Length;
			}
			return result;
		}

		#region Static Open
		public static new Monochrome OpenResource(System.Reflection.Assembly assembly, string name)
		{
			return Image.OpenResource<Monochrome>(assembly, name);
		}
		public static new Monochrome OpenResource(string name)
		{
			return Image.OpenResource<Monochrome>(System.Reflection.Assembly.GetCallingAssembly(), name);
		}
		public static new Monochrome Open(string filename)
		{
			return Image.Open<Monochrome>(filename);
		}
		public static new Monochrome Open(System.IO.Stream stream)
		{
			return Image.Open<Monochrome>(stream);
		}
		#endregion
	}
}
