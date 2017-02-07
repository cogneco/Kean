// Copyright (C) 2016  Simon Mika <simon@mika.se>
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
using Kean.Extension;

namespace Kean.Text
{
	public class Encoding
	{
		readonly System.Text.Encoding encoding;
		readonly Func<byte, int> getSymbolLength;
		byte[][] filters;
		public int MaximumSymbolLength { get; }
		private Encoding(System.Text.Encoding encoding, int maximumSymbolLength, Func<byte, int> getSymbolLength, params byte[][] filters)
		{
			this.encoding = encoding;
			this.MaximumSymbolLength = maximumSymbolLength;
			this.getSymbolLength = getSymbolLength;
			this.filters = filters;
		}
		public int GetSymbolLength(byte b)
		{
			return this.getSymbolLength(b);
		}
		public char? Decode(Func<byte?> next)
		{
			char? result = null;
			byte[] buffer = new byte[this.MaximumSymbolLength];
			int length = 0;
			byte? current;
			if ((current = next()).HasValue)
			{
				buffer[0] = current.Value;
				length = this.getSymbolLength(buffer[0]);
				if (length > 0)
				{
					int i = 1;
					for (; i < length && (current = next()).HasValue; i++)
						buffer[i] = current.Value;
					foreach (var filter in this.filters)
						if (buffer.StartsWith(filter)) {
							length = 0;
							break;
						}
					else if (i == length)
						result = encoding.GetChars(buffer, 0, length)[0];
				}
			}
			return result;
		}
		public char? Decode(Generic.IEnumerator<byte> data)
		{
			return this.Decode(() =>
			{
				byte? result = null;
				if (data.MoveNext())
					result = data.Current;
				return result;
			});
		}
		public char[] Decode(params byte[] data)
		{
			return this.encoding.GetChars(data);
		}
		public byte[] Encode(params char[] data)
		{
			return this.encoding.GetBytes(data);
		}
		public byte[] Encode(string data)
		{
			return this.encoding.GetBytes(data);
		}
		public static Encoding Utf8 { get; } = new Encoding(System.Text.Encoding.UTF8, 6,
					b => b < 0x80 ? 1 :
						b < 0xc0 ? 0 :
						b < 0xe0 ? 2 :
						b < 0xf0 ? 3 :
						b < 0xf8 ? 4 :
						b < 0xfc ? 5 :
						6,
					new byte[] { 0xef, 0xbb, 0xbf } /* "zero width no break space" (0xefbbbf) */,
					new byte[] { 0x20, 0x60 } /* "word joiner" (0x2060) */);
	}
}
