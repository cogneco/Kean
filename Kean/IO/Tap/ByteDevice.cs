// 
//  ByteDevice.cs
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

namespace Kean.IO.Tap
{
	public class ByteDevice :
		IByteDevice
	{
		IByteDevice backend;
		public event Action<byte> OnRead;
		public event Action<byte> OnWrite;

		#region Constructors
		public ByteDevice(IByteDevice backend, Action<byte> onRead, Action<byte> onWrite) :
			this(backend)
		{
			this.OnRead = onRead;
			this.OnWrite = onWrite;
		}
		public ByteDevice(IByteDevice backend)
		{
			this.backend = backend;
		}
		#endregion
		#region IByteDevice Members
		public bool Readable { get { return this.backend.NotNull() && this.backend.Readable; } }
		public bool Writable { get { return this.backend.NotNull() && this.backend.Writable; } }
		#endregion
		byte? Convert(int value)
		{
			return value < 0 ? null : (byte?)value;
		}
		#region IByteInDevice Members
		public byte? Peek()
		{
			return this.backend.Peek();
		}
		public byte? Read()
		{
			byte? result = this.backend.Read();
			if (result.HasValue)
				this.OnRead.Call(result.Value);
			return result;
		}
		#endregion
		#region IByteOutDevice Members
		public bool Write(System.Collections.Generic.IEnumerable<byte> buffer)
		{
			return this.backend.Write(buffer.Cast(b => { this.OnWrite.Call(b); return b; }));
		}
		#endregion
		#region IInDevice Members
		public bool Empty { get { return this.backend.Empty; } }
		#endregion
		#region IOutDevice Members
		public bool AutoFlush
		{
			get { return this.backend.AutoFlush; }
			set { this.backend.AutoFlush = value; }
		}
		public bool Flush()
		{
			return this.backend.Flush();
		}
		#endregion
		#region IDevice Members
		public Uri.Locator Resource { get { return this.backend.Resource; } }
		public virtual bool Opened { get { return this.backend.NotNull() && this.backend.Opened; } }
		public virtual bool Close()
		{
			bool result;
			if (result = this.backend.NotNull())
			{
				this.backend.Close();
				this.backend = null;
			}
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
		public static ByteDevice Open(IByteDevice backend)
		{
			return backend.NotNull() ? new ByteDevice(backend) : null;
		}
		public static ByteDevice Open(IByteDevice backend, Action<byte> onRead, Action<byte> onWrite)
		{
			ByteDevice result = ByteDevice.Open(backend);
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
