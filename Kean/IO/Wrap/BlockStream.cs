// 
//  BlockDevice.cs
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
	public class BlockStream :
		System.IO.Stream
	{
		IBlockDevice backend;
		public bool CatchClose { get; set; }
		public override bool CanRead { get { return this.backend.Readable; } }
		public override bool CanSeek { get { return false; } }
		public override bool CanWrite { get { return this.backend.Writable; } }
		long length;
		public override long Length { get { return this.length; } }
		public override long Position { get; set; }
		Collection.IVector<byte> readBuffer;
		BlockStream(IBlockDevice backend)
		{
			this.backend = backend;
		}
		public override void Flush()
		{
		}
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this.readBuffer.IsNull())
				this.readBuffer = this.backend.Read();
			Collection.IVector<byte> chunk;
			if (this.readBuffer.Count > count)
			{
				chunk = this.readBuffer.Slice(0, count);
				this.readBuffer = this.readBuffer.Slice(count, this.readBuffer.Count);
			}
			else
			{
				chunk = this.readBuffer;
				this.readBuffer = null;
			}
			int result = 0;
			foreach (byte b in chunk) // TODO: replace with use of bulk copy interface on IVector<T>
				buffer[offset + result++] = b;
			this.Position += result;
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
			this.Position += count;
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
		public static BlockStream Open(IBlockDevice device)
		{
			return device.NotNull() ? new BlockStream(device) : null;
		}
		#endregion
		#region Wrap
		public static BlockStream Wrap(IBlockDevice device)
		{
			return device.NotNull() ? new BlockStream(device) { CatchClose = true } : null;
		}
		public static BlockStream Wrap(IBlockOutDevice device)
		{
			return BlockStream.Wrap(BlockDeviceCombiner.Open(device));
		}
		#endregion
		#endregion
	}
}
