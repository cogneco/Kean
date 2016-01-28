// 
//  Sender.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2012 Simon Mika
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
using Text = System.Text;
using Uri = Kean.Uri;
using Collection = Kean.Collection;
using Kean.Collection.Extension;

namespace Kean.IO.Net.Udp
{
	public class Sender :
	IBlockOutDevice
	{
		System.Net.Sockets.UdpClient backend;
		#region Constructors
		Sender(System.Net.Sockets.UdpClient backend)
		{
			this.backend = backend;
		}
		#endregion
		#region Static Creators
		public static Sender Connect(Uri.Endpoint endPoint)
		{
			return endPoint.Port.HasValue ? new Sender(new System.Net.Sockets.UdpClient(endPoint.Host, (int)endPoint.Port.Value)) { Resource = new Uri.Locator("udp://", new Uri.Authority(null, endPoint), null) } : null;
		}
		#endregion
		#region IBlockOutDevice Members
		public bool Write(Collection.IVector<byte> buffer)
		{
			bool result;
			byte[] data = buffer.ToArray();
			if (result = (data.NotEmpty() && this.backend.NotNull()))
				this.backend.Send(data, data.Length);
			return result;
		}
		#endregion
		#region IOutDevice Members
		public bool Writable { get { return this.backend.NotNull(); } }
		public bool AutoFlush
		{
			get { return true; }
			set { ; }
		}
		public bool Flush()
		{
			return false;
		}
		#endregion
		#region IDevice Members
		public Uri.Locator Resource { get; private set; }
		public bool Opened { get { return this.backend.NotNull(); } }
		public bool Close()
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
		#region IDisposable Members
		void IDisposable.Dispose()
		{
			this.Close();
		}
		#endregion
	}
}
