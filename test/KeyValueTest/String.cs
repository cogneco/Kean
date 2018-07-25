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

using Xunit;
using Generic = System.Collections.Generic;

namespace Kean.KeyValueTest
{
	public class String
	{
		public static Generic.IEnumerable<object[]> Data {
			get {
				yield return new object[] { KeyValue.Create("key", "value"), "(key = value)" };
				yield return new object[] { KeyValue.Create("key", 42), "(key = 42)" };
				yield return new object[] { KeyValue.Create(42, "value"), "(42 = value)" };
				yield return new object[] { KeyValue.Create(42, 1337), "(42 = 1337)" };
			}
		}
		[Theory, MemberData(nameof(Data))]
		public void To(object keyValue, string correct)
		{
			Assert.Equal(keyValue.ToString(), correct);
		}
	}
}