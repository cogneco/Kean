// 
//  BufferBlockOutDevice.cs
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
using Generic = System.Collections.Generic;

namespace Kean.IO
{
	public class BufferBlockOutDevice :
		Synchronized,
		IBlockOutDevice
	{
		public Collection.IVector<byte> Buffer { get; private set; }
		#region Constructors
		public BufferBlockOutDevice()
		{
			this.Resource = "buffer:///";
		}
		#endregion
		#region IBlockOutDevice
		public bool Write(Collection.IVector<byte> buffer)
		{
			lock (this.Lock)
				this.Buffer = this.Buffer.Merge(buffer);
			return true;
		}
		#endregion
		#region IOutDevice Members
		public bool Writable { get { return true; } }
		public bool AutoFlush { get { return true; } set { ; } }
		public bool Flush()
		{
			return false; 
		}
		#endregion
		#region IDevice Members
		public Uri.Locator Resource { get; private set; }
		public virtual bool Opened { get { return true; } }
		public virtual bool Close()
		{
			return true;
		}
		#endregion
		#region IDisposable Members
		void IDisposable.Dispose()
		{
			this.Close();
		}
		#endregion
	}
}
