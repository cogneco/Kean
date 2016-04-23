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
using Parallel = Kean.Parallel;
using Geometry2D = Kean.Math.Geometry2D;

namespace Kean.Draw.OpenGL
{
	public class Window :
		IDisposable
	{
		public event Action Initialized;
		public event Action<Surface> Draw;

		public Parallel.ThreadPool ThreadPool { get { return this.backend.NotNull() ? this.backend.ThreadPool : null; } }

		public bool Visible
		{
			get { return this.backend.Visible; } 
			set { this.backend.Visible = value; }
		}

		Backend.Window backend;
		public Window()
		{
			this.backend = Backend.Window.Create();
			this.backend.Initialized += () => this.Initialized.Call();
			this.backend.Draw += renderer =>
			{
				using (Surface surface = new Surface(renderer))
				{
					surface.Use();
					surface.Transform = Geometry2D.Single.Transform.CreateTranslation(surface.Size / 2);
					this.Draw(surface);
					surface.Unuse();
				}
			};
		}

		~Window()
		{
			this.Dispose(false);
		}
		void IDisposable.Dispose()
		{
			this.Dispose(true);
		}
		protected virtual void Dispose(bool disposing)
		{
			this.Close();
		}
		public void Invalidate() {
			this.backend.Redraw();
		}
		public bool Close()
		{
			bool result;
			if (result = this.backend.NotNull())
			{
				this.backend.Dispose();
				this.backend = null;
			}
			return result;
		}
		public void Run() {
			this.backend.Run();
		}
	}
}
