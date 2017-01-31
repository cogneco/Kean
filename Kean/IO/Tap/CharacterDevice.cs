// 
//  CharacterDevice.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2011 Simon Mika
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

namespace Kean.IO.Tap
{
	public class CharacterDevice :
		ICharacterDevice
	{
		ICharacterDevice backend;
		public event Action<char> OnRead;
		public event Action<char> OnWrite;
		#region Constructors
		public CharacterDevice(ICharacterDevice backend, Action<char> onRead, Action<char> onWrite) :
			this(backend)
		{
			this.OnRead = onRead;
			this.OnWrite = onWrite;
		}
		protected CharacterDevice(ICharacterDevice backend)
		{
			this.backend = backend;
		}
		~CharacterDevice()
		{
			Error.Log.Wrap((Func<bool>)this.Close)();
		}
		#endregion
		#region ICharacterDevice Members
		public bool Readable { get { return this.backend.NotNull() && this.backend.Readable; } }
		public bool Writable { get { return this.backend.NotNull() && this.backend.Writable; } }
		#endregion
		#region ICharacterOutDevice Members
		public bool Write(System.Collections.Generic.IEnumerable<char> buffer)
		{
			return this.backend.Write(buffer.Cast(c =>
			{
				this.OnWrite.Call(c);
				return c;
			}));
		}
		#endregion
		#region ICharacterInDevice Members
		public char? Peek()
		{
			return this.backend.Peek();
		}
		public char? Read()
		{
			char? result = this.backend.Read();
			if (result.HasValue)
				this.OnRead.Call(result.Value);
			return result;
		}
		#endregion
		#region IInDevice Members
		public bool Empty { get { return this.backend.NotNull() && this.backend.Empty; } }
		#endregion
		#region IOutDevice Members
		public bool AutoFlush
		{
			get { return this.backend.AutoFlush; }
			set { this.backend.AutoFlush = value; }
		}
		public bool Flush()
		{
			return this.backend.NotNull() && this.backend.Flush();
		}
		#endregion
		#region IDevice Members
		public Uri.Locator Resource { get { return this.backend.Resource; } }
		public virtual bool Opened { get { return this.backend.NotNull() && this.backend.Opened; } }
		public virtual bool Close()
		{
			bool result = this.backend.NotNull() && this.backend.Close();
			if (result)
				this.backend = null;
			return result;
		}
		#endregion
		#region IDisposable Members
		void IDisposable.Dispose()
		{
			this.Close();
		}
		#endregion
		#region Static Creators
		public static CharacterDevice Open(ICharacterDevice backend)
		{
			return backend.NotNull() ? new CharacterDevice(backend) : null;
		}
		public static CharacterDevice Open(ICharacterDevice backend, Action<char> onRead, Action<char> onWrite)
		{
			CharacterDevice result = CharacterDevice.Open(backend);
			if (result.NotNull())
			{
				result.OnRead += onRead;
				result.OnWrite += onWrite;
			}
			return result;
		}
		#endregion
	}
}
