// 
//  Gradient.cs
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
using Kean.Collection.Extension;

namespace Kean.Draw.Paint
{
	public abstract class Gradient :
		Collection.Abstract.List<GradientStop>,
		IPaint
	{
		public Geometry2D.Single.Transform Transform { get; set; }

		#region List Overrides
		Collection.IList<GradientStop> stops = new Kean.Collection.Sorted.List<GradientStop>();
		public override int Count { get { return this.stops.Count; } }
		public override GradientStop this[int index]
		{
			get { return this.stops[index]; }
			set { this.stops[index] = value; }
		}
		public override Collection.IList<GradientStop> Add(GradientStop item)
		{
			return this.stops.Add(item);
		}
		public override Collection.IList<GradientStop> Insert(int index, GradientStop item)
		{
			return this.stops.Insert(index, item);
		}
		public override GradientStop Remove()
		{
			return this.stops.Remove();
		}
		public override GradientStop Remove(int index)
		{
			return this.stops.Remove(index);
		}
		#endregion

		public Gradient()
		{ }
	}
}
