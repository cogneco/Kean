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
		public class Join
		{
			public static Generic.IEnumerable<object[]> Data
			{
				get
				{
					yield return new object[] { null, null };
					yield return new object[] { "", (Generic.IEnumerable<string>)new string[0] };
					yield return new object[] { "", (Generic.IEnumerable<string>)new string[] { null } };
					yield return new object[] { "", (Generic.IEnumerable<string>)new string[] { "" } };
					yield return new object[] { "", (Generic.IEnumerable<string>)new string[] { "", "" } };
					yield return new object[] { "", (Generic.IEnumerable<string>)new string[] { null, "" } };
					yield return new object[] { "", (Generic.IEnumerable<string>)new string[] { "", null } };
					yield return new object[] { "42", (Generic.IEnumerable<string>)new string[] { "42" } };
					yield return new object[] { "42", (Generic.IEnumerable<string>)new string[] { "4", "2" } };
					yield return new object[] { "42", (Generic.IEnumerable<string>)new string[] { null, "4", "2" } };
					yield return new object[] { "42", (Generic.IEnumerable<string>)new string[] { "4", null, "2" } };
					yield return new object[] { "42", (Generic.IEnumerable<string>)new string[] { "4", "2", null } };
					yield return new object[] { "42", (Generic.IEnumerable<string>)new string[] { "", "4", "2" } };
					yield return new object[] { "42", (Generic.IEnumerable<string>)new string[] { "4", "", "2" } };
					yield return new object[] { "42", (Generic.IEnumerable<string>)new string[] { "4", "2", "" } };
					yield return new object[] { "421337There are 10 types of people, the ones that know binary and the ones that don't.", new string[] { "42", null, "1337", "There are 10 types of people, the ones that know binary and the ones that don't.", "" } };
				}
			}
			[Theory, MemberData(nameof(Data))]
			public void Equal(string expected, Generic.IEnumerable<string> actual)
			{
				Assert.Equal(expected, actual.Join());
			}
		}
}