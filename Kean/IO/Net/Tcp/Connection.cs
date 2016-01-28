// 
//  Connection.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2011 Simon Mika
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

namespace Kean.IO.Net.Tcp
{
	public class Connection :
	IDisposable
	{
		#region Backend
		System.IO.Stream stream;
		System.Net.Sockets.TcpClient client;
		#endregion
		#region Device
		Device device;
		Device Device
		{
			get
			{
				if (this.device.IsNull())
				{
					this.device = IO.Device.Open(this.stream);
					this.device.AutoFlush = true;
				}
				return this.device;
			}
		}
		public IByteDevice ByteDevice { get { return this.Device; } }
		public IBlockDevice BlockDevice { get { return this.Device; } }
		public ICharacterDevice CharacterDevice { get { return this.Device; } }
		#endregion
		public event Action Closed;
		public bool AutoClose { get; set; }
		public Uri.Endpoint Peer { get { return client.Client.RemoteEndPoint.AsString() ?? new Uri.Endpoint("unknown", null); } }
		#region Constructors
		Connection(System.Net.Sockets.TcpClient client) :
			this(client.GetStream())
		{
			this.client = client;
			this.client.NoDelay = true;
		}
		Connection(System.Net.Sockets.NetworkStream stream)
		{
			this.stream = stream;
			this.AutoClose = true;
		}
		~Connection ()
		{
			this.Close();
		}
		#endregion
		public bool Close()
		{
			bool result = false;
			if (this.device.NotNull())
			{
				result = this.device.Close();
				this.device = null;
			}
			if (this.client.NotNull())
			{
				this.client.Close();
				this.client = null;
				this.Closed.Call();
			}
			if (this.stream.NotNull())
			{
				this.stream.Close();
				this.stream = null;
			}
			return result;
		}
		#region IDisposable implementation
		void IDisposable.Dispose()
		{
			this.Close();
		}
		#endregion
		#region Static Creators
		internal static Connection Connect(System.Net.Sockets.TcpClient client)
		{
			return new Connection(client);
		}
		public static Connection Connect(Uri.Endpoint endPoint)
		{
			return endPoint.Port.HasValue ? new Connection(new System.Net.Sockets.TcpClient(endPoint.Host, (int)endPoint.Port.Value)) : null;
		}
		#endregion
	}
}
