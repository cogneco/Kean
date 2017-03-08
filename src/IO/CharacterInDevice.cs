// Copyright (C) 2012, 2017	Simon Mika <simon@mika.se>
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

using System;
using Tasks = System.Threading.Tasks;
using Generic = System.Collections.Generic;
using Kean.Extension;

namespace Kean.IO
{
	public class CharacterInDevice :
		ICharacterInDevice
	{
		Generic.IEnumerator<char> backend;
		private CharacterInDevice(Generic.IEnumerator<char> value)
		{
			this.backend = value;
			this.backend.MoveNext();
		}
		#region ICharacterInDevice Members
		public Tasks.Task<char?> Peek()
		{
			return Tasks.Task.FromResult(this.backend?.Current);
		}
		public Tasks.Task<char?> Read()
		{
			var result = this.Peek();
			if (!(this.backend?.MoveNext() ?? false))
				this.backend = null;
			return result;
		}
		#endregion
		#region IInDevice Members
		public Tasks.Task<bool> Empty { get { return Tasks.Task.FromResult(!this.Readable); } }
		public bool Readable { get { return this.backend.NotNull(); } }
		#endregion
		#region IDevice Members
		public Uri.Locator Resource
		{
			get { return "text://"; }
		}
		public bool Opened
		{
			get { return this.backend.NotNull(); }
		}
		public Tasks.Task<bool> Close()
		{
			bool result = this.Opened;
			this.backend = null;
			return Tasks.Task.FromResult(result);
		}
		#endregion
		#region IDisposable Members
		void IDisposable.Dispose()
		{
			if (this.Opened)
				this.backend = null;
		}
		#endregion
		#region Static Open
		internal static ICharacterInDevice Open(string content)
		{
			return CharacterInDevice.Open((Generic.IEnumerable<char>)content);
		}
		internal static ICharacterInDevice Open(Generic.IEnumerable<char> content)
		{
			return CharacterInDevice.Open(content.GetEnumerator());
		}
		internal static ICharacterInDevice Open(Generic.IEnumerator<char> content)
		{
			return new CharacterInDevice(content);
		}
		#endregion
	}
}
