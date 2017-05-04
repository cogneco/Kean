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
				yield return new object[] { Equality.Data.GetEnumerator(), Equality.Data.GetEnumerator() };
			}
		}
		public static Generic.IEnumerable<object[]> NotEqualData {
			get {
				var expect = Equality.Data;
				var actual = Equality.Data;
				actual[0] = null;
				yield return new object[] { expect.GetEnumerator(), actual.GetEnumerator() };
				yield return new object[] { expect.GetEnumerator(), null };
				yield return new object[] { null, actual.GetEnumerator() };
				yield return new object[] { expect.GetEnumerator(), new string [0] };
				yield return new object[] { new string[0], actual.GetEnumerator() };
				yield return new object[] { expect.GetEnumerator(), new string [1] };
				yield return new object[] { new string[1], actual.GetEnumerator() };
				yield return new object[] { expect.GetEnumerator(), new string [] { "The power of attraction." } };
				yield return new object[] { new string[] { "The power of attraction." }, actual.GetEnumerator() };
			}
		}
		[Theory]
		[MemberData("SameOrEqualsData")]
		public void ArrayEqual(Generic.IEnumerator<string> left, Generic.IEnumerator<string> right)
		{
			Assert.Equal(left.ToArray(), right.ToArray());
		}
		[Theory]
		[MemberData("NotEqualsData")]
		public void ArrayNotEqual(Generic.IEnumerator<string> left, Generic.IEnumerator<string> right)
		{
			Assert.NotEqual(left.ToArray(), right.ToArray());
		}
		[Theory]
		[MemberData("SameOrEqualsData")]
		public void EnumeratorEqual(Generic.IEnumerator<string> left, Generic.IEnumerator<string> right)
		{
			Assert.True(left.SameOrEquals(right));
		}
		[Theory]
		[MemberData("NotEqualsData")]
		public void EnumeratorNotEqual(Generic.IEnumerator<string> left, Generic.IEnumerator<string> right)
		{
			Assert.False(left.SameOrEquals(right));
		}
	}
}
