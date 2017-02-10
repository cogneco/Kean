// Copyright (C) 2011, 2016  Simon Mika <simon@mika.se>
//
// This file is part of Kean.
//
// Kean is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Kean is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with Kean.  If not, see <http://www.gnu.org/licenses/>.
//

using System;
using Tasks = System.Threading.Tasks;
using Generic = System.Collections.Generic;
using Kean.Extension;
using Kean.Text.Extension;

namespace Kean.IO
{
	public class CharacterDevice :
		ICharacterDevice
	{
		PeekBuffer peeked;
		IByteDevice backend;
		readonly Text.Encoding encoding;
		bool dontClose;
		#region Constructors
		CharacterDevice(IByteDevice backend, Text.Encoding encoding)
		{
			this.backend = backend;
			this.encoding = encoding;
			this.peeked = new PeekBuffer(async () =>
			{
				// TODO: Could we use this.encoder.Encode(Func<byte?>)?
				char? result = null;
				var next = await this.backend.Read();
				if (next.HasValue)
				{
					var buffer = new byte[this.encoding.GetSymbolLength(next.Value)];
					buffer[0] = next.Value;
					for (var i = 1; i < buffer.Length && (next = await this.backend.Read()).HasValue; i++)
						buffer[i] = next.Value;
					var r = this.encoding.Decode(buffer);
					if (r.Length > 0)
						result = r[0];
				}
				return result;
			});
		}
		~CharacterDevice()
		{
			this.Close().Wait();
		}
		#endregion
		#region ICharacterOutDevice Members
		public Tasks.Task<bool> Write(Generic.IEnumerator<char> buffer)
		{
			return this.backend.Write(buffer.Encode(this.encoding));
		}
		#endregion
		#region ICharacterInDevice Members
		public Tasks.Task<char?> Peek()
		{
			return this.peeked.Peek();
		}
		public Tasks.Task<char?> Read()
		{
			return this.peeked.Read();
		}
		#endregion
		#region IInDevice Members
		public Tasks.Task<bool> Empty { get { return this.Peek().Then(value => value.NotNull()); } }
		public bool Readable { get { return this.backend?.Readable ?? false; } }
		#endregion
		#region IOutDevice Members
		public bool Writable { get { return this.backend?.Writable ?? false; } }
		public bool AutoFlush
		{
			get { return this.backend.AutoFlush; }
			set { this.backend.AutoFlush = value; }
		}
		public Tasks.Task<bool> Flush()
		{
			return this.backend?.Flush();
		}
		#endregion
		#region IDevice Members
		public Uri.Locator Resource { get { return this.backend?.Resource; } }
		public virtual bool Opened { get { return this.backend?.Opened ?? false; } }
		public virtual async Tasks.Task<bool> Close()
		{
			bool result;
			if (result = this.backend.NotNull() && (this.dontClose || await this.backend.Close()))
				this.backend = null;
			return result;
		}
		#endregion
		#region IDisposable Members
		void IDisposable.Dispose()
		{
			this.Close().Wait();
		}
		#endregion
		#region Static Open, Wrap & Create
		public static ICharacterDevice Open(System.IO.Stream stream)
		{
			return CharacterDevice.Open(ByteDevice.Open(stream));
		}
		public static ICharacterDevice Open(IByteDevice backend)
		{
			return CharacterDevice.Open(backend, Text.Encoding.Utf8);
		}
		public static ICharacterDevice Open(IByteDevice backend, Text.Encoding encoding)
		{
			return backend.NotNull() ? new CharacterDevice(backend, encoding) : null;
		}
		public static ICharacterDevice Open(Uri.Locator resource)
		{
			return CharacterDevice.Open(ByteDevice.Open(resource));
		}
		public static ICharacterDevice Wrap(IByteDevice backend)
		{
			return CharacterDevice.Wrap(backend, Text.Encoding.Utf8);
		}
		public static ICharacterDevice Wrap(IByteDevice backend, Text.Encoding encoding)
		{
			return backend.NotNull() ? new CharacterDevice(backend, encoding) { dontClose = true } : null;
		}
		public static ICharacterDevice Create(Uri.Locator resource)
		{
			return CharacterDevice.Open(ByteDevice.Create(resource));
		}
		#endregion
		class PeekBuffer
		{
			readonly object @lock = new object();
			readonly Func<Tasks.Task<char?>> read;
			Tasks.Task<char?> data;
			public PeekBuffer(Func<Tasks.Task<char?>> read)
			{
				this.read = read;
			}
			public void Reset()
			{
				lock (this.@lock)
					this.data = null;
			}
			public Tasks.Task<char?> Peek()
			{
				lock (this.@lock)
					return this.data ?? (this.data = this.read());
			}
			public Tasks.Task<char?> Read()
			{
				lock (this.@lock)
				{
					var result = this.data ?? this.read();
					this.data = null;
					return result;
				}
			}
		}
	}
}
