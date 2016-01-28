//
//  Renderer.cs
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
	public class Renderer :
		Backend.Renderer
	{
		protected internal Renderer(Backend.Context context, Func<Geometry2D.Integer.Size> getSize, Func<TextureType> getType) :
			base(context, getSize, getType)
		{
		}
		protected Renderer(Renderer original) :
			base(original)
		{ 
		}
		public override void Use()
		{
			base.Use();
		}
		public override void Unuse()
		{
			base.Unuse();
		}
		public override void SetClip(Geometry2D.Single.Box region)
		{
			if (!region.Empty)
			{
				double[] left = new double[] { 1.0, 0.0, 0.0, -region.Left };
				GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.ClipPlane0);
				GL.ClipPlane(OpenTK.Graphics.OpenGL.ClipPlaneName.ClipPlane0, ref left[0]);
				double[] right = new double[] { -1.0, 0.0, 0.0, region.Right };
				GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.ClipPlane1);
				GL.ClipPlane(OpenTK.Graphics.OpenGL.ClipPlaneName.ClipPlane1, ref right[0]);
				double[] top = new double[] { 0.0, 1.0, 0.0, -region.Top };
				GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.ClipPlane2);
				GL.ClipPlane(OpenTK.Graphics.OpenGL.ClipPlaneName.ClipPlane2, ref top[0]);
				double[] bottom = new double[] { 0.0, -1.0, 0.0, region.Bottom };
				GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.ClipPlane3);
				GL.ClipPlane(OpenTK.Graphics.OpenGL.ClipPlaneName.ClipPlane3, ref bottom[0]);
			}
			else
				this.UnSetClip();
		}
		public override void UnSetClip()
		{
			GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.ClipPlane0);
			GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.ClipPlane1);
			GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.ClipPlane2);
			GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.ClipPlane3);
		}
		public override void SetTransform(Geometry2D.Single.Transform transform)
		{
			transform.Load();
		}
		public override void SetIdentityTransform()
		{
			GL.LoadIdentity();
		}
		public override void Read(IntPtr pointer, Geometry2D.Integer.Box region)
		{
			switch (this.Type)
			{
				default:
				case TextureType.Rgba:
					GL.ReadPixels(region.Left, region.Top, region.Width, region.Height, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, OpenTK.Graphics.OpenGL.PixelType.UnsignedByte, pointer);
					break;
				case TextureType.Rgb:
					GL.ReadPixels(region.Left, region.Top, region.Width, region.Height, OpenTK.Graphics.OpenGL.PixelFormat.Bgr, OpenTK.Graphics.OpenGL.PixelType.UnsignedByte, pointer);
					break;
				case TextureType.Monochrome:
					GL.ReadPixels(region.Left, region.Top, region.Width, region.Height, OpenTK.Graphics.OpenGL.PixelFormat.Luminance, OpenTK.Graphics.OpenGL.PixelType.UnsignedByte, pointer);
					break;
			}
		}
		public override void Clear()
		{
			GL.ClearColor(0f, 0f, 0f, 0f);
			GL.Clear(OpenTK.Graphics.OpenGL.ClearBufferMask.ColorBufferBit | OpenTK.Graphics.OpenGL.ClearBufferMask.StencilBufferBit);
		}

		public override void Clear(Kean.Math.Geometry2D.Single.Box region)
		{
			GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.Texture2D);
			GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.Blend);
			GL.BlendFunc(OpenTK.Graphics.OpenGL.BlendingFactorSrc.Zero, OpenTK.Graphics.OpenGL.BlendingFactorDest.Zero);
			this.CreateRectangle(region);
		}
		public override void Blend(float factor)
		{
			GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.Texture2D);
			GL.BlendFunc(OpenTK.Graphics.OpenGL.BlendingFactorSrc.One, OpenTK.Graphics.OpenGL.BlendingFactorDest.One);
			GL.BlendEquation(OpenTK.Graphics.OpenGL.BlendEquationMode.FuncReverseSubtract);
			GL.Color4(0f, 0f, 0f, factor);
			this.CreateRectangle();
		}
		public override void Draw(IColor color, Geometry2D.Single.Box region)
		{
			GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.Texture2D);
			Color.Bgra bgra = color.Convert<Color.Bgra>();
			GL.Color4(bgra.Red, bgra.Green, bgra.Blue, bgra.Alpha);
			this.CreateRectangle(region);
		}
		protected void CreateRectangle()
		{
			GL.PushMatrix();
			Geometry2D.Single.Transform.Identity.Load();
			this.CreateRectangle(new Geometry2D.Single.Box(this.Size));
			GL.PopMatrix();
		}
		protected void CreateRectangle(Geometry2D.Single.Box region)
		{
			GL.Begin(OpenTK.Graphics.OpenGL.BeginMode.Quads);
			GL.Vertex2(region.Left, region.Top);
			GL.Vertex2(region.Right, region.Top);
			GL.Vertex2(region.Right, region.Bottom);
			GL.Vertex2(region.Left, region.Bottom);
			GL.End();
		}
		public override Backend.Renderer Refurbish()
		{
			return new Renderer(this);
		}
	}
}
