//
//  Program.cs
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
	public class Program :
		Backend.Program
	{
		protected internal Program(Context context) :
			base(context)
		{ }
		protected Program(Program program) :
			base(program)
		{ }
		public override void Use()
		{
			GL.UseProgram(this.Identifier);
		}
		public override void UnUse()
		{
			GL.UseProgram(0);
		}
		public override void Attach(Backend.Shader shader)
		{
			GL.AttachShader(this.Identifier, shader.Identifier);
		}
		public override void Detach(Backend.Shader shader)
		{
			OpenTK.Graphics.OpenGL.GL.DetachShader(this.Identifier, shader.Identifier);
		}
		public override void Link()
		{
			OpenTK.Graphics.OpenGL.GL.LinkProgram(this.Identifier);
		}
		public override void SetTexture(string name, int number, Backend.IData texture)
		{
			OpenTK.Graphics.OpenGL.GL.ActiveTexture(this.GetUnit(number));
			texture.Use();
			this.SetVariable(name, number);
		}
		public override void UnSetTexture(int number)
		{
			OpenTK.Graphics.OpenGL.GL.ActiveTexture(this.GetUnit(number));
			OpenTK.Graphics.OpenGL.GL.BindTexture(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, 0);
		}
		public override void SetVariable(string variableName, float[,] values)
		{
			this.Use();
			unsafe
			{
				fixed (float* pointer = &values[0, 0])
				{
					switch (values.GetLength(1))
					{
						default:
						case 1:
							OpenTK.Graphics.OpenGL.GL.Uniform1(OpenTK.Graphics.OpenGL.GL.GetUniformLocation(this.Identifier, variableName), values.GetLength(0), pointer);
							break;
						case 2:
							OpenTK.Graphics.OpenGL.GL.Uniform2(OpenTK.Graphics.OpenGL.GL.GetUniformLocation(this.Identifier, variableName), values.Length, pointer);
							break;
						case 3:
							OpenTK.Graphics.OpenGL.GL.Uniform3(OpenTK.Graphics.OpenGL.GL.GetUniformLocation(this.Identifier, variableName), values.Length, pointer);
							break;
						case 4:
							OpenTK.Graphics.OpenGL.GL.Uniform4(OpenTK.Graphics.OpenGL.GL.GetUniformLocation(this.Identifier, variableName), values.Length, pointer);
							break;
					}
				}
			}
			this.UnUse();
		}
		public override void SetVariable(string variableName, params int[] values)
		{
			this.Use();
			switch (values.Length)
			{
				default:
				case 1:
					OpenTK.Graphics.OpenGL.GL.Uniform1(OpenTK.Graphics.OpenGL.GL.GetUniformLocation(this.Identifier, variableName), values[0]);
					break;
				case 2:
					OpenTK.Graphics.OpenGL.GL.Uniform2(OpenTK.Graphics.OpenGL.GL.GetUniformLocation(this.Identifier, variableName), values[0], values[1]);
					break;
				case 3:
					OpenTK.Graphics.OpenGL.GL.Uniform3(OpenTK.Graphics.OpenGL.GL.GetUniformLocation(this.Identifier, variableName), values[0], values[1], values[2]);
					break;
				case 4:
					OpenTK.Graphics.OpenGL.GL.Uniform4(OpenTK.Graphics.OpenGL.GL.GetUniformLocation(this.Identifier, variableName), values[0], values[1], values[2], values[3]);
					break;
			}
			this.UnUse();
		}
		public override void SetVariable(string variableName, params float[] values)
		{
			this.Use();
			switch (values.Length)
			{
				default:
				case 1:
					OpenTK.Graphics.OpenGL.GL.Uniform1(OpenTK.Graphics.OpenGL.GL.GetUniformLocation(this.Identifier, variableName), values[0]);
					break;
				case 2:
					OpenTK.Graphics.OpenGL.GL.Uniform2(OpenTK.Graphics.OpenGL.GL.GetUniformLocation(this.Identifier, variableName), values[0], values[1]);
					break;
				case 3:
					OpenTK.Graphics.OpenGL.GL.Uniform3(OpenTK.Graphics.OpenGL.GL.GetUniformLocation(this.Identifier, variableName), values[0], values[1], values[2]);
					break;
				case 4:
					OpenTK.Graphics.OpenGL.GL.Uniform4(OpenTK.Graphics.OpenGL.GL.GetUniformLocation(this.Identifier, variableName), values[0], values[1], values[2], values[3]);
					break;
			}
			this.UnUse();
		}
		public override Backend.Data1D CreateData1D()
		{
			return new Data1D(this.Context);
		}
		public override Backend.Data2D CreateData2D()
		{
			return new Data2D(this.Context);
		}
		public override Backend.Data3D CreateData3D()
		{
			return new Data3D(this.Context);
		}
		OpenTK.Graphics.OpenGL.TextureUnit GetUnit(int number)
		{
			OpenTK.Graphics.OpenGL.TextureUnit result;
			switch (number)
			{
				default:
				case 0: result = OpenTK.Graphics.OpenGL.TextureUnit.Texture0; break;
				case 1: result = OpenTK.Graphics.OpenGL.TextureUnit.Texture1; break;
				case 2: result = OpenTK.Graphics.OpenGL.TextureUnit.Texture2; break;
				case 3: result = OpenTK.Graphics.OpenGL.TextureUnit.Texture3; break;
				case 4: result = OpenTK.Graphics.OpenGL.TextureUnit.Texture4; break;
				case 5: result = OpenTK.Graphics.OpenGL.TextureUnit.Texture5; break;
				case 6: result = OpenTK.Graphics.OpenGL.TextureUnit.Texture6; break;
			}
			return result;
		}
		protected override Backend.Program Refurbish()
		{
			return new Program(this);
		}
		protected internal override void Delete()
		{
			if (this.Identifier != 0)
			{
				GL.DeleteProgram(this.Identifier);
				base.Delete();
			}
		}
	}
}
