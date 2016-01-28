// 
//  ReadOnlyVector.cs
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

namespace Kean.Collection.Cast
{
	public class ReadOnlyVector<T, S> :
		Abstract.ReadOnlyVector<S>
	{
		protected Func<T, S> Cast { get; private set; }
		protected IReadOnlyVector<T> Data { get; private set; }

		public ReadOnlyVector(Func<T, S> cast, IReadOnlyVector<T> data)
		{
			this.Cast = cast;
			this.Data = data;
		}

		#region IReadOnlyVector<S> Members
		public override S this[int index]
		{
			get { return this.Cast(this.Data[index]); }
		}
		public override int Count
		{
			get { return this.Data.Count; }
		}
		#endregion
	}
}
