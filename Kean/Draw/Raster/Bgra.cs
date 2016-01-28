// 
//  Bgra.cs
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
	public class Bgra :
		Packed
	{
		public Color.Bgra this[Geometry2D.Integer.Point position]
		{
			get { return this[position.X, position.Y]; }
			set { this[position.X, position.Y] = value; }
		}
		public Color.Bgra this[int x, int y]
		{
			get { unsafe { return *((Color.Bgra*)((byte*)this.Buffer + y * this.Stride) + x); } }
			set { unsafe { *((Color.Bgra*)((byte*)this.Buffer + y * this.Stride) + x) = value; } }
		}
		public Color.Bgra this[Geometry2D.Single.Point position]
		{
			get { return this[position.X, position.Y]; }
		}
		public Color.Bgra this[float x, float y]
		{
			get
			{
				float left = x - Integer.Floor(x);
				float top = y - Integer.Floor(y);

				Color.Bgra topLeft = this[Integer.Floor(x), Integer.Floor(y)];
				Color.Bgra bottomLeft = this[Integer.Floor(x), Integer.Ceiling(y)];
				Color.Bgra topRight = this[Integer.Ceiling(x), Integer.Floor(y)];
				Color.Bgra bottomRight = this[Integer.Ceiling(x), Integer.Ceiling(y)];

				return new Color.Bgra(
					(byte)(top * (left * topLeft.Blue + (1 - left) * topRight.Blue) + (1 - top) * (left * bottomLeft.Blue + (1 - left) * bottomRight.Blue)),
					(byte)(top * (left * topLeft.Green + (1 - left) * topRight.Green) + (1 - top) * (left * bottomLeft.Green + (1 - left) * bottomRight.Green)),
					(byte)(top * (left * topLeft.Red + (1 - left) * topRight.Red) + (1 - top) * (left * bottomLeft.Red + (1 - left) * bottomRight.Red)),
					(byte)(top * (left * topLeft.Alpha + (1 - left) * topRight.Alpha) + (1 - top) * (left * bottomLeft.Alpha + (1 - left) * bottomRight.Alpha)));
			}
		}

		protected override int BytesPerPixel { get { return 4; } }
		public Bgra(Geometry2D.Integer.Size size) :
			this(new Buffer.Vector<byte>(Packed.CalculateLength(size, 4)), size) { }
		public Bgra(Geometry2D.Integer.Size size, CoordinateSystem coordinateSystem, Geometry2D.Integer.Shell crop = new Geometry2D.Integer.Shell()) :
			base(new Buffer.Vector<byte>(Packed.CalculateLength(size, 4)), size, coordinateSystem, crop) { }
		public Bgra(byte[] data, Geometry2D.Integer.Size size) :
			this(new Buffer.Vector<byte>(data), size) { }
		public Bgra(IntPtr pointer, Geometry2D.Integer.Size size) :
			this(new Buffer.Sized(pointer, size.Area * 4), size) { }
		public Bgra(Buffer.Sized buffer, Geometry2D.Integer.Size size) :
			base(buffer, size, CoordinateSystem.Default, new Geometry2D.Integer.Shell()) { }
		public Bgra(Buffer.Sized buffer, Geometry2D.Integer.Size size, CoordinateSystem coordinateSystem, Geometry2D.Integer.Shell crop = new Geometry2D.Integer.Shell()) :
			base(buffer, size, coordinateSystem, crop) { }
		protected Bgra(Bgra original) :
			base(original) { }
		internal Bgra(Image original) :
			this(original.Size, original.CoordinateSystem, original.Crop)
		{
			unsafe
			{
				int* destination = (int*)this.Pointer;
				original.Apply(color => *((Color.Bgra*)destination++) = new Color.Bgra(color, 255));
			}
		}
		protected override Draw.Cairo.Image CreateCairoImage(Buffer.Sized buffer, Geometry2D.Integer.Size size)
		{
			return new Cairo.Bgra(buffer, size);
		}
		public override Draw.Image Create(Geometry2D.Integer.Size size)
		{
			return new Bgra(size) { Crop = this.Crop, Wrap = this.Wrap };
		}
		public override Draw.Image Copy()
		{
			return new Bgra(this);
		}
		public override void Apply(Action<Color.Bgr> action)
		{
			unsafe
			{
				int* end = (int*)this.Pointer + this.Size.Area;
				for (int* source = (int*)this.Pointer; source < end; source++)
					action(*((Color.Bgr*)source));
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
			else if (!(other is Bgra))
				using (Bgra o = other.Convert<Bgra>())
					result = this.Distance(o);
			else if (this.Size != other.Size)
				using (Bgra o = other.ResizeTo(this.Size) as Bgra)
					result = this.Distance(o);
			else
			{
				for (int y = 0; y < this.Size.Height; y++)
					for (int x = 0; x < this.Size.Width; x++)
					{
						Color.Bgra c = this[x, y];
						Color.Bgra o = (other as Bgra)[x, y];
						if (c.Distance(o) > 0)
						{
							Color.Bgra maximum = o;
							Color.Bgra minimum = o;
							for (int otherY = Integer.Maximum(0, y - this.DistanceRadius); otherY < Integer.Minimum(y + 1 + this.DistanceRadius, this.Size.Height); otherY++)
								for (int otherX = Integer.Maximum(0, x - this.DistanceRadius); otherX < Integer.Minimum(x + 1 + this.DistanceRadius, this.Size.Width); otherX++)
									if (otherX != x || otherY != y)
									{
										Color.Bgra pixel = (other as Bgra)[otherX, otherY];
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
										if (maximum.Alpha < pixel.Alpha)
											maximum.Alpha = pixel.Alpha;
										else if (minimum.Alpha > pixel.Alpha)
											minimum.Alpha = pixel.Alpha;
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
							if (c.Alpha < minimum.Alpha)
								distance += Single.Squared(minimum.Alpha - c.Alpha);
							else if (c.Alpha > maximum.Alpha)
								distance += Single.Squared(c.Alpha - maximum.Alpha);
							result += Single.SquareRoot(distance) / 4;
						}
					}
				result /= this.Size.Length;
			}
			return result;
		}
		#region Static Open
		public static new Bgra OpenResource(System.Reflection.Assembly assembly, string name)
		{
			return Image.OpenResource<Bgra>(assembly, name);
		}
		public static new Bgra OpenResource(string name)
		{
			return Image.OpenResource<Bgra>(System.Reflection.Assembly.GetCallingAssembly(), name);
		}
		public static new Bgra Open(string filename)
		{
			return Image.Open<Bgra>(filename);
		}
		public static new Bgra Open(System.IO.Stream stream)
		{
			return Image.Open<Bgra>(stream);
		}
		#endregion
	}
}
