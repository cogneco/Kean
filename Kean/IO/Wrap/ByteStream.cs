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
	public class ByteStream :
		System.IO.Stream
	{
		IByteDevice backend;
		public bool CatchClose { get; set; }
		public override bool CanRead { get { return this.backend.Readable; } }
		public override bool CanSeek { get { return false; } }
		public override bool CanWrite { get { return this.backend.Writable; } }
		long length;
		public override long Length { get { return this.length; } }
		public override long Position 
		{ 
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }
		}

		ByteStream(IByteDevice backend)
		{
			this.backend = backend;
		}
		public override void Flush()
		{
		}
		public override int Read(byte[] buffer, int offset, int count)
		{
			int result = 0;
			byte? next;
			while (result < count && !this.backend.Empty && (next = this.backend.Read()).HasValue)
				buffer[offset + result++] = next.Value;
			return result;
		}
		public override long Seek(long offset, System.IO.SeekOrigin origin)
		{
			throw new NotImplementedException();
		}
		public override void SetLength(long value)
		{
			this.length = value;
		}
		public override void Write(byte[] buffer, int offset, int count)
		{
			this.backend.Write(new Collection.Slice<byte>(buffer, offset, count));
		}
		public override void Close()
		{
			if (!this.CatchClose)
				this.backend.Close();
			base.Close();
		}
		protected override void Dispose(bool disposing)
		{
			if (this.backend.NotNull())
			{
				this.backend.Dispose();
				this.backend = null;
			}
			base.Dispose(disposing);
		}
		#region Static Open & Wrap
		#region Open
		public static ByteStream Open(IByteDevice device)
		{
			return device.NotNull() ? new ByteStream(device) : null;
		}
		public static ByteStream Open(IByteInDevice device)
		{
			return ByteStream.Open(ByteDeviceCombiner.Open(device));
		}
		#endregion
		#region Wrap
		public static ByteStream Wrap(IByteDevice device)
		{
			return device.NotNull() ? new ByteStream(device) { CatchClose = true } : null;
		}
		public static ByteStream Wrap(IByteInDevice device)
		{
			return ByteStream.Wrap(ByteDeviceCombiner.Open(device));
		}
		public static ByteStream Wrap(IByteOutDevice device)
		{
			return ByteStream.Wrap(ByteDeviceCombiner.Open(device));
		}
		#endregion
		#endregion
	}
}
