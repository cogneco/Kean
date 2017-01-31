// 
//  Window.cs
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
using Kean.Extension;
using Geometry2D = Kean.Math.Geometry2D;
using GL = OpenTK.Graphics.OpenGL.GL;
using Parallel = Kean.Parallel;
using Error = Kean.Error;

namespace Kean.Draw.OpenGL.Backend.OpenGL21
{
	public class Window :
		Backend.Window
	{
		public Window()
		{ }
		internal Window(Context context) :
			base(context)
		{ }
		protected override Backend.Context CreateContext()
		{
			return new Context();
		}
		protected override Backend.Renderer CreateRenderer()
		{
			return new Renderer(this.Context, () => new Geometry2D.Integer.Size(this.Width, this.Height), () => TextureType.Rgb);
		}
		protected override Backend.ThreadPool CreateThreadPool(string name, int workers)
		{
			return new ThreadPool(this.WindowInfo, name, workers);
		}
		protected override void SetupViewport()
		{
			GL.ClearColor(System.Drawing.SystemColors.Control);
			GL.Viewport(0, 0, this.Width, this.Height);
			GL.Ortho(0.0, 0.0, 1.0, 1.0, 0.0, 0.0);
			GL.MatrixMode(OpenTK.Graphics.OpenGL.MatrixMode.Projection);
			OpenTK.Matrix4 projection = new OpenTK.Matrix4()
			{
				Row0 = new OpenTK.Vector4(2.0f / this.Width, 0.0f, 0, 0),
				Row1 = new OpenTK.Vector4(0.0f, -2.0f / this.Height, 0, 0),
				Row2 = new OpenTK.Vector4(0, 0, 1, 0),
				Row3 = new OpenTK.Vector4(-1.0f, 1.0f, 0, 1),
			};
			GL.LoadMatrix(ref projection);
			GL.MatrixMode(OpenTK.Graphics.OpenGL.MatrixMode.Modelview);
			OpenTK.Matrix4 identity = new OpenTK.Matrix4()
			{
				Row0 = new OpenTK.Vector4(1, 0, 0, 0),
				Row1 = new OpenTK.Vector4(0, 1, 0, 0),
				Row2 = new OpenTK.Vector4(0, 0, 1, 0),
				Row3 = new OpenTK.Vector4(0, 0, 0, 1),
			};
			GL.LoadMatrix(ref identity);
		}
	}
}
