// 
//  Worker.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2010-2012 Simon Mika
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

namespace Kean.Parallel
{
	internal class Worker
	{
		public int Number { get; private set; }
		public string Name { get; private set; }
		System.Threading.Thread thread;
		ThreadPool pool;
		object wakeUp;
		object semaphore;
		Collection.Queue<ITask> tasks;
		bool occupied;
		public bool Waiting { get { return thread.ThreadState == System.Threading.ThreadState.WaitSleepJoin; } }
		public bool Occupied
		{
			get
			{
				lock (this.semaphore)
					return this.occupied;
			}
			protected set
			{
				lock (this.semaphore)
					this.occupied = value;
			}
		}
		bool end;
		bool End
		{
			get { lock (this.semaphore) return this.end; }
			set { lock (this.semaphore) this.end = value; }
		}

		internal Worker(ThreadPool pool, int number)
		{
			this.pool = pool;
			this.wakeUp = new object();
			this.semaphore = new object();
			this.tasks = new Collection.Queue<ITask>();
			this.thread = new System.Threading.Thread(new System.Threading.ThreadStart(() =>
			{
				try
				{
					while (!this.End)
					{
						ITask task = null;
						do
						{
							if (task.NotNull())
								Error.Log.Call(() => {
									try
									{
										this.Occupied = true;
										task.Run();
									}
									catch (System.Threading.ThreadInterruptedException)
									{
									}
									catch (System.Threading.ThreadAbortException)
									{
										System.Threading.Thread.ResetAbort();
									}
									finally
									{
										task = null;
										this.Occupied = false;
									}
								}, (System.Exception e) => Error.Log.Append(Error.Level.Recoverable, string.Format("Worker {0} in Thread Pool {1} Failed", this.Name, this.pool.Name), e));
							lock (this.semaphore) task = this.tasks.Empty ? this.pool.Dequeue() : this.tasks.Dequeue();
						} while (!this.End && task.NotNull());
						if (!this.End)
							lock (this.wakeUp) System.Threading.Monitor.Wait(this.wakeUp);
					}
				}
				catch (System.Threading.ThreadAbortException)
				{
					System.Threading.Thread.ResetAbort();
				}
				catch (System.Threading.ThreadInterruptedException)
				{
				}
			})) { IsBackground = true };
			this.thread.Name = this.Name = pool.Name + ":" + number;
			this.Number = number;
			this.thread.Start();
		}
		~Worker()
		{
			Error.Log.Wrap((Action)this.Dispose)();
		}
		public void Dispose()
		{
			if (this.thread.NotNull())
			{
				this.End = true;
				this.thread.Interrupt();
				this.Start();
				this.thread.Join(100);
				this.thread.Abort();
				this.thread.Join(100);
				this.thread = null;
			}
		}
		public void Start()
		{
			lock (this.wakeUp) System.Threading.Monitor.Pulse(this.wakeUp);
		}
		public void Abort()
		{
			if (this.thread.NotNull())
				this.thread.Abort();
		}
		public void Enqueue(ITask task)
		{
			lock (this.semaphore)
				this.tasks.Enqueue(task);
			this.Start();
		}
	}
}
