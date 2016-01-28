// 
//  Canvas.cs
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

namespace Kean.Draw.Vector
{
	public class Canvas :
		Draw.Canvas
	{
		public Element.Group Root { get; private set; }
		internal Canvas(Image image) :
			base(new Surface(image), image)
		{
			this.Root = new Element.Group();
		}
		Canvas(Image image, Canvas original) :
			base(new Surface(image), image)
		{
			this.Root = original.Root.Copy() as Element.Group;
		}
		internal Canvas Copy(Image image)
		{
			return new Canvas(image, this);
		}
		internal void Render(Draw.Canvas target)
		{
			this.Root.Render(target);
		}
		public override Draw.Canvas CreateSubcanvas(Geometry2D.Single.Box bounds)
		{
			Canvas result = new Canvas(this.Image as Image);
			this.Root.Add(result.Root);
			return result;
		}
	}
}
