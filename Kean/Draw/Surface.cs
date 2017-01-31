// 
//  Surface.cs
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

namespace Kean.Draw
{
	public abstract class Surface :
		IDisposable
	{
		#region Size
		public virtual Geometry2D.Integer.Size Size { get; private set; }
		#endregion
		#region CoordinateSystem
		public virtual CoordinateSystem CoordinateSystem { get; private set; }
		#endregion

		#region Text Antialias
		bool textAntialias;
		[Notify("TextAntialiasChanged")]
		public bool TextAntialias
		{
			get { return this.textAntialias; }
			set
			{
				if (this.textAntialias != value)
				{
					this.textAntialias = value;
					this.TextAntialiasChanged(value);
				}
			}
		}
		public event Action<bool> TextAntialiasChanged;
		#endregion
		#region Constructors, Destructor
		protected Surface(Geometry2D.Integer.Size size, CoordinateSystem coordinateSystem)
		{
			this.Size = size;
			this.CoordinateSystem = coordinateSystem;
		}
		protected Surface(Surface original) :
			this(original.Size, original.CoordinateSystem)
		{
		}
		~Surface()
		{
			this.Dispose();
		}
		#endregion
		#region Use, Unuse
		protected internal virtual void Use() { }
		protected internal virtual void Unuse() { }
		#endregion
		#region Clip, Transform, Push & Pop
		Collection.IStack<Tuple<Geometry2D.Single.Box, Geometry2D.Single.Transform>> stack = new Collection.Stack<Tuple<Geometry2D.Single.Box,Geometry2D.Single.Transform>>();
		Geometry2D.Single.Box clip;
		public Geometry2D.Single.Box Clip 
		{
			get { return this.clip; }
			set { this.clip = this.OnClipChange(value); }
		}
		Geometry2D.Single.Transform transform = Geometry2D.Single.Transform.Identity;
		public Geometry2D.Single.Transform Transform
		{
			get { return this.transform; }
			set { this.transform = this.OnTransformChange(value); }
		}
		protected virtual Geometry2D.Single.Box OnClipChange(Geometry2D.Single.Box clip)
		{
			return clip;
		}
		protected virtual Geometry2D.Single.Transform OnTransformChange(Geometry2D.Single.Transform transform)
		{
			return transform;
		}
		public void Push()
		{
			this.stack.Push(Tuple.Create(this.Clip, this.Transform));
		}
		public void Push(Geometry2D.Single.Box clip, Geometry2D.Single.Transform transform)
		{
			this.Push();
			this.Clip = clip.Intersection(this.Clip);
			this.Transform *= transform;
		}
		public void Push(Geometry2D.Single.Box clip)
		{
			this.Push(clip, Geometry2D.Single.Transform.Identity);
		}
		public void Push(Geometry2D.Single.Transform transform)
		{
			this.Push(this.Clip, transform);
		}
		public void PushAndTranslate(Geometry2D.Single.Box clip)
		{
			this.Push(clip, Geometry2D.Single.Transform.Identity);
		}
		public void Pop()
		{
			var previous = this.stack.Pop();
			this.Clip = previous.Item1;
			this.Transform = previous.Item2;
		}
		#endregion
		#region Convert
		/// <summary>
		/// Converts, for caching purposes, <paramref name="image"/> to the format most suitable for drawing operations on this surface.
		/// </summary>
		/// <param name="image">Image to convert.</param>
		/// <returns>A copy of <paramref name="image"/> converted to the most suitable format for this surface or null if it is already in the most suitable format. The caller is responsible for disposing the returned image.</returns>
		public abstract Draw.Image Convert(Draw.Image image);
		#endregion
		#region Draw, Blend, Clear
		#region Draw Image
		public virtual void Draw(Draw.Image image)
		{
			this.Draw(null, image);
		}
		public virtual void Draw(Map map, Image image)
		{
			this.Draw(map, image, new Geometry2D.Single.Point());
		}
		public virtual void Draw(Draw.Image image, Geometry2D.Single.Point position)
		{
			this.Draw(null, image, position);
		}
		public virtual void Draw(Map map, Draw.Image image, Geometry2D.Single.Point position)
		{
			this.Draw(map, image, new Geometry2D.Single.Box(new Geometry2D.Single.Point(), image.Size), new Geometry2D.Single.Box(position, image.Size));
		}
		public virtual void Draw(Draw.Image image, Geometry2D.Single.Box source, Geometry2D.Single.Box destination)
		{
			this.Draw(null, image, source, destination);
		}
		public abstract void Draw(Draw.Map map, Draw.Image image, Geometry2D.Single.Box source, Geometry2D.Single.Box destination);
		#endregion
		#region Draw Box
		public virtual void Draw(IColor color)
		{
			this.Draw(color, new Geometry2D.Single.Box((Geometry2D.Single.Size)this.Size));
		}
		public virtual void Draw(IColor color, Geometry2D.Single.Box region)
		{
			this.Draw(color, Path.Rectangle(region));
		}
		#endregion
		#region Draw Path
		public void Draw(Stroke stroke, Path path)
		{
			this.Draw(null, stroke, path);
		}
		public void Draw(IPaint fill, Path path)
		{
			this.Draw(fill, null, path);
		}
		public abstract void Draw(IPaint fill, Stroke stroke, Path path);
		#endregion
		#region Draw Text
		public void Draw(Stroke stroke, Text text, Geometry2D.Single.Point position)
		{
			this.Draw(null, stroke, text, position);
		}
		public void Draw(IPaint fill, Text text, Geometry2D.Single.Point position)
		{
			this.Draw(fill, null, text, position);
		}
		public abstract void Draw(IPaint fill, Stroke stroke, Text text, Geometry2D.Single.Point position);
		#endregion
		#region Blend
		public abstract void Blend(float factor);
		#endregion
		#region Clear
		public virtual void Clear()
		{
			this.Clear(new Geometry2D.Single.Box((Geometry2D.Single.Size)this.Size));
		}
		public abstract void Clear(Geometry2D.Single.Box region);
		#endregion
		#endregion
		#region Read
		/// <summary>
		/// Returns a copy of the contents of the surface.
		/// </summary>
		/// <typeparam name="T">Type of the returned data.</typeparam>
		/// <returns>A copy of the contents of the surface converted to T. Null if it was impossible to fulfill the request.</returns>
		public abstract T Read<T>() where T : Draw.Image;
		#endregion
		public virtual void Flush()
		{
		}
		public virtual bool Finish()
		{
			return true;
		}
		public virtual void Dispose()
		{
		}
	}
}
