// 
//  Response.cs
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
using Kean.IO.Extension;
using Collection = Kean.Collection;
using Kean.Collection.Extension;
using Generic = System.Collections.Generic;

namespace Kean.IO.Net.Http
{
	public class Client : 
		IDisposable
	{
		System.Net.HttpWebRequest request;
		System.Net.HttpWebResponse response;
		public Header.Response Response { get; private set; }
		Client(System.Net.HttpWebRequest request, System.Net.HttpWebResponse response)
		{
			this.request = request;
			this.response = response;
			this.Response = new Header.Response("HTTP/" + response.ProtocolVersion, (Status)(int)response.StatusCode, response.Headers.AllKeys.Map(key => KeyValue.Create(key, response.Headers[key])));
		}
		~Client()
		{
			Error.Log.Call((this as IDisposable).Dispose);
		}
		public IO.IByteInDevice Open()
		{
			return IO.ByteDevice.Open(this.response.GetResponseStream());
		}
		public bool Open(Func<string, IO.IByteInDevice, bool> process)
		{
			return this.Process(this.Response.ContentType, this.Open(), process);
		}
		bool Process(string contentType, IO.IByteInDevice device, Func<string, IO.IByteInDevice, bool> process)
		{
			bool result = false;
			if (contentType.NotNull() && contentType.StartsWith("multipart/x-mixed-replace"))
			{
				byte[] boundary = this.GetBoundary(contentType);
				device.Skip(boundary);
				while (this.response.NotNull() && device.Opened)
				{
					Collection.IDictionary<string, string> headers = new Collection.Dictionary<string, string>(
						                                                 device.Read(13, 10, 13, 10).Decode().Join()
						.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)
						.Map(header =>
						{
							string[] splitted = header.Split(new string[] { ": " }, 2, StringSplitOptions.None);
							return splitted.Length == 2 ? KeyValue.Create(splitted[0], splitted[1]) : KeyValue.Create((string)null, (string)null);
						}));
					this.Process(headers["Content-Type"], Wrap.PartialByteInDevice.Open(device, boundary), process);
				}
			}
			else
				result = process(contentType, device);
			return result;
		}
		byte[] GetBoundary(string contentType)
		{
			byte[] result = null;
			string extractBoundary = contentType.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Find(s => s.Contains("boundary"));
			string[] keyValue = extractBoundary.Split(new char[] { ' ', '=' }, StringSplitOptions.RemoveEmptyEntries);
			if (keyValue.Length == 2 && keyValue[0] == "boundary")
				result = ("\r\n" + (keyValue[1].StartsWith("--") ? "" : "--") + keyValue[1] + "\r\n").AsBinary();
			return result;
		}
		public bool Close()
		{
			bool result;
			if (result = this.request.NotNull())
			{
				this.request.Abort();
				this.request = null;
			}
			if (result |= this.response.NotNull())
			{
				this.response.Close();
				this.response = null;
			}
			return result;
		}
		void IDisposable.Dispose()
		{
			this.Close();
		}
		public static Client Open(Uri.Locator url)
		{
			Client result = null;
			System.Net.HttpWebRequest backendRequest = System.Net.WebRequest.Create(url) as System.Net.HttpWebRequest;
			if (backendRequest.NotNull())
			{
				backendRequest.Credentials = url.Authority.User.NotNull() && url.Authority.User.Name.NotEmpty() && url.Authority.User.Password.NotEmpty() ? new System.Net.NetworkCredential(url.Authority.User.Name, url.Authority.User.Password) : null;
				System.Net.HttpWebResponse backendResponse = null;
				try
				{
					backendResponse = backendRequest.GetResponse() as System.Net.HttpWebResponse;
				}
				catch (System.Net.WebException ex)
				{
					backendResponse = ex.Response as System.Net.HttpWebResponse;
				}
				if (backendResponse.NotNull())
					result = new Client(backendRequest, backendResponse);
			}
			return result;
		}
	}
}
