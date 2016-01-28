// 
//  Server.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2011-2015 Simon Mika
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
using Kean.IO.Extension;
using Kean.Collection.Extension;
using Generic = System.Collections.Generic;
using Long = Kean.Math.Long;

namespace Kean.IO.Net.Http
{
	public class Server :
		IDisposable
	{
		public Header.Request Request { get; private set; }
		public event Action Closed
		{ 
			add { this.connection.Closed += value; } 
			remove { this.connection.Closed -= value; } 
		}
		public bool AutoClose
		{
			get { return this.connection.AutoClose; }
			set { this.connection.AutoClose = value; }
		}
		Tcp.Connection connection;
		public IByteDevice ByteDevice { get; private set; }
		public IBlockDevice BlockDevice { get; private set; }
		ICharacterReader reader;
		public ICharacterReader Reader
		{
			get
			{
				if (this.reader.IsNull())
					this.reader = CharacterReader.Open(this.connection.CharacterDevice);
				return this.reader;
			}
		}
		ICharacterWriter writer;
		public ICharacterWriter Writer
		{
			get
			{
				if (this.writer.IsNull())
				{
					this.writer = CharacterWriter.Open(this.connection.CharacterDevice);
					this.writer.NewLine = new char[] { '\r', '\n' };
				}
				return this.writer;
			}
		}
		#region SendStorage
		Serialize.Storage sendStorage;
		Serialize.Storage SendStorage
		{
			get
			{ 
				if (this.sendStorage.IsNull())
					switch ("application/json")
					{
						case "application/json":
							this.sendStorage = new Json.Serialize.Storage() { NoTypes = true };
							break;
						case "application/xml":
							this.sendStorage = new Xml.Serialize.Storage() { NoTypes = true };
							break;
					}
				return this.sendStorage;
			}
		}
		#endregion
		#region ReceiveStorage
		Serialize.Storage receiveStorage;
		Serialize.Storage ReceiveStorage
		{
			get
			{ 
				if (this.receiveStorage.IsNull())
					switch ("application/json")
					{
						case "application/json":
							this.receiveStorage = new Json.Serialize.Storage() { NoTypes = true };
							break;
						case "application/xml":
							this.receiveStorage = new Xml.Serialize.Storage() { NoTypes = true };
							break;
					}
				return this.receiveStorage;
			}
		}
		#endregion
		public Uri.Domain Peer { get { return this.Request["X-Real-IP"] ?? this.connection.Peer.Host; } }
		Server(Tcp.Connection connection)
		{
			this.connection = connection;
			this.Request = Header.Request.Parse(this.connection.ByteDevice);
			this.ByteDevice = ByteDeviceCombiner.Open(IO.Wrap.FixedLengthByteInDevice.Open(this.connection.ByteDevice, this.Request.ContentLength), this.connection.ByteDevice);
			this.BlockDevice = this.connection.BlockDevice;
		}
		~Server()
		{
			Error.Log.Call(() => this.Close());
		}
		void IDisposable.Dispose()
		{
			this.Close();
		}
		#region SendHeader
		public bool SendHeader(Status status, params KeyValue<string, string>[] headers)
		{
			return this.SendHeader(status, (Generic.IEnumerable<KeyValue<string, string>>)headers);
		}
		public bool SendHeader(Status status, Generic.IEnumerable<KeyValue<string, string>> headers)
		{
			return this.SendHeader(this.Request.Protocol, status, headers);
		}
		public bool SendHeader(string protocol, Status status, params KeyValue<string, string>[] headers)
		{
			return this.SendHeader(protocol, status, (Generic.IEnumerable<KeyValue<string, string>>)headers);
		}
		public bool SendHeader(string protocol, Status status, Generic.IEnumerable<KeyValue<string, string>> headers)
		{
			return this.SendHeader(new Header.Response(protocol, status, headers));
		}
		public bool SendHeader(Header.Response response)
		{
			return response.Send(this.Writer);
		}
		#endregion
		#region Respond
		public IBlockOutDevice Respond(Header.Response response, long contentLength)
		{
			response.ContentLength = contentLength;
			return this.SendHeader(response) ? this.BlockDevice : null;
		}
		public IBlockOutDevice Respond(Header.Response response)
		{
			response["Transfer-Encoding"] = "chunked";
			return this.SendHeader(response) ? ChunkedBlockOutDevice.Wrap(this.BlockDevice) : null;
		}
		#endregion
		#region Send
		public bool Send<T>(T data)
		{
			using (var device = this.Respond(new Header.Response() { Status = Status.OK, ContentType = "application/json; charset=UTF-8" }))
				return this.SendStorage.Store(data, device);
		}
		public bool Send(Json.Dom.Object data)
		{
			return this.Send((Json.Dom.Item)data);
		}
		public bool Send(Json.Dom.Array data)
		{
			return this.Send((Json.Dom.Item)data);
		}
		public bool Send(Json.Dom.Item data)
		{
			using (var device = this.Respond(new Header.Response() { Status = Status.OK, ContentType = "application/json; charset=UTF-8" }))
				return data.Save(device);
		}
		#endregion
		#region SendFile
		public Status SendFile(Uri.Locator file, Header.Response response = new Header.Response())
		{
			if (this.Request.Path.Folder)
				file += "index.html";
			if (System.IO.Directory.Exists(file.Path.PlatformPath))
				this.SendHeader(response = Header.Response.MovedPermanently(this.Request.Url + "/"));
			else
			{
				using (var source = IO.BlockDevice.Open(file))
					if (source.NotNull())
					{
						switch (file.Path.Extension)
						{
							case "html":
								response.ContentType = "text/html; charset=utf8";
								break;
							case "css":
								response.ContentType = "text/css; charset=UTF-8";
								break;
							case "csv":
								response.ContentType = "text/csv; charset=UTF-8";
								break;
							case "mp4":
								response.ContentType = "video/mp4";
								break;
							case "webm":
								response.ContentType = "video/webm";
								break;
							case "png":
								response.ContentType = "image/png";
								break;
							case "jpeg":
							case "jpg":
								response.ContentType = "image/jpeg";
								break;
							case "svg":
								response.ContentType = "image/svg+xml";
								break;
							case "gif":
								response.ContentType = "image/gif";
								break;
							case "json":
								response.ContentType = "application/json; charset=UTF-8";
								break;
							case "js":
								response.ContentType = "application/javascript";
								break;
							case "pdf":
								response.ContentType = "application/pdf";
								break;
							case "xml":
								response.ContentType = "application/xml";
								break;
							case "zip":
								response.ContentType = "application/zip";
								break;
							default:
								response.ContentType = null;
								break;
						}
						response.Status = Status.OK;
						if (source is ISeekableBlockInDevice && ((ISeekable)source).Size.HasValue)
						{
							if (this.Request.Range.NotNull() && this.Request.Range.Type == "bytes")
							{
								response.Status = Status.PartialContent;
								response.ContentRange = new Header.Range("bytes", Long.Maximum(this.Request.Range.First.GetValueOrDefault(0L), 0L), Long.Minimum(this.Request.Range.Last.GetValueOrDefault(((ISeekable)source).Size.Value), ((ISeekable)source).Size.Value - 1), ((ISeekable)source).Size);
								using (var s = ((ISeekableBlockInDevice)source).Slice(response.ContentRange.First.Value, response.ContentRange.Last.Value))
								using (var destination = this.Respond(response, s.Size.Value))
									destination.Write(source);
							}
							else
							{
								response["Accept-Ranges"] = "bytes";
								using (var destination = this.Respond(response, ((ISeekable)source).Size.Value))
									destination.Write(source);
							}
						}
						else
							using (var destination = this.Respond(response))
								destination.Write(source);
					}
					else
						this.SendHeader(response = Header.Response.NotFound);
			}
			return response.Status;
		}
		#endregion
		#region SendMessage
		public bool SendMessage(Status status, params KeyValue<string, string>[] headers)
		{
			return this.SendMessage(status, (Generic.IEnumerable<KeyValue<string, string>>)headers);
		}
		public bool SendMessage(Status status, Generic.IEnumerable<KeyValue<string, string>> headers)
		{
			return this.SendMessage(new Header.Response(status, headers));
		}
		public bool SendMessage(Header.Response response)
		{
			var message = response.Status.AsHtml.AsBinary();
			response.ContentLength = message.Length;
			response.ContentType = "text/html; charset=utf8";
			using (var device = this.Respond(response))
				return device.NotNull() && device.Write(message);
		}
		#endregion
		#region Receive
		public T Receive<T>()
		{
			return this.ReceiveStorage.Load<T>(this.ByteDevice);
		}
		#endregion
		public bool Close()
		{
			bool result = false;
			if (this.connection.NotNull())
			{
				result = this.connection.Close();
				this.connection = null;
			}
			return result;
		}
		#region Static Create & CreateThreaded
		public static Tcp.Server Create(Action<Server> process)
		{
			return new Tcp.Server(connection => process(new Server(connection)));
		}
		public static Tcp.ThreadedServer CreateThreaded(Action<Server> process)
		{
			return new Tcp.ThreadedServer("HttpServer", connection => process(new Server(connection)));
		}
		#endregion
		#region static Start & Run
		public static Tcp.Server Start(Action<Server> process, Uri.Endpoint endPoint)
		{
			return Tcp.Server.Start(connection => process(new Server(connection)), endPoint);
		}
		public static Tcp.Server Start(Action<Server> process, uint port)
		{
			return Tcp.Server.Start(connection => process(new Server(connection)), port);
		}
		public static Tcp.ThreadedServer StartThreaded(Action<Server> process, Uri.Endpoint endPoint)
		{
			return Tcp.ThreadedServer.Start(connection => process(new Server(connection)), endPoint);
		}
		public static Tcp.ThreadedServer StartThreaded(Action<Server> process, uint port)
		{
			return Tcp.ThreadedServer.Start(connection => process(new Server(connection)), port);
		}
		public static bool Run(Action<Server> process, Uri.Endpoint endPoint)
		{
			return Tcp.Server.Run(connection => process(new Server(connection)), endPoint);
		}
		public static bool Run(Action<Server> process, uint port)
		{
			return Tcp.Server.Run(connection => process(new Server(connection)), port);
		}
		public static bool RunThreaded(Action<Server> process, Uri.Endpoint endPoint)
		{
			return Tcp.ThreadedServer.Run(connection => process(new Server(connection)), endPoint);
		}
		public static bool RunThreaded(Action<Server> process, uint port)
		{
			return Tcp.ThreadedServer.Run(connection => process(new Server(connection)), port);
		}
		#endregion
	}
}

