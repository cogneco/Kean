// 
//  Merge.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2009-2011 Simon Mika
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
namespace Kean.Collection
{
	public class Merge<T> :
		Abstract.Vector<T>
	{
		IVector<T> left;
		IVector<T> right;
		public override int Count { get { return this.left.Count + this.right.Count; } }
		public override T this[int index]
		{
			get
			{
				try
				{
					int leftCount = this.left.Count;
					return index < leftCount ? this.left[index] : this.right[index - leftCount];
				}
				catch (IndexOutOfRangeException e)
				{
					throw new Exception.InvalidIndex(e);
				}
			}
			set
			{
				try
				{
					int leftCount = this.left.Count;
					if (index < leftCount)
						this.left[index] = value;
					else
						this.right[index - leftCount] = value;
				}
				catch (IndexOutOfRangeException e)
				{
					new Exception.InvalidIndex(e).Throw();
				}
			}
		}
		public Merge(T[] left, T[] right) :
			this((Vector<T>)left, (Vector<T>)right)
		{ }
		public Merge(IVector<T> left) :
			this(left, new Vector<T>(0))
		{ }
		public Merge(IVector<T> left, IVector<T> right)
		{
			this.left = left ?? new Vector<T>(0);
			this.right = right ?? new Vector<T>(0);
		}
		public static implicit operator Merge<T>(T[] value)
		{
			return new Merge<T>(value, new T[0]);
		}
		public static Merge<T> operator +(Merge<T> left, IVector<T> right)
		{
			return new Merge<T>(left, right);
		}
		public static Merge<T> operator +(IVector<T> left, Merge<T> right)
		{
			return new Merge<T>(left, right);
		}
	}
}
