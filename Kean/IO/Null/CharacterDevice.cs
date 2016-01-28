// 
//  CharacterDevice.cs
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
using Kean;
using Kean.Extension;
using Collection = Kean.Collection;
using Kean.Collection.Extension;
using Uri = Kean.Uri;
using Error = Kean.Error;

namespace Kean.IO.Null
{
	public class CharacterDevice :
		ICharacterDevice
	{
		#region Constructors
		public CharacterDevice()
		{
		}
		#endregion
		#region ICharacterDevice Members
		public bool Readable { get { return true; } }
		public bool Writable { get { return true; } }
		#endregion
		#region ICharacterOutDevice Members
		public bool Write(System.Collections.Generic.IEnumerable<char> buffer)
		{
			return true;
		}
		#endregion
		#region ICharacterInDevice Members
		public char? Peek()
		{
			return null;
		}
		public char? Read()
		{
			return null;
		}
		#endregion
		#region IInDevice Members
		public bool Empty { get { return true; } }
		#endregion
		#region IOutDevice Members
		public bool AutoFlush { get; set; }
		public bool Flush()
		{
			return true;
		}
		#endregion
		#region IDevice Members
		public Uri.Locator Resource { get { return "null://"; } }
		public bool Opened { get { return true; } }
		public bool Close()
		{
			return true;
		}
		#endregion
		#region IDisposable Members
		void IDisposable.Dispose()
		{
		}
		#endregion
	}
}
