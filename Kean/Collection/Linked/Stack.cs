// 
//  LinkedStack.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2009 Simon Mika
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
using Kean.Collection.Linked.Extension;

namespace Kean.Collection.Linked
{
	public class Stack<T> :
		Stack<Link<T>, T>
	{
		public Stack () { }
	}
	public class Stack<L, T> :
		IStack<T>
		where L : class, ILink<L, T>, new()
	{
		L top;
		public bool Empty { get { return this.top == null; } }
		public Stack() { }
		public Collection.IStack<T> Push(T item)
		{
			this.top = this.top.Add(item);
			return this;
		}
		public T Pop()
		{
			try
			{
				T result = this.top.Head;
				this.top = this.top.Tail;
				return result;
			}
			catch (NullReferenceException e) { throw new Exception.Empty(e); }
		}
		public T Peek()
		{
			try { return this.top.Head; } catch (NullReferenceException e) { throw new Exception.Empty(e); }
		}
	}
}
