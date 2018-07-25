// Copyright (C) 2017  Simon Mika <simon@mika.se>
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
using Xunit;
using Generic = System.Collections.Generic;
using Kean.Extension;

namespace Kean.Collection
{
	public class EnumerableTest
	{
		static Generic.IEnumerable<Func<string[], Generic.IEnumerable<string>>> Create {
			get {
				yield return data => data;
				yield return data => Enumerable.Create((Generic.IEnumerable<string>)data);
				foreach (var item in BlockTest.Create)
					yield return item;

			}
		}
		public static Generic.IEnumerable<string[]> Data {
			get {
				yield return new string[] { };
				yield return new string[] { "42" };
				var @short = new string[10];
				for (var i = 0; i < @short.Length; i++)
					@short[i] = i.ToString();
				yield return @short;
				var @long = new string[10000];
				for (var i = 0; i < @long.Length; i++)
					@long[i] = i.ToString();
				yield return @long;
			}
		}
		public static Generic.IEnumerable<object[]> All {
			get { return EnumerableTest.Create.AllPermutations(EnumerableTest.Data, (create, expected) => new object[] { create, expected }); }
		}
		[Theory, MemberData(nameof(All))]
		public void Equal(Func<string[], Generic.IEnumerable<string>> create, string[] expected)
		{
			var actual = create(expected);
			Assert.Equal(expected, actual);
		}
		[Theory, MemberData(nameof(All))]
		public void NotEqual(Func<string[], Generic.IEnumerable<string>> create, string[] expected)
		{
			var actual = create(expected);
			Assert.NotEqual(expected.Append("Missing"), actual);
			Assert.NotEqual(expected.Prepend("Missing"), actual);
			if (expected.Length > 3)
			{
				var changed = expected.Copy();
				changed[2] = "Changed";
				Assert.NotEqual(changed, actual);
			}
		}
		[Theory, MemberData(nameof(All))]
		public void SameOrEquals(Func<string[], Generic.IEnumerable<string>> create, string[] expected)
		{
			var actual = create(expected);
			// Same
			Assert.True(actual.SameOrEquals(actual));
			// Equals
			Assert.True(actual.SameOrEquals(expected));
		}
		[Theory, MemberData(nameof(All))]
		public void NotSameOrEquals(Func<string[], Generic.IEnumerable<string>> create, string[] expected)
		{
			var actual = create(expected);
			Assert.False(actual.SameOrEquals(expected.Append("Missing")));
			Assert.False(actual.SameOrEquals(expected.Prepend("Missing")));
			foreach (var index in new[] { 2, 0, expected.Length - 1 })
				if (expected.Length > index + 1)
				{
					var changed = expected.Copy();
					changed[index] = "Changed";
					Assert.False(actual.SameOrEquals(changed));
				}
		}
	}
}
