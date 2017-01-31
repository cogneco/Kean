//
//  List.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2010 Simon Mika
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
	public class List<T> :
		Vector<T>,
		IList<T>
	{
		Collection.IList<T> data;
		public event Action<int, T> Added;
		public event Func<int, T, bool> OnAdd;
		public event Action<int, T> Removed;
		public event Func<int, T, bool> OnRemove;
		public List() :
			this(new Collection.List<T>())
		{ }
		public List(int count) :
			this(new Collection.List<T>(count))
		{ }
		public List(params T[] items) :
			this(new Collection.List<T>(items))
		{ }
		public List (Collection.IList<T> data) :
			base(data)
		{
			this.data = data;
		}
		public Collection.IList<T> Add(T item)
		{
			if (this.OnAdd.AllTrue(this.Count, item)) 
			{
				this.data.Add(item);
				this.Added.Call(this.Count, item);
			}
			return this;
		}
		public T Remove()
		{
			T result = this.data[this.Count - 1];
			if (this.OnRemove.AllTrue(this.Count - 1, result)) 
			{
				result = this.data.Remove();
				this.Removed.Call(this.Count - 1, result);
			} else
				result = default(T);
			return result;
		}
		public Collection.IList<T> Insert(int index, T item)
		{
			if (this.OnAdd.AllTrue(index, item))
			{
				this.data.Insert(index, item);
				this.Added.Call(index, item);
			}
			return this;
		}
		public T Remove(int index)
		{
			T result = this.data[index];
			if (this.OnRemove.AllTrue(index, result))
			{
				result = this.data.Remove(index);
				this.Removed.Call(index, result);
			} else
				result = default(T);
			return result;
		}
	}
}

