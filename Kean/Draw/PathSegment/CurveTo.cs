// 
//  CurveTo.cs
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
using Collection = Kean.Collection;

namespace Kean.Draw.PathSegment
{
	public class CurveTo :
		Abstract
	{
		public Geometry2D.Single.Point First { get; set; }
		public Geometry2D.Single.Point Second { get; set; }
		public CurveTo(Geometry2D.Single.Point first, Geometry2D.Single.Point second, Geometry2D.Single.Point end) :
			base(end)
		{
			this.First = first;
			this.Second = second;
		}
		public override string String
		{
			get { return "C" + this.First.ToString("{0},{1}") + " " + this.Second.ToString("{0},{1}") + " " + this.End.ToString("{0},{1}"); }
		}
		protected override Geometry2D.Single.Box SegmentBounds(Geometry2D.Single.Transform transform)
		{
			Geometry2D.Single.Point p0 = this.Start;
			Geometry2D.Single.Point p1 = this.First;
			Geometry2D.Single.Point p2 = this.Second;
			Geometry2D.Single.Point p3 = this.End;
			Geometry2D.Single.Transform derivative = new Geometry2D.Single.Transform(transform.A, transform.B, transform.D, transform.E, 0, 0);
			Geometry2D.Single.Transform inverse = derivative.Inverse;
			Geometry2D.Single.Point e1p = inverse * new Geometry2D.Single.Point(1, 0);
			Geometry2D.Single.Point e2p = inverse * new Geometry2D.Single.Point(0, 1);
			Geometry2D.Single.Point zero = -3 * p0 + 3 * p1;
			Geometry2D.Single.Point one = 6 * p0 - 12 * p1 + 6 * p2;
			Geometry2D.Single.Point two = -3 * p0 + 9 * p1 - 9 * p2 + 3 * p3;
			Collection.List<float> parameters = new Collection.List<float>();
			parameters.Add(0f);
			parameters.Add(1f);
			this.AddParameterValues(parameters, e1p, zero, one, two);
			this.AddParameterValues(parameters, e2p, zero, one, two);
			Geometry2D.Single.Point[] points = new Geometry2D.Single.Point[parameters.Count];
			for (int i = 0; i < points.Length; i++)
			{
				//float alpha = i / (float)(points.Length - 1);
				float alpha = parameters[i];
				Geometry2D.Single.Point current = Kean.Math.Single.Power(1 - alpha, 3) * p0 + 3 * Kean.Math.Single.Power(1 - alpha, 2) * alpha * p1 + 3 * (1 - alpha) * Kean.Math.Single.Power(alpha, 2) * p2 + Kean.Math.Single.Power(alpha, 3) * p3;
				points[i] = transform * current;
			}
			return Geometry2D.Single.Box.Bounds(points);
		}
		void AddParameterValues(Collection.List<float> parameters, Geometry2D.Single.Point vector, Geometry2D.Single.Point zero, Geometry2D.Single.Point one, Geometry2D.Single.Point two)
		{
			Geometry2D.Single.Point zerop = new Geometry2D.Single.Point(zero.X * vector.Y, zero.Y * vector.X);
			Geometry2D.Single.Point onep = new Geometry2D.Single.Point(one.X * vector.Y, one.Y * vector.X);
			Geometry2D.Single.Point twop = new Geometry2D.Single.Point(two.X * vector.Y, two.Y * vector.X);
			float zeroCoefficient = zerop.X - zerop.Y;
			float oneCoefficient = onep.X - onep.Y;
			float twoCoefficient = twop.X - twop.Y;
			if (twoCoefficient != 0)
			{
				zeroCoefficient /= twoCoefficient;
				oneCoefficient /= twoCoefficient;
				float s = Kean.Math.Single.Power(-oneCoefficient / 2, 2) - zeroCoefficient;
				if (s >= 0)
				{
					s = Kean.Math.Single.SquareRoot(s);
					float x1 = -oneCoefficient / 2 + s;
					float x2 = -oneCoefficient / 2 - s;
					if (x1 >= 0 && x1 <= 1)
						parameters.Add(x1);
					if (x2 >= 0 && x2 <= 1)
						parameters.Add(x2);
				}
			}
			else if (oneCoefficient != 0)
			{
				float x1 = -zeroCoefficient / oneCoefficient;
				if (x1 >= 0 && x1 <= 1)
					parameters.Add(x1);
			}
		}
	}
}
