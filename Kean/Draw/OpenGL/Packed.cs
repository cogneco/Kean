// 
//  Packed.cs
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

using System;
using Kean;
using Kean.Extension;
using Geometry2D = Kean.Math.Geometry2D;

namespace Kean.Draw.OpenGL
{
	public abstract class Packed :
		Image
	{
		protected internal Backend.Texture Backend { get; private set; }

		#region Constructors
		protected Packed(Planar original) :
			base(original) { }
		protected Packed(Backend.Texture backend, CoordinateSystem coordinateSystem, Geometry2D.Integer.Shell crop) :
			base(backend.Size, coordinateSystem, crop)
		{
			this.Backend = backend;
			this.Backend.Wrap = this.Wrap;
			this.WrapChanged += wrap => this.Backend.Wrap = wrap;
		}
		#endregion
		internal override void Render(Map map, Geometry2D.Single.Point leftTop, Geometry2D.Single.Point rightTop, Geometry2D.Single.Point leftBottom, Geometry2D.Single.Point rightBottom, Geometry2D.Single.Box destination)
		{
			if (map.NotNull())
				map.Render(() => this.Backend.Render(leftTop, rightTop, leftBottom, rightBottom, destination), this);
			else
			{
				this.Backend.Use();
				this.Backend.Render(leftTop, rightTop, leftBottom, rightBottom, destination);
				this.Backend.UnUse();
			}
		}
		#region Draw.Image Overrides
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
		public override T Convert<T>()
		{
			return null;
		}
		public override Draw.Image Copy(Geometry2D.Integer.Size size, Geometry2D.Single.Transform transform)
		{
			return null;
		}
		public override Draw.Image Shift(Geometry2D.Integer.Size offset)
		{
			Packed result = this.Create((Geometry2D.Integer.Size)this.Size) as Packed;
			result.Canvas.Clear();
			this.Canvas.Render(surface =>
			{
				result.Backend.Use();
				result.Backend.CopyFromTarget(offset);
				result.Backend.UnUse();
			});
			return result;
		}
		#region Flush, Finish
		public override void Flush()
		{
			this.Backend.Flush();
			base.Flush();
		}
		public override bool Finish()
		{
			return this.Backend.Finish() && base.Finish();
		}
		#endregion
		public override float Distance(Draw.Image other)
		{
			float result;
			if (other.NotNull() && this.Size == other.Size)
				using (Raster.Bgra t = this.Convert<Raster.Bgra>())
				using (Raster.Bgra o = other.Convert<Raster.Bgra>())
					result = t.Distance(o);
			else
				result = float.MaxValue;
			return result;
		}
		public override void Dispose()
		{
			if (this.Backend.NotNull())
			{
				if (this.canvas.NotNull())
					this.canvas.Dispose();
				else
					this.Backend.Dispose();
				this.canvas = null;
				this.Backend = null;
			}
		}
		#endregion
	}
}
