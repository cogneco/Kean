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

namespace Kean.IO.Extension
{
	public static class TextWriterExtenion
	{
		public static bool Write(this ITextWriter me, Generic.IEnumerable<char> buffer)
		{
			return me.Write(buffer.GetEnumerator());
		}
		public static bool Write(this ITextWriter me, params char[] buffer)
		{
			return me.Write((Generic.IEnumerable<char>)buffer);
		}
		public static bool Write(this ITextWriter me, string value)
		{
			return me.Write((Generic.IEnumerable<char>)value);
		}
		public static bool Write<T>(this ITextWriter me, T value) where T : IConvertible
		{
			return me.Write(value.ToString((IFormatProvider)System.Globalization.CultureInfo.InvariantCulture.GetFormat(typeof(T))));
		}
		public static bool Write(this ITextWriter me, string format, params object[] arguments)
		{
			return me.Write(string.Format(format, arguments));
		}
		public static bool WriteLine(this ITextWriter me)
		{
			return me.Write('\n'); // The newline characters are converted by bool Write(this ITextWriter me, Generic.IEnumerable<char> buffer)
		}
		public static bool WriteLine(this ITextWriter me, params char[] buffer)
		{
			return me.WriteLine((Generic.IEnumerable<char>)buffer);
		}
		public static bool WriteLine(this ITextWriter me, string value)
		{
			return me.WriteLine((Generic.IEnumerable<char>)value);
		}
		public static bool WriteLine<T>(this ITextWriter me, T value) where T : IConvertible
		{
			return me.WriteLine(value.ToString((IFormatProvider)System.Globalization.CultureInfo.InvariantCulture.GetFormat(typeof(T))));
		}
		public static bool WriteLine(this ITextWriter me, string format, params object[] arguments)
		{
			return me.WriteLine(string.Format(format, arguments));
		}
		public static bool WriteLine(this ITextWriter me, Generic.IEnumerable<char> value)
		{
			return me.WriteLine(value.GetEnumerator());
		}
		public static bool WriteLine(this ITextWriter me, Generic.IEnumerator<char> value)
		{
			return me.Write(value) && me.WriteLine();
		}
	}
}
