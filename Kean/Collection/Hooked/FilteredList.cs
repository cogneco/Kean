//
//  WrapedList.cs
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
using Kean.Extension;

namespace Kean.Collection.Hooked
{
	public class FilteredList<T> :
		Abstract.List<T>,
		IList<T>
	{
		IList<T> data;
		Collection.IList<T> filter;

		#region Constructors
		public FilteredList(IList<T> data, Collection.IList<T> filter)
		{
			this.filter = filter;
			this.data = data;
		}
		#endregion
		#region Hooked.IVector<T> Members
		public event Func<int, T, T, bool> OnReplace
		{
			add { this.data.OnReplace += value; }
			remove { this.data.OnReplace -= value; }
		}
		public event Action<int, T, T> Replaced
		{
			add { this.data.Replaced += value; }
			remove { this.data.Replaced -= value; }
		}
		#endregion
		#region Hooked.IList<T> Members
		public event Action<int, T> Added
		{
			add { this.data.Added += value; }
			remove { this.data.Added -= value; }
		}
		public event Func<int, T, bool> OnAdd
		{
			add { this.data.OnAdd += value; }
			remove { this.data.OnAdd -= value; }
		}
		public event Func<int, T, bool> OnRemove
		{
			add { this.data.OnRemove += value; }
			remove { this.data.OnRemove -= value; }
		}
		public event Action<int, T> Removed
		{
			add { this.data.Removed += value; }
			remove { this.data.Removed -= value; }
		}
		#endregion

		#region IVector<T> Members
		public override int Count
		{
			get { return this.filter.Count; }
		}
		public override T this[int index]
		{
			get { return this.filter[index]; }
			set { this.filter[index] = value; }
		}
		#endregion
		#region IList<T> Members
		public override Collection.IList<T> Add(T item)
		{
			this.filter.Add(item);
			return this;
		}
		public override T Remove()
		{
			return this.filter.Remove();
		}
		public override Collection.IList<T> Insert(int index, T item)
		{
			this.filter.Insert(index, item);
			return this;
		}
		public override T Remove(int index)
		{
			return this.filter.Remove(index);
		}
		#endregion
	}
}
