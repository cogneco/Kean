// 
//  Planar.cs
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
using Buffer = Kean.Buffer;
using Geometry2D = Kean.Math.Geometry2D;
using Kean.Extension;

namespace Kean.Draw.OpenGL
{
	public abstract class Planar :
		Image
	{
		Packed[] channels;
		protected Packed[] Channels
		{
			get { return this.channels; }
			private set
			{
				this.channels = value;
				Action<bool> updateWrap = wrap =>
				{
					if (this.channels.NotNull())
						this.channels.Apply(channel => channel.Wrap = wrap);
				};
				updateWrap(this.Wrap);
				this.WrapChanged += updateWrap;
			}
		}
		protected Planar(Geometry2D.Integer.Size size, CoordinateSystem coordinateSystem, Geometry2D.Integer.Shell crop, params Packed[] channels) :
			base(size, coordinateSystem, crop) 
		{
			this.Channels = channels;
		}
		protected Planar(Planar original) :
			base(original) 
		{
			this.Channels = original.Channels.Map(c => c.Copy() as Packed);
		}
		internal override void Render(Map map, Geometry2D.Single.Point leftTop, Geometry2D.Single.Point rightTop, Geometry2D.Single.Point leftBottom, Geometry2D.Single.Point rightBottom, Geometry2D.Single.Box destination)
		{
			if (map.NotNull())
				map.Render(() => this.Channels[0].Backend.Render(leftTop, rightTop, leftBottom, rightBottom, destination), this.Channels);
			else
			{
				this.Channels[0].Backend.Use();
				this.Channels[0].Backend.Render(leftTop, rightTop, leftBottom, rightBottom, destination);
				this.Channels[0].Backend.UnUse();
			}
		}
		#region Draw.Image Overrides
		Canvas canvas;
		public override Draw.Canvas Canvas
		{
			get
			{
				if (this.canvas.IsNull())
					this.canvas = new Canvas(this, this.Channels);
				return this.canvas;
			}
		}
		#region Flush, Finish
		public override void Flush()
		{
			this.Channels[0].Flush();
			base.Flush();
		}
		public override bool Finish()
		{
			return this.Channels[0].Finish() && base.Finish();
		}
		#endregion
		public override void Dispose()
		{
			if (this.Channels.NotNull())
			{
				this.Channels.Apply(c => c.Dispose());
				this.Channels = null;
			}
		}
		#endregion
	}
}
