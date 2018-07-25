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

namespace Kean.Text.BuilderTest
{
	public class Equal
	{
		public static Generic.IEnumerable<object[]> Data {
			get {
				yield return new object[] { "string", new Builder("string") };
				yield return new object[] { "string", new Builder(new [] { 's', 't', 'r', 'i', 'n', 'g' }) };
			}
		}
		[Theory, MemberData(nameof(Data))]
		public void IsEqual(string expected, Builder actual)
		{
			Assert.Equal(new Builder(expected), actual);
		}
	}
}
