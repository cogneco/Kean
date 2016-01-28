// 
//  CharacterInDevice.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2012 Simon Mika
// 
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
// 
//  You should have received a copy of the GNU Lesser General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using Kean.Extension;
using Collection = Kean.Collection;
using Kean.Collection.Extension;
using Uri = Kean.Uri;

namespace Kean.IO.Text
{
	public class CharacterInDevice :
		ICharacterInDevice
	{
		int next = 0;
		string backend;
		public CharacterInDevice(string value)
		{
			this.backend = value;
		}
		#region ICharacterInDevice Members
		public char? Peek()
		{
			return this.next < this.backend.Length ? (char?)this.backend[this.next] : null;
		}
		public char? Read()
		{
			return this.next < this.backend.Length ? (char?)this.backend[this.next++] : null;
		}
		#endregion
		#region IInDevice Members
		public bool Empty { get { return this.next >= this.backend.Length; } }
		public bool Readable { get { return !this.Empty; } }
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
		public bool Close()
		{
			bool result = this.Opened;
			this.backend = null;
			return result;
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
		public static explicit operator string(CharacterInDevice device)
		{
			return device.ToString();
		}
	}
}
