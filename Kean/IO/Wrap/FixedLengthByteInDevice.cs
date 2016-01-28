//
//  FixedLengthByteInDevice.cs
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
using Uri = Kean.Uri;
using IO = Kean.IO;

namespace Kean.IO.Wrap
{
	public class FixedLengthByteInDevice :
	IByteInDevice
	{
		IByteInDevice backend;
		int length;
		protected FixedLengthByteInDevice(IByteInDevice backend, int length)
		{
			this.backend = backend;
			this.length = length;
		}
		#region IByteInDevice Implementation
		public virtual byte? Peek()
		{
			return this.backend.NotNull() && this.length > 0 ? this.backend.Peek() : null;
		}
		public virtual byte? Read()
		{
			return this.backend.NotNull() && this.length-- > 0 ? this.backend.Read() : null;
		}
		#endregion
		#region IInDevice Implementation
		public virtual bool Empty { get { return this.length <= 0 || this.backend.IsNull() || this.backend.Empty; } }
		public virtual bool Readable { get { return this.length > 0 && this.backend.NotNull() && this.backend.Readable; } }
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
		#region IDisposable Implementation
		void IDisposable.Dispose()
		{
			this.Close();
		}
		#endregion
		#region Static Open & Create
		public static IByteInDevice Open(IByteInDevice device, int length)
		{
			return (device.NotNull() && length > 0) ? new FixedLengthByteInDevice(device, length) : null;
		}
		#endregion
	}
}

