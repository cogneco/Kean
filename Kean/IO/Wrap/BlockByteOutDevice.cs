//
//  BlockByteOutDevice.cs
//
//  Author:
//       Simon Mika <simon@mika.se>
//
//  Copyright (c) 2014 Simon Mika
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
using Kean.IO.Extension;

namespace Kean.IO.Wrap
{
	public class BlockByteOutDevice :
	IByteOutDevice
	{
		IBlockOutDevice backend;
		public bool Wrapped { get; set; }
		BlockByteOutDevice(IBlockOutDevice backend)
		{
			this.backend = backend;
		}
		~BlockByteOutDevice ()
		{
			Error.Log.Call(this.Dispose);
		}
		Collection.IVector<byte> buffer;
		#region IByteOutDevice implementation
		public bool Write(System.Collections.Generic.IEnumerable<byte> buffer)
		{
			this.buffer = this.buffer.Merge(buffer.ToArray());
			return !this.AutoFlush || this.Flush();
		}
		#endregion
		#region IOutDevice implementation
		public bool Writable { get { return this.backend.NotNull() && this.backend.Writable; } }
		bool WriteBuffer()
		{
			bool result;
			if (result = this.backend.Write(buffer.ToArray()))
				buffer = null;
			return result;
		}
		public bool AutoFlush
		{
			get { return this.backend.NotNull() && this.backend.AutoFlush; }
			set
			{
				if (this.backend.NotNull())
					this.backend.AutoFlush = value;
			}
		}
		public bool Flush()
		{
			return this.backend.NotNull() && (buffer.IsNull() || buffer.Count < 1 || this.WriteBuffer() && this.backend.Flush());
		}
		#endregion
		#region IDevice implementation
		public bool Close()
		{
			this.Flush();
			bool result = (this.backend.NotNull() && (this.Wrapped || this.backend.Close()));
			if (result)
				this.backend = null;
			return result;
		}
		public Uri.Locator Resource { get { return this.backend.NotNull() ? this.backend.Resource : null; } }
		public bool Opened { get { return this.backend.NotNull() && this.backend.Opened; } }
		#endregion
		#region IDisposable implementation
		public void Dispose()
		{
			this.Close();
		}
		#endregion
		#region Static Open & Wrap
		public static IByteOutDevice Open(IBlockOutDevice device)
		{
			return device.NotNull() ? new BlockByteOutDevice(device) : null;
		}
		public static IByteOutDevice Wrap(IBlockOutDevice device)
		{
			return device.NotNull() ? new BlockByteOutDevice(device) { Wrapped = true } : null;
		}
		#endregion
	}
}

