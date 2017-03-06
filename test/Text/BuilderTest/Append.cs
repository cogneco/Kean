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

namespace Kean.Text.BuilderTest
{
	public class Append
	{
		public static Generic.IEnumerable<object[]> Data {
			get {
				yield return new object[] { "", null, new Generic.IEnumerable<char>[] { new char[] { } } };
				yield return new object[] { "", new Builder(), new Generic.IEnumerable<char>[] { new char[] { } } };
				yield return new object[] { "", new Builder(), new Generic.IEnumerable<char>[] { "" } };
				yield return new object[] { "42", null, new Generic.IEnumerable<char>[] { "42" } };
				yield return new object[] { "42", new Builder(), new Generic.IEnumerable<char>[] { "42" } };
				yield return new object[] { "42", new Builder("42"), new Generic.IEnumerable<char>[] { "" } };
				yield return new object[] { "42", new Builder("42"), new Generic.IEnumerable<char>[] { null } };
				yield return new object[] { "1337", new Builder("13"), new Generic.IEnumerable<char>[] { "37" } };
				yield return new object[] { "1337", new Builder(), new Generic.IEnumerable<char>[] { new[] { '1', '3', '3', '7' } } };
			}
		}
		[Theory, MemberData("Data")]
		public void Method(string expected, Builder @base, Generic.IEnumerable<char>[] append)
		{
			if (@base.IsNull())
				@base = new Builder();
			foreach (var a in append)
				@base = @base.Append(a);
			Assert.Equal(expected, @base);
		}
		[Theory, MemberData("Data")]
		public void Operator(string expected, Builder @base, Generic.IEnumerable<char>[] append)
		{
			foreach (var a in append)
				@base += a;
			Assert.Equal(expected, @base);
		}
		[Theory, MemberData("Data")]
		public void EnumeratorMethod(string expected, Builder @base, Generic.IEnumerable<char>[] append)
		{
			if (@base.IsNull())
				@base = new Builder();
			foreach (var a in append)
				@base = @base.Append(a?.GetEnumerator());
			Assert.Equal(expected, @base);
		}
		[Theory, MemberData("Data")]
		public void EnumeratorOperator(string expected, Builder @base, Generic.IEnumerable<char>[] append)
		{
			foreach (var a in append)
				@base += a?.GetEnumerator();
			Assert.Equal(expected, @base);
		}
	}
}
