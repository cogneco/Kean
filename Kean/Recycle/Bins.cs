// 
//  Bins.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2012 Simon Mika
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

using Collection = Kean.Collection;
using Kean.Collection.Extension;
using Kean.Extension;
using System;

namespace Kean.Recycle
{
	public class Bins<T, S> : 
		IBin<T, S>
	{
		Func<T, int> recycleIndex;
		Func<S, int> findIndex;
		IBin<T, S>[] bins;
		public int Capacity 
		{
			get { return this.bins[0].Capacity; }
			set { this.bins.Apply(bin => bin.Capacity  = value); }
		}
		public Bins(int capacity, int categoryCount, Func<T, S, bool> comparer, Action<T> free, Func<T, int> recycleIndex, Func<S, int> findIndex)
		{
			this.recycleIndex = recycleIndex;
			this.findIndex = findIndex;
			this.bins = new IBin<T, S>[categoryCount].Initialize(() => new Bin<T, S>(capacity, comparer, free));
		}
		public T Find(S specifier)
		{
			return this.bins[this.findIndex(specifier)].Find(specifier);
		}
		public void Recycle(T item)
		{
			this.bins[this.recycleIndex(item)].Recycle(item);
		}
		public void Free()
		{
			this.bins.Apply(bin => bin.Free());
		}
	}
}
