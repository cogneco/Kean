//
// Program.cs
//
// Author:
// Simon Mika <smika@hx.se>
//
// Copyright (c) 2011 Simon Mika
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with this program. If not, see <http://www.gnu.org/licenses/>.

using System;
using Kean.Extension;
using Parallel = Kean.Parallel;

namespace Kean.IO.Net.Test
{
	class Program
	{
		public static void Run(string[] args)
		{
			using (IDisposable server = Program.Echo())
			//Program.Proxy(23, "192.168.1.102:23");
			//Program.Proxy();
			{
				while (true)
					;
				Program.Terminal("127.0.0.1:23");
			}
		}

		static void Proxy()
		{
			System.Net.Sockets.TcpListener server = new System.Net.Sockets.TcpListener(System.Net.IPAddress.Any, 23);
			server.Start();
			while (true)
			{
				Console.Write("Waiting for a connection... ");
				System.Net.Sockets.TcpClient serverConnection = server.AcceptTcpClient();
				System.Net.Sockets.NetworkStream serverStream = serverConnection.GetStream();
				Console.WriteLine("Connected");
				System.Net.Sockets.TcpClient clientConnection = new System.Net.Sockets.TcpClient("192.168.1.101", 23);
				System.Net.Sockets.NetworkStream clientStream = clientConnection.GetStream();

				Parallel.Thread.Start(() =>
				{
					while (serverConnection.Connected && clientConnection.Connected)
					{
						int value = clientStream.ReadByte();
						if (value >= 0)
						{
							Console.WriteLine("< " + value + "\t" + new string((System.Text.Encoding.ASCII.GetChars(new byte[] { (byte)value }))));
							serverStream.WriteByte((byte)value);
						}
					}
				});
				while (serverConnection.Connected && clientConnection.Connected)
				{
					int value = serverStream.ReadByte();
					if (value >= 0)
					{
						Console.WriteLine("> " + value + "\t" + new string((System.Text.Encoding.ASCII.GetChars(new byte[] { (byte)value }))));
						clientStream.WriteByte((byte)value);
					}
				}
			}
		}

		static void Proxy(uint listenPort, string connectTo)
		{
			Console.Write("Waiting for a connection... ");
			IO.Net.Tcp.Server.Start(server =>
			{
				Tcp.Connection client = Tcp.Connection.Connect(connectTo);
				Console.WriteLine("Connected");
				Parallel.Thread send = Parallel.Thread.Start(() =>
				{
					while (server.ByteDevice.Opened && client.ByteDevice.Opened)
					{
						byte? value = client.ByteDevice.Read();
						if (value.HasValue)
						{
							Console.WriteLine("< " + value.Value + "\t" + System.Text.Encoding.ASCII.GetChars(new byte[] { value.Value })[0]);
							server.ByteDevice.Write(new byte[] { value.Value });
						}
					}
				});
				while (server.ByteDevice.Opened && client.ByteDevice.Opened)
				{
					byte? value = server.ByteDevice.Read();
					if (value.HasValue)
					{
						Console.WriteLine("> " + value.Value + "\t" + System.Text.Encoding.ASCII.GetChars(new byte[] { value.Value })[0]);
						client.ByteDevice.Write(new byte[] { value.Value });
					}
				}
				send.Abort();
			}
				, listenPort);
		}

		static void Terminal(Uri.Endpoint server)
		{
			var connection = Tcp.Connection.Connect(server);
			var writer = CharacterWriter.Open(connection.CharacterDevice);
			Parallel.Thread receiver = Parallel.Thread.Start(() =>
			{
				char? incoming;
				while (connection.CharacterDevice.Opened)
					while ((incoming = connection.CharacterDevice.Read()).HasValue)
						Console.Write(incoming.Value);
			});
			while (writer.Opened)
			{
				string outgoing = Console.In.ReadLine();
				if (outgoing.NotEmpty())
					writer.WriteLine(outgoing);
			}
			receiver.Abort();
		}

		static IDisposable Echo()
		{
			return IO.Net.Tcp.Server.Start(c =>
			{
				ICharacterDevice connection = CharacterDevice.Open(c.ByteDevice);
				while (connection.NotNull() && connection.Opened)
				{
					char? value = connection.Read();
					if (value.HasValue)
					{
						Console.Write(value.Value);
						connection.Write(new char[] { char.ToUpper(value.Value) });
					}
				}
			}
				, 7007);
		}
	}
}