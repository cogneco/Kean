// 
//  Request.cs
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
using Generic = System.Collections.Generic;
using Kean.IO.Extension;
using Kean.Collection.Extension;
using Integer = Kean.Math.Integer;

namespace Kean.IO.Net.Http.Header
{
	public struct Request :
	Generic.IEnumerable<KeyValue<string, string>>
	{
		#region Headers
		Collection.IDictionary<string, string> headers;
		public string this [string key]
		{ 
			get { return this.headers.NotNull() ? this.headers[key] ?? (key == "Host" ? this.Host : null) : null; } 
			set
			{ 
				if (key.NotEmpty())
				{
					if (this.headers.IsNull())
						this.headers = new Collection.Dictionary<string, string>();
					this.headers[key] = value;
					switch (key)
					{
						case "Host":
							this.Url.Authority = value;
							break;
						case "X-Forwarded-Proto":
							this.Scheme = value;
							break;
					}
				}
			}
		}
		#endregion
		public string Protocol { get; set; }
		public Method Method { get; set; }
		public Uri.Scheme Scheme
		{ 
			get { return this.Url.Scheme; }
			set { this.Url.Scheme = value; }
		}
		public Uri.Domain Host
		{ 
			get { return this.Url.Authority.NotNull() && this.Url.Authority.Endpoint.NotNull() ? this.Url.Authority.Endpoint.Host : new Uri.Domain(); }
			set { this.Url.Authority = new Uri.Authority(this.Url.Authority.User, new Uri.Endpoint(value, this.Url.Authority.Endpoint.Port)); }
		}
		public Uri.Path Path
		{ 
			get { return this.Url.Path; }
			set { this.Url.Path = value; }
		}
		public Uri.Query Query
		{ 
			get { return this.Url.Query; }
			set { this.Url.Query = value; }
		}
		public string Fragment
		{ 
			get { return this.Url.Fragment; }
			set { this.Url.Fragment = value; }
		}
		#region Authorization
		Authorization authorization;
		public Authorization Authorization
		{
			get
			{ 
				if (this.authorization.IsNull())
					this.authorization = this["Authorization"];
				return this.authorization; 
			} 
		}
		#endregion
		#region Url
		Uri.Locator url;
		public Uri.Locator Url
		{ 
			get
			{ 
				if (this.url.IsNull())
					this.url = new Uri.Locator("http", "", "");
				return this.url;
			}
			set
			{
				if ((this.url = value).NotNull())
					this["Host"] = this.Host;
			}
		}
		#endregion
		#region Content-Length
		int? contentLength;
		public int ContentLength
		{
			get
			{
				if (!this.contentLength.HasValue)
					this.contentLength = Integer.Parse(this["Content-Length"], 0);
				return this.contentLength.Value;
			}
		}
		#endregion
		#region Range
		Range range;
		public Range Range
		{
			get 
			{
				if (this.range == null)
					this.range = this["Range"];
				return this.range;
			}
		}
		#endregion
		#region IEnumerable implementation
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
		public Generic.IEnumerator<KeyValue<string, string>> GetEnumerator()
		{
			return this.headers.GetEnumerator();
		}
		#endregion
		public static Request Parse(IByteInDevice device)
		{
			Request result = new Request();
			string[] firstLine = Request.ReadLine(device).Decode().Join().Split(' ');
			if (firstLine.Length == 3)
			{
				result.Method = firstLine[0].Parse<Method>();
				string[] splitted = firstLine[1].Split(new char[] { '?' }, 2);
				result.Path = splitted[0];
				if (splitted.Length > 1)
					result.Query = splitted[1];
				result.Protocol = firstLine[2];
				string line;
				while ((line = Request.ReadLine(device).Decode().Join()).NotEmpty())
				{
					string[] parts = line.Split(new char[] { ':' }, 2);
					result[parts[0].Trim()] = parts[1].Trim();
				}
			}
			return result;
		}
		static Generic.IEnumerable<byte> ReadLine(IByteInDevice device)
		{
			foreach (byte b in device.Read(13, 10))
				if (b != 13 && b != 10)
					yield return b;
		}
		public static implicit operator Request(Uri.Locator url)
		{
			return new Request { Url = url };
		}
		public static implicit operator Request(string url)
		{
			return new Request { Url = url };
		}
	}
}
