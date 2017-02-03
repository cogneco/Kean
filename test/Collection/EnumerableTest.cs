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
				Func<Func<string[], Generic.IEnumerable<string>>, object[]> pack = ( create) => new object[] { create };
				yield return data => data;
				foreach (var item in BlockTest.Create)
					yield return item;

			}
		}
		public static Generic.IEnumerable<string[]> Data {
			get {
				yield return new string[] { };
				yield return new string[] { "42" };
				var @long = new string[10000];
				for (var i = 0; i < @long.Length; i++)
					@long[i] = i.ToString();
				yield return @long;
			}
		}
		public static Generic.IEnumerable<object[]> All {
			get { return EnumerableTest.Create.AllPermutations(EnumerableTest.Data, (create, expected) => new object[] { create, expected }); }
		}
		[Theory, MemberData("All")]
		public void Equal(Func<string[], Generic.IEnumerable<string>> create, string[] expected)
		{
			var actual = create(expected);
			Assert.Equal(expected, actual);
		}
	}
}
