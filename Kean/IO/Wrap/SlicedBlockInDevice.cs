//
//  SlicedBlockInDevice.cs
//
//  Author:
//       Simon Mika <simon@mika.se>
//
//  Copyright (c) 2015 Simon Mika
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
using Kean.Extension;
using Collection = Kean.Collection;
using Long = Kean.Math.Long;

namespace Kean.IO.Wrap
{
	public class SlicedBlockInDevice :
	ISeekableBlockInDevice
	{
		ISeekableBlockInDevice backend;
		long first;
		long last;
		long size;
		SlicedBlockInDevice()
		{			
		}
		~SlicedBlockInDevice ()
		{
			Error.Log.Wrap(() => this.Close());
		}
		internal static ISeekableBlockInDevice Slice(ISeekableBlockInDevice backend, long first, long last = 0)
		{
			SlicedBlockInDevice result;
			if (backend.NotNull() && backend.Size.HasValue)
			{
				result = new SlicedBlockInDevice() { backend = backend, first = first, last = last > 0 ? last : backend.Size.Value + last };
				result.size = result.last - result.first + 1;
				backend.Position = first;
			}
			else
				result = null;
			return result;
		}			
		#region IBlockInDevice implementation
		public Collection.IVector<byte> Peek()
		{
			return this.backend.NotNull() ? this.backend.Peek() : null;
		}
		public Collection.IVector<byte> Read()
		{
			return this.backend.NotNull() ? this.backend.Read() : null;
		}
		#endregion
		#region IDevice implementation
		public Uri.Locator Resource { get { return this.backend.NotNull() ? this.backend.Resource : null; } }
		public bool Opened { get { return this.backend.NotNull() && this.backend.Opened; } }
		public bool Close()
		{
			return this.backend.NotNull() && this.backend.Close();
		}
		#endregion
		#region IDisposable implementation
		void IDisposable.Dispose()
		{
			this.Close();
		}
		#endregion
		#region ISeekable implementation
		public bool Seekable { get { return this.backend.NotNull() && this.backend.Seekable; } }
		public long? Position
		{
			get 
			{ 
				var position = this.backend.NotNull() ? this.backend.Position : null;
				return position.NotNull() && position.HasValue && position.Value >= this.first && position.Value < this.last ? this.backend.Position - this.first : null; 
			}
			set
			{
				if (this.backend.NotNull() && value.HasValue)
					this.backend.Position = this.first + (value.Value < 0 ? Long.Maximum(value.Value, -(size - 1)) : Long.Minimum(value.Value, size - 1));
			}
		}
		public long? Size { get { return this.size; } }
		public long? Seek(long delta)
		{
			var position = this.Position;
			return this.backend.NotNull() && position.HasValue ? this.backend.Seek(delta < 0 ? Long.Maximum(delta, -position.Value) : Long.Minimum(delta, size - position.Value)) : null;
		}
		#endregion
		#region IInDevice implementation
		public bool Empty { get { return this.backend.IsNull() || this.backend.Empty || this.backend.Position >= this.last; } }
		public bool Readable { get { return this.backend.NotNull() && this.backend.Readable; } }
		#endregion
	}
}

