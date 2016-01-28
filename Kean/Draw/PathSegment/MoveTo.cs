// 
//  MoveTo.cs
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

namespace Kean.Draw.PathSegment
{
	public class MoveTo :
		Abstract
	{
		protected override Geometry2D.Single.Point SubpathStart 
		{ 
			get { return this.End; }
			set { this.End = value; }
		}
		public override string String
		{
			get { return "M" + this.End.ToString(); }
		}
		public MoveTo(Geometry2D.Single.Point end) :
			base(end)
		{ }
		protected override Geometry2D.Single.Box SegmentBounds(Geometry2D.Single.Transform transform)
		{
			Geometry2D.Single.Point end = transform * this.End;
			return new Geometry2D.Single.Box(end, new Geometry2D.Single.Size());
		}
	}
}
