// 
//  CharacterDevice.cs
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
using Kean;
using Kean.Extension;
using Collection = Kean.Collection;
using Kean.Collection.Extension;
using Uri = Kean.Uri;
using Error = Kean.Error;

namespace Kean.IO
{
	public class CharacterDevice :
		ICharacterDevice
	{
		IByteDevice backend;
		Decoder decoder;
		System.Text.Encoding encoding;
		public bool Wrapped { get; set; }
		#region Constructors
		CharacterDevice(IByteDevice backend, System.Text.Encoding encoding)
		{
			this.backend = backend;
			this.decoder = new Decoder(this.backend, encoding);
			this.encoding = encoding;
		}
		~CharacterDevice()
		{
			Error.Log.Wrap((Func<bool>)this.Close)();
		}
		#endregion
		#region ICharacterOutDevice Members
		public bool Write(System.Collections.Generic.IEnumerable<char> buffer)
		{
			return this.backend.Write(new Collection.Enumerable<byte>(() => new Encoder(buffer.GetEnumerator(), this.encoding)));
		}
		#endregion
		#region ICharacterInDevice Members
		public char? Peek()
		{
			return this.decoder.Peek();
		}
		public char? Read()
		{
			return this.decoder.Read();
		}
		#endregion
		#region IInDevice Members
		public bool Empty { get { return this.decoder.NotNull() && this.decoder.Empty; } }
		public bool Readable { get { return this.decoder.NotNull() && this.decoder.Readable; } }
		#endregion
		#region IOutDevice Members
		public bool Writable { get { return this.backend.NotNull() && this.backend.Writable; } }
		public bool AutoFlush
		{
			get { return this.backend.AutoFlush; }
			set { this.backend.AutoFlush = value; }
		}
		public bool Flush()
		{
			return this.backend.NotNull() && this.backend.Flush();
		}
		#endregion
		#region IDevice Members
		public Uri.Locator Resource { get { return this.backend.Resource; } }
		public virtual bool Opened { get { return this.backend.NotNull() && this.backend.Opened; } }
		public virtual bool Close()
		{
			bool result = this.backend.NotNull() && (this.Wrapped || this.backend.Close() && this.decoder.Close());
			if (result)
			{
				this.backend = null;
				this.decoder = null;
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
		#region Static Open, Wrap & Create
		public static ICharacterDevice Open(System.IO.Stream stream)
		{
			return CharacterDevice.Open(ByteDevice.Open(stream));
		}
		public static ICharacterDevice Open(IByteDevice backend)
		{
			return CharacterDevice.Open(backend, System.Text.Encoding.UTF8);
		}
		public static ICharacterDevice Open(IByteDevice backend, System.Text.Encoding encoding)
		{
			return backend.NotNull() ? new CharacterDevice(backend, encoding) : null;
		}
		public static ICharacterDevice Open(Uri.Locator resource)
		{
			return CharacterDevice.Open(ByteDevice.Open(resource));
		}
		public static ICharacterDevice Wrap(IByteDevice backend)
		{
			return CharacterDevice.Wrap(backend, System.Text.Encoding.UTF8);
		}
		public static ICharacterDevice Wrap(IByteDevice backend, System.Text.Encoding encoding)
		{
			return backend.NotNull() ? new CharacterDevice(backend, encoding) { Wrapped = true } : null;
		}
		public static ICharacterDevice Create(Uri.Locator resource)
		{
			return CharacterDevice.Open(ByteDevice.Create(resource));
		}
		#endregion
	}
}
