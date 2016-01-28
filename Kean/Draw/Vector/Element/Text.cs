// 
//  Text.cs
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
	public class Text :
		Paintable
	{
		public Draw.Text Data { get; set; }
		public Geometry2D.Single.Point Position { get; set; }
		public Text(IPaint fill, Stroke stroke, Draw.Text text, Geometry2D.Single.Point position) :
			base(fill, stroke)
		{
			this.Data = text;
			this.Position = position;
		}
		protected Text(Text original) :
			base(original)
		{
			this.Data = original.Data;
			this.Position = original.Position;
		}
		public override Abstract Copy()
		{
			return new Text(this);
		}
		internal override void Render(Draw.Canvas target)
		{
			base.Render(target);
			target.Draw(this.Fill, this.Stroke, this.Data, this.Position);
		}
	}
}
