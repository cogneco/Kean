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

namespace Kean.IO
{
	public abstract class SeekableDevice :
			ISeekableDevice
	{
		protected System.IO.Stream backend;
		readonly bool dontClose;
		public Uri.Locator Resource { get; }
		public bool AutoFlush { get; set; }
		public bool Opened { get { return this.backend.NotNull(); } }
		public bool Readable { get { return this.backend?.CanRead ?? false; } }
		public bool Writable { get { return this.backend?.CanWrite ?? false; } }
		public bool Seekable { get { return this.backend?.CanSeek ?? false; } }
		public abstract Tasks.Task<bool> Empty { get; }
		protected abstract Tasks.Task<int> GetPeekedCount();
		async Tasks.Task<long?> getPosition() {
			return this.backend?.Position - await this.GetPeekedCount();
		}
		public Tasks.Task<long?> Position
		{
			get { return this.getPosition(); }
		}
		public long? Size { get { return this.backend?.Length; } }
		public SeekableDevice(System.IO.Stream backend, Uri.Locator resource, bool dontClose = false)
		{
			this.backend = backend;
			this.Resource = resource;
			this.dontClose = dontClose;
		}
		void IDisposable.Dispose()
		{
			this.Close().Wait();
		}
		public Tasks.Task<bool> Close()
		{
			return Tasks.Task.Run(() =>
			{
				bool r;
				if (r = this.backend != null && !this.dontClose)
				{
					this.backend.Dispose();
					this.backend = null;
				}
				return r;
			});
		}
		protected abstract void OnSeek();
		public async Tasks.Task<bool> Flush()
		{
			bool result;
			if (result = this.backend.NotNull())
				await this.backend.FlushAsync();
			return result;
		}
		public Tasks.Task<long?> Seek(long delta)
		{
			return Tasks.Task.Run(async () =>
			{
				var result = this?.backend.Seek(delta - await this.GetPeekedCount(), System.IO.SeekOrigin.Current);
				this.OnSeek();
				return result;
			});
		}
		public async Tasks.Task<long?> SeekTo(long position) {
			await Tasks.Task.Run(() =>
			{
				this.backend.Seek(position < 0 ? -position : position, position < 0 ? System.IO.SeekOrigin.End : System.IO.SeekOrigin.Begin);
				this.OnSeek();
			});
			return await this.getPosition();
		}
	}
}
