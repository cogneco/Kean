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