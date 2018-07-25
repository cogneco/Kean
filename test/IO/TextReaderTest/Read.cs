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
	public class Read
	{
		public static Generic.IEnumerable<object[]> Data {
			get
			{
				yield return new object[] { new char[] { }, null};
				yield return new object[] { new char[] { }, ""};
				yield return new object[] { new [] { '0' }, "0"};
				yield return new object[] { new char[] { '\n' }, "\n"};
				yield return new object[] { new [] { '4', '2', '\n' }, "42\n"};
				yield return new object[] { "42\n1337\nThe power of Attraction\n".ToCharArray(), "42\n1337\nThe power of Attraction\n"};
			}
		}
		[Theory, MemberData(nameof(Data))]
		public async void UnconditionalRead(char[] expect, string actual)
		{
			int i = 0;
			using (var device = TextReader.From(actual))
			{
				while (!await device.Empty)
				{
					var next = await device.Read();
					if (next.HasValue)
					{
						Assert.InRange(i, 0, expect.Length - 1);
						Assert.Equal(expect[i++], next.Value);
					}
					else
						Assert.Equal(expect.Length - 1, i);
				}
				Assert.Equal(expect.Length, i);
				Assert.Null(await device.ReadLine());
			}
		}
		[Theory, MemberData(nameof(Data))]
		public async void ConditionalRead(char[] expect, string actual)
		{
			int i = 0;
			using (var device = TextReader.From(actual))
			{
				while (!await device.Empty)
				{
					var next = await device.Read(expect[i]);
					if (next.HasValue)
					{
						Assert.InRange(i, 0, expect.Length - 1);
						Assert.Equal(expect[i++], next.Value);
					}
					else
						Assert.Equal(expect.Length - 1, i);
				}
				Assert.Equal(expect.Length, i);
				Assert.Null(await device.ReadLine());
			}
		}
		[Theory, MemberData(nameof(Data))]
		public async void ConditionalArrayRead(char[] expect, string actual)
		{
			int i = 0;
			using (var device = TextReader.From(actual))
			{
				while (!await device.Empty)
				{
					var next = await device.Read(expect);
					if (next.HasValue)
					{
						Assert.InRange(i, 0, expect.Length - 1);
						Assert.Equal(expect[i++], next.Value);
					}
					else
						Assert.Equal(expect.Length - 1, i);
				}
				Assert.Equal(expect.Length, i);
				Assert.Null(await device.ReadLine());
			}
		}
		[Theory, MemberData(nameof(Data))]
		public async void ConditionalFunctionRead(char[] expect, string actual)
		{
			int i = 0;
			using (var device = TextReader.From(actual))
			{
				while (!await device.Empty)
				{
					var next = await device.Read(c => true);
					if (next.HasValue)
					{
						Assert.InRange(i, 0, expect.Length - 1);
						Assert.Equal(expect[i++], next.Value);
					}
					else
						Assert.Equal(expect.Length - 1, i);
				}
				Assert.Equal(expect.Length, i);
				Assert.Null(await device.ReadLine());
			}
		}
	}
}
