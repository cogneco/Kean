// Copyright (C) 2014, 2017  Simon Mika <simon@mika.se>
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
	public static class TextReaderExtenion
	{
		public static TextMark Mark(this ITextReader me)
		{
			return new TextMark(me);
		}
		public static async Tasks.Task<string> ReadLine(this ITextReader me)
		{
			Text.Builder result = null;
			while (!(await me.Empty) && (await me.Next()) && me.Last != '\n')
				result += me.Last;
			return result;
		}
		public static Generic.IEnumerable<Tasks.Task<string>> ReadAllLines(this ITextReader me)
		{
			Tasks.Task<string> result;
			while ((result = me.ReadLine()).NotNull())
				yield return result;
		}
		public static async Tasks.Task<string> ReadFromCurrentUntil(this ITextReader me, Func<char, bool> predicate)
		{
			Text.Builder result = !(await me.Empty) ? (Text.Builder)me.Last : null;
			while (!(await me.Empty) && (await me.Next()) && !predicate(me.Last))
				result += me.Last;
			return result;
}	}
}
