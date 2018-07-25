// Copyright (C) 2010  Simon Mika <simon@mika.se>
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

using Xunit;
using Generic = System.Collections.Generic;

namespace Kean.Extension.StringTest
{
	public class Escape
	{
		public static Generic.IEnumerable<object[]> Data {
			get {
				yield return new object[] { "string\nwithout\nescape\\ characters", "string\\nwithout\\nescape\\\\ characters" };
			}
		}
		[Theory, MemberData(nameof(Data))]
		public void Encode(string decoded, string encoded)
		{
			Assert.Equal(encoded, decoded.Escape());
		}
		[Theory, MemberData(nameof(Data))]
		public void Decode(string decoded, string encoded)
		{
			Assert.Equal(decoded, encoded.Unescape());
		}
	}
}
