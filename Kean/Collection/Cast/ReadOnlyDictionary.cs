// 
//  ReadOnlyDictionary.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2012-2013 Simon Mika
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
using Kean;
using Kean.Extension;

namespace Kean.Collection.Cast
{
	public class ReadOnlyDictionary<TKey, T, S> :
		Abstract.ReadOnlyDictionary<TKey, S>
	{
		Func<T, S> cast;
		IReadOnlyDictionary<TKey, T> data;
		#region Constructors
		public ReadOnlyDictionary(Func<T, S> cast, IReadOnlyDictionary<TKey, T> data)
		{
			this.cast = cast;
			this.data = data;
		}
		#endregion

		#region IReadOnlyDictionary<TKey,TValue> Members
		public override S this[TKey key] { get { return this.cast(this.data[key]); } }
		public override bool Contains(TKey key) { return this.data.Contains(key); }
		#endregion
		#region IEnumerable<KeyValue<TKey,TValue>> Members
		public override System.Collections.Generic.IEnumerator<KeyValue<TKey, S>> GetEnumerator()
		{
			foreach (KeyValue<TKey, T> item in this.data)
				yield return KeyValue.Create(item.Key, this.cast(item.Value));
		}
		#endregion
	}
}