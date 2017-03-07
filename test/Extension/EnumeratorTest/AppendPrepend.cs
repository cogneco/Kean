// Copyright (C) 2017	Simon Mika <simon@mika.se>
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

using Xunit;
using Generic = System.Collections.Generic;

namespace Kean.Extension.EnumeratorTest
{
	public class AppendPrepend
	{
		public static Generic.IEnumerable<object[]> Data
		{
			get
			{
				yield return new object[] { "", null,	null };
				yield return new object[] { "1337", null,	"1337".GetEnumerator() };
				yield return new object[] { "1337", "".GetEnumerator(),	"1337".GetEnumerator() };
				yield return new object[] { "1337", "1".GetEnumerator(),	"337".GetEnumerator() };
				yield return new object[] { "1337", "13".GetEnumerator(),	"37".GetEnumerator() };
				yield return new object[] { "1337", "133".GetEnumerator(),	"7".GetEnumerator() };
				yield return new object[] { "1337", "1337".GetEnumerator(),	"".GetEnumerator() };
				yield return new object[] { "1337", "1337".GetEnumerator(),	null };
			}
		}
		[Theory]
		[MemberData("Data")]
		public void Append(Generic.IEnumerable<char> expect, Generic.IEnumerator<char> prefix, Generic.IEnumerator<char> postfix)
		{
			Assert.Equal(expect.ToArray(), prefix.Append(postfix).ToArray());
		}
		[Theory]
		[MemberData("Data")]
		public void Prepend(Generic.IEnumerable<char> expect, Generic.IEnumerator<char> prefix, Generic.IEnumerator<char> postfix)
		{
			Assert.Equal(expect.ToArray(), postfix.Prepend(prefix).ToArray());
		}
	}
}