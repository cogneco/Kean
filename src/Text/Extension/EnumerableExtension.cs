// Copyright (C) 2011  Simon Mika <simon@mika.se>
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

using Generic = System.Collections.Generic;

namespace Kean.Text.Extension
{
	public static class EnumerableExtension
	{
		#region Join
		public static string Join(this Generic.IEnumerable<char> me)
		{
			System.Text.StringBuilder result = new System.Text.StringBuilder();
			foreach (char c in me)
				result.Append(c);
			return result.ToString();
		}
		public static string Join(this Generic.IEnumerable<string> me)
		{
			System.Text.StringBuilder result = new System.Text.StringBuilder();
			Generic.IEnumerator<string> enumerator = me.GetEnumerator();
			while (enumerator.MoveNext())
				result.Append(enumerator.Current);
			return result.ToString();
		}
		public static string Join(this Generic.IEnumerable<string> me, string separator)
		{
			System.Text.StringBuilder result = new System.Text.StringBuilder();
			Generic.IEnumerator<string> enumerator = me.GetEnumerator();
			if (enumerator.MoveNext())
			{
				result.Append(enumerator.Current);
				while (enumerator.MoveNext())
					result.Append(separator).Append(enumerator.Current);
			}
			return result.ToString();
		}
		#endregion
		#region Decode & Encode
		public static Generic.IEnumerable<char> Decode(this Generic.IEnumerable<byte> me)
		{
			return me.Decode(Encoding.Utf8);
		}
		public static Generic.IEnumerable<char> Decode(this Generic.IEnumerable<byte> me, Encoding encoding)
		{
			Generic.IEnumerator<char> enumerator = me.GetEnumerator().Decode(encoding);
			while (enumerator.MoveNext())
				yield return enumerator.Current;
		}
		public static Generic.IEnumerable<byte> Encode(this Generic.IEnumerable<char> me)
		{
			return me.Encode(Encoding.Utf8);
		}
		public static Generic.IEnumerable<byte> Encode(this Generic.IEnumerable<char> me, Encoding encoding)
		{
			Generic.IEnumerator<byte> enumerator = me.GetEnumerator().Encode(encoding);
			while (enumerator.MoveNext())
				yield return enumerator.Current;
		}
		#endregion
	}
}
