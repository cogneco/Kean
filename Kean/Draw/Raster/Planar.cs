// 
//  Planar.cs
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
	public abstract class Planar :
		Image
	{
		protected Planar(Planar original) :
			base(original) { }
		protected Planar(Buffer.Sized buffer, Geometry2D.Integer.Size size, CoordinateSystem coordinateSystem, Geometry2D.Integer.Shell crop) :
			base(buffer, size, coordinateSystem, crop) { }

	}
}
