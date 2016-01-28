// 
//  IDictonary.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2009 - 2011 Simon Mika
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
namespace Kean.Collection
{
	public interface IDictionary<TKey, TValue> :
		System.Collections.Generic.IEnumerable<KeyValue<TKey, TValue>>,
		IEquatable<IDictionary<TKey, TValue>>
	{
		TValue this[TKey key] { get; set; }
		bool Contains(TKey key);
		bool Remove(TKey key);
	}
}

