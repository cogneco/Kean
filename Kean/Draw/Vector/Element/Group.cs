// 
//  Group.cs
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
using Kean.Collection.Extension;

namespace Kean.Draw.Vector.Element
{
	public class Group :
		Abstract,
		Collection.IList<Abstract>
	{
		Collection.List<Abstract> elements = new Collection.List<Abstract>();
		public Group()
		{ }
		public Group(params Abstract[] elements)
		{
			this.elements.Add(elements);
		}
		protected Group(Group original) :
			base(original)
		{
			this.elements.Add(original.Map(e => e.Copy()));
		}
		public override Abstract Copy()
		{
			return new Group(this);
		}
		internal override void Render(Draw.Canvas target)
		{
			base.Render(target);
			this.elements.Apply(e => e.Render(target));
		}

		#region IList<Abstract> Members
		public Collection.IList<Abstract> Add(Abstract item)
		{
			this.elements.Add(item);
			return this;
		}
		public Abstract Remove()
		{
			return this.elements.Remove();
		}
		public Collection.IList<Abstract> Insert(int index, Abstract item)
		{
			this.elements.Insert(index, item);
			return this;
		}
		public Abstract Remove(int index)
		{
			return this.elements.Remove(index);
		}
		#endregion

		#region IVector<Abstract> Members
		public int Count { get { return this.elements.Count; } }
		public Abstract this[int index] 
		{ 
			get { return this.elements[index]; }
			set { this.elements[index] = value; }
		}
		#endregion

		#region IEnumerable<Abstract> Members
		public System.Collections.Generic.IEnumerator<Abstract> GetEnumerator()
		{
			return (this.elements as System.Collections.Generic.IEnumerable<Abstract>).GetEnumerator();
		}
		#endregion

		#region IEnumerable Members
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
		#endregion

		#region IEquatable<IVector<Abstract>> Members
		public bool Equals(Kean.Collection.IVector<Abstract> other)
		{
			return this.elements.Equals(other);
		}
		#endregion
	}
}
