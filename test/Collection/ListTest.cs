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
	public class ListTest
	{
		public static Generic.IEnumerable<Func<string[], IList<string>>> Create {
			get {
				Func<Func<string[], IBlock<string>>, object[]> pack = ( create) => new object[] { create };
				yield return data => new List<string>(data);
				yield return data => new Array.List<string>(data);
				yield return data => new Linked.List<string>(data);
				yield return data => new Wrapped.List<string>(data);
			}
		}
		public static Generic.IEnumerable<object[]> All {
			get { return ListTest.Create.AllPermutations(EnumerableTest.Data, (create, expected) => new object[] { create, expected }); }
		}
		[Theory, MemberData("All")]
		public void Add(Func<string[], IList<string>> create, string[] expected)
		{
			var actual = create(expected);
			for (var i = 0; i < 100; i++)
			{
				actual.Add("next" + i);
				Assert.Equal(expected.Length + i + 1, actual.Count);
			}
			for (var i = 0; i < expected.Length; i++)
				Assert.Equal(expected[i], actual[i]);
			for (var i = 0; i < 100; i++)
				Assert.Equal("next" + i, actual[i + expected.Length]);
		}
	}
}
