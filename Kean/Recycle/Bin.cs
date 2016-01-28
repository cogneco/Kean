// 
//  Bin.cs
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

using Collection = Kean.Collection;
using Kean.Collection.Extension;
using Kean.Extension;
using System;

namespace Kean.Recycle
{
	public class Bin<T, S> :
		Synchronized,
		IBin<T, S>
	{
		public int Capacity { get; set; }
		Collection.IList<T> recycled;
		Func<T, S, bool> comparer;
		Action<T> free;
		public Bin(int capacity, Func<T, S, bool> comparer, Action<T> free)
		{
			this.Capacity = capacity;
			this.comparer = comparer;
			this.free = free;
			this.recycled = new Collection.Synchronized.List<T>(new Collection.List<T>(this.Capacity), this.Lock);
		}
		public T Find(S specifier)
		{
			T result;
			lock (this.Lock)
			{
				result = this.recycled.RemoveFirst(item => this.comparer(item, specifier));
				while (this.recycled.Count >= this.Capacity)
					this.free.Call(this.recycled.Remove(this.recycled.Count - 1));
			}
			return result;
		}
		public void Recycle(T item)
		{
			if (item.NotNull())
				this.recycled.Add(item);
		}
		public void Free()
		{
			while (this.recycled.Count > 0)
				this.free.Call(this.recycled.Remove());
		}
	}
}
