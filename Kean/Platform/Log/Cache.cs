//
//  Cache
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2010 Simon Mika
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
using Error = Kean.Error;
using Collection = Kean.Collection;
using Basis = Kean;
using Kean.Extension;
using Kean.Collection.Extension;

namespace Kean.Platform.Log
{
	public class Cache :
		Basis.Synchronized,
		IDisposable
	{
		Collection.IQueue<Error.IError> log = new Collection.Array.Queue<Error.IError>();
		Collection.Array.List<Error.IError> cacheList = new Collection.Array.List<Error.IError>(100);
		Collection.IQueue<Error.IError> cache;
		public Error.Level LogThreshold { get; set; }
		public Error.Level AllThreshold { get; set; }
		public Collection.IList<Writer.Abstract> Writers { get; private set; }
		public int CacheSize 
		{
			get { lock(this.Lock) return this.cacheList.Capacity;}
			set 
			{
				lock (this.Lock)
				{
					while (value > this.cacheList.Count)
						this.ReduceCache();
					this.cacheList.Capacity = value;
				}
			}
		}
		public Cache()
		{
			this.LogThreshold = Kean.Error.Level.Message;
			this.AllThreshold = Kean.Error.Level.Critical;
			this.cache = new Collection.Wrap.ListQueue<Error.IError>(this.cacheList);
			this.Writers = new Kean.Collection.List<Writer.Abstract>();
		}
		#region IDisposable Members
		public void Dispose()
		{
			if (this.Writers.NotNull())
			{
				this.Writers.Apply(writer => writer.Close());
				this.Writers = null;
			}
		}
		#endregion
		public void Append(Error.Level level, string title, System.Exception exception)
		{
			this.Append(Error.Entry.Create(level, title, exception));
		}
		public void Append(Error.Level level, string title, string message)
		{
			this.Append(Error.Entry.Create(level, title, message));
		}
		public void Append(Error.IError entry)
		{
			lock (this.Lock)
			{
				if (entry.Level >= this.AllThreshold)
					this.log.Enqueue(this.cache);
				else
				{
					if (this.cacheList.Count >= this.CacheSize)
						this.ReduceCache();
					this.cache.Enqueue(entry);
				}
			}
		}
		void ReduceCache()
		{
			Error.IError entry = this.cache.Dequeue();
			if (entry.NotNull() && entry.Level >= this.LogThreshold)
				this.log.Enqueue(entry);
		}
		public void Flush()
		{
			lock (this.Lock)
			{
				Func<Error.IError, bool> save = this.Writers.Fold((w, a) => a += w.Append, (Func<Error.IError, bool>)null);
				while (!this.cache.Empty)
					this.ReduceCache();
				while (!this.log.Empty)
					save(this.log.Dequeue());
				this.Writers.Apply(w => w.Flush());
			}
		}
	}	
}

