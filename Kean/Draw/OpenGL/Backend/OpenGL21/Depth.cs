//
//  Depth.cs
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
using Raster = Kean.Draw.Raster;

namespace Kean.Draw.OpenGL.Backend.OpenGL21
{
	public class Depth : 
		Backend.Depth
	{
		protected internal Depth(Context context) :
			base(context)
		{ }
		protected Depth(Depth original) :
			base(original)
		{ }
		protected override int CreateIdentifier()
		{
			return GL.GenTexture();
		}
		public override void Use()
		{
			GL.BindTexture(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, this.Identifier);
		}
		public override void UnUse()
		{
			GL.BindTexture(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, 0);
		}
		public override void Create()
		{
			GL.TexImage2D(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, 0, (OpenTK.Graphics.OpenGL.PixelInternalFormat)OpenTK.Graphics.OpenGL.All.DepthComponent32, this.Size.Width, this.Size.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.DepthComponent, OpenTK.Graphics.OpenGL.PixelType.UnsignedInt, IntPtr.Zero);
			GL.TexParameter(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, OpenTK.Graphics.OpenGL.TextureParameterName.TextureMinFilter, (int)OpenTK.Graphics.OpenGL.TextureMinFilter.Linear);
			GL.TexParameter(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, OpenTK.Graphics.OpenGL.TextureParameterName.TextureMagFilter, (int)OpenTK.Graphics.OpenGL.TextureMagFilter.Linear);
			GL.TexParameter(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, OpenTK.Graphics.OpenGL.TextureParameterName.TextureWrapS, (int)OpenTK.Graphics.OpenGL.TextureWrapMode.ClampToBorder);
			GL.TexParameter(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, OpenTK.Graphics.OpenGL.TextureParameterName.TextureWrapT, (int)OpenTK.Graphics.OpenGL.TextureWrapMode.ClampToBorder);
		}
		protected internal override Backend.Depth Refurbish()
		{
			return new Depth(this);
		}
		protected internal override void Delete()
		{
			if (this.Identifier != 0)
			{
				GL.DeleteTexture(this.Identifier);
				base.Delete();
			}
		}
	}
}
