// 
//  Slice.cs
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
	public class Slice<T> :
		Abstract.Vector<T>
	{
		int offset;
		int count;
		IVector<T> data;
		public override int Count { get { return this.count; } }
		public override T this [int index]
		{
			get
			{
				if (index >= count)
					throw new Exception.InvalidIndex();
				try
				{
					return this.data[this.offset + index];
				}
				catch (IndexOutOfRangeException e)
				{
					throw new Exception.InvalidIndex(e);
				}
			}
			set
			{
				if (index >= count)
					new Exception.InvalidIndex().Throw();
				try
				{
					this.data[this.offset + index] = value;
				}
				catch (IndexOutOfRangeException e)
				{
					new Exception.InvalidIndex(e).Throw();
				}
			}
		}
		public Slice(T[] data, int offset) :
			this(data, offset, data.Length - offset)
		{
		}
		public Slice(T[] data, int offset, int count) :
			this((Vector<T>)data, offset, count)
		{
		}
		public Slice(IVector<T> data, int offset) :
			this(data, offset, data.Count - offset)
		{
		}
		public Slice(IVector<T> data, int offset, int count)
		{
			if (count < 0)
				count = data.Count + count;
			if (data.Count < offset + count)
				new Exception.InvalidIndex().Throw();
			this.offset = offset;
			this.count = count;
			this.data = data;
		}
		public static explicit operator Slice<T>(T[] array)
		{
			return new Slice<T>(array, 0, array.Length);
		}
		public static explicit operator T[](Slice<T> slice)
		{
			T[] result = new T[slice.Count];
			if (slice.data is Vector<T>)
				System.Array.Copy((T[])(slice.data as Vector<T>), slice.offset, result, 0, slice.Count);
			else if (slice.data is Array.Vector<T>)
				System.Array.Copy((T[])(slice.data as Array.Vector<T>), slice.offset, result, 0, slice.Count);
			else
				for (int i = 0; i < slice.Count; i++)
					result[i] = slice[i];
			return result;
		}
	}
}
