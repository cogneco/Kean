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
using Kean.IO.Extension;

namespace Kean.IO.TextReaderTest
{
	public class ReadPast
	{
		public static Generic.IEnumerable<object[]> Data {
			get
			{
				yield return new object[] { new string[] { }, null};
				yield return new object[] { new string[] { }, ""};
				yield return new object[] { new string[] { " " }, " "};
				yield return new object[] { new string[] { "0" }, "0"};
				yield return new object[] { new [] { "42 " }, "42 "};
				yield return new object[] { new [] { "42" }, "42"};
				yield return new object[] { new [] { "42\n1337\nThe ", "power ", "of ", "Attraction\n" }, "42\n1337\nThe power of Attraction\n"};
			}
		}
		[Theory, MemberData("Data")]
		public async void Test(string[] expect, string actual)
		{
			int i = 0;
			using (var device = TextReader.From(actual))
			{
				while (!await device.Empty)
				{
					var line = await device.ReadPast(' ');
					Assert.Equal(expect[i++], line);
				}
				Assert.Equal(expect.Length, i);
			}
		}
	}
}
