// 
//  Canvas.cs
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

namespace Kean.Draw
{
	public abstract class Canvas :
		Surface
	{
		Surface surface;
		public Image Image { get; private set; }

		public override Geometry2D.Integer.Size Size { get { return this.Image.Size; } }
		public override CoordinateSystem CoordinateSystem { get { return this.Image.CoordinateSystem; } }

		protected Canvas(Surface surface, Image image) :
			base(image.Size, image.CoordinateSystem)
		{
			this.surface = surface;
			this.Image = image;
		}
		#region Create
		public abstract Canvas CreateSubcanvas(Geometry2D.Single.Box bounds);
		#endregion
		#region Render
		public void Render(Action<Surface> render)
		{
			this.Use();
			render.Call(this.surface);
			this.Unuse();
		}
		public void Render(params Action<Surface>[] render)
		{
			this.Use();
			foreach (Action<Surface> r in render)
				r.Call(this.surface);
			this.Unuse();
		}
		#endregion
		#region Surface Implementation
		#region Use, Unuse
		protected internal override void Use()
		{
			this.surface.Use();
		}
		protected internal override void Unuse()
		{
			this.surface.Unuse();
		}
		#endregion
		#region Clip, Transform
		protected override Geometry2D.Single.Box OnClipChange(Geometry2D.Single.Box clip)
		{
			return this.surface.Clip = clip;
		}
		protected override Geometry2D.Single.Transform OnTransformChange(Geometry2D.Single.Transform transform)
		{
			return this.surface.Transform = transform;
		}
		#endregion
		#region Convert
		public override Image Convert(Image image)
		{
			return this.surface.Convert(image);
		}
		#endregion
		#region Draw Image
		public override void Draw(Image image)
		{
			this.Render(s => s.Draw(image));
		}
		public override void Draw(Map map, Image image)
		{
			this.Render(s => s.Draw(map, image));
		}
		public override void Draw(Map map, Image image, Geometry2D.Single.Point position)
		{
			this.Render(s => s.Draw(map, image, position));
		}
		public override void Draw(Map map, Image image, Geometry2D.Single.Box source, Geometry2D.Single.Box destination)
		{
			this.Render(s => s.Draw(map, image, source, destination));
		}
		#endregion
		#region Draw Box
		public override void Draw(IColor color)
		{
			this.Render(s => s.Draw(color));
		}
		public override void Draw(IColor color, Geometry2D.Single.Box region)
		{
			this.Render(s => s.Draw(color, region));
		}
		#endregion
		#region Draw Path
		public override void Draw(IPaint fill, Stroke stroke, Path path)
		{
			this.Render(s => s.Draw(fill, stroke, path));
		}
		#endregion
		#region Draw Text
		public override void Draw(IPaint fill, Stroke stroke, Text text, Geometry2D.Single.Point position)
		{
			this.Render(s => s.Draw(fill, stroke, text, position));
		}
		#endregion
		#region Blend
		public override void Blend(float factor)
		{
			this.Render(s => s.Blend(factor));
		}
		#endregion
		#region Clear
		public override void Clear()
		{
			this.Render(s => s.Clear());
		}
		public override void Clear(Geometry2D.Single.Box region)
		{
			this.Render(s => s.Clear(region));
		}
		#endregion
		#region Flush, Finish
		public override void Flush()
		{
			this.surface.Flush();
		}
		public override bool Finish()
		{
			return this.surface.Finish();
		}
		#endregion
		public override T Read<T>()
		{
			return this.Image.Convert<T>();
		}
		#endregion
		public override void Dispose()
		{
			if (this.surface.NotNull())
			{
				this.surface.Dispose();
				this.surface = null;
			}
			base.Dispose();
		}
	}
}
