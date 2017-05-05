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
		static string[] Data => new string[] { "42", null, "1337", "There are 10 types of people, the ones that know binary and the ones don't.", "" };
		public static Generic.IEnumerable<object[]> SameOrEqualsData
		{
			get
			{
				yield return new object[] { Equality.Data, Equality.Data };
			}
		}
		public static Generic.IEnumerable<object[]> NotEqualsData {
			get {
				var expect = (Generic.IEnumerable<string>)Equality.Data;
				var actual = Equality.Data;
				actual[0] = null;
				yield return new object[] { expect, actual };
				actual = Equality.Data;
				yield return new object[] { expect, null };
				yield return new object[] { null, actual };
				yield return new object[] { expect, new string [0] };
				yield return new object[] { new string[0], actual };
				yield return new object[] { expect, new string [1] };
				yield return new object[] { new string[1], actual };
				yield return new object[] { expect, new string [] { "The power of attraction." } };
				yield return new object[] { new string[] { "The power of attraction." }, actual };

			}
		}
		[Theory]
		[MemberData("SameOrEqualsData")]
		public void ArrayEqual(Generic.IEnumerable<string> left, Generic.IEnumerable<string> right)
		{
			Assert.Equal(left?.GetEnumerator().ToArray(), right?.GetEnumerator().ToArray());
		}
		[Theory]
		[MemberData("NotEqualsData")]
		public void ArrayNotEqual(Generic.IEnumerable<string> left, Generic.IEnumerable<string> right)
		{
			Assert.NotEqual(left?.GetEnumerator().ToArray(), right?.GetEnumerator().ToArray());
		}
		[Theory]
		[MemberData("SameOrEqualsData")]
		public void EnumeratorEqual(Generic.IEnumerable<string> left, Generic.IEnumerable<string> right)
		{
			Assert.True(left?.GetEnumerator().SameOrEquals(right?.GetEnumerator()));
		}
		[Theory]
		[MemberData("NotEqualsData")]
		public void EnumeratorNotEqual(Generic.IEnumerable<string> left, Generic.IEnumerable<string> right)
		{
			Assert.False(left?.GetEnumerator().SameOrEquals(right?.GetEnumerator()));
		}
	}
}
