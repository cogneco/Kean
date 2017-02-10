// Copyright (C) 2012  Simon Mika <simon@mika.se>
//
// This file is part of Kean.
//
// Kean is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Kean is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with Kean.  If not, see <http://www.gnu.org/licenses/>.
//

using System;
using Tasks = System.Threading.Tasks;
using Generic = System.Collections.Generic;
using Kean.Extension;

namespace Kean.IO
{
	public class ByteDeviceCombiner :
		IByteDevice
	{
		IByteInDevice inDevice;
		IByteOutDevice outDevice;
		public bool Wrapped { get; set; }
		#region Constructors
		protected ByteDeviceCombiner(IByteInDevice inDevice) :
			this(inDevice, null)
		{
		}
		protected ByteDeviceCombiner(IByteInDevice inDevice, IByteOutDevice outDevice)
		{
			this.inDevice = inDevice;
			this.outDevice = outDevice;
		}
		#endregion
		#region IByteInDevice Members
		public Tasks.Task<byte?> Peek()
		{
			return this.inDevice.NotNull() ? this.inDevice.Peek() : null;
		}
		public Tasks.Task<byte?> Read()
		{
			return this.inDevice.NotNull() ? this.inDevice.Read() : null;
		}
		#endregion
		#region IByteOutDevice Members
		public Tasks.Task<bool> Write(Generic.IEnumerator<byte> buffer)
		{
			return this.outDevice?.Write(buffer);
		}
		#endregion
		#region IInDevice Members
		public Tasks.Task<bool> Empty { get { return this.inDevice?.Empty; } }
		public bool Readable { get { return this.inDevice.NotNull() && this.inDevice.Readable; } }
		#endregion
		#region IOutDevice Members
		public bool Writable { get { return this.outDevice.NotNull() && this.outDevice.Writable; } }
		public bool AutoFlush
		{
			get { return this.outDevice.AutoFlush; }
			set { this.outDevice.AutoFlush = value; }
		}
		public Tasks.Task<bool> Flush()
		{
			return this.outDevice?.Flush();
		}
		#endregion
		#region IDevice Members
		public Uri.Locator Resource { get { return this.inDevice.Resource; } }
		public virtual bool Opened { get { return this.Readable || this.Writable; } }
		public virtual async Tasks.Task<bool> Close()
		{
			bool result;
			if (result = this.inDevice.NotNull() && !this.Wrapped)
			{
				result = await this.inDevice.Close();
				this.inDevice = null;
			}
			if (this.outDevice.NotNull() && !this.Wrapped)
			{
				result = await this.outDevice.Close();
				this.outDevice = null;
			}
			return result;
		}
		#endregion
		#region IDisposable Members
		void IDisposable.Dispose()
		{
			this.Close().Wait();
		}
		#endregion
		#region Static Open & Wrapped
		public static IByteDevice Open(IByteInDevice inDevice)
		{
			return ByteDeviceCombiner.Open(inDevice, null);
		}
		public static IByteDevice Open(IByteOutDevice outDevice)
		{
			return ByteDeviceCombiner.Open(null, outDevice);
		}
		public static IByteDevice Open(IByteInDevice inDevice, IByteOutDevice outDevice)
		{
			return inDevice.NotNull() || outDevice.NotNull() ? new ByteDeviceCombiner(inDevice, outDevice) : null;
		}
		public static IByteDevice Open(System.IO.Stream input, System.IO.Stream output)
		{
			return ByteDeviceCombiner.Open(ByteDevice.Open(input), ByteDevice.Open(output));
		}
		public static IByteDevice Wrap(IByteInDevice inDevice)
		{
			return ByteDeviceCombiner.Wrap(inDevice, null);
		}
		public static IByteDevice Wrap(IByteOutDevice outDevice)
		{
			return ByteDeviceCombiner.Wrap(null, outDevice);
		}
		public static IByteDevice Wrap(IByteInDevice inDevice, IByteOutDevice outDevice)
		{
			return inDevice.NotNull() || outDevice.NotNull() ? new ByteDeviceCombiner(inDevice, outDevice) { Wrapped = true } : null;
		}
		#endregion
	}
}
