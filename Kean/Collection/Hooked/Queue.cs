//
//  Queue.cs
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
	public class Queue<T> :
		IQueue<T>
	{
		Collection.IQueue<T> data;
		public event Func<T, bool> OnEnqueue;
		public event Action<T> Enqueued;
		public event Func<T, bool> OnDequeue;
		public event Action<T> Dequeued;
		public Queue() :
			this(new Collection.Queue<T>())
		{ }
		public Queue(Collection.IQueue<T> data)
		{
			this.data = data;
		}
		#region Collection.IQueue<T> Members
		public bool Empty
		{
			get { return this.data.Empty; }
		}
		public int Count
		{
			get { return this.data.Count; }
		}
		public Collection.IQueue<T> Enqueue(T item)
		{
			if (this.OnEnqueue.AllTrue(item))
			{
				this.data.Enqueue(item);
				this.Enqueued.Call(item);
			}
			return this;
		}
		public T Peek()
		{
			return this.data.Peek();
		}
		public T Dequeue()
		{
			T result;
			if (this.OnDequeue.AllTrue(this.Peek()))
			{
				result = this.data.Dequeue();
				this.Dequeued.Call(result);
			}
			else
				result = default(T);
			return result;
		}
		#endregion
	}
}
