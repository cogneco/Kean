// 
//  YuvPlanar.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2011-2012 Simon Mika
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
	public abstract class YuvPlanar :
		Planar
	{
		public Color.Yuv this[Geometry2D.Integer.Point position]
		{
			get { return this[position.X, position.Y]; }
			set { this[position.X, position.Y] = value; }
		}
		public abstract Color.Yuv this[int x, int y] { get; set; }
		public Color.Yuv this[Geometry2D.Single.Point position]
		{
			get { return this[position.X, position.Y]; }
		}
		public Color.Yuv this[float x, float y]
		{
			get
			{
				float left = x - Integer.Floor(x);
				float top = y - Integer.Floor(y);

				Color.Yuv topLeft = this[Integer.Floor(x), Integer.Floor(y)];
				Color.Yuv bottomLeft = this[Integer.Floor(x), Integer.Ceiling(y)];
				Color.Yuv topRight = this[Integer.Ceiling(x), Integer.Floor(y)];
				Color.Yuv bottomRight = this[Integer.Ceiling(x), Integer.Ceiling(y)];

				return new Color.Yuv(
					(byte)(top * (left * topLeft.Y + (1 - left) * topRight.Y) + (1 - top) * (left * bottomLeft.Y + (1 - left) * bottomRight.Y)),
					(byte)(top * (left * topLeft.U + (1 - left) * topRight.U) + (1 - top) * (left * bottomLeft.U + (1 - left) * bottomRight.U)),
					(byte)(top * (left * topLeft.V + (1 - left) * topRight.V) + (1 - top) * (left * bottomLeft.V + (1 - left) * bottomRight.V)));
			}
		}

		public Monochrome Y { get; private set; }
		public Monochrome U { get; private set; }
		public Monochrome V { get; private set; }

		protected YuvPlanar(Buffer.Sized buffer, Geometry2D.Integer.Size size, CoordinateSystem coordinateSystem, Geometry2D.Integer.Shell crop) :
			base(buffer, size, coordinateSystem, crop)
		{
			this.Y = this.CreateY();
			this.U = this.CreateU();
			this.V = this.CreateV();
		}
		protected YuvPlanar(YuvPlanar original) :
			base(original)
		{
			this.Y = this.CreateY();
			this.U = this.CreateU();
			this.V = this.CreateV();
		}
		protected abstract Monochrome CreateY();
		protected abstract Monochrome CreateU();
		protected abstract Monochrome CreateV();

		public override void Apply(Action<Color.Bgr> action)
		{
			this.Apply(Color.Convert.FromYuv(action));
		}
		public override void Apply(Action<Color.Monochrome> action)
		{
			this.Apply(Color.Convert.FromYuv(action));
		}
		public override float Distance(Draw.Image other)
		{
			float result = 0;
			if (other.IsNull())
				result = float.MaxValue;
			else if (!(other is YuvPlanar))
				using (YuvPlanar o = other.Convert<YuvPlanar>())
					result = this.Distance(o);
			else if (this.Size != other.Size)
				using (YuvPlanar o = other.ResizeTo(this.Size) as YuvPlanar)
					result = this.Distance(o);
			else
			{
				for (int y = 0; y < this.Size.Height; y++)
					for (int x = 0; x < this.Size.Width; x++)
					{
						Color.Yuv c = this[x, y];
						Color.Yuv o = (other as YuvPlanar)[x, y];
						if (c.Distance(o) > 0)
						{
							Color.Yuv maximum = o;
							Color.Yuv minimum = o;
							for (int otherY = Integer.Maximum(0, y - this.DistanceRadius); otherY < Integer.Minimum(y + 1 + this.DistanceRadius, this.Size.Height); otherY++)
								for (int otherX = Integer.Maximum(0, x - this.DistanceRadius); otherX < Integer.Minimum(x + 1 + this.DistanceRadius, this.Size.Width); otherX++)
									if (otherX != x || otherY != y)
									{
										Color.Yuv pixel = (other as YuvPlanar)[otherX, otherY];
										if (maximum.Y < pixel.Y)
											maximum.Y = pixel.Y;
										else if (minimum.Y > pixel.Y)
											minimum.Y = pixel.Y;
										if (maximum.U < pixel.U)
											maximum.U = pixel.U;
										else if (minimum.U > pixel.U)
											minimum.U = pixel.U;
										if (maximum.V < pixel.V)
											maximum.V = pixel.V;
										else if (minimum.V > pixel.V)
											minimum.V = pixel.V;
									}
							float distance = 0;
							if (c.Y < minimum.Y)
								distance += Single.Squared(minimum.Y - c.Y);
							else if (c.Y > maximum.Y)
								distance += Single.Squared(c.Y - maximum.Y);
							if (c.U < minimum.U)
								distance += Single.Squared(minimum.U - c.U);
							else if (c.U > maximum.U)
								distance += Single.Squared(c.U - maximum.U);
							if (c.V < minimum.V)
								distance += Single.Squared(minimum.V - c.V);
							else if (c.V > maximum.V)
								distance += Single.Squared(c.V - maximum.V);
							result += Single.SquareRoot(distance) / 3;
						}
					}
				result /= this.Size.Length;
			}
			return result;
		}
		public override bool Equals(Draw.Image other)
		{
			return other is YuvPlanar && base.Equals(other);
		}
		#region Static Open
		public static new YuvPlanar OpenResource(System.Reflection.Assembly assembly, string name)
		{
			return Image.OpenResource<YuvPlanar>(assembly, name);
		}
		public static new YuvPlanar OpenResource(string name)
		{
			return Image.OpenResource<YuvPlanar>(System.Reflection.Assembly.GetCallingAssembly(), name);
		}
		public static new YuvPlanar Open(string filename)
		{
			return Image.Open<YuvPlanar>(filename);
		}
		public static new YuvPlanar Open(System.IO.Stream stream)
		{
			return Image.Open<YuvPlanar>(stream);
		}
		#endregion
	}
}
