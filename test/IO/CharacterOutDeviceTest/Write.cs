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
using Kean.Extension;
using Kean.IO.Extension;

namespace Kean.IO.CharacterOutDeviceTest
{
	public class Write
	{
		public static Generic.IEnumerable<object[]> Data {
			get
			{
				yield return new object[] { null,	new string[] { }};
				yield return new object[] { "",	new string[] { "" }};
				yield return new object[] { "",	new string[] { "", "" }};
				yield return new object[] { "42",	new string[] { "42" }};
			}
		}
		[Theory, MemberData("Data")]
		public void Task(string expect, string[] append)
		{
			var	result = CharacterDevice.Open();
			foreach (var a in append)
				result.Item1.Write(a.GetEnumerator());
			result.Item1.Close();
			Assert.Equal(expect, result.Item2.WaitFor());
		}
		[Theory, MemberData("Data")]
		public void String(string expect, string[] append)
		{
			string result = null;
			var	device = CharacterDevice.Open(r => result = r);
			foreach (var a in append)
				device.Write(a.GetEnumerator());
			device.Close();
			Assert.Equal(expect, result);
		}
	}
}