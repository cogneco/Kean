// 
//  PartialByteInDevice.cs
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
using Uri = Kean.Uri;
using IO = Kean.IO;

namespace Kean.IO.Wrap
{
	public class PartialByteInDevice :
		IByteInDevice
	{
		IByteInDevice backend;
		byte[] endMark;
		int matchingLength;
		int nextPosition;
		public event Action Closed;
		byte? peeked;
		public bool Empty { get { return this.backend.IsNull() || this.backend.Empty; } }
		public Uri.Locator Resource { get { return this.backend.NotNull() ? this.backend.Resource : null; } }
		public bool Opened { get { return this.backend.NotNull() && this.backend.Opened; } }
		public bool Readable { get { return this.backend.NotNull() && this.backend.Readable; } }
		PartialByteInDevice(IByteInDevice backend, byte[] endMark)
		{
			this.backend = backend;
			this.endMark = endMark;
		}
		byte? Next()
		{
			byte? result;
			if (this.nextPosition < this.matchingLength)
				result = this.endMark[this.nextPosition++];
			else if (this.backend.IsNull())
				result = null;
			else
			{
				byte? next = result = this.backend.Read();
				if (next.HasValue && next.Value == this.endMark[0])
				{
					this.nextPosition = 1;
					this.matchingLength = 1;
					while (true)
					{
						if (this.matchingLength < this.endMark.Length)
						{
							while (!(next = this.backend.Peek()).HasValue)
								;
							if (next.Value != this.endMark[this.matchingLength])
								break;
							this.matchingLength++;
							next = this.backend.Read();
						}
						else
						{
							this.matchingLength = 0;
							this.backend = null;
							result = null;
							break;
						}
					}
				}
			}
			return result;
		}
		public byte? Peek()
		{
			return this.peeked.HasValue ? this.peeked : this.peeked = this.Next();
		}
		public byte? Read()
		{
			byte? result;
			if (this.peeked.HasValue)
			{
				result = this.peeked;
				this.peeked = null;
			}
			else
				result = this.Next();
			return result;
		}
		public bool Close()
		{
			bool result;
			this.backend = null;
			if (result = this.Closed.NotNull())
			{
				this.Closed();
				this.Closed = null;
			}
			return result;
		}
		void IDisposable.Dispose()
		{
			this.Close();
		}
		public static IByteInDevice Open(IByteInDevice backend, byte[] endMark)
		{
			return backend.NotNull() ? endMark.NotEmpty() ? new PartialByteInDevice(backend, endMark) : backend : null;
		}
	}
}
