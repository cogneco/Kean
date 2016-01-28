// 
//  Array.cs
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

namespace Kean.Collection.Synchronized
{
	public class Vector<T> :
		Abstract.Vector<T>
	{
		protected object Lock { get; private set; }
		IVector<T> data;
		#region Constructor
		public Vector() :
			this(new Collection.Vector<T>())
		{ }
		public Vector(IVector<T> data) :
			this(data, new object())
		{ }
		public Vector(IVector<T> data, object @lock)
		{
			this.data = data;
			this.Lock = @lock;
		}
		#endregion
		#region IVector<T>
		public override int Count { get { lock (this.Lock) return this.data.Count; } }
		public override T this[int index] 
		{
			get { lock (this.Lock) return this.data[index]; }
			set { lock (this.Lock) this.data[index] = value; }
		}
		#endregion
	}

}
