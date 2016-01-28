//
//  Composition.cs
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
using Kean.Draw.OpenGL.Backend.Extension;
using Kean.Extension;
using Collection = Kean.Collection;
using Color = Kean.Draw.Color;
using Error = Kean.Error;
using Geometry2D = Kean.Math.Geometry2D;
using GL = OpenTK.Graphics.OpenGL.GL;

namespace Kean.Draw.OpenGL.Backend.OpenGL21
{
	public class Composition :
		Backend.Composition
	{
		protected internal Composition(Context context) :
			base(context, new Texture(context), new Depth(context), new FrameBuffer(context))
		{ }
		protected internal Composition(Context context, Texture texture) :
			base(context, texture, new Depth(context), new FrameBuffer(context))
		{ }
		protected Composition(Composition original) :
			base(original)
		{} 

		protected override Backend.Renderer CreateRenderer(Backend.Context context, Func<Geometry2D.Integer.Size> getSize, Func<TextureType> getType)
		{
			Renderer result = new Renderer(context, getSize, getType);
			result.OnUse += () =>
			{
				GL.PushAttrib(OpenTK.Graphics.OpenGL.AttribMask.AllAttribBits);
				GL.Viewport(0, 0, this.Size.Width, this.Size.Height);
				GL.Ortho(0.0, 0.0, 1.0, 1.0, 0.0, 0.0);
				GL.MatrixMode(OpenTK.Graphics.OpenGL.MatrixMode.Projection);
				GL.PushMatrix();
				new Geometry2D.Single.Transform(2.0f / this.Size.Width, 0.0f, 0.0f, 2.0f / this.Size.Height, -1.0f, -1.0f).Load();
				GL.MatrixMode(OpenTK.Graphics.OpenGL.MatrixMode.Modelview);
				GL.PushMatrix();
				GL.LoadIdentity();
				GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.Blend);
				if (this.Type == TextureType.Rgba)
					GL.BlendFunc(OpenTK.Graphics.OpenGL.BlendingFactorSrc.SrcAlpha, OpenTK.Graphics.OpenGL.BlendingFactorDest.OneMinusSrcAlpha);
				else
					GL.BlendFunc(OpenTK.Graphics.OpenGL.BlendingFactorSrc.One, OpenTK.Graphics.OpenGL.BlendingFactorDest.Zero);

				this.FrameBuffer.Use();
				Exception.Framebuffer.Check();
			};
			result.OnUnuse += () =>
			{
				GL.MatrixMode(OpenTK.Graphics.OpenGL.MatrixMode.Modelview);
				GL.PopMatrix();
				GL.MatrixMode(OpenTK.Graphics.OpenGL.MatrixMode.Projection);
				GL.PopMatrix();
				GL.PopAttrib();
				GL.Ext.BindFramebuffer(OpenTK.Graphics.OpenGL.FramebufferTarget.FramebufferExt, 0);
				Exception.Framebuffer.Check();
			};
			return result;
		}
		protected override Backend.Composition Refurbish()
		{
			return new Composition(this);
		}
	}
}
