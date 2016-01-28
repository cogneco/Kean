//
//  CharacterReader.cs
//
//  Author:
//       Simon Mika <simon@mika.se>
//
//  Copyright (c) 2014 Simon Mika
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
using Generic = System.Collections.Generic;
using Kean;
using Kean.Extension;

namespace Kean.IO.Extension
{
	public static class CharacterReaderExtension
	{
		public static string ReadLine(this ICharacterReader me)
		{
			Text.Builder result = null;
			while (!me.Empty && me.Next() && me.Last != '\n')
				result += me.Last;
			return result;
		}
		public static Generic.IEnumerable<string> ReadAllLines(this ICharacterReader me)
		{
			string result;
			while ((result = me.ReadLine()).NotNull())
				yield return result;
		}
	}
}

