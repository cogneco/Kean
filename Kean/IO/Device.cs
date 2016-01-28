// 
//  BlockDevice.cs
//  
//  Author:
//       Simon Mika <simon@mika.se>
//  
//  Copyright (c) 2013-2014 Simon Mika
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
using Generic = System.Collections.Generic;
using Kean.IO.Extension;

namespace Kean.IO
{
	public class Device :
	IBlockDevice,
	IByteDevice,
	ICharacterDevice
	{
		System.IO.Stream stream;
		public System.Text.Encoding Encoding { get; set; }
		public bool Wrapped { get; set; }
		public bool FixedLength { get; set; }
		readonly object peekedLock = new object();
		Collection.IVector<byte> peeked;
		#region Constructors
		protected Device(System.IO.Stream stream) :
			this(stream, "stream:///")
		{
		}
		protected Device(System.IO.Stream stream, Uri.Locator resource)
		{
			this.stream = stream;
			this.Resource = resource;
			this.Encoding = System.Text.Encoding.UTF8;
		}
		#endregion
		#region IBlockInDevice
		Collection.IVector<byte> RawRead()
		{
			var buffer = new byte[64 * 1024];
			int count = this.stream.Read(buffer, 0, buffer.Length);
			return count > 0 ? new Collection.Slice<byte>(buffer, 0, count) : null;
		}
		public Collection.IVector<byte> Peek()
		{
			lock (this.peekedLock)
				return this.peeked ?? (this.peeked = this.RawRead());
		}
		public Collection.IVector<byte> Read()
		{
			lock (this.peekedLock)
			{
				Collection.IVector<byte> result = this.peeked ?? this.RawRead();
				this.peeked = null;
				return result;
			}
		}
		#endregion
		#region IByteInDevice Members
		byte? IByteInDevice.Peek()
		{
			lock (this.peekedLock)
			{
				var peeked = this.Peek();
				return peeked.NotNull() && peeked.Count > 0 ? (byte?)peeked[0] : null;
			}
		}
		byte? IByteInDevice.Read()
		{
			lock (this.peekedLock)
			{
				var peeked = this.Peek();
				byte? result;
				if (peeked.NotNull() && peeked.Count > 0)
				{
					result = peeked[0];
					this.peeked = peeked.Count > 1 ? peeked.Slice(1) : null;
				}
				else
					result = null;
				return result;
			}
		}
		#endregion
		#region ICharacterInDevice Members
		char? ICharacterInDevice.Peek()
		{
			lock (this.peekedLock)
			{
				var peeked = this.Peek();
				return peeked.NotNull() && peeked.Count > 0 ? (char?)peeked.Decode(this.Encoding).FirstOrNull() : null;
			}
		}
		char? ICharacterInDevice.Read()
		{
			return ((IByteInDevice)this).AsEnumerable().Decode(this.Encoding).FirstOrNull();
		}
		#endregion
		#region ICharacterOutDevice Members
		public bool Write(System.Collections.Generic.IEnumerable<char> buffer)
		{
			return this.Write(new Collection.Enumerable<byte>(() => new Encoder(buffer.GetEnumerator(), this.Encoding)));
		}
		#endregion
		#region IBlockOutDevice
		public bool Write(Collection.IVector<byte> buffer)
		{
			this.Flush();
			bool result = true;
			try
			{
				byte[] array = buffer.ToArray(); // TODO: fix this with some kind of array-slice api
				this.stream.Write(array, 0, array.Length);
				if (this.AutoFlush)
					this.Flush();
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
		public bool Write(Generic.IEnumerable<byte> buffer)
		{
			bool result = true;
			try
			{
				lock (this.outBufferLock)
					this.outBuffer.Add(buffer);
				if (this.AutoFlush)
					this.Flush();
			}
			catch (System.Exception)
			{
				result = false;
			}
			return result;
		}
		#endregion
		#region IInDevice Members
		public bool Empty { get { return !this.Peek().NotNull(); } }
		public bool Readable { get { return this.stream.NotNull() && this.stream.CanRead && !(this.FixedLength && this.Empty); } }
		#endregion
		#region IOutDevice Members
		public bool Writable { get { return this.stream.NotNull() && this.stream.CanWrite; } }
		public bool AutoFlush { get; set; }
		bool FlushBuffer()
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
			{
				this.stream.Write(array, 0, count);
			}
			return result;
		}
		public bool Flush()
		{
			this.FlushBuffer();
			bool result;
			if (result = this.stream.NotNull())
				this.stream.Flush();
			return result; 
		}
		#endregion
		#region IDevice Members
		public Uri.Locator Resource { get; private set; }
		public virtual bool Opened { get { return this.Readable || this.Writable; } }
		public virtual bool Close()
		{
			bool result;
			if (result = this.stream.NotNull())
			{
				this.Flush();
				if (!this.Wrapped)
					this.stream.Close();
				this.stream = null;
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
						if (mode == System.IO.FileMode.Open)
						{
							try
							{
								using (var client = new System.Net.WebClient())
									result = new Device(new System.IO.MemoryStream(client.DownloadData(resource))) {
										Resource = resource,
										FixedLength = true
									};
							}
							catch (System.Net.WebException)
							{
								result = null;
							}
						}
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
