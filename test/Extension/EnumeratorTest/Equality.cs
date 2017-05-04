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
	public class Equality
	{
		public static Generic.IEnumerable<object[]> SameOrEqualsData
		{
			get
			{
				var data = ((Generic.IEnumerable<string>)new string[] { "42", null, "1337", "There are 10 types of people, the ones that know binary and the ones don't.", "" });
				yield return new object[] { data.GetEnumerator(), data.GetEnumerator() };
			}
		}
		[Theory]
		[MemberData("SameOrEqualsData")]
		public void ArrayEqual(Generic.IEnumerator<string> left, Generic.IEnumerator<string> right)
		{
			Assert.Equal(left.ToArray(), right.ToArray());
		}
		[Theory]
		[MemberData("SameOrEqualsData")]
		public void EnumeratorEqual(Generic.IEnumerator<string> left, Generic.IEnumerator<string> right)
		{
			Assert.True(left.SameOrEquals(right));
		}
	}
}
