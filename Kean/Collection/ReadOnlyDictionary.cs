// 
//  ReadOnlyDictionary.cs
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

using System;
using Kean;
using Kean.Extension;

namespace Kean.Collection
{
	public class ReadOnlyDictionary<TKey, TValue> :
		Abstract.ReadOnlyDictionary<TKey, TValue>
	{
		IDictionary<TKey, TValue> data;
		#region Constructors
		public ReadOnlyDictionary(IDictionary<TKey, TValue> data)
		{
			this.data = data;
		}
		public ReadOnlyDictionary() :
			this(new Dictionary<TKey, TValue>())
		{ }
		public ReadOnlyDictionary(System.Collections.Generic.IEnumerable<KeyValue<TKey, TValue>> data) :
			this()
		{
			foreach (KeyValue<TKey, TValue> element in data)
				this.data[element.Key] = element.Value;
		}
		public ReadOnlyDictionary(params KeyValue<TKey, TValue>[] data) :
			this((System.Collections.Generic.IEnumerable<KeyValue<TKey, TValue>>)data)
		{ }
		#endregion

		#region IReadOnlyDictionary<TKey,TValue> Members
		public override TValue this[TKey key] { get { return this.data[key]; } }
		public override bool Contains(TKey key) { return this.data.Contains(key); }
		#endregion
		#region IEnumerable<KeyValue<TKey,TValue>> Members
		public override System.Collections.Generic.IEnumerator<KeyValue<TKey, TValue>> GetEnumerator()
		{
			return this.data.GetEnumerator();
		}
		#endregion
	}
}