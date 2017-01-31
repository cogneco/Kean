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
using Geometry2D = Kean.Math.Geometry2D;
using Kean.Extension;
using GL = OpenTK.Graphics.OpenGL.GL;
using Target = Kean.Math.Geometry2D;

namespace Kean.Draw.OpenGL.Backend.Extension
{
    public static class Geometry2DExtension
    {
        #region Vector
        public static OpenTK.Vector2 Vector2(this Target.Single.Size size)
        {
            return new OpenTK.Vector2(size.Width, size.Height);
        }
        public static OpenTK.Vector2 Vector2(this Target.Single.Point point)
        {
            return new OpenTK.Vector2(point.X, point.Y);
        }
        #endregion
        #region Transform
        public static OpenTK.Matrix4 Matrix4(this Target.Single.Transform transform)
        {
            return new OpenTK.Matrix4()
            {
                Row0 = new OpenTK.Vector4(transform.A, transform.B, 0, transform.C),
                Row1 = new OpenTK.Vector4(transform.D, transform.E, 0, transform.F),
                Row2 = new OpenTK.Vector4(0, 0, 1, 0),
                Row3 = new OpenTK.Vector4(transform.G, transform.H, 0, transform.I), 
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
