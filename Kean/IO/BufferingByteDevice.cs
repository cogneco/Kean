// 
//  BufferingByteDevice.cs
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
using Monitor = System.Threading.Monitor;
using Generic = System.Collections.Generic;

namespace Kean.IO
{
	public class BufferingByteDevice :
		Wrap.ByteDevice
	{
		object queueLock = new object();
		object writeLock = new object();
		Collection.Queue<Generic.IEnumerable<byte>> queue = new Collection.Queue<Generic.IEnumerable<byte>>();

		BufferingByteDevice(IByteDevice backend) :
			base(backend)
		{ }

		public override bool Write(Generic.IEnumerable<byte> buffer)
		{
			bool result;
			lock (this.queueLock)
			{
				if (Monitor.TryEnter(this.writeLock))
					try
					{
						Monitor.Exit(this.queueLock);
						result = base.Write(buffer);
						Monitor.Enter(this.queueLock);
						if (!this.queue.Empty)
						{
							Generic.IEnumerable<byte> accumulator = this.queue.Dequeue();
							while (!this.queue.Empty)
								accumulator.Append(this.queue.Dequeue());
							base.Write(accumulator);
						}
					}
					finally { Monitor.Exit(this.writeLock); }
				else if (result = this.queue.NotNull())
					this.queue.Enqueue(buffer);
			}
			return result;
		}

		#region Static Open & Create
		public static new IByteDevice Open(IByteDevice device)
		{
			return device.NotNull() ? new BufferingByteDevice(device) : null;
		}
		#endregion
	}
}
