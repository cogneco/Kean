//
//  Shader.cs
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
using Collection = Kean.Collection;
using Error = Kean.Error;
using GL = OpenTK.Graphics.OpenGL.GL;

namespace Kean.Draw.OpenGL.Backend.OpenGL21
{
	public class Shader :
		Backend.Shader
	{
		protected internal Shader(Context context, ShaderType type) :
			base(context, type)
		{ }
		protected Shader(Shader original) :
			base(original)
		{ }
		protected override int Create(ShaderType type)
		{
			OpenTK.Graphics.OpenGL.ShaderType t;
			switch (type)
			{
				default:
				case ShaderType.Fragment:
					t = OpenTK.Graphics.OpenGL.ShaderType.FragmentShader;
					break;
				case ShaderType.Vertex:
					t = OpenTK.Graphics.OpenGL.ShaderType.VertexShader;
					break;
				case ShaderType.Geometry:
					t = OpenTK.Graphics.OpenGL.ShaderType.GeometryShaderExt;
					break;
			}
			return GL.CreateShader(t);
		}
		public override string Compile(string code)
		{
			OpenTK.Graphics.OpenGL.GL.ShaderSource(this.Identifier, code);
			OpenTK.Graphics.OpenGL.GL.CompileShader(this.Identifier);
			string compilerMesage;
			OpenTK.Graphics.OpenGL.GL.GetShaderInfoLog(this.Identifier, out compilerMesage);
			int statusCode;
			OpenTK.Graphics.OpenGL.GL.GetShader(this.Identifier, OpenTK.Graphics.OpenGL.ShaderParameter.CompileStatus, out statusCode);
			return statusCode != 1 ? compilerMesage : null;
		}
		protected override Backend.Shader Refurbish()
		{
			return new Shader(this);
		}
		protected internal override void Delete()
		{
			if (this.Identifier != 0)
			{
				GL.DeleteShader(this.Identifier);
				base.Delete();
			}
		}
	}
}
