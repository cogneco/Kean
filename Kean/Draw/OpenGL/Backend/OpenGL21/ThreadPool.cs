// 
//  ThreadPool.cs
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
using Error = Kean.Error;
using Collection = Kean.Collection;
using Kean.Collection.Extension;
using Geometry2D = Kean.Math.Geometry2D;
using Kean.Extension;
using GL = OpenTK.Graphics.OpenGL.GL;
using Parallel = Kean.Parallel;

namespace Kean.Draw.OpenGL.Backend.OpenGL21
{
	public class ThreadPool :
		Backend.ThreadPool
	{
		public ThreadPool(OpenTK.Platform.IWindowInfo windowInformation, string name, int workers) :
			base(windowInformation, name, workers)
		{ }
		protected override OpenTK.Graphics.GraphicsContext CreateGraphicsContext(OpenTK.Platform.IWindowInfo windowInformation)
		{
			return new OpenTK.Graphics.GraphicsContext(OpenTK.Graphics.GraphicsMode.Default, windowInformation, 2, 1, OpenTK.Graphics.GraphicsContextFlags.Default);
		}
	}
}
