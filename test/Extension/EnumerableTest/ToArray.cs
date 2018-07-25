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

namespace Kean.Extension.EnumerableTest
{
		public class ToArray
		{
			public static Generic.IEnumerable<object[]> Data
			{
				get
				{
					yield return new object[] { null, null };
					yield return new object[] { new char[0], (Generic.IEnumerable<char>)new char[0] };
					yield return new object[] { new char[0], (Generic.IEnumerable<char>)"" };
					yield return new object[] { new [] { '1', '3', '3', '7' }, (Generic.IEnumerable<char>)"1337" };
					yield return new object[] { new [] { '\0' }, (Generic.IEnumerable<char>)"\0" };
				}
			}
			[Theory, MemberData(nameof(Data))]
			public void Test(char[] expected, Generic.IEnumerable<char> actual)
			{
				Assert.Equal(expected, actual.ToArray());
			}
		}
}
