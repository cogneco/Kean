// Copyright (C) 2013, 2016  Simon Mika <simon@mika.se>
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
using Kean.Extension;
using Kean.Collection.Extension;

namespace Kean.IO
{
	public class BlockDevice :
			SeekableDevice,
			ISeekableBlockDevice
	{
		PeekBuffer peeked;
		protected override Tasks.Task<int> GetPeekedCount() { return this.peeked.Count; }
		public override Tasks.Task<bool> Empty { get { return this.Peek().ContinueWith(value => value.NotNull()); } }
		internal BlockDevice(System.IO.Stream backend, Uri.Locator resource, bool dontClose = false) :
			base(backend, resource, dontClose)
		{
			this.peeked = new PeekBuffer(async () =>
			{
				var buffer = new byte[64 * 1024];
				int count = await this.backend.ReadAsync(buffer, 0, buffer.Length);
				return count > 0 ? buffer.Slice(0, count) : null;
			});
		}
		protected override void OnSeek()
		{
			this.peeked.Reset();
		}
		public Tasks.Task<Collection.IBlock<byte>> Peek()
		{
			return this.peeked.Peek();
		}
		public Tasks.Task<Collection.IBlock<byte>> Read()
		{
			return this.peeked.Read();
		}
		public async Tasks.Task<bool> Write(Collection.IBlock<byte> buffer)
		{
			bool result = true;
			try
			{
				int seek = this.peeked.Reset();
				if (seek != 0)
					await this.Seek(seek);
				byte[] array = buffer.AsArray();
				await this.backend.WriteAsync(array, 0, array.Length);
				if (this.AutoFlush)
					await this.Flush();
			}
			catch (System.Exception)
			{
				result = false;
			}
			return result;
		}
		#region Static Open, Wrap & Create
		#region Open
		public static IBlockDevice Open(System.IO.Stream stream, Uri.Locator resource = null)
		{
			return stream.NotNull() ? new BlockDevice(stream, resource) : null;
		}
		public static IBlockDevice Open(Uri.Locator resource)
		{
			return BlockDevice.Open(resource, System.IO.FileMode.Open);
		}
		static IBlockDevice Open(Uri.Locator resource, System.IO.FileMode mode)
		{
			IBlockDevice result = null;
			if (resource.NotNull())
				switch (resource.Scheme)
				{
					case "assembly":
						result = resource.Authority == "" ? BlockDevice.Open(System.Reflection.Assembly.GetEntryAssembly(), resource.Path) : BlockDevice.Open(System.Reflection.Assembly.Load(new System.Reflection.AssemblyName(resource.Authority)), resource.Path);
						break;
					case "file":
						try
						{
							System.IO.FileStream stream = System.IO.File.Open(System.IO.Path.GetFullPath(resource.PlatformPath), mode, System.IO.FileAccess.ReadWrite, System.IO.FileShare.ReadWrite);
							if (stream.NotNull())
								result = new BlockDevice(stream, resource);
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
							// TODO: support http and https.
						}
						break;
				}
			return result;
		}
		public static IBlockDevice Open(System.Reflection.Assembly assembly, Uri.Path resource)
		{
			return new BlockDevice(assembly.GetManifestResourceStream(assembly.GetName().Name + ((string)resource).Replace('/', '.')), new Uri.Locator("assembly", assembly.GetName().Name, resource));
		}
		#endregion
		#region Create
		public static IBlockDevice Create(Uri.Locator resource)
		{
			IBlockDevice result = BlockDevice.Open(resource, System.IO.FileMode.Create);
			if (result.IsNull() && resource.NotNull())
			{
				System.IO.Directory.CreateDirectory(resource.Path.FolderPath.PlatformPath);
				result = BlockDevice.Open(resource, System.IO.FileMode.Create);
			}
			return result;
		}
		#endregion
		#region Wrap
		public static IBlockDevice Wrap(System.IO.Stream stream, Uri.Locator resource = null)
		{
			return stream.NotNull() ? new BlockDevice(stream, resource, true) : null;
		}
		#endregion
		#endregion
		class PeekBuffer
		{
			readonly object @lock = new object();
			readonly Func<Tasks.Task<Collection.IBlock<byte>>> read;
			Tasks.Task<Collection.IBlock<byte>> data;
			int? count;
			async Tasks.Task<int> GetCount() { return this.count ?? (await this.data)?.Count ?? 0; }
			public Tasks.Task<int> Count { get { return this.GetCount(); } }
			public PeekBuffer(Func<Tasks.Task<Collection.IBlock<byte>>> read)
			{
				this.read = read;
			}
			public int Reset()
			{
				int result;
				lock (this.@lock)
				{
					this.data = null;
					result = this.count ?? 0;
					this.count = 0;
				}
				return result;
			}
			public Tasks.Task<Collection.IBlock<byte>> Read()
			{
				Tasks.Task<Collection.IBlock<byte>> result;
				lock (this.@lock)
				{
					result = this.data;
					this.data = null;
					this.count = 0;
				}
				return result ?? this.read();
			}
			public async Tasks.Task<Collection.IBlock<byte>> Peek()
			{
				Tasks.Task<Collection.IBlock<byte>> task;
				bool updateCount;
				lock (this.@lock)
				{
					if (updateCount = this.data.NotNull())
						task = this.data;
					else
					{
						this.count = null;
						task = this.data = this.read();
					}
				}
				var result = await task;
				if (updateCount)
					lock (this.@lock)
						this.count = result?.Count ?? 0;
				return result;
			}
		}
	}
}
