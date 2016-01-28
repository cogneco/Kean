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

namespace Kean.Draw.OpenGL
{
	public class Window :
		IDisposable
	{
		Backend.Window backend;
		public Window()
		{
			this.backend = Backend.Window.Create();
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
	}
}
