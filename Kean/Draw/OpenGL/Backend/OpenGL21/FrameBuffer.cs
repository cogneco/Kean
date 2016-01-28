//
//  FrameBuffer.cs
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

namespace Kean.Draw.OpenGL.Backend.OpenGL21
{
	public class FrameBuffer :
		Backend.FrameBuffer
	{
		protected internal FrameBuffer(Context context) :
			base(context)
		{ }
		protected FrameBuffer(FrameBuffer frameBuffer) :
			base(frameBuffer)
		{ }
		protected override int CreateIdentifier()
		{
			int result;
			GL.Ext.GenFramebuffers(1, out result);
			return result;
		}
		public override void Use()
		{
			GL.Ext.BindFramebuffer(OpenTK.Graphics.OpenGL.FramebufferTarget.FramebufferExt, this.Identifier);
		}
		public override void UnUse()
		{
			GL.Ext.BindFramebuffer(OpenTK.Graphics.OpenGL.FramebufferTarget.FramebufferExt, 0);
		}
		public override void Create(Backend.Texture texture, Backend.Depth depth)
		{
			GL.Ext.FramebufferTexture2D(OpenTK.Graphics.OpenGL.FramebufferTarget.FramebufferExt, OpenTK.Graphics.OpenGL.FramebufferAttachment.ColorAttachment0Ext, OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, texture.Identifier, 0);
			GL.Ext.FramebufferTexture2D(OpenTK.Graphics.OpenGL.FramebufferTarget.FramebufferExt, OpenTK.Graphics.OpenGL.FramebufferAttachment.DepthAttachmentExt, OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, depth.Identifier, 0);
			Exception.Framebuffer.Check();
			GL.Ext.BindFramebuffer(OpenTK.Graphics.OpenGL.FramebufferTarget.FramebufferExt, 0);
		}
		protected internal override Backend.FrameBuffer Refurbish()
		{
			return new FrameBuffer(this);
		}
		protected internal override void Delete()
		{
			if (this.Identifier != 0)
			{
				int identifier = this.Identifier;
				GL.DeleteFramebuffers(1, ref identifier);
				base.Delete();
			}
		}
	}
}
