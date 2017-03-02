// Copyright (C) 2012, 2016  Simon Mika <simon@mika.se>
//
// This file is part of Kean.
//
// Kean is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Kean is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with Kean.  If not, see <http://www.gnu.org/licenses/>.
//

using System;
using Generic = System.Collections.Generic;
using Tasks = System.Threading.Tasks;
using Kean.Extension;

namespace Kean.IO.Extension
{
	public static class TextWriterExtenion
	{
		public static Tasks.Task<bool> Write(this ITextWriter me, Generic.IEnumerable<char> buffer)
		{
			return me.Write(buffer.GetEnumerator());
		}
		public static Tasks.Task<bool> Write(this ITextWriter me, params char[] buffer)
		{
			return me.Write((Generic.IEnumerable<char>)buffer);
		}
		public static Tasks.Task<bool> Write(this ITextWriter me, string value)
		{
			return me.Write((Generic.IEnumerable<char>)value);
		}
		public static Tasks.Task<bool> Write<T>(this ITextWriter me, T value) where T : IConvertible
		{
			return me.Write(value.ToString((IFormatProvider)System.Globalization.CultureInfo.InvariantCulture.GetFormat(typeof(T))));
		}
		public static Tasks.Task<bool> Write(this ITextWriter me, string format, params object[] arguments)
		{
			return me.Write(string.Format(format, arguments));
		}
		public static Tasks.Task<bool> WriteLine(this ITextWriter me)
		{
			return me.Write('\n'); // The newline characters are converted by bool Write(this ITextWriter me, Generic.IEnumerable<char> buffer)
		}
		public static Tasks.Task<bool> WriteLine(this ITextWriter me, params char[] buffer)
		{
			return me.WriteLine((Generic.IEnumerable<char>)buffer);
		}
		public static Tasks.Task<bool> WriteLine(this ITextWriter me, string value)
		{
			return me.WriteLine((Generic.IEnumerable<char>)value);
		}
		public static Tasks.Task<bool> WriteLine<T>(this ITextWriter me, T value) where T : IConvertible
		{
			return me.WriteLine(value.ToString((IFormatProvider)System.Globalization.CultureInfo.InvariantCulture.GetFormat(typeof(T))));
		}
		public static Tasks.Task<bool> WriteLine(this ITextWriter me, string format, params object[] arguments)
		{
			return me.WriteLine(string.Format(format, arguments));
		}
		public static Tasks.Task<bool> WriteLine(this ITextWriter me, Generic.IEnumerable<char> value)
		{
			return me.WriteLine(value.GetEnumerator());
		}
		public static async Tasks.Task<bool> WriteLine(this ITextWriter me, Generic.IEnumerator<char> value)
		{
			return await me.Write(value) && await me.WriteLine();
		}
		public static async Tasks.Task<bool> Join(this ITextWriter me, Generic.IEnumerator<Func<ITextWriter, Tasks.Task<bool>>> items, Func<ITextWriter, Tasks.Task<bool>> separator = null)
		{
			bool result = items.MoveNext() && await items.Current(me);
			while (result && items.MoveNext())
			{
				if (separator.NotNull())
					result &= await separator(me);
				result &= await items.Current(me);
			}
			return result;
		}
		public static Tasks.Task<bool> Join(this ITextWriter me, Generic.IEnumerable<Func<ITextWriter, Tasks.Task<bool>>> items, Func<ITextWriter, Tasks.Task<bool>> separator = null)
		{
			return me.Join(items?.GetEnumerator(), separator);
		}
		public static Tasks.Task<bool> Join(this ITextWriter me, Generic.IEnumerator<Func<ITextWriter, Tasks.Task<bool>>> items, string separator = null)
		{
			return me.Join(items, separator.NotNull() ? (Func<ITextWriter, Tasks.Task<bool>>)(writer => writer.Write(separator)) : null);
		}
		public static Tasks.Task<bool> Join(this ITextWriter me, Generic.IEnumerable<Func<ITextWriter, Tasks.Task<bool>>> items, string separator = null)
		{
			return me.Join(items?.GetEnumerator(), separator);
		}
		public static async Tasks.Task<bool> Join(this ITextWriter me, Generic.IEnumerator<string> items, string separator = null)
		{
			bool result = items.MoveNext() && await me.Write(items.Current);
			while (result && items.MoveNext())
			{
				if (separator.NotNull())
					result &= await me.Write(separator);
				result &= await me.Write(items.Current);
			}
			return result;
		}
		public static Tasks.Task<bool> Join(this ITextWriter me, Generic.IEnumerable<string> items, string separator = null)
		{
			return me.Join(items?.GetEnumerator(), separator);
		}
	}
}
