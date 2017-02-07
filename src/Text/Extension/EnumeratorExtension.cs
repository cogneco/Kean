// Copyright (C) 2012  Simon Mika <simon@mika.se>
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
	public static class EnumeratorExtension
	{
		#region Join
		public static string Join(this Generic.IEnumerator<string> me, string separator)
		{
			System.Text.StringBuilder result = new System.Text.StringBuilder();
			if (me.MoveNext())
			{
				result.Append(me.Current);
				while (me.MoveNext())
					result.Append(separator).Append(me.Current);
			}
			return result.ToString();
		}
		#endregion
		#region Decode & Encode
		public static Generic.IEnumerator<char> Decode(this Generic.IEnumerator<byte> me)
		{
			return me.Decode(Encoding.Utf8);
		}
		public static Generic.IEnumerator<char> Decode(this Generic.IEnumerator<byte> me, Encoding encoding)
		{
			char? result;
			while ((result = encoding.Decode(me)).HasValue) {
				yield return result.Value;
			}
		}
		public static Generic.IEnumerator<byte> Encode(this Generic.IEnumerator<char> me)
		{
			return me.Encode(Encoding.Utf8);
		}
		public static Generic.IEnumerator<byte> Encode(this Generic.IEnumerator<char> me, Encoding encoding)
		{
			while (me.MoveNext())
				foreach (var b in encoding.Encode(me.Current))
					yield return b;
		}
		#endregion
	}
}
