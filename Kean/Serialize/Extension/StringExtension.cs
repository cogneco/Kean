//
//  StringExtension.cs
//
//  Author:
//       Simon Mika <smika@hx.se>
//
//  Copyright (c) 2013 Simon Mika
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
using Generic = System.Collections.Generic;

namespace Kean.Serialize.Extension
{
	public static class StringExtension
	{
		public static string Convert(this string me, Casing from, Casing to)
		{
			string result;
			if (me.IsNull())
				result = null;
			else if (to == Casing.Raw)
				result = me;
			else
			{			
				System.Text.StringBuilder builder = new System.Text.StringBuilder();
				foreach (var c in me.ConvertFrom(from).ConvertTo(to))
					builder.Append(c);
				result = builder.ToString();
//				Console.WriteLine(result);
			}
			return result;
		}
		static Generic.IEnumerable<char> ConvertFrom(this string me, Casing from)
		{
			switch (from)
			{
				default:
				case Casing.Raw:
					bool lastSpace = true;
					foreach (var c in me)
					{
						if (lastSpace = (c == ' ' || c == '_') && !lastSpace)
							yield return ' ';
						else
						{
							if (lastSpace = char.IsUpper(c) && !lastSpace)
								yield return ' ';
							yield return char.ToLower(c);
						}
					}
					break;
				case Casing.Normal:
					foreach (var c in me)
						yield return char.ToLower(c);
					break;
				case Casing.Pascal:
				case Casing.Camel:
					bool first = from == Casing.Pascal;
					foreach (var c in me)
					{
						if (char.IsUpper(c) && !first)
							yield return ' ';
						yield return char.ToLower(c);
						first = false;
					}
					break;
				case Casing.Lower:
				case Casing.Upper:
					foreach (var c in me)
					{
						if (c == '_')
							yield return ' ';
						yield return char.ToLower(c);
					}
					break;
			}
		}
		static Generic.IEnumerable<char> ConvertTo(this Generic.IEnumerable<char> me, Casing to)
		{
			switch (to)
			{
				default:
				case Casing.Normal:
					foreach (var c in me)
						yield return c;
					break;
				case Casing.Pascal:
				case Casing.Camel:
					bool nextUpper = to == Casing.Pascal;
					foreach (var c in me)
					{
//						Console.Write(c);
						if (c == ' ')
							nextUpper = true;
						else if (nextUpper)
						{
							nextUpper = false;
							yield return char.ToUpper(c);
						}
						else
							yield return c;
					}
//					Console.WriteLine();
					break;
				case Casing.Lower:
					foreach (var c in me)
						if (c == ' ')
							yield return '_';
						else
							yield return c;
					break;
				case Casing.Upper:
					foreach (var c in me)
						if (c == ' ')
							yield return '_';
						else
							yield return char.ToUpper(c);
					break;
			}
		}
	}
}

