// 
//  Map.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2011 Simon Mika
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
using Kean;
using Kean.Extension;
using Geometry2D = Kean.Math.Geometry2D;
using Geometry3D = Kean.Math.Geometry3D;

namespace Kean.Draw
{
	public abstract class Map :
		IDisposable
	{
		~Map()
		{
			this.Dispose();
		}
		public virtual void Dispose()
		{
		}
		public abstract bool Remove(string name);
		public abstract void Add(string name, params int[] data);
		public abstract void Add(string name, params float[] data);
		public abstract void Add(string name, float[,] data);
		public abstract void Add<T>(string name, params T[] data) where T : struct;
		public abstract void Add<T>(string name, T[,] data) where T : struct;
		public abstract void Add<T>(string name, T[,,] data) where T : struct;
		public abstract void Add(string name, Draw.Image data);
		public abstract void Add(string name, Geometry2D.Integer.Size data);
		public abstract void Add(string name, Geometry3D.Single.Point data);
		public abstract void Add(string name, Geometry3D.Single.Size data);
		public abstract void Add(string name, Geometry3D.Single.Transform data);
	}
}
