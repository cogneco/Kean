// 
//  Bgra.cs
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

namespace Kean.Draw.Cairo
{
	public class Bgra :
		Raster
	{
		public Bgra(Geometry2D.Integer.Size size) :
			this(new Buffer.Vector<Color.Bgra>(size.Area), size)
		{ }
		public Bgra(Buffer.Sized buffer, Geometry2D.Integer.Size size) :
			base(buffer, new global::Cairo.ImageSurface(buffer, global::Cairo.Format.Argb32, size.Width, size.Height, size.Width * 4), size)
		{ }
		public override Draw.Image Create(Geometry2D.Integer.Size size)
		{
			return new Bgra(size);
		}
		public override float Distance(Draw.Image other)
		{
			Bgra o = other.Convert<Bgra>();
			return this.Buffer.Distance(o.Buffer);
		}
	}
}
