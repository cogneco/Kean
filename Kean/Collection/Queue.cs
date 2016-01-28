// 
//  Queue.cs
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
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.using System;
using System;

namespace Kean.Collection
{
	public class Queue<T> :
		IQueue<T>
	{
		Linked.Queue<T> data;
		#region Constructors
		public Queue()
		{
			this.data = new Linked.Queue<T>();
		}
		#endregion

		#region IQueue<T> Members
		public bool Empty
		{
			get { return this.data.Empty; }
		}
		public int Count
		{
			get { return this.data.Count; }
		}
		public IQueue<T> Enqueue(T item)
		{
			return this.data.Enqueue(item);
		}
		public T Peek()
		{
			return this.data.Peek();
		}
		public T Dequeue()
		{
			return this.data.Dequeue();
		}
		#endregion
	}
}
