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
using Kean.Extension;
using Collection = Kean.Collection;
using Kean.Collection.Extension;
using Uri = Kean.Uri;

namespace Kean.IO
{
	public class StringCharacterInDevice :
		ICharacterInDevice
	{
		int next = 0;
		string backend;
		internal StringCharacterInDevice(string value)
		{
			this.backend = value;
		}
		#region ICharacterInDevice Members
		public Tasks.Task<char?> Peek()
		{
			return Tasks.Task.FromResult(this.next < this.backend.Length ? (char?)this.backend[this.next] : null);
		}
		public Tasks.Task<char?> Read()
		{
			return Tasks.Task.FromResult(this.next < this.backend.Length ? (char?)this.backend[this.next++] : null);
		}
		#endregion
		#region IInDevice Members
		public Tasks.Task<bool> Empty { get { return Tasks.Task.FromResult(this.Readable); } }
		public bool Readable { get { return !(this.next >= this.backend.Length); } }
		#endregion
		#region IDevice Members
		public Uri.Locator Resource
		{
			get { return "string://"; }
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
		public override string ToString()
		{
			return this.backend.ToString();
		}
		public static explicit operator string(StringCharacterInDevice device)
		{
			return device.ToString();
		}
	}
}
