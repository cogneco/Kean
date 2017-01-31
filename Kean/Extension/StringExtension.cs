// 
//  StringExtension.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2010 Simon Mika
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
using RegularExpressions = System.Text.RegularExpressions;
using Kean.Extension;
using Generic = System.Collections.Generic;
using Kean.Reflect.Extension;

namespace Kean.Extension
{
	public static class StringExtension
	{
		public static bool NotEmpty(this string me)
		{
			return me.NotNull() && me != "";
		}
		public static bool IsEmpty(this string me)
		{
			return me.IsNull() || me == "";
		}
		public static bool Like(this string me, string pattern)
		{
			return new RegularExpressions.Regex("^" + RegularExpressions.Regex.Escape(pattern).Replace(@"\*", ".*").Replace(@"\?", ".") + "$", RegularExpressions.RegexOptions.IgnoreCase | RegularExpressions.RegexOptions.Singleline).IsMatch(me);
		}
		public static void AppendTo(this string me, Uri.Locator path)
		{
			System.IO.Directory.CreateDirectory(path.Path.FolderPath.PlatformPath);
			System.IO.File.AppendAllText(path.PlatformPath, me);
		}
		public static void Save(this string me, Uri.Locator path)
		{
			System.IO.Directory.CreateDirectory(path.Path.FolderPath.PlatformPath);
			System.IO.File.WriteAllText(path.PlatformPath, me);
		}
		#region Format
		public static string Format(this string me, object argument)
		{
			return string.Format(me, argument);
		}
		public static string Format(this string me, object argument0, object argument1)
		{
			return string.Format(me, argument0, argument1);
		}
		public static string Format(this string me, object argument0, object argument1, object argument2)
		{
			return string.Format(me, argument0, argument1, argument2);
		}
		public static string Format(this string me, params object[] arguments)
		{
			return string.Format(me, arguments);
		}
		#endregion
		#region Index
		public static int Index(this string me, string needle)
		{
			return me.Index(0, needle);
		}
		public static int Index(this string me, int start, string needle)
		{
			return me.Index(start, me.Length - 1, needle);
		}
		public static int Index(this string me, int start, int end, string needle)
		{
			int length = me.Length - 1;
			if (start < 0)
				start = length + start;
			if (end < 0)
				end = length + end;
			int result = -1;
			for (int i = start; i <= end; i++)
				if (me[i] == needle[0] && me.Substring(i, needle.Length) == needle)
				{
					result = i;
					break;
				}
			return result;
		}
		public static int Index(this string me, char needle)
		{
			return me.Index(0, needle);
		}
		public static int Index(this string me, int start, char needle)
		{
			return me.Index(start, me.Length - 1, needle);
		}
		public static int Index(this string me, int start, int end, char needle)
		{
			int length = me.Length;
			if (start < 0)
				start = length + start;
			if (end < 0)
				end = length + end;
			int result = -1;
			for (int i = start; i <= end; i++)
				if (me[i] == needle)
				{
					result = i;
					break;
				}
			return result;
		}
		public static int Index(this string me, params char[] needles)
		{
			return me.Index(0, needles);
		}
		public static int Index(this string me, int start, params char[] needles)
		{
			return me.Index(start, me.Length - 1, needles);
		}
		public static int Index(this string me, int start, int end, params char[] needles)
		{
			int length = me.Length;
			if (start < 0)
				start = length + start;
			if (end < 0)
				end = length + end;
			int result = -1;
			for (int i = start; i <= end; i++)
				if (needles.Contains(me[i]))
				{
					result = i;
					break;
				}
			return result;
		}
		#endregion
		public static string Get(this string me, int index, int length)
		{
			string result = "";
			if (length != 0)
			{
				int count = me.Length;
				int sliceIndex = (index >= 0) ? index : count + index;
				int sliceLength = (length > 0) ? length : count + length - sliceIndex;
				result = me.Substring(sliceIndex, sliceLength);
			}
			return result;
		}
		public static string Get(this string me, int index)
		{
			return index >= 0 ?
				me.Get(index, me.Length - index) :
				me.Get(me.Length + index, -index);
		}
		public static Generic.IEnumerable<string> FromCsv(this string me)
		{
			return me.SplitAt(',');
		}
		public static Generic.IEnumerable<string> SplitAt(this string me)
		{
			return me.SplitAt(' ');
		}
		public static Generic.IEnumerable<string> SplitAt(this string me, params char[] separators)
		{
			var current = new IO.Text.Buffer();
			me = me.Trim();
			int index = 0;
			while (index < me.Length)
			{
				char unit = me[index];
				switch (unit)
				{
					case '\\':
						if (++index < me.Length)
							switch (me[index])
							{
								case 'r':
									current += '\r';
									break;
								case 'n':
									current += '\n';
									break;
								case 't':
									current += '\t';
									break;
								case 'b':
									current += '\b';
									break;
								case '\\':
									current += '\\';
									break;
								case '"':
									current += '"';
									break;
							}
						break;
					case '"':
						while (++index < me.Length && (unit = me[index]) != '"')
							switch (unit)
							{
								case '\\':
									if (++index < me.Length)
										switch (me[index])
										{
											case 'r':
												current += '\r';
												break;
											case 'n':
												current += '\n';
												break;
											case 't':
												current += '\t';
												break;
											case 'b':
												current += '\b';
												break;
											case '\\':
												current += '\\';
												break;
											case '"':
												current += '"';
												break;
										}
									break;
								default:
									current += unit;
									break;
							}
						break;
					default:
						if (separators.Contains(unit))
						{
							yield return (string)current;
							current = new IO.Text.Buffer();
						}
						else
							current += unit;
						break;
				}
				index++;
			}
			if (current.Length > 0)
				yield return current.ToString();
		}
		public static T Parse<T>(this string me)
		{
			return (me is T ? me : me.Parse(typeof(T))).ConvertType<T>();
		}
		public static object Parse(this string me, Reflect.Type type)
		{
			object result;
			if (type.Implements(me.Type()))
				result = me;
			else if (type == typeof(char))
				result = me.ToString();
			else if (type == typeof(bool))
			{
				bool value;
				result = (bool.TryParse(me, out value) && value);
			}
			else if (type == typeof(byte))
			{
				byte value;
				result = (byte.TryParse(me, out value) ? value : (byte)0);
			}
			else if (type == typeof(sbyte))
			{
				sbyte value;
				result = (sbyte.TryParse(me, out value) ? value : (sbyte)0);
			}
			else if (type == typeof(short))
			{
				short value;
				result = (short.TryParse(me, out value) ? value : (short)0);
			}
			else if (type == typeof(int))
			{
				int value;
				result = (int.TryParse(me, out value) ? value : 0);
			}
			else if (type == typeof(uint))
			{
				uint value;
				result = (uint.TryParse(me, out value) ? value : 0);
			}
			else if (type == typeof(long))
			{
				long value;
				result = (long.TryParse(me, out value) ? value : 0);
			}
			else if (type == typeof(ulong))
			{
				ulong value;
				result = (ulong.TryParse(me, out value) ? value : 0);
			}
			else if (type == typeof(float))
			{
				float value;
				result = (float.TryParse(me, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture.NumberFormat, out value) ? value : 0);
			}
			else if (type == typeof(double))
			{
				double value;
				result = (double.TryParse(me, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture.NumberFormat, out value) ? value : 0);
			}
			else if (type == typeof(decimal))
			{
				decimal value;
				result = (decimal.TryParse(me, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture.NumberFormat, out value) ? value : 0);
			}
			else if (type == typeof(DateTime))
			{
				DateTime value;
				result = (DateTime.TryParse(me, System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat, System.Globalization.DateTimeStyles.AssumeLocal, out value) ? value : new DateTime());
			}
			else if (type == typeof(DateTimeOffset))
			{
				DateTimeOffset value;
				result = (DateTimeOffset.TryParse(me, System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat, System.Globalization.DateTimeStyles.AssumeLocal, out value) ? value : new DateTimeOffset());
			}
			else if (type == typeof(TimeSpan))
			{
				TimeSpan value;
				result = (TimeSpan.TryParse(me, System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat, out value) ? value : new TimeSpan());
			}
			else if (type == typeof(IntPtr))
			{
				int value;
				result = (int.TryParse(me, out value) ? (IntPtr)value : IntPtr.Zero);
			}
			else if (type.Category == Reflect.TypeCategory.Enumeration)
				result = Enum.Parse(type, me.Replace('|', ','), true);
			else if (type.Implements<IString>())
			{
				result = type.Create();
				(result as IString).String = me;
			}
			else
			{
				Func<string, object> cast = ((System.Type)type).FromStringCast();
				if (cast.NotNull())
					result = cast(me);
				else
					result = me;
			}
			return result;
		}
		public static string FirstToLower(this string me)
		{
			return char.IsLower(me[0]) ? me : char.ToLower(me[0]) + me.Get(1);
		}
		public static string FirstToUpper(this string me)
		{
			return char.IsUpper(me[0]) ? me : char.ToUpper(me[0]) + me.Get(1);
		}
		public static string AddDoubleQuotes(this string me)
		{
			return "\"" + me.Replace("\"", "\\\"") + "\"";
		}
		public static string RemoveDoubleQuotes(this string me)
		{
			return me[0] == '"' && me[me.Length - 1] == '"' ? me.Get(1, -1).Replace("\\\"", "\"") : me;
		}
		public static string PercentEncode(this string me, params char[] characters)
		{
			if (characters.Empty())
				characters = new char[] {
					' ',
					'"',
					'!',
					'#',
					'$',
					'%',
					'&',
					'\'',
					'(',
					')',
					'*',
					'+',
					',',
					'/',
					':',
					';',
					'=',
					'?',
					'@',
					'[',
					']'
				};
			IO.Text.Builder result = null;
			if (me.NotNull())
				foreach (char c in me)
					if (characters.Contains(c))
						result += "%" + System.Text.Encoding.ASCII.GetBytes(new char[] { c }).First().ToString("X2");
					else
						result += c;
			return result;
		}
		public static string PercentDecode(this string me)
		{
			IO.Text.Builder result = null;
			if (me.NotNull())
			{
				Generic.IEnumerator<char> enumerator = me.GetEnumerator();
				while (enumerator.MoveNext())
					if (enumerator.Current == '%')
					{
						char[] characters = new char[] { enumerator.Next(), enumerator.Next() };
						byte value;
						if (byte.TryParse(new string(characters), System.Globalization.NumberStyles.HexNumber, System.Globalization.NumberFormatInfo.InvariantInfo, out value))
							result += System.Text.Encoding.ASCII.GetChars(new byte[] { value });
						else
							result += "%" + characters;
					}
					else
						result += enumerator.Current;
			}
			return result;
		}
	}
}
