//
//  VectorByteInDevice.cs
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

namespace Kean.IO
{
	public class BufferByteInDevice :
	IByteInDevice
	{
		int position;
		Collection.IVector<byte> buffer;
		public bool EscalateClose { get; set; }
		#region Constructors
		protected BufferByteInDevice(Collection.IVector<byte> buffer)
		{
			this.EscalateClose = true;
			this.buffer = buffer;
			this.Resource = "buffer:///";
		}
		#endregion
		public void Append(Collection.IVector<byte> buffer)
		{
			this.buffer = this.buffer.Merge(buffer);
		}
		#region IByteInDevice Members
		public byte? Peek()
		{
			return this.Empty ? null : (byte?)this.buffer[this.position];
		}
		public byte? Read()
		{
			return this.Empty ? null : (byte?)this.buffer[this.position++];
		}
		#endregion
		#region IInDevice Members
		public bool Empty { get { return !this.Opened || this.position >= this.buffer.Count; } }
		public bool Readable { get { return !this.Empty; } }
		#endregion
		#region IDevice Members
		public Uri.Locator Resource { get; private set; }
		public virtual bool Opened { get { return this.buffer.NotNull(); } }
		public virtual bool Close()
		{
			bool result;
			if (result = this.buffer.NotNull())
			{
				if (this.EscalateClose && this.buffer is IDisposable)
					(this.buffer as IDisposable).Dispose();
				this.buffer = null;
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
		#region Static Open, Wrap & Create
		#region Open
		public static BufferByteInDevice Open(Collection.IVector<byte> buffer)
		{
			return buffer.NotNull() ? new BufferByteInDevice(buffer) : null;
		}
		#endregion
		#region Wrap
		public static BufferByteInDevice Wrap(Collection.IVector<byte> buffer)
		{
			return buffer.NotNull() ? new BufferByteInDevice(buffer) { EscalateClose = false } : null;
		}
		#endregion
		#region Create
		public static BufferByteInDevice Create()
		{
			return new BufferByteInDevice(new Collection.Vector<byte>(0));
		}
		#endregion
		#endregion
	}
}
