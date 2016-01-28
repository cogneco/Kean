//
//  Chunked.cs
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
using Kean.Collection.Extension;
using Kean.IO.Extension;

namespace Kean.IO.Net.Http
{
	public class ChunkedBlockOutDevice :
	IBlockOutDevice
	{
		IOutDevice backend;
		readonly Func<Collection.IVector<byte>, bool> backendWrite;
		public bool Wrapped { get; set; }
		ChunkedBlockOutDevice(IOutDevice backend)
		{
			this.backend = backend;
		}
		ChunkedBlockOutDevice(IByteOutDevice backend) :
			this((IOutDevice)backend)
		{
			this.backendWrite = backend.Write;
		}
		ChunkedBlockOutDevice(IBlockOutDevice backend) :
			this((IOutDevice)backend)
		{
			this.backendWrite = backend.Write;
		}
		bool BackendWrite(string data)
		{
			return this.BackendWrite(data.AsBinary());
		}
		bool BackendWrite(byte[] data)
		{
			return this.BackendWrite(new Collection.Vector<byte>(data));
		}
		bool BackendWrite(Collection.IVector<byte> data)
		{
			return this.backend.NotNull() && this.backendWrite(data);
		}
		#region IBlockOutDevice implementation
		public bool Write(Collection.IVector<byte> buffer)
		{
			return buffer.Count < 1 || this.BackendWrite((buffer.Count.ToString("x") + "\r\n").AsBinary().Merge(buffer).Merge("\r\n".AsBinary()));
		}
		#endregion
		#region IOutDevice implementation
		public bool Writable { get { return this.backend.NotNull() && this.backend.Writable; } }
		public bool Flush()
		{
			return this.backend.NotNull() && this.backend.Flush();
		}
		public bool AutoFlush
		{
			get { return this.backend.AutoFlush; }
			set { this.backend.AutoFlush = value; }
		}
		#endregion
		#region IDevice implementation
		public bool Close()
		{
			bool result;
			if (result = (this.BackendWrite("0\r\n\r\n") && (result = this.Wrapped || this.backend.Close())))
				this.backend = null;
			return result;
		}
		public Uri.Locator Resource
		{
			get { return this.backend.NotNull() ? this.backend.Resource : null; }
		}
		public bool Opened { get { return this.backend.NotNull() && this.backend.Opened; } }
		#endregion
		#region IDisposable implementation
		public void Dispose()
		{
			this.Close();
		}
		#endregion
		public static ChunkedBlockOutDevice Open(IBlockOutDevice backend)
		{
			return backend.NotNull() ? new ChunkedBlockOutDevice(backend) : null;
		}
		public static ChunkedBlockOutDevice Wrap(IBlockOutDevice backend)
		{
			return backend.NotNull() ? new ChunkedBlockOutDevice(backend) { Wrapped = true } : null;
		}
		public static ChunkedBlockOutDevice Open(IByteOutDevice backend)
		{
			return backend.NotNull() ? new ChunkedBlockOutDevice(backend) : null;
		}
		public static ChunkedBlockOutDevice Wrap(IByteOutDevice backend)
		{
			return backend.NotNull() ? new ChunkedBlockOutDevice(backend) { Wrapped = true } : null;
		}
	}
}

