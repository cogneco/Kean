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

namespace Kean.KeyValueTest
{
	public class Creating
	{
		[Theory, InlineData("key", "value")]
		public void Create(string key, string value) {
			this.CreateHelper(key, value);
		}
		[Theory, InlineData("key", 42)]
		public void Create(string key, int value) {
			this.CreateHelper(key, value);
		}
		[Theory, InlineData(42, "value")]
		public void Create(int key, string value) {
			this.CreateHelper(key, value);
		}
		[Theory, InlineData(42, 1337)]
		public void Create(int key, int value) {
			this.CreateHelper(key, value);
		}
		void CreateHelper<TKey, TValue>(TKey key, TValue value)
		{
			var result = KeyValue.Create(key, value);
			Assert.Equal(result.Key, key);
			Assert.Equal(result.Value, value);
		}
	}
}