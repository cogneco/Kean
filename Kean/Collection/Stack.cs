// 
//  Stack.cs
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
using Kean.Extension;

namespace Kean.Collection
{
	public class Stack<T> :
		IStack<T>
	{
		Linked.Stack<T> data;
		#region Constructors
		public Stack()
		{
			this.data = new Linked.Stack<T>();
		}
		#endregion
		#region IStack<T> Members
		public bool Empty
		{
			get { return this.data.Empty; }
		}
		public Collection.IStack<T> Push(T item)
		{
			return this.data.Push(item);
		}
		public T Pop()
		{
			return this.data.Pop();
		}
		public T Peek()
		{
			return this.data.Peek();
		}
		#endregion
	}
}
