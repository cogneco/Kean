//
//  Screen.cs
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
using Kean.Extension;
using Geometry2D = Kean.Math.Geometry2D;
using Color = Kean.Draw.Color;

namespace Kean.Draw.OpenGL.Backend
{
	public abstract class Renderer
	{
		protected Context Context { get; private set; }
		Func<Geometry2D.Integer.Size> getSize;
		public Geometry2D.Integer.Size Size { get { return this.getSize(); } }
		Func<TextureType> getType;
		public TextureType Type { get { return this.getType(); } }

		public event Action OnUse;
		public event Action OnUnuse;

		protected Renderer(Context context, Func<Geometry2D.Integer.Size> getSize, Func<TextureType> getType)
		{
			this.Context = context;
			this.getSize = getSize;
			this.getType = getType;
		}
		protected Renderer(Renderer original) :
			this(original.Context, original.getSize, original.getType)
		{
			original.Context = null;
			original.getSize = () => new Geometry2D.Integer.Size();
			original.getType = () => TextureType.Rgba;
		}
		#region Use, Unuse
		public virtual void Use()
		{
			this.OnUse.Call();
		}
		public virtual void Unuse()
		{
			this.OnUnuse.Call();
		}
		#endregion
		#region Flush, Finish
		public void Flush()
		{
			this.Context.Flush();
		}
		public bool Finish()
		{
			return this.Context.Finish();
		}
		#endregion
		#region Implementors Interface
		public abstract void SetClip(Geometry2D.Single.Box region);
		public abstract void UnSetClip();
		public abstract void SetTransform(Geometry2D.Single.Transform transform);
		public abstract void SetIdentityTransform();
		public abstract void Read(IntPtr pointer, Geometry2D.Integer.Box region);

		public abstract void Clear();
		public abstract void Clear(Geometry2D.Single.Box region);
		public abstract void Blend(float factor);
		public abstract void Draw(IColor color, Geometry2D.Single.Box region);
		public abstract Renderer Refurbish();
		#endregion
	}
}
