// 
//  Surface.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2012-2013 Simon Mika
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
	public class Surface :
		Draw.Surface
	{
		public Element.Group Root { get; private set; }
		internal Surface(Geometry2D.Integer.Size size, CoordinateSystem coordinateSystem) :
			base(size, coordinateSystem)
		{
			this.Root = new Element.Group();
		}
		internal Surface(Image image) :
			this(image.Size, image.CoordinateSystem)
		{ }
		Surface(Surface original) :
			base(original)
		{
			this.Root = original.Root.Copy() as Element.Group;
		}
		public override Draw.Image Convert(Draw.Image image)
		{
			return null;
		}
		public override void Draw(Map map, Draw.Image image, Geometry2D.Single.Box source, Geometry2D.Single.Box destination)
		{
			this.Root.Add(new Element.Image(map, image, source, destination));
		}
		public override void Draw(IPaint fill, Stroke stroke, Path path)
		{
			this.Root.Add(new Element.Path(fill, stroke, path));
		}
		public override void Draw(IPaint fill, Stroke stroke, Text text, Geometry2D.Single.Point position)
		{
			this.Root.Add(new Element.Text(fill, stroke, text, position));
		}
		public override void Blend(float factor)
		{
			throw new System.NotImplementedException();
		}
		public override void Clear()
		{
			this.Root = new Element.Group();
		}
		public override void Clear(Geometry2D.Single.Box region)
		{
			throw new System.NotImplementedException();
		}
		public override T Read<T>()
		{
			return null;
		}
	}
}
