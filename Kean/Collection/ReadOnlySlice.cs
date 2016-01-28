// 
//  ReadOnlySlice.cs
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
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;

namespace Kean.Collection
{
	public class ReadOnlySlice<T> :
		Abstract.ReadOnlyVector<T>,
		IReadOnlyVector<T>
	{
		int offset;
		int count;
		IReadOnlyVector<T> data;
		#region Constructor
		public ReadOnlySlice(T[] data, int offset, int count) :
			this((ReadOnlyVector<T>)data, offset, count)
		{ }
		public ReadOnlySlice(IReadOnlyVector<T> data, int offset, int count)
		{
			if (data.Count < offset + count)
				new Exception.InvalidIndex().Throw();
			this.offset = offset;
			this.count = count;
			this.data = data;
		}
		#endregion
		#region IReadOnlyVector<T>
		public override int Count { get { return this.count; } }
		public override T this[int index]
		{
			get
			{
				if (index >= this.count)
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
		}
		#endregion
	}
}
