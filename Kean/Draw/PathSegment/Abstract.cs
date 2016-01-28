// 
//  Abstract.cs
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
	public abstract class Abstract
	{
		Abstract previous;
		public Abstract Previous 
		{
			get { return this.previous; }
			set 
			{ 
				this.previous = value;
				if (this.previous.NotNull() && this.previous.Next != this)
					this.previous.Next = this;
			}
		}
		Abstract next;
		public Abstract Next
		{
			get { return this.next; }
			set
			{
				this.next = value;
				if (this.next.NotNull() && this.next.Previous != this)
					this.next.Previous = this;
			}
		}
		protected virtual Geometry2D.Single.Point SubpathStart 
		{ 
			get { return this.Previous.NotNull() ? this.Previous.SubpathStart : new Geometry2D.Single.Point(); }
			set { if (this.Previous.NotNull()) this.Previous.SubpathStart = value; }
		}
		public virtual Geometry2D.Single.Point End { get; set; }
		public Geometry2D.Single.Point Start { get { return this.Previous.NotNull() ? this.Previous.End : new Geometry2D.Single.Point(); } }

        public abstract string String { get; }
        protected Abstract(Geometry2D.Single.Point end)
		{
			this.End = end;
		}

		protected abstract Geometry2D.Single.Box SegmentBounds(Geometry2D.Single.Transform transform);
		public Geometry2D.Single.Box Bounds(Geometry2D.Single.Transform transform)
		{
			Geometry2D.Single.Box result = this.SegmentBounds(transform);
			if (this.Next.NotNull())
				result = result.Union(this.Next.Bounds(transform));
			return result;
		}

		public void Append(Abstract segment)
		{
			if (this.Next.IsNull())
				this.Next = segment;
			else
				this.Next.Append(segment);
		}
		public void Append(string path)
		{
			this.Append(Abstract.Parse(path));
		}
        public override string ToString()
        {
            return this.String + " " + (this.Next.NotNull() ? this.Next.ToString() : "");
        }
        public static Abstract Parse(string path)
        {
			return new Parser().Parse(path);
        }
    }
}
