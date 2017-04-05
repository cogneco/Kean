// Copyright (C) 2013-2014, 2017	Simon Mika <simon@mika.se>
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
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.	See the
// GNU Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with Kean.	If not, see <http://www.gnu.org/licenses/>.
//

using System;
using Kean.Extension;
using Kean.Collection.Extension;
using Generic = System.Collections.Generic;
using Tasks = System.Threading.Tasks;
using Kean.IO.Extension;
using Kean.Text.Extension;

namespace Kean.IO
{
	public class Device :
	IBlockDevice,
	IByteDevice,
	ICharacterDevice
	{
		System.IO.Stream stream;
		public Text.Encoding Encoding { get; set; }
		public bool Wrapped { get; set; }
		public bool FixedLength { get; set; }
		readonly object peekedLock = new object();
		Tasks.Task<Collection.IBlock<byte>> peeked;
		#region Constructors
		protected Device(System.IO.Stream stream) :
			this(stream, "stream:///")
		{
		}
		protected Device(System.IO.Stream stream, Uri.Locator resource)
		{
			this.stream = stream;
			this.Resource = resource;
			this.Encoding = Text.Encoding.Utf8;
		}
		#endregion
		#region IBlockInDevice
		async Tasks.Task<Collection.IBlock<byte>> RawRead()
		{
			var buffer = new byte[64 * 1024];
			int count = await this.stream.ReadAsync(buffer, 0, buffer.Length);
			return count > 0 ? new Collection.Slice<byte>(buffer, 0, count) : null;
		}
		public Tasks.Task<Collection.IBlock<byte>> Peek()
		{
			lock (this.peekedLock)
				return this.peeked ?? (this.peeked = this.RawRead());
		}
		public Tasks.Task<Collection.IBlock<byte>> Read()
		{
			lock (this.peekedLock)
			{
				var result = this.peeked ?? this.RawRead();
				this.peeked = null;
				return result;
			}
		}
		#endregion
		#region IByteInDevice Members
		async Tasks.Task<byte?> IByteInDevice.Peek()
		{
			var peeked = await this.Peek();
			return peeked.NotNull() && peeked.Count > 0 ? (byte?)peeked[0] : null;
		}
		async Tasks.Task<byte?> IByteInDevice.Read()
		{
			byte? result = null;
			Tasks.Task done;
			lock (this.peekedLock)
			{
				done = this.peeked = this.Peek().Then(p =>
				{
					Collection.IBlock<byte> r = null;
					if (p.NotNull() && p.Count > 0)
					{
						result = p[0];
						r = p.Count > 1 ? p.Slice(1) : null;
					}
					else
						result = null;
					return r;
				});
			}
			await done;
			return result;
		}
		#endregion
		#region ICharacterInDevice Members
		async Tasks.Task<char?> ICharacterInDevice.Peek()
		{
			var peeked = await this.Peek();
			return peeked.NotNull() && peeked.Count > 0 ? (char?)peeked.Decode(this.Encoding).FirstOrNull() : null;
		}
		Tasks.Task<char?> ICharacterInDevice.Read()
		{
			return ((IByteInDevice)this).GetEnumerator().Decode(this.Encoding).First();
		}
		#endregion
		#region ICharacterOutDevice Members
		public Tasks.Task<bool> Write(Generic.IEnumerator<char> buffer)
		{
			return this.Write(buffer.Encode(this.Encoding));
		}
		#endregion
		#region IBlockOutDevice
		public async Tasks.Task<bool> Write(Collection.IBlock<byte> buffer)
		{
			await this.Flush();
			bool result = true;
			try
			{
				byte[] array = buffer.ToArray(); // TODO: fix this with some kind of array-slice api
				await this.stream.WriteAsync(array, 0, array.Length);
				if (this.AutoFlush)
					await this.Flush();
			}
			catch (System.Exception)
			{
				result = false;
			}
			return result;
		}
		#endregion
		#region IByteOutDevice Members
		readonly object outBufferLock = new object();
		Collection.Array.List<byte> outBuffer = new Collection.Array.List<byte>();
		public async Tasks.Task<bool> Write(Generic.IEnumerator<byte> buffer)
		{
			bool result = true;
			try
			{
				lock (this.outBufferLock)
					this.outBuffer.Add(buffer);
				if (this.AutoFlush)
					result = await this.Flush();
			}
			catch (System.Exception)
			{
				result = false;
			}
			return result;
		}
		#endregion
		#region IInDevice Members
		async Tasks.Task<bool> EmptyHelper() { return (await this.Peek()).IsNull(); }
		public Tasks.Task<bool> Empty { get { return this.EmptyHelper(); } }
		public bool Readable { get { return this.stream.NotNull() && this.stream.CanRead; } }
		#endregion
		#region IOutDevice Members
		public bool Writable { get { return this.stream.NotNull() && this.stream.CanWrite; } }
		public bool AutoFlush { get; set; }
		async Tasks.Task<bool> FlushBuffer()
		{
			byte[] array;
			int count;
			lock (this.outBufferLock)
			{
				array = (byte[])this.outBuffer;
				count = this.outBuffer.Count;
				this.outBuffer = new Collection.Array.List<byte>(this.outBuffer.Capacity);
			}
			bool result;
			if (result = count > 0)
				await this.stream.WriteAsync(array, 0, count);
			return result;
		}
		public async Tasks.Task<bool> Flush()
		{
			await this.FlushBuffer();
			bool result;
			if (result = this.stream.NotNull())
				await this.stream.FlushAsync();
			return result;
		}
		#endregion
		#region IDevice Members
		public Uri.Locator Resource { get; private set; }
		public virtual bool Opened { get { return this.Readable || this.Writable; } }
		public virtual async Tasks.Task<bool> Close()
		{
			bool result;
			if (result = this.stream.NotNull())
			{
				await this.Flush();
				if (!this.Wrapped)
					this.stream.Dispose();
				this.stream = null;
			}
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
		#region Open
		public static Device Open(System.IO.Stream stream)
		{
			return stream.NotNull() ? new Device(stream) : null;
		}
		public static Device Open(Uri.Locator resource)
		{
			return Device.Open(resource, System.IO.FileMode.Open);
		}
		static Device Open(Uri.Locator resource, System.IO.FileMode mode)
		{
			Device result = null;
			if (resource.NotNull())
				switch (resource.Scheme)
				{
					case "assembly":
						result = resource.Authority == "" ? Device.Open(System.Reflection.Assembly.GetEntryAssembly(), resource.Path) : Device.Open(System.Reflection.Assembly.Load(new System.Reflection.AssemblyName(resource.Authority)), resource.Path);
						break;
					case "file":
						try
						{
							System.IO.FileStream stream = System.IO.File.Open(System.IO.Path.GetFullPath(resource.PlatformPath), mode, System.IO.FileAccess.ReadWrite, System.IO.FileShare.ReadWrite);
							if (stream.NotNull())
								result = new Device(stream, resource) { FixedLength = true };
						}
						catch (System.IO.DirectoryNotFoundException)
						{
							result = null;
						}
						catch (System.IO.FileNotFoundException)
						{
							result = null;
						}
						break;
					case "http":
					case "https":
						break;
				}
			return result;
		}
		public static Device Open(System.Reflection.Assembly assembly, Uri.Path resource)
		{
			return new Device(assembly.GetManifestResourceStream(assembly.GetName().Name + ((string)resource).Replace('/', '.'))) {
				Resource = new Uri.Locator("assembly", assembly.GetName().Name, resource),
				FixedLength = true
			};
		}
		#endregion
		#region Create
		public static Device Create(Uri.Locator resource)
		{
			Device result = Device.Open(resource, System.IO.FileMode.Create);
			if (result.IsNull() && resource.NotNull())
			{
				System.IO.Directory.CreateDirectory(resource.Path.FolderPath.PlatformPath);
				result = Device.Open(resource, System.IO.FileMode.Create);
			}
			return result;
		}
		#endregion
		#region Wrap
		public static Device Wrap(System.IO.Stream stream)
		{
			return stream.NotNull() ? new Device(stream) { Wrapped = true } : null;
		}
		#endregion
		#endregion
	}
}
