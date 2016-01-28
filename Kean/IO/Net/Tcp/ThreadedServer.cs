// 
//  ThreadedServer.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2011-2014 Simon Mika
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
using Parallel = Kean.Parallel;

namespace Kean.IO.Net.Tcp
{
	public class ThreadedServer :
		Server
    {
		public Parallel.ThreadPool ThreadPool { get; private set; }
		public ThreadedServer(Action<Connection> process) : 
		this("TcpServer", process) { }
		public ThreadedServer(string name, Action<Connection> process) :
		this(new Parallel.ThreadPool(name, 1) { MaximumThreadCount = -1 }, process)
		{ }
		public ThreadedServer(Parallel.ThreadPool threadPool, Action<Connection> process) :
		base(process)
		{
			this.ThreadPool = threadPool;
		}
		protected override void OnConnect (Connection connection)
		{
			this.ThreadPool.Enqueue(base.OnConnect, connection);
		}
		public override void Dispose ()
		{
			if (this.ThreadPool.NotNull())
			{
				this.ThreadPool.Dispose();
				this.ThreadPool = null;
			}
		}
		#region static Start & Run
		public static new ThreadedServer Start(Action<Connection> connected, Uri.Endpoint endPoint)
		{
			ThreadedServer result = new ThreadedServer(connected);
			if (!result.Start(endPoint))
			{
				result.Dispose();
				result = null;
			}
			return result;
		}
		public static new ThreadedServer Start(Action<Connection> connected, uint port)
		{
			ThreadedServer result = new ThreadedServer(connected);
			if (!result.Start(port))
			{
				result.Dispose();
				result = null;
			}
			return result;
		}
		public static new bool Run(Action<Connection> connected, Uri.Endpoint endPoint)
		{
			bool result;
			using (ThreadedServer server = new ThreadedServer(connected))
				result = server.Run(endPoint);
			return result;
		}
		public static new bool Run(Action<Connection> connected, uint port)
		{
			bool result;
			using (ThreadedServer server = new ThreadedServer(connected))
				result = server.Run(port);
			return result;
		}
		#endregion
    }
}

