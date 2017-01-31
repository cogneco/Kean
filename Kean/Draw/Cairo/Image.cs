// 
//  Image.cs
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
using Kean;
using Kean.Extension;
using Geometry2D = Kean.Math.Geometry2D;

namespace Kean.Draw.Cairo
{
	public abstract class Image :
		Draw.Image
	{
		internal global::Cairo.Surface Backend { get; private set; }
		public override IColor this[int x, int y]
		{
			get { throw new NotImplementedException(); }
			set { }
		}

		Canvas canvas;
		public override Draw.Canvas Canvas
		{
			get 
			{ 
				if (this.canvas.IsNull())
					this.canvas = new Canvas(this);
				return this.canvas;
			}
		}
		protected Image(global::Cairo.Surface backend, Geometry2D.Integer.Size size) :
			base(size, CoordinateSystem.Default, new Geometry2D.Integer.Shell())
		{
			this.Backend = backend;
		}
		#region Draw.Image overrides
		public override T Convert<T>()
		{
			return null;
		}
		public override Draw.Image ResizeTo(Geometry2D.Integer.Size size)
		{
			return null;
		}
		public override Draw.Image Copy()
		{
			return null;
		}
		public override Draw.Image Copy(Geometry2D.Integer.Size size, Geometry2D.Single.Transform transform)
		{
			return null;
		}
		public override Draw.Image Shift(Geometry2D.Integer.Size offset)
		{ 
			return null;
		}
		#endregion
		public void Save(string filename)
		{
			this.Backend.WriteToPng(filename);
		}
		public override void Dispose()
		{
			base.Dispose();
			if (this.canvas.NotNull())
			{
				this.canvas.Dispose();
				this.canvas = null;
			}
		}
	}
}
