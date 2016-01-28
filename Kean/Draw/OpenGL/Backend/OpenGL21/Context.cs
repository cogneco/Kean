//
//  Context.cs
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

namespace Kean.Draw.OpenGL.Backend.OpenGL21
{
	public class Context :
		Backend.Context
	{
		int maximumTextureSize;
		int MaximumTextureSize
		{
			get 
			{
				if (this.maximumTextureSize == 0)
					GL.GetInteger(OpenTK.Graphics.OpenGL.GetPName.MaxTextureSize, out this.maximumTextureSize);
				return maximumTextureSize; 
			}
		}
		public Context()
		{
			GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.Texture2D);
		}
		protected override Backend.Texture AllocateTexture()
		{
			return new Texture(this);
		}
		protected override Backend.Composition AllocateComposition()
		{
			return new Composition(this);
		}
		protected internal override Backend.Composition CreateComposition(Backend.Texture texture)
		{
			return texture is Texture ? new Composition(this, texture as Texture) : null;
		}
		public override Backend.Program CreateProgram()
		{
			return new Program(this);
		}
		protected override Backend.Program CreateProgram(Programs program)
		{
			string code = null;
			switch (program)
			{
				case Programs.MonochromeToBgr:
					code = @"uniform sampler2D texture; void main() { vec4 value = texture2D(texture, gl_TexCoord[0].xy); float y = value.x; gl_FragColor = vec4(y, y, y, 1.0); }";
					break;
				case Programs.BgrToMonochrome:
					code = @"uniform sampler2D texture; void main() { vec4 value = texture2D(texture, gl_TexCoord[0].xy); float y = value.x * 0.299 + value.y * 0.587 + value.z * 0.114; gl_FragColor = vec4(y, y, y, 1.0); }";
					break;
				case Programs.BgrToU:
					code = @"uniform sampler2D texture; void main() { vec4 value = texture2D(texture, gl_TexCoord[0].xy); float u = value.x * (-0.168736) + value.y * (-0.331264) + value.z * 0.5000 + 0.5; gl_FragColor = vec4(u, u, u, 1.0); }";
					break;
				case Programs.BgrToV:
					code = @"uniform sampler2D texture; void main() { vec4 value = texture2D(texture, gl_TexCoord[0].xy); float v = value.x * 0.5 + value.y * (-0.418688) + value.z * (-0.081312) + 0.5; gl_FragColor = vec4(v, v, v, 1.0); }";
					break;
				case Programs.Yuv420ToBgr:
					code = @"
						uniform sampler2D texture0;
						uniform sampler2D texture1;
						uniform sampler2D texture2;
						vec4 YuvToRgba(vec4 t);
						void main()
						{
							vec2 position = gl_TexCoord[0].xy;
							gl_FragColor = YuvToRgba(vec4(texture2D(texture0, position).x, texture2D(texture1, position).x - 0.5, texture2D(texture2, position).x - 0.5, 1.0));
						}
						// Convert yuva to rgba
						vec4 YuvToRgba(vec4 t)
						{	
								mat4 matrix = mat4(1,	                1,                  1,   	                    0,
												-0.000001218894189, -0.344135678165337,  1.772000066073816,         0,
												1.401999588657340, -0.714136155581812,   0.000000406298063,         0, 
												0,	                0,                  0,                          1);   
										
							return matrix * t;
						}";
					break;
				default:
					break;
			}
			Backend.Program result = null;
			if (code.NotEmpty())
				result = this.CreateProgram(null, code);
			return result;
		}
		public override Backend.Program CreateProgram(string vertex, string fragment)
		{
			Shader vertexShader = new Shader(this, ShaderType.Vertex);
			vertexShader.Compile(vertex ?? @"void main() { gl_Position = ftransform(); gl_TexCoord[0] = gl_MultiTexCoord0; }");
			Shader fragmentShader = new Shader(this, ShaderType.Fragment);
			fragmentShader.Compile(fragment ?? @"uniform sampler2D texture; void main() { gl_FragColor = texture2D(texture, gl_TexCoord[0].xy); }");
			Program result = this.CreateProgram() as Program;
			result.Attach(vertexShader);
			result.Link();
			result.Attach(fragmentShader);
			result.Link();
			return result;
		}
		public override Backend.Shader CreateShader(ShaderType type)
		{
			return new Shader(this, type);
		}
		public override Backend.Control CreateControl()
		{
			return new Control(this);
		}
		public override Backend.Window CreateWindow()
		{
			return new Window(this);
		}
		#region Flush, Finish
		public override void Flush()
		{
			GL.Flush();
		}
		public override bool Finish()
		{
			GL.Finish();
			return true;
		}
		#endregion
		protected internal override Geometry2D.Integer.Size ClampTextureSize(Geometry2D.Integer.Size size)
		{
			if (size.Width > this.MaximumTextureSize)
			{
				Error.Log.Append(Error.Level.Warning, "Texture Width Exceeds Limit", string.Format("The requested texture size \"{0}\" is bigger than the maximum texture size \"{1}, {1}\". The textures width will be clamped.", size, this.MaximumTextureSize));
				size = new Geometry2D.Integer.Size(this.MaximumTextureSize, size.Height);
			}
			if (size.Height > this.MaximumTextureSize)
			{
				Error.Log.Append(Error.Level.Warning, "Texture Height Exceeds Limit", string.Format("The requested texture size \"{0}\" is bigger than the maximum texture size \"{1}, {1}\". The textures height will be clamped.", size, this.MaximumTextureSize));
				size = new Geometry2D.Integer.Size(size.Width, this.MaximumTextureSize);
			}
			return size;
		}
		protected internal override TextureType GetTextureType(Raster.Image image)
		{
			TextureType result;
			if (image is Raster.Bgra)
				result = TextureType.Rgba;
			else if (image is Raster.Bgr)
				result = TextureType.Rgb;
			else
				result = TextureType.Monochrome;
			return result;
		}
	}
}
