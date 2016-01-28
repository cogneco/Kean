//
//  Stack.cs
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
	public class Stack<T> :
		IStack<T>
	{
		Collection.IStack<T> data;
		public event Func<T, bool> OnPush;
		public event Action<T> Pushed;
		public event Func<T, bool> OnPop;
		public event Action<T> Poped;
		public Stack() :
			this(new Collection.Stack<T>())
		{ }
		public Stack(Collection.IStack<T> data)
		{
			this.data = data;
		}
		#region Collection.IStack<T> Members
		public bool Empty { get { return this.data.Empty; } }

		public Collection.IStack<T> Push(T item)
		{
			if (this.OnPush.AllTrue(item))
			{
				this.data.Push(item);
				this.Pushed.Call(item);
			}
			return this;
		}

		public T Pop()
		{
			T result;
			if (this.OnPop.AllTrue(this.Peek()))
			{
				result = this.data.Pop();
				this.Poped.Call(result);
			}
			else
				result = default(T);
			return result;
		}

		public T Peek()
		{
			return this.data.Peek();
		}
		#endregion
	}
}
