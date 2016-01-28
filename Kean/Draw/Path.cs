// 
//  Path.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2011-2013 Simon Mika
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

namespace Kean.Draw
{
	public class Path :
		System.Collections.Generic.IEnumerable<PathSegment.Abstract>
	{
		PathSegment.Abstract first;

		public Path()
		{ }
		public Path(PathSegment.Abstract first) :
			this()
		{
			this.first = first;
		}
		public Geometry2D.Single.Box Bounds()
		{
			return this.Bounds(Geometry2D.Single.Transform.Identity);
		}
		public Geometry2D.Single.Box Bounds(Geometry2D.Single.Transform transform)
		{
			return this.first.NotNull() ? this.first.Bounds(transform) : new Geometry2D.Single.Box();
		}

		Path Append(PathSegment.Abstract segment)
		{
			if (this.first.IsNull())
				this.first = segment;
			else
				this.first.Append(segment);
			return this;
		}
		public Path MoveTo(float x, float y)
		{
			return this.MoveTo(new Geometry2D.Single.Point(x, y));
		}
		public Path MoveTo(Geometry2D.Single.Point end)
		{
			return this.Append(new PathSegment.MoveTo(end));
		}
		public Path LineTo(float x, float y)
		{
			return this.LineTo(new Geometry2D.Single.Point(x, y));
		}
		public Path LineTo(Geometry2D.Single.Point end)
		{
			return this.Append(new PathSegment.LineTo(end));
		}
		public Path CurveTo(float firstX, float firstY, float secondX, float secondY, float endX, float endY)
		{
			return this.CurveTo(new Geometry2D.Single.Point(firstX, firstY), new Geometry2D.Single.Point(secondX, secondY), new Geometry2D.Single.Point(endX, endY));
		}
		public Path CurveTo(Geometry2D.Single.Point first, Geometry2D.Single.Point second, Geometry2D.Single.Point end)
		{
			return this.Append(new PathSegment.CurveTo(first, second, end));
		}
		public Path EllipticalArcTo(float radiusWidth, float radiusHeight, float angle, bool largeArc, bool sweep, float endX, float endY)
		{
			return this.EllipticalArcTo(new Geometry2D.Single.Size(radiusWidth, radiusHeight), angle, largeArc, sweep, new Geometry2D.Single.Point(endX, endY));
		}
		public Path EllipticalArcTo(Geometry2D.Single.Size radius, float angle, bool largeArc, bool sweep, Geometry2D.Single.Point end)
		{
			return this.Append(new PathSegment.EllipticalArcTo(radius, angle, largeArc, sweep, end));
		}
		public Path Close()
		{
			return this.Append(new PathSegment.Close());
		}
		#region IEnumerable<Abstract> and IEnumerable Members
		System.Collections.Generic.IEnumerator<PathSegment.Abstract> System.Collections.Generic.IEnumerable<PathSegment.Abstract>.GetEnumerator()
		{
			PathSegment.Abstract next = this.first;
			while (next.NotNull())
			{
				yield return next;
				next = next.Next;
			}
		}
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return (this as System.Collections.Generic.IEnumerable<PathSegment.Abstract>).GetEnumerator();
		}
		#endregion
		#region Object overrides
		public override string ToString()
		{
			return this.first.NotNull() ? this.first.ToString() : "";
		}
		#endregion
		#region Static Create Methods
		public static Path Rectangle(Geometry2D.Single.Box rectangle)
		{
			return new Path().MoveTo(rectangle.LeftTop).LineTo(rectangle.RightTop).LineTo(rectangle.RightBottom).LineTo(rectangle.LeftBottom).Close();
		}
		public static Path Rectangle(Geometry2D.Single.Box rectangle, Geometry2D.Single.Size radius)
		{
			return (radius.Width <= 0 || radius.Height <= 0) ? Path.Rectangle(rectangle) : new Path()
				.MoveTo(new Geometry2D.Single.Point(rectangle.Left + radius.Width, rectangle.Top))
				.LineTo(new Geometry2D.Single.Point(rectangle.Right - radius.Width, rectangle.Top))
				.EllipticalArcTo(radius, 0, false, true, new Geometry2D.Single.Point(rectangle.Right, rectangle.Top + radius.Height))
				.LineTo(new Geometry2D.Single.Point(rectangle.Right, rectangle.Bottom - radius.Height))
				.EllipticalArcTo(radius, 0, false, true, new Geometry2D.Single.Point(rectangle.Right - radius.Width, rectangle.Bottom))
				.LineTo(new Geometry2D.Single.Point(rectangle.Left + radius.Width, rectangle.Bottom))
				.EllipticalArcTo(radius, 0, false, true, new Geometry2D.Single.Point(rectangle.Left, rectangle.Bottom - radius.Height))
				.LineTo(new Geometry2D.Single.Point(rectangle.Left, rectangle.Top + radius.Height))
				.EllipticalArcTo(radius, 0, false, true, new Geometry2D.Single.Point(rectangle.Left + radius.Width, rectangle.Top))
				.Close();
		}
		public static Path Circle(Geometry2D.Single.Point center, float radius) { return Path.Ellipse(center, new Geometry2D.Single.Size(radius, radius)); }
		public static Path Ellipse(Geometry2D.Single.Point center, Geometry2D.Single.Size radius)
		{
			Geometry2D.Single.Point left = new Geometry2D.Single.Point(center.X + radius.Width, center.Y);
			Geometry2D.Single.Point right = new Geometry2D.Single.Point(center.X - radius.Width, center.Y);
			return new Path()
				.MoveTo(left)
				.EllipticalArcTo(radius, 0, false, true, right)
				.EllipticalArcTo(radius, 0, false, true, left)
				.Close();
		}
		#endregion
		#region Casts
		public static explicit operator Path(string path)
		{
			PathSegment.Abstract first = PathSegment.Abstract.Parse(path);
			return first.NotNull() ? new Path(first) : null;
		}
		public static implicit operator string(Path path)
		{
			return path.NotNull() ? path.ToString() : "";
		}
		#endregion
	}
}
