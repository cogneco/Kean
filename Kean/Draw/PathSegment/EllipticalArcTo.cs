// 
//  EllipticalArcTo.cs
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
	public class EllipticalArcTo :
		Abstract
	{
		public Geometry2D.Single.Size Radius { get; set; }
		public float Angle { get; set; }
		public bool LargeArc { get; set; }
		public bool Sweep { get; set; }

		public EllipticalArcTo(Geometry2D.Single.Size radius, float angle, bool largeArc, bool sweep, Geometry2D.Single.Point end) :
			base(end)
		{
			this.Radius = radius;
			this.Angle = angle;
			this.LargeArc = largeArc;
			this.Sweep = sweep;
			this.End = end;
		}
		public override string String
		{
			get { return "A" + this.Radius.ToString() + "," + this.Angle.ToString() + " " + (this.LargeArc ? 1 : 0) + "," + (this.Sweep ? 1 : 0) + " "+ this.End.ToString(); }
		}
		protected override Geometry2D.Single.Box SegmentBounds(Geometry2D.Single.Transform transform)
		{
			Geometry2D.Single.Box result = new Geometry2D.Single.Box();
			Tuple<Geometry2D.Single.Point, float, float> coordinates = this.PlatformctArcCoordinates();
			float startAngle = coordinates.Item2;
			float endAngle = coordinates.Item3;
			Geometry2D.Single.Transform derivative = new Geometry2D.Single.Transform(transform.A, transform.B, transform.C, transform.D, 0, 0);
			Geometry2D.Single.Transform inverse = derivative.Inverse;
			Geometry2D.Single.Point e1p = inverse * new Geometry2D.Single.Point(1, 0);
			Geometry2D.Single.Point e2p = inverse * new Geometry2D.Single.Point(0, 1);
			Collection.List<float> angles = new Collection.List<float>();
			angles.Add(startAngle);
			angles.Add(endAngle);
			this.AnglesAdd(angles, Kean.Math.Single.ArcusTangensExtended(-e1p.X * this.Radius.Height, e1p.Y * this.Radius.Width), startAngle, endAngle);
			this.AnglesAdd(angles, Kean.Math.Single.ArcusTangensExtended(-e2p.X * this.Radius.Height, e2p.Y * this.Radius.Width), startAngle, endAngle);
			Geometry2D.Single.Point[] points = new Geometry2D.Single.Point[angles.Count];
			for (int i = 0; i < points.Length; i++)
			{
				float angle = angles[i];
				points[i] = transform * (new Geometry2D.Single.Point(this.Radius.Width * Kean.Math.Single.Cosinus(angle), this.Radius.Height * Kean.Math.Single.Sinus(angle)) + coordinates.Item1);
			}
			result = Geometry2D.Single.Box.Bounds(points);
			return result;
		}
		void AnglesAdd(Collection.List<float> angles, float angle, float startAngle, float endAngle)
		{
			for (int i = -2; i <= 2; i++)
			{
				float currentAngle = angle + Kean.Math.Single.Pi * i;
				if (startAngle <= currentAngle && currentAngle <= endAngle || startAngle >= currentAngle && currentAngle >= endAngle)
					angles.Add(currentAngle);
			}
		}
		public Tuple<Geometry2D.Single.Point, float, float> PlatformctArcCoordinates()
		{
			float x1 = this.Start.X,
				  y1 = this.Start.Y,
				  x2 = this.End.X,
				  y2 = this.End.Y,
				  rx = this.Radius.Width,
				  ry = this.Radius.Height,
				  phi = this.Angle;

			float cx, cy; // Center point
			float cxp, cyp;
			float deltaTheta;
			float k1, k2, k3, k4;

			float cosPhi = Kean.Math.Single.Cosinus(phi),
				  sinPhi = Kean.Math.Single.Sinus(phi);

			if (rx < 0)
				rx = -rx;
			if (ry < 0)
				ry = -ry;

			float xp = (x1 - x2) / 2 * cosPhi + (y1 - y2) / 2 * sinPhi;
			float yp = (x1 - x2) / 2 * -sinPhi + (y1 - y2) / 2 * cosPhi;

			// Compute the center point
			k1 = rx * rx * yp * yp + ry * ry * xp * xp;
			if (k1 == 0)
				return null;

			k1 = Kean.Math.Single.SquareRoot(Kean.Math.Single.Absolute((rx * rx * ry * ry) / k1 - 1));
			if (this.Sweep == this.LargeArc)
				k1 = -k1;

			cxp = k1 * rx * yp / ry;
			cyp = -k1 * ry * xp / rx;

			cx = cosPhi * cxp - sinPhi * cyp + (x1 + x2) / 2;
			cy = sinPhi * cxp + cosPhi * cyp + (y1 + y2) / 2;

			k1 = (xp - cxp) / rx;
			k2 = (yp - cyp) / ry;
			k3 = (-xp - cxp) / rx;
			k4 = (-yp - cyp) / ry;

			Geometry2D.Single.Point first = new Geometry2D.Single.Point(k1, k2);
			Geometry2D.Single.Point second = new Geometry2D.Single.Point(k3, k4);
			float startAngle = Kean.Math.Geometry2D.Single.Point.BasisX.Angle(first);
			deltaTheta = first.Angle(second);
			/*
			// Start currentAngle
			k1 = (xp - cxp) / rx;
			k2 = (yp - cyp) / ry;
			k3 = (-xp - cxp) / rx;
			k4 = (-yp - cyp) / ry;

			k5 = Function.Single.Sqrt(k1 * k1 + k2 * k2);

			k5 = k1 / k5;
			if (k5 < -1)
				k5 = -1;
			else if (k5 > 1)
				k5 = 1;
			float startAngle = Function.Single.ArcCos(k5);
			if (k2 < 0)
				startAngle = -startAngle;

			// End currentAngle
			k5 = Function.Single.Sqrt((k1 * k1 + k2 * k2) * (k3 * k3 + k4 * k4));

			k5 = (k1 * k3 + k2 * k4) / k5;
			if (k5 < -1)
				k5 = -1;
			else if (k5 > 1)
				k5 = 1;
			deltaTheta = Function.Single.ArcCos(k5);
			if (k1 * k4 - k3 * k2 < 0)
				deltaTheta = -deltaTheta;
			*/
			if (this.Sweep && deltaTheta < 0)
				deltaTheta += Kean.Math.Single.Pi * 2;
			else if (!this.Sweep && deltaTheta > 0)
				deltaTheta -= Kean.Math.Single.Pi * 2;
			return Tuple.Create(new Geometry2D.Single.Point(cx, cy), startAngle, startAngle + deltaTheta);
		}
	}
}
