//
//  CharacterInDeviceExtension.cs
//
//  Author:
//       Simon Mika <smika@hx.se>
//
//  Copyright (c) 2013-2014 Simon Mika
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
using Collection = Kean.Collection;
using Kean.Collection.Extension;
using Uri = Kean.Uri;
using Generic = System.Collections.Generic;

namespace Kean.IO.Extension
{
	public static class CharacterInDeviceExtension
	{
		#region Skip
		public static T Skip<T> (this T me, int count)
			where T : ICharacterInDevice
		{
			while (count > 0 && me.Read().HasValue)
				count--;
			return me;
		}
		public static T Skip<T> (this T me, string separator)
			where T : ICharacterInDevice
		{
			return me.Skip(separator.ToCharArray());
		}
		public static T Skip<T> (this T me, params char[] separator)
			where T : ICharacterInDevice
		{
			char? next;
			int position = 0;
			while ((next = me.Read()).HasValue)
			{
				if (next != separator[position++])
					position = 0;
				else if (separator.Length == position)
					break;
			}
			return me;
		}
		#endregion
		#region Read
		public static Generic.IEnumerable<char> Read (this ICharacterInDevice me, int count)
		{
			char? next;
			while (count-- > 0 && (next = me.Read()).HasValue)
				yield return next.Value;
		}
		public static Generic.IEnumerable<char> Read (this ICharacterInDevice me, string separator)
		{
			return me.Read(separator.ToCharArray());
		}
		public static Generic.IEnumerable<char> Read (this ICharacterInDevice me, params char[] separator)
		{
			char? next;
			int position = 0;
			while ((next = me.Read()).HasValue)
			{
				if (next != separator[position++])
					position = 0;
				else if (separator.Length == position)
					break;
				yield return next.Value;
			}
		}
		#endregion
		public static Generic.IEnumerable<char> AsEnumerable (this ICharacterInDevice me)
		{
			char? next;
			while ((next = me.Read()).HasValue)
				yield return next.Value;
		}
		public static Generic.IEnumerator<char> AsEnumerator (this ICharacterInDevice me)
		{
			char? next;
			while ((next = me.Read()).HasValue)
				yield return next.Value;
		}
	}
}
