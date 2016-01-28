// 
//  ReadOnlyMerge.cs
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
	public class ReadOnlyMerge<T> :
		Abstract.ReadOnlyVector<T>
	{
		IReadOnlyVector<T> left;
		IReadOnlyVector<T> right;
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
		}
		public ReadOnlyMerge(T[] left, T[] right) :
			this((ReadOnlyVector<T>)left, (ReadOnlyVector<T>)right)
		{ }
		public ReadOnlyMerge(IReadOnlyVector<T> left) :
			this(left, new ReadOnlyVector<T>(new T[0]))
		{ }
		public ReadOnlyMerge(IReadOnlyVector<T> left, IReadOnlyVector<T> right)
		{
			this.left = left;
			this.right = right;
		}
		public static implicit operator ReadOnlyMerge<T>(T[] value)
		{
			return new ReadOnlyMerge<T>(value, new T[0]);
		}
		public static ReadOnlyMerge<T> operator +(ReadOnlyMerge<T> left, IReadOnlyVector<T> right)
		{
			return new ReadOnlyMerge<T>(left, right);
		}
		public static ReadOnlyMerge<T> operator +(IReadOnlyVector<T> left, ReadOnlyMerge<T> right)
		{
			return new ReadOnlyMerge<T>(left, right);
		}
	}
}
