// 
//  ListQueue.cs
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

namespace Kean.Collection.Wrap
{
	public class ListQueue<T> :
		IQueue<T>
	{
		IList<T> data;
		int head;
		int tail;
		int size;
		public ListQueue(IList<T> data)
		{
			this.data = data;
		}
		#region IQueue<T>
		public bool Empty { get { return this.size == 0; } }
		public int Count { get { return this.size; } }
		public IQueue<T> Enqueue(T item)
		{
			if (this.size == this.data.Count)
			{
				this.data.Insert(this.head, item);
				if (this.head > this.tail)
					this.head++;
				this.tail = (this.tail + 1) % this.data.Count;
			}
			else
			{
				this.head = (this.head - 1 + this.data.Count) % this.data.Count;
				this.data[this.head] = item;
			}
			this.size++;
			return this;
		}
		public T Peek()
		{
			return this.size == 0 ? default(T) : this.data[this.tail];
		}
		public T Dequeue()
		{
			T result = this.Peek();
			// let garbage collector do its job
			if (this.data.Count > 0)
			{
				this.data[this.tail] = default(T);
				this.tail = (this.tail - 1 + this.data.Count) % this.data.Count;
				this.size--;
			}
			return result;
		}
		#endregion
	}
}
