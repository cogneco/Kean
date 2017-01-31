//
//  BlockDeviceCombiner.cs
//
//  Author:
//       Simon Mika <smika@hx.se>
//
//  Copyright (c) 2012-2013 Simon Mika
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

namespace Kean.IO
{
	public class BlockDeviceCombiner :
		IBlockDevice
	{
		IBlockInDevice inDevice;
		IBlockOutDevice outDevice;
		public bool Wrapped { get; set; }
		#region Constructors
		protected BlockDeviceCombiner(IBlockInDevice inDevice) :
			this(inDevice, null)
		{
		}
		protected BlockDeviceCombiner(IBlockInDevice inDevice, IBlockOutDevice outDevice)
		{
			this.inDevice = inDevice;
			this.outDevice = outDevice;
		}
		#endregion
		#region IBlockInDevice Members
		public Collection.IVector<byte> Peek()
		{
			return this.inDevice.Peek();
		}
		public Collection.IVector<byte> Read()
		{
			return this.inDevice.Read();
		}
		#endregion
		#region IBlockOutDevice Members
		public bool Write(Collection.IVector<byte> buffer)
		{
			return this.outDevice.NotNull() && this.outDevice.Write(buffer);
		}
		#endregion
		#region IInDevice Members
		public bool Empty { get { return this.inDevice.IsNull() || this.inDevice.Empty; } }
		public bool Readable { get { return this.inDevice.NotNull() && this.inDevice.Readable; } }
		#endregion
		#region IOutDevice Members
		public bool Writable { get { return this.outDevice.NotNull() && this.outDevice.Writable; } }
		public bool AutoFlush
		{
			get { return this.outDevice.AutoFlush; }
			set { this.outDevice.AutoFlush = value; }
		}
		public bool Flush()
		{
			return this.outDevice.NotNull() && this.outDevice.Flush();
		}
		#endregion
		#region IDevice Members
		public Uri.Locator Resource { get { return this.inDevice.Resource; } }
		public virtual bool Opened { get { return this.Readable || this.Writable; } }
		public virtual bool Close()
		{
			bool result = this.inDevice.NotNull();
			if (result && !this.Wrapped)
			{
				this.inDevice.Close();
				this.inDevice = null;
			}
			if (this.outDevice.NotNull() && !this.Wrapped)
			{
				result |= this.outDevice.NotNull();
				this.outDevice.Close();
				this.outDevice = null;
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
		#region Static Open & Wrapped
		#region Open
		public static IBlockDevice Open(IBlockInDevice inDevice)
		{
			return BlockDeviceCombiner.Open(inDevice, null);
		}
		public static IBlockDevice Open(IBlockOutDevice outDevice)
		{
			return BlockDeviceCombiner.Open(null, outDevice);
		}
		public static IBlockDevice Open(IBlockInDevice inDevice, IBlockOutDevice outDevice)
		{
			return inDevice.NotNull() || outDevice.NotNull() ? new BlockDeviceCombiner(inDevice, outDevice) : null;
		}
		public static IBlockDevice Open(System.IO.Stream input, System.IO.Stream output)
		{
			return BlockDeviceCombiner.Open(BlockDevice.Open(input), BlockDevice.Open(output));
		}
		#endregion
		#region Wrap
		public static IBlockDevice Wrap(IBlockInDevice inDevice)
		{
			return BlockDeviceCombiner.Wrap(inDevice, null);
		}
		public static IBlockDevice Wrap(IBlockOutDevice outDevice)
		{
			return BlockDeviceCombiner.Wrap(null, outDevice);
		}
		public static IBlockDevice Wrap(IBlockInDevice inDevice, IBlockOutDevice outDevice)
		{
			return inDevice.NotNull() || outDevice.NotNull() ? new BlockDeviceCombiner(inDevice, outDevice) { Wrapped = true } : null;
		}
		#endregion
		#endregion
	}
}
