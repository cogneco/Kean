// 
//  Yvu420.cs
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

using Buffer = Kean.Buffer;
using Geometry2D = Kean.Math.Geometry2D;

namespace Kean.Draw.Raster
{
	[System.Runtime.InteropServices.ComVisible(true)]
	public class Yvu420 :
		Yuv420
	{
		protected Yvu420(Yvu420 original) :
			base(original) { }
		public Yvu420(Geometry2D.Integer.Size size) :
			this(new Buffer.Vector<byte>(Packed.CalculateLength(size, 1) + 2 * Packed.CalculateLength(size / 2, 1)), size) { }
		public Yvu420(Geometry2D.Integer.Size size, CoordinateSystem coordinateSystem, Geometry2D.Integer.Shell crop = new Geometry2D.Integer.Shell()) :
			this(new Buffer.Vector<byte>(Packed.CalculateLength(size, 1) + 2 * Packed.CalculateLength(size / 2, 1)), size, coordinateSystem, crop) { }
		public Yvu420(byte[] data, Geometry2D.Integer.Size size) :
			this(new Buffer.Vector<byte>(data), size) { }
		public Yvu420(Buffer.Sized buffer, Geometry2D.Integer.Size size) :
			this(buffer, size, CoordinateSystem.Default, new Geometry2D.Integer.Shell()) { }
		public Yvu420(Buffer.Sized buffer, Geometry2D.Integer.Size size, CoordinateSystem coordinateSystem, Geometry2D.Integer.Shell crop = new Geometry2D.Integer.Shell()) :
			base(buffer, size, coordinateSystem, crop) { }
		internal Yvu420(Image original) :
			base(original) { }
		protected override Monochrome CreateU()
		{
			return base.CreateV();
		}
		protected override Monochrome CreateV()
		{
			return base.CreateU();
		}
		public override Draw.Image Create(Geometry2D.Integer.Size size)
		{
			return new Yvu420(size) { Crop = this.Crop, Wrap = this.Wrap };
		}
		public override Draw.Image Copy()
		{
			return new Yvu420(this);
		}
		public override Kean.Draw.Image Shift(Kean.Math.Geometry2D.Integer.Size offset)
		{
			Yvu420 result;
			Monochrome y = this.Y.Shift(offset) as Monochrome;
			Monochrome u = this.U.Shift(offset / 2) as Monochrome;
			Monochrome v = this.V.Shift(offset / 2) as Monochrome;
			result = new Yvu420(this.Size);
			result.Buffer.CopyFrom(y.Buffer, 0, 0, y.Length);
			result.Buffer.CopyFrom(v.Buffer, 0, y.Length, v.Length);
			result.Buffer.CopyFrom(u.Buffer, 0, y.Length + v.Length, u.Length);
			return result;
		}
		public override float Distance(Draw.Image other)
		{
			return other is Yvu420 ? base.Distance(other) : float.MaxValue;
		}
		public override bool Equals(Draw.Image other)
		{
			return other is Yvu420 && base.Equals(other);
		}
		#region Static Open
		public static new Yvu420 OpenResource(System.Reflection.Assembly assembly, string name)
		{
			return Image.OpenResource<Yvu420>(assembly, name);
		}
		public static new Yvu420 OpenResource(string name)
		{
			return Image.OpenResource<Yvu420>(System.Reflection.Assembly.GetCallingAssembly(), name);
		}
		public static new Yvu420 Open(string filename)
		{
			return Image.Open<Yvu420>(filename);
		}
		public static new Yvu420 Open(System.IO.Stream stream)
		{
			return Image.Open<Yvu420>(stream);
		}
		#endregion
	}
}
