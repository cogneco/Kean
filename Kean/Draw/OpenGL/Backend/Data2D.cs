//
//  Data2D.cs
//
//  Author:
//       Simon Mika <smika@hx.se>
//
//  Copyright (c) 2013 Simon Mika
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

using Kean.Collection.Extension;
using System;
using Collection = Kean.Collection;
using Error = Kean.Error;
using GL = OpenTK.Graphics.OpenGL.GL;
using Geometry2D = Kean.Math.Geometry2D;
using Geometry3D = Kean.Math.Geometry3D;
using Raster = Kean.Draw.Raster;
using Kean.Extension;

namespace Kean.Draw.OpenGL.Backend
{
	public abstract class Data2D :
		Data
	{
		public Geometry2D.Integer.Size Size { get; private set; }
		protected Data2D(Context context) : 
			base(context)
		{ }
		public Data2D Update<T>(T[,] data)
			where T : struct
		{
			if (this.SetType<T>() && this.Size.Width == data.GetLength(0) && this.Size.Height == data.GetLength(1))
				this.FixCall(this.Load, data);
			else
			{
				this.Size = new Geometry2D.Integer.Size(data.GetLength(0), data.GetLength(1));
				this.FixCall(this.Allocate, data);
			}
			return this;
		}
	}
}
