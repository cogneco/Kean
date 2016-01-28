// 
//  Path.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2012 Simon Mika
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

using Kean;
using Kean.Extension;
using Geometry2D = Kean.Math.Geometry2D;
using Collection = Kean.Collection;

namespace Kean.Draw.Vector.Element
{
	public class Path :
		Paintable
	{
		public Draw.Path Data { get; set; }
		public Path(IPaint fill, Stroke stroke, Draw.Path path) :
			base(fill, stroke)
		{
			this.Data = path;
		}
		protected Path(Path original) :
			base(original)
		{
			this.Data = original.Data;
		}
		public override Abstract Copy()
		{
			return new Path(this);
		}
		internal override void Render(Draw.Canvas target)
		{
			base.Render(target);
			target.Draw(this.Fill, this.Stroke, this.Data);
		}
	}
}
