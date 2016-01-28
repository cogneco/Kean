// 
//  ByteDevice.cs
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
using Generic = System.Collections.Generic;

namespace Kean.IO
{
	public class ByteDevice :
		IByteDevice
	{
		byte? peeked;
		System.IO.Stream stream;
		public bool Wrapped { get; set; }
		public bool FixedLength { get; set; }
		#region Constructors
		protected ByteDevice(System.IO.Stream stream)
		{
			this.stream = stream;
			this.Resource = "stream:///";
		}
		#endregion
		readonly byte[] inBuffer = new byte[64 * 1024];
		int inBufferEnd;
		int inBufferStart;
		byte? RawRead()
		{
			if (this.inBufferStart >= this.inBufferEnd && this.stream.NotNull())
			{
				try
				{
					this.inBufferEnd = this.stream.Read(this.inBuffer, 0, this.inBuffer.Length);
				}
				catch (ObjectDisposedException)
				{
					this.inBufferEnd = 0;
				}
				finally
				{
					this.inBufferStart = 0;
				}
			}
			return this.inBufferStart == this.inBufferEnd ? null : (byte?)this.inBuffer[this.inBufferStart++];
		}
		#region IByteInDevice Members
		public byte? Peek()
		{
			return this.peeked.HasValue ? this.peeked : this.peeked = this.RawRead();
		}
		public byte? Read()
		{
			byte? result;
			if (this.peeked.HasValue)
			{
				result = this.peeked;
				this.peeked = null;
			}
			else
				result = this.RawRead();
			return result;
		}
		#endregion
		#region IByteOutDevice Members
		object outBufferLock = new object();
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
		public bool Empty { get { return !this.Peek().HasValue; } }
		public bool Readable { get { return this.stream.NotNull() && this.stream.CanRead && !(this.FixedLength && this.Empty); } }
		#endregion
		#region IOutDevice Members
		public bool Writable { get { return this.stream.NotNull() && this.stream.CanWrite; } }
		public bool AutoFlush { get; set; }
		public bool Flush()
		{
			byte[] array;
			int count;
			lock (this.outBufferLock)
			{
				array = (byte[])this.outBuffer;
				count = this.outBuffer.Count;
				this.outBuffer = new Collection.Array.List<byte>(this.outBuffer.Capacity);
			}
			bool result = this.stream.NotNull();
			if (result && count > 0)
			{
				this.stream.Write(array, 0, count);
				this.stream.Flush();
			}
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
		public static IByteDevice Open(System.IO.Stream stream)
		{
			return stream.NotNull() ? new ByteDevice(stream) : null;
		}
		public static IByteDevice Open(Uri.Locator resource)
		{
			return ByteDevice.Open(resource, System.IO.FileMode.Open);
		}
		public static IByteDevice Open(Uri.Locator input, Uri.Locator output)
		{
			return ByteDeviceCombiner.Open(ByteDevice.Open(input), ByteDevice.Create(output));
		}
		static IByteDevice Open(Uri.Locator resource, System.IO.FileMode mode)
		{
			IByteDevice result = null;
			if (resource.NotNull())
				switch (resource.Scheme)
				{
					case "assembly":
						result = resource.Authority == "" ? ByteDevice.Open(System.Reflection.Assembly.GetEntryAssembly(), resource.Path) : ByteDevice.Open(System.Reflection.Assembly.Load(new System.Reflection.AssemblyName(resource.Authority)), resource.Path);
						break;
					case "file":
						try
						{
							System.IO.FileStream stream = System.IO.File.Open(System.IO.Path.GetFullPath(resource.PlatformPath), mode, System.IO.FileAccess.ReadWrite, System.IO.FileShare.ReadWrite);
							if (stream.NotNull())
								result = new ByteDevice(stream) { Resource = resource, FixedLength = true }; 
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
								using (System.Net.WebClient client = new System.Net.WebClient())
									result = new ByteDevice(new System.IO.MemoryStream(client.DownloadData(resource))) {
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
		public static IByteDevice Open(System.Reflection.Assembly assembly, Uri.Path resource)
		{
			return new ByteDevice(assembly.GetManifestResourceStream(assembly.GetName().Name + ((string)resource).Replace('/', '.'))) {
				Resource = new Uri.Locator("assembly", assembly.GetName().Name, resource),
				FixedLength = true
			};
		}
		#endregion
		#region Create
		public static IByteDevice Create(Uri.Locator resource)
		{
			IByteDevice result = ByteDevice.Open(resource, System.IO.FileMode.Create);
			if (result.IsNull() && resource.NotNull())
			{
				System.IO.Directory.CreateDirectory(resource.Path.FolderPath.PlatformPath);
				result = ByteDevice.Open(resource, System.IO.FileMode.Create);
			}
			return result;
		}
		#endregion
		#region Wrap
		public static IByteDevice Wrap(System.IO.Stream stream)
		{
			return stream.NotNull() ? new ByteDevice(stream) { Wrapped = true } : null;
		}
		#endregion
		#endregion
	}
}
