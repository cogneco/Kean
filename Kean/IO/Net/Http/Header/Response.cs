//
//  Response.cs
//
//  Author:
//       Simon Mika <simon@mika.se>
//
//  Copyright (c) 2014 Simon Mika
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

namespace Kean.IO.Net.Http.Header
{
	public struct Response
	{
		string protocol;
		public string Protocol
		{ 
			get
			{
				if (this.protocol.IsNull())
					this.protocol = "HTTP/1.1";
				return this.protocol;
			}
			set { this.protocol = value; }
		}
		#region Status
		public Status Status { get; set; }
		#endregion
		#region Headers
		Collection.IDictionary<string, string> headers;
		public string this [string key]
		{ 
			get { return this.headers.NotNull() ? this.headers[key] : null; } 
			set
			{ 
				if (key.NotEmpty())
				{
					if (this.headers.IsNull())
						this.headers = new Collection.Dictionary<string, string>();
					this.headers[key] = value;
					switch (key)
					{
						case "Link":
							this.link = null;
							break;
						case "Location":
							this.location = null;
							break;
						case "Content-Length":
							this.contentLength = null;
							break;
					}
				}
			}
		}
		#endregion
		#region ContentType, Location
		public string ContentType { get { return this["Content-Type"]; } set { this["Content-Type"] = value; } }
		Uri.Locator location;
		public Uri.Locator Location { get { return this.location ?? (this.location = this["Location"]); } set { this["Location"] = this.location = value; } }
		#endregion
		#region ContentLength
		long? contentLength;
		public long ContentLength
		{
			get
			{ 
				long result;
				if (contentLength.HasValue)
					result = this.contentLength.Value;
				else if (long.TryParse(this["Content-Length"], out result))
					this.contentLength = result;
				else
					result = 0;
				return result; 
			} 
			set
			{ 
				this.contentLength = value;
				this["Content-Length"] = this.contentLength.HasValue ? this.contentLength.ToString() : null; 
			} 
		}
		#endregion
		#region ContentRange
		Range contentRange;
		public Range ContentRange
		{
			get 
			{
				if (this.contentRange == null)
					this.contentRange = this["Content-Range"];
				return this.contentRange;
			}
			set { this["Content-Range"] = this.contentRange = value; }
		}
		#endregion
		#region Link
		Links link;
		public Links Link
		{
			get
			{
				if (this.link.IsNull())
				{
					var headers = this.headers;
					this.link = new Links(this["Link"], l => headers["Link"] = l);
				}
				return this.link;
			}
		}
		#endregion
		public Response(Status status, params KeyValue<string, string>[] headers) :
			this(status, (Generic.IEnumerable<KeyValue<string, string>>)headers)
		{
		}
		public Response(Status status, Generic.IEnumerable<KeyValue<string, string>> headers) :
			this()
		{
			this.Status = status;
			foreach (var header in headers)
				this[header.Key] = header.Value;
		}
		public Response(string protocol, Status status, params KeyValue<string, string>[] headers) :
			this(protocol, status, (Generic.IEnumerable<KeyValue<string, string>>)headers)
		{
		}
		public Response(string protocol, Status status, Generic.IEnumerable<KeyValue<string, string>> headers) :
			this(status, headers)
		{
			this.Protocol = protocol;
		}
		#region Send
		public bool Send(ICharacterWriter writer)
		{
			return writer.WriteLine(this.Protocol + " " + this.Status) &&
			(headers.IsNull() || this.headers.All(header => writer.WriteLine(header.Key + ": " + header.Value))) &&
			writer.WriteLine() && writer.Flush();
		}
		#endregion
		#region Static Creators
		public static Response NotFound { get { return new Response() { Status = Status.NotFound }; } }
		public static Response BadRequest { get { return new Response() { Status = Status.BadRequest }; } }
		public static Response InternalServerError { get { return new Response() { Status = Status.InternalServerError }; } }
		public static Response MovedPermanently(Uri.Locator location)
		{
			return new Response() { Status = Status.MovedPermanently, Location = location };
		}
		public static Response UnauthorizedBasic(string realm)
		{
			return new Response(Status.Unauthorized, KeyValue.Create("WWW-Authenticate", "Basic realm=\"" + realm + "\""));
		}
		#endregion
	}
}

