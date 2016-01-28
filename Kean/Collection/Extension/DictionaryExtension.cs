// 
//  DictionaryExtension.cs
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

namespace Kean.Collection.Extension
{
	public static class DictionaryExtension
	{
		public static IDictionary<TKey, TValue> Update<TKey, TValue>(this IDictionary<TKey, TValue> me, System.Collections.Generic.IEnumerable<KeyValue<TKey, TValue>> items)
			where TKey : IEquatable<TKey>
		{
			foreach (KeyValue<TKey, TValue> item in items)
				me[item.Key] = item.Value;
			return me;
		}
		public static IDictionary<TKey, TValue> Update<TKey, TValue>(this IDictionary<TKey, TValue> me, params KeyValue<TKey, TValue>[] items)
			where TKey : IEquatable<TKey>
		{
			return me.Update<TKey, TValue>((System.Collections.Generic.IEnumerable<KeyValue<TKey,TValue>>) items);
		}
	}
}
