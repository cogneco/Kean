// 
//  Yuv444.cs
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

namespace Kean.Draw.Raster
{
	[System.Runtime.InteropServices.ComVisible(true)]
	public class Yuv444 :
		YuvPlanar
	{
		public override Color.Yuv this[int x, int y]
		{
			get { return new Color.Yuv(this.Y[x, y], this.U[x, y], this.V[x, y]); }
			set
			{
				this.Y[x, y] = value.Y;
				this.U[x, y] = value.U;
				this.V[x, y] = value.V;
			}
		}
		public Yuv444(Geometry2D.Integer.Size size) :
			this(size, CoordinateSystem.Default, new Geometry2D.Integer.Shell()) { }
		public Yuv444(Geometry2D.Integer.Size size, CoordinateSystem coordinateSystem, Geometry2D.Integer.Shell crop) :
			this(new Buffer.Vector<byte>(3 * Packed.CalculateLength(size, 1)), size, coordinateSystem, crop) { }
		public Yuv444(byte[] data, Geometry2D.Integer.Size size) :
			this(new Buffer.Vector<byte>(data), size) { }
		public Yuv444(Buffer.Sized buffer, Geometry2D.Integer.Size size) :
			this(buffer, size, CoordinateSystem.Default, new Geometry2D.Integer.Shell()) { }
		public Yuv444(Buffer.Sized buffer, Geometry2D.Integer.Size size, CoordinateSystem coordinateSystem, Geometry2D.Integer.Shell crop) :
			base(buffer, size, coordinateSystem, crop)
		{ }
		protected Yuv444(Yuv444 original) :
			base(original)
		{ }
		internal Yuv444(Image original) :
			this(original.Size, original.CoordinateSystem, original.Crop)
		{
			unsafe
			{
				int x = 0;
				int width = this.Size.Width;

				byte* yRow = (byte*)this.Y.Pointer;
				byte* yDestination = yRow;

				byte* uRow = (byte*)this.U.Pointer;
				byte* uDestination = uRow;

				byte* vRow = (byte*)this.V.Pointer;
				byte* vDestination = vRow;

				original.Apply(color =>
				{
					*(yDestination++) = color.Y;
					*(uDestination++) = color.U;
					*(vDestination++) = color.V;
					x++;
					if (x >= width)
					{
						x = 0;

						yRow += this.Y.Stride;
						yDestination = yRow;

						uRow += this.U.Stride;
						uDestination = uRow;

						vRow += this.V.Stride;
						vDestination = vRow;
					}
				});
			}
		}
		protected override Monochrome CreateY()
		{
			return new Monochrome(this.Pointer, this.Size);
		}
		protected override Monochrome CreateU()
		{
			return new Monochrome(new IntPtr(this.Pointer.ToInt32() + Packed.CalculateLength(this.Size, 1)), this.Size);
		}
		protected override Monochrome CreateV()
		{
			return new Monochrome(new IntPtr(this.Pointer.ToInt32() + 2 * Packed.CalculateLength(this.Size, 1)), this.Size);
		}
		public override Draw.Image Create(Geometry2D.Integer.Size size)
		{
			return new Yuv444(size) { Crop = this.Crop, Wrap = this.Wrap };
		}
		public override Draw.Image Copy()
		{
			return new Yuv444(this);
		}
		public override Draw.Image Shift(Geometry2D.Integer.Size offset)
		{
			Yuv444 result;
			Monochrome y = this.Y.Shift(offset) as Monochrome;
			Monochrome u = this.U.Shift(offset) as Monochrome;
			Monochrome v = this.V.Shift(offset) as Monochrome;
			result = new Yuv444(this.Size);
			result.Buffer.CopyFrom(y.Buffer, 0, 0, y.Length);
			result.Buffer.CopyFrom(u.Buffer, 0, y.Length, u.Length);
			result.Buffer.CopyFrom(v.Buffer, 0, y.Length + u.Length, v.Length);
			return result;
		}
		public override void Apply(Action<Color.Bgr> action)
		{
			this.Apply(Color.Convert.FromYuv(action));
		}
		public override void Apply(Action<Color.Yuv> action)
		{
			unsafe
			{
				byte* yRow = (byte*)this.Y.Pointer;
				byte* ySource = yRow;

				byte* uRow = (byte*)this.U.Pointer;
				byte* uSource = uRow;

				byte* vRow = (byte*)this.V.Pointer;
				byte* vSource = vRow;

				int width = this.Size.Width;
				int height = this.Size.Height;

				for (int y = 0; y < height; y++)
				{
					for (int x = 0; x < width; x++)
						action(new Color.Yuv(*(ySource++), *(uSource++), *(vSource++)));
					yRow += this.Y.Stride;
					uRow += this.U.Stride;
					vRow += this.V.Stride;
					ySource = yRow;
					uSource = uRow;
					vSource = vRow;
				}
			}
		}
		public override void Apply(Action<Color.Monochrome> action)
		{
			this.Apply(Color.Convert.FromYuv(action));
		}
		public override float Distance(Draw.Image other)
		{
			return other is Yuv444 ? base.Distance(other) : float.MaxValue;
		}
		public override bool Equals(Draw.Image other)
		{
			return other is Yuv444 && base.Equals(other);
		}
		#region Static Open
		public static new Yuv444 OpenResource(System.Reflection.Assembly assembly, string name)
		{
			return Image.OpenResource<Yuv444>(assembly, name);
		}
		public static new Yuv444 OpenResource(string name)
		{
			return Image.OpenResource<Yuv444>(System.Reflection.Assembly.GetCallingAssembly(), name);
		}
		public static new Yuv444 Open(string filename)
		{
			return Image.Open<Yuv444>(filename);
		}
		public static new Yuv444 Open(System.IO.Stream stream)
		{
			return Image.Open<Yuv444>(stream);
		}
		#endregion
	}
}
