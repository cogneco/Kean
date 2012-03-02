﻿// 
//  Canvas.cs
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
using Kean.Core;
using Kean.Core.Extension;
using Geometry2D = Kean.Math.Geometry2D;
using Color = Kean.Draw.Color;
using Kean.Draw.Cairo.Extension;

namespace Kean.Draw.Cairo
{
	public class Canvas :
		Draw.Canvas
	{
		global::Cairo.Context backend;
		internal Canvas(Image image) :
			base(image)

		{
			this.backend = new global::Cairo.Context(image.Backend);
		}
		#region Clip, Transform, Push & Pop
		protected override Kean.Math.Geometry2D.Single.Box OnClipChange(Kean.Math.Geometry2D.Single.Box clip)
		{
			// TODO: this.backend.Mask(new global::Cairo.Pattern());
			return base.OnClipChange(clip);
		}
		protected override Geometry2D.Single.Transform OnTransformChange(Geometry2D.Single.Transform transform)
		{
			this.backend.Matrix = new global::Cairo.Matrix(transform.A, transform.B, transform.C, transform.D, transform.E, transform.F);
			return transform;
		}
		#endregion
		#region Create
		public override Draw.Canvas CreateSubcanvas(Geometry2D.Single.Box bounds)
		{
			return null;
		}
		#endregion
		#region Draw, Blend, Clear
		#region Draw Image
		public override void Draw(Draw.Map map, Draw.Image image)
		{
		}
		public override void Draw(Map map, Kean.Draw.Image image, Kean.Math.Geometry2D.Single.Box source, Kean.Math.Geometry2D.Single.Box destination)
		{
			throw new NotImplementedException();
		}
		public override void Draw(Draw.Image image, Geometry2D.Single.Box source, Geometry2D.Single.Box destination)
		{
		}
		#endregion
		#region Draw Box
		public override void Draw(IColor color, Geometry2D.Single.Box region)
		{
		}
		#endregion
		#region Draw Path
		void Draw(Draw.PathSegment.MoveTo segment)
		{
			this.backend.MoveTo(segment.End.X, segment.End.Y);
		}
		void Draw(Draw.PathSegment.LineTo segment)
		{
			this.backend.LineTo(segment.End.X, segment.End.Y);
		}
		void Draw(Draw.PathSegment.CurveTo segment)
		{
			this.backend.CurveTo(
				segment.First.X,
				segment.First.Y,
				segment.Second.X,
				segment .Second.Y,
				segment.End.X,
				segment.End.Y);
		}
		void Draw(Draw.PathSegment.EllipticalArcTo arc)
		{
			Tuple<Geometry2D.Single.Point, float, float> arcParameters = arc.PlatformctArcCoordinates();
			if (arc.Radius.X == 0 || arc.Radius.Y == 0 || arcParameters.IsNull())
				// If no solution to the coordinate problem just do:
				this.backend.LineTo(arc.End.X, arc.End.Y);
			else
			{
				float ratio = arc.Radius.Y / arc.Radius.X;
				this.backend.Save();
				this.backend.Scale(1, ratio);
				if (arcParameters.Item2 < arcParameters.Item3)
					this.backend.Arc(arcParameters.Item1.X, arcParameters.Item1.Y / ratio, arc.Radius.X, arcParameters.Item2, arcParameters.Item3);
				else
					this.backend.ArcNegative(arcParameters.Item1.X, arcParameters.Item1.Y / ratio, arc.Radius.X, arcParameters.Item2, arcParameters.Item3);
				this.backend.Restore();
			}
		}
		public override void Draw(IPaint fill, Stroke stroke, Path path)
		{
			foreach (Draw.PathSegment.Abstract segment in path)
			{
				if (segment is Draw.PathSegment.MoveTo)
					this.Draw(segment as Draw.PathSegment.MoveTo);
				else if (segment is Draw.PathSegment.LineTo)
					this.Draw(segment as Draw.PathSegment.LineTo);
				else if (segment is Draw.PathSegment.CurveTo)
					this.Draw(segment as Draw.PathSegment.CurveTo);
				else if (segment is Draw.PathSegment.EllipticalArcTo)
					this.Draw(segment as Draw.PathSegment.EllipticalArcTo);
			}
			if (fill.NotNull())
			{
				//Geometry2D.Single.Transform original = this.Transform;
				if (this.Set(fill))
				{
					if (stroke.NotNull() && stroke.Width > 0)
						this.backend.FillPreserve();
					else
						this.backend.Fill();
				}
				//this.Transform = original;
			}
			if (stroke.NotNull() && stroke.Width > 0 && this.Set(stroke.Paint))
			{
				this.backend.LineWidth = stroke.Width;
				this.backend.LineCap = stroke.LineCap.ToCairo();
				this.backend.LineJoin = stroke.LineJoin.ToCairo();
				this.backend.Stroke();
			}
		}
		bool Set(IPaint paint)
		{
			bool result;
			if (result = paint is IColor)
				this.backend.Color = (paint as IColor).ToCairo();
			else if (paint is Paint.Gradient)
			{
				global::Cairo.Gradient gradient = null;
				if (paint is Paint.LinearGradient)
					gradient = new global::Cairo.LinearGradient((paint as Paint.LinearGradient).Start.X, (paint as Paint.LinearGradient).Start.Y, (paint as Paint.LinearGradient).Stop.X, (paint as Paint.LinearGradient).Stop.Y);
				else if (paint is Paint.RadialGradient)
					gradient = new global::Cairo.RadialGradient((paint as Paint.RadialGradient).Focal.X, (paint as Paint.RadialGradient).Focal.Y, 0, (paint as Paint.RadialGradient).Center.X, (paint as Paint.RadialGradient).Center.Y, (paint as Paint.RadialGradient).Radius);
				if (result = gradient.NotNull())
				{
					foreach (Paint.GradientStop stop in paint as Paint.Gradient)
						gradient.AddColorStop(stop.Offset, stop.Color.ToCairo());
					this.backend.Pattern = gradient;
				}
			}
			return result;
		}
		#endregion
		#region Draw Text
		public override void Draw(IPaint fill, Stroke stroke, Text text, Geometry2D.Single.Point position)
		{

		}
		#endregion
		#region Blend
		public override void Blend(float factor)
		{
		}
		#endregion
		#region Clear
		public override void Clear()
		{
		}
		public override void Clear(Geometry2D.Single.Box region)
		{
		}
		#endregion
		#endregion
	}
}