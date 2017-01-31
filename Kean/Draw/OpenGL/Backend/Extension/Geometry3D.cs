//
//  Geometry2D.cs
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
using Kean.Collection.Extension;
using Geometry3D = Kean.Math.Geometry3D;
using Kean.Extension;
using GL = OpenTK.Graphics.OpenGL.GL;
using Target = Kean.Math.Geometry3D;

namespace Kean.Draw.OpenGL.Backend.Extension
{
	public static class Geometry3DExtension
	{
		#region Vector
		public static OpenTK.Vector3 Vector3(this Target.Single.Size size)
		{
			return new OpenTK.Vector3(size.Width, size.Height, size.Depth);
		}
		public static OpenTK.Vector3 Vector3(this Target.Single.Point point)
		{
			return new OpenTK.Vector3(point.X, point.Y, point.Z);
		}
		#endregion
		#region Transform
		public static OpenTK.Matrix4 Matrix4(this Target.Single.Transform transform)
		{
			return new OpenTK.Matrix4()
			{
				Row0 = new OpenTK.Vector4(transform.A, transform.B, transform.C, 0),
				Row1 = new OpenTK.Vector4(transform.D, transform.E, transform.F, 0),
				Row2 = new OpenTK.Vector4(transform.G, transform.H, transform.I, 0),
				Row3 = new OpenTK.Vector4(transform.J, transform.K, transform.L, 1),
			};
		}
		public static void Multiply(this Target.Single.Transform transform)
		{
			OpenTK.Matrix4 matrix = transform.Matrix4();
			GL.MultMatrix(ref matrix);
		}
		public static void Load(this Target.Single.Transform transform)
		{
			OpenTK.Matrix4 matrix = transform.Matrix4();
			GL.LoadMatrix(ref matrix);
		}
		#endregion
	}
}
