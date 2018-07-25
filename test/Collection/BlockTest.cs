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
	public class BlockTest
	{
		public static Generic.IEnumerable<Func<string[], IBlock<string>>> Create {
			get {
				Func<Func<string[], IBlock<string>>, object[]> pack = ( create) => new object[] { create };
				yield return data => new Block<string>(data);
				yield return data => new Array.Block<string>(data);
				foreach (var item in ListTest.Create)
					yield return item;
			}
		}
		public static Generic.IEnumerable<object[]> All {
			get { return BlockTest.Create.AllPermutations(EnumerableTest.Data, (create, expected) => new object[] { create, expected }); }
		}
		[Theory, MemberData(nameof(All))]
		public void Count(Func<string[], IBlock<string>> create, string[] expected)
		{
			var actual = create(expected);
			Assert.Equal(expected.Length, actual.Count);
		}
		[Theory, MemberData(nameof(All))]
		public void Get(Func<string[], IBlock<string>> create, string[] expected)
		{
			var actual = create(expected);
			for (var i = 0; i < expected.Length; i++)
				Assert.Equal(expected[i], actual[i]);
		}
		[Theory, MemberData(nameof(All))]
		public void Set(Func<string[], IBlock<string>> create, string[] expected)
		{
			var actual = create(expected);
			for (var i = 0; i < expected.Length; i++)
				actual[i] = expected[expected.Length - 1 - i];
			for (var i = 0; i < expected.Length; i++)
				Assert.Equal(expected[expected.Length - 1 - i], actual[i]);
		}
	}
}
