// Copyright (C) 2014, 2017	Simon Mika <simon@mika.se>
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
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.	See the
// GNU Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with Kean.	If not, see <http://www.gnu.org/licenses/>.
//

using System;
using Generic = System.Collections.Generic;
using Tasks = System.Threading.Tasks;
using Kean.Extension;

namespace Kean.IO.Extension
{
	public static class TextReaderExtenion
	{
		public static TextMark Mark(this ITextReader me)
		{
			return new TextMark(me);
		}
		public static Tasks.Task<char?> Read(this ITextReader me, char character)
		{
			return me.Read(last => last == character);
		}
		public static Tasks.Task<char?> Read(this ITextReader me, params char[] characters)
		{
			return me.Read(last => characters.Contains(last));
		}
		public static async Tasks.Task<char?> Read(this ITextReader me, Func<char, bool> predicate)
		{
			char? peeked;
			return !(await me.Empty) && (peeked = await me.Peek()).HasValue && predicate(peeked.Value) ? await me.Read() : null;
		}
		public static async Tasks.Task<string> ReadLine(this ITextReader me)
		{
			string result;
			if (me.IsNull() || await me.Empty)
				result = null;
			else
			{
				result = await me.ReadUpTo('\n');
				me.Read('\n').Forget();
			}
			return result;
		}
		public static Generic.IEnumerable<Tasks.Task<string>> ReadAllLines(this ITextReader me)
		{
			Tasks.Task<string> result;
			while ((result = me.ReadLine()).NotNull())
				yield return result;
		}
		public static Tasks.Task<string> ReadUpTo(this ITextReader me, char character)
		{
			return me.ReadUpTo(last => last == character);
		}
		public static Tasks.Task<string> ReadUpTo(this ITextReader me, params char[] characters)
		{
			return me.ReadUpTo(last => characters.Contains(last));
		}
		public static async Tasks.Task<string> ReadUpTo(this ITextReader me, Func<char, bool> predicate)
		{
			Text.Builder result = await me.Empty ? null : "";
			char? next;
			while ((next = await me.Read(predicate.Negate())).HasValue)
				result += next.Value;
			return result;
		}
		public static Tasks.Task<string> ReadPast(this ITextReader me, char character)
		{
			return me.ReadPast(last => last == character);
		}
		public static Tasks.Task<string> ReadPast(this ITextReader me, params char[] characters)
		{
			return me.ReadPast(last => characters.Contains(last));
		}
		public static async Tasks.Task<string> ReadPast(this ITextReader me, Func<char, bool> predicate)
		{
			Text.Builder result = await me.Empty ? null : "";
			char? next;
			while ((next = await me.Read(predicate.Negate())).HasValue)
				result += next.Value;
			if ((next = await me.Read(predicate)).HasValue)
				result += next.Value;
			return result;
		}
	}
}
