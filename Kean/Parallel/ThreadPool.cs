// 
//  Worker.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2010-2013 Simon Mika
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
	public class ThreadPool :
		IDisposable
	{
		public int TaskCount { get { return this.tasks.Count; } }
		public string Name { get; private set; }
		public int QueueMaximum { get; set; }
		public int ThreadCount { get { return this.workers.Count; } }
		public int MinimumThreadCount { get; private set; }
		int maximumThreadCount;
		public int MaximumThreadCount 
		{
			get { return this.maximumThreadCount > this.MinimumThreadCount || this.maximumThreadCount == -1 ? this.maximumThreadCount : this.MinimumThreadCount; }
			set { this.maximumThreadCount = value; } 
		}
		int workerIndex = 0;
		Collection.IList<Worker> workers;
		Collection.Queue<ITask> tasks;
		public ThreadPool(string name) : this(name, System.Environment.ProcessorCount + 2) { }
		public ThreadPool(string name, int workers)
		{
			this.Name = name;
			this.tasks = new Collection.Queue<ITask>();
			this.MinimumThreadCount = workers;
			this.workers = new Collection.List<Worker>(this.MinimumThreadCount);
			for (int i = 0; i < this.MinimumThreadCount; i++)
				this.AddWorker();
		}
		~ThreadPool()
		{
			Error.Log.Wrap((Action)this.Dispose)();
		}
		public virtual void Dispose()
		{
			if (this.workers.NotNull())
			{
				foreach (Worker worker in this.workers)
					if (worker.NotNull() && !worker.Occupied)
						worker.Dispose();
				foreach (Worker worker in this.workers)
					if (worker.NotNull())
						worker.Dispose();
				this.workers = null;
			}
		}
		void AddWorker()
		{
			this.workers.Add(new Worker(this, this.workerIndex++));
		}
		#region Enqueue Action
		public void Enqueue(Action task) 
		{ 
			if(task.NotNull()) 
				this.Enqueue(new Task(task)); 
		}
		public void Enqueue<T>(Action<T> task, T argument) 
		{ 
			if(task.NotNull())
				this.Enqueue(new Task<T>(task, argument)); 
		}
		public void Enqueue<T1, T2>(Action<T1, T2> task, T1 argument1, T2 argument2) 
		{ 
			if(task.NotNull())
				this.Enqueue(new Task<T1, T2>(task, argument1, argument2)); 
		}
		public void Enqueue<T1, T2, T3>(Action<T1, T2, T3> task, T1 argument1, T2 argument2, T3 argument3)
		{
			if (task.NotNull())
				this.Enqueue(new Task<T1, T2, T3>(task, argument1, argument2, argument3));
		}
		public void Enqueue<T1, T2, T3, T4>(Action<T1, T2, T3, T4> task, T1 argument1, T2 argument2, T3 argument3, T4 argument4)
		{
			if (task.NotNull())
				this.Enqueue(new Task<T1, T2, T3, T4>(task, argument1, argument2, argument3, argument4));
		}
		public void Enqueue<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> task, T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5)
		{
			if (task.NotNull())
				this.Enqueue(new Task<T1, T2, T3, T4, T5>(task, argument1, argument2, argument3, argument4, argument5));
		}
		public void Enqueue<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> task, T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6)
		{
			if (task.NotNull())
				this.Enqueue(new Task<T1, T2, T3, T4, T5, T6>(task, argument1, argument2, argument3, argument4, argument5, argument6));
		}
		public void Enqueue<T1, T2, T3, T4, T5, T6, T7>(Action<T1, T2, T3, T4, T5, T6, T7> task, T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6, T7 argument7)
		{
			if (task.NotNull())
				this.Enqueue(new Task<T1, T2, T3, T4, T5, T6, T7>(task, argument1, argument2, argument3, argument4, argument5, argument6, argument7));
		}
		public void Enqueue<T1, T2, T3, T4, T5, T6, T7, T8>(Action<T1, T2, T3, T4, T5, T6, T7, T8> task, T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6, T7 argument7, T8 argument8)
		{
			if (task.NotNull())
				this.Enqueue(new Task<T1, T2, T3, T4, T5, T6, T7, T8>(task, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8));
		}
		public void Enqueue<T1, T2, T3, T4, T5, T6, T7, T8, TRest>(Action<T1, T2, T3, T4, T5, T6, T7, T8, TRest> task, T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6, T7 argument7, T8 argument8, TRest argumentRest)
		{
			if (task.NotNull())
				this.Enqueue(new Task<T1, T2, T3, T4, T5, T6, T7, T8, TRest>(task, argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argumentRest));
		}
		#endregion
		public void Enqueue(ITask task)
		{
			try
			{
				lock (this.tasks)
				{
					if (this.QueueMaximum > 0 && this.tasks.Count >= this.QueueMaximum)
					{
						this.tasks.Dequeue(); // Throw old tasks away if queue grows to much.
						//this.Log(Error.Entry.Create(Error.Level.Warning, string.Format("Thread Pool {0} reached queue maxium at {1}.", this.Name, this.QueueMaximum))); 
					}
					this.tasks.Enqueue(task);
				}
				lock (this.workers)
				{
					int waiting = -1;
					int i;
					for (i = 0; i < this.workers.Count; i++)
					{
						if (!workers[i].Occupied)
						{
							if (++waiting == 0)
								workers[i].Start();
							else if (this.ThreadCount <= this.MinimumThreadCount)
								break;
							else if (waiting == 2)
							{
								this.workers.Remove(i).Dispose();
								break;
							}
						}
					}
					if (waiting == -1 && (this.MaximumThreadCount > this.ThreadCount || this.MaximumThreadCount == -1))
						this.AddWorker();
				}
			}
			catch (ArgumentNullException e)
			{
				throw new Exception.ThreadPoolDisposed(e);
			}
		}
		#region Process
		public T Process<T>(Func<T> process)
		{
			T result = default(T);
			if (System.Threading.Thread.CurrentThread.Name.NotNull() && System.Threading.Thread.CurrentThread.Name.StartsWith(this.Name))
				result = process();
			else
			{
				System.Threading.AutoResetEvent wait = new System.Threading.AutoResetEvent(false);
				this.Enqueue(() =>
				{
					result = process();
					wait.Set();
				});
				wait.WaitOne();
			}
			return result;
		}
		public TOut Process<TIn, TOut>(Func<TIn, TOut> process, TIn argument)
		{
			TOut result = default(TOut);
			if (System.Threading.Thread.CurrentThread.Name.NotNull() && System.Threading.Thread.CurrentThread.Name.StartsWith(this.Name))
				result = process(argument);
			else
			{
				System.Threading.AutoResetEvent wait = new System.Threading.AutoResetEvent(false);
				this.Enqueue(m =>
				{
					result = process(m);
					wait.Set();
				}, argument);
				wait.WaitOne();
			}
			return result;
		}
		#endregion
		internal ITask Dequeue()
		{
			lock (this.tasks)
				return this.tasks.Dequeue();
		}
		public void Abort()
		{
			if (this.workers.NotNull())
			{
				foreach (Worker worker in this.workers)
					if (worker.NotNull() && worker.Occupied)
						worker.Abort();
			}
		}
		public void ForEachWorker(Action task)
		{
			if(task.NotNull())
				this.ForEachWorker(new Task(task));
		}
		public void ForEachWorker(Action<int> task)
		{
			lock (this.workers)
				foreach (Worker worker in this.workers)
					worker.Enqueue(new Task<int>(task, worker.Number));
		}
		public void ForEachWorker<T>(Action<T> task, T argument)
		{
			if (task.NotNull())
				this.ForEachWorker(new Task<T>(task, argument));
		}
		public void ForEachWorker(ITask task)
		{
			lock (this.workers)
				foreach (Worker worker in this.workers)
					worker.Enqueue(task);
		}
		static ThreadPool global;
		public static ThreadPool Global
		{
			set { ThreadPool.global = value; }
			get 
			{
				if (ThreadPool.global.IsNull())
					ThreadPool.global = new ThreadPool("Global");
				return ThreadPool.global;
			}
		}
	}
}
