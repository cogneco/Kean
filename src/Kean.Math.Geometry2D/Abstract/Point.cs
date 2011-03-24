// 
//  Point.cs
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
namespace Kean.Math.Geometry2D.Abstract
{
    public abstract class Point<TransformType, TransformValue, PointType, PointValue, R, V> :
		Vector<TransformType, TransformValue, PointType, PointValue, R, V>,
		IPoint<V>
        where TransformType : Transform<TransformType, TransformValue, R, V>, ITransform<V>, new()
        where TransformValue : struct, ITransform<V>
        where PointType : Point<TransformType, TransformValue, PointType, PointValue, R, V>, new()
        where PointValue : struct, IPoint<V>, IVector<V>
        where R : Kean.Math.Abstract<R, V>, new()
        where V : struct
	{
		public new R X { get { return base.X; } }
		public new R Y { get { return base.Y; } }
		#region IPoint<V> Members
		V IPoint<V>.X { get { return this.X; } }
		V IPoint<V>.Y { get { return this.Y; } }
		#endregion
		#region Constructors
        protected Point() { }
        protected Point(R x, R y) :
			base(x, y)
		{ }
        #region Arithmetic Operators
        public static Point<TransformType, TransformValue, PointType, PointValue, R, V> operator *(TransformType left, Point<TransformType, TransformValue, PointType, PointValue, R, V> right)
        {
            VectorType result;
            if (right is Point<TransformType, TransformValue, PointType, PointValue, R, V>)
            {
                result = new VectorType()
                {
                    X = left.A * right.X + (R)left.C * right.Y + left.E,
                    Y = (R)left.B * right.X + (R)left.D * right.Y + left.F,
                };
            }
            return result;
        }
        #endregion
    }
}

