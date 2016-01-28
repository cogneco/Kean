// 
//  ByteDevice.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2013 Simon Mika
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

namespace Kean.IO.Wrap
{
	public class ByteDevice :
		IByteDevice
	{
		IByteDevice backend;
		protected ByteDevice(IByteDevice backend)
		{
			this.backend = backend;
		}

		#region IByteDevice Implementation
		public virtual bool Readable { get { return this.backend.NotNull() && this.backend.Readable; } }
		public virtual bool Writable { get { return this.backend.NotNull() && this.backend.Writable; } }
		#endregion
		#region IByteInDevice Implementation
		public virtual byte? Peek() { return this.backend.NotNull() ? this.backend.Peek() : null; }
		public virtual byte? Read() { return this.backend.NotNull() ? this.backend.Read() : null; }
		#endregion
		#region IInDevice Implementation
		public virtual bool Empty { get { return this.backend.IsNull() || this.backend.Empty; } }
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
		#region IDevice Implementation
		public virtual Uri.Locator Resource { get { return this.backend.NotNull() ? this.backend.Resource : null; } }
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
		#region IByteOutDevice Implementation
		public virtual bool Write(System.Collections.Generic.IEnumerable<byte> buffer)
		{
			return this.backend.NotNull() && this.backend.Write(buffer);
		}
		#endregion
		#region IDisposable Implementation
		void IDisposable.Dispose()
		{
			this.Close();
		}
		#endregion

		#region Static Open & Create
		public static IByteDevice Open(IByteDevice device)
		{
			return device.NotNull() ? new ByteDevice(device) : null;
		}
		#endregion
	}
}
