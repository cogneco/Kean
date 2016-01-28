// 
//  ThreadPool.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2013 Simon Mika
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
using Kean.Collection.Extension;
using Geometry2D = Kean.Math.Geometry2D;
using Kean.Extension;
using GL = OpenTK.Graphics.OpenGL.GL;
using Parallel = Kean.Parallel;

namespace Kean.Draw.OpenGL.Backend
{
	public abstract class ThreadPool :
		Parallel.ThreadPool,
		IDisposable
	{
		public bool NoDispose { get; set; }

		OpenTK.Graphics.IGraphicsContext[] contexts;
		// TODO: should be internal as soon as possible.
		public OpenTK.Graphics.IGraphicsContext MainContext { get { return this.contexts[0]; } }

		protected ThreadPool(OpenTK.Platform.IWindowInfo windowInformation, string name, int workers) :
			base(name, workers)
		{
			OpenTK.Graphics.GraphicsContext.ShareContexts = true;
			this.contexts = new OpenTK.Graphics.GraphicsContext[workers + 1];
			try
			{
				this.contexts[0] = this.CreateGraphicsContext(windowInformation);
				this.contexts[0].MakeCurrent(windowInformation);
#pragma warning disable 0618
				this.contexts[0].VSync = false; //TODO: VSync is deprecated - fix this whenever OpenTK has another solution.
#pragma warning restore
				(this.contexts[0] as OpenTK.Graphics.IGraphicsContextInternal).LoadAll();

				for (int i = 1; i < this.contexts.Length; i++)
					this.contexts[i] = this.CreateGraphicsContext(windowInformation);
				this.ForEachWorker(c => this.contexts[c + 1].MakeCurrent(windowInformation));
			}
			catch (System.Exception e)
			{
				this.Dispose();
				new Exception.ContextNotCreatable(e).Throw();
			}
		}
		protected abstract OpenTK.Graphics.GraphicsContext CreateGraphicsContext(OpenTK.Platform.IWindowInfo windowInformation);

		#region IDisposable Members
		public override void Dispose()
		{
			if (!this.NoDispose)
				this.Delete();
		}
		#endregion
		public void Delete()
		{
			if (this.contexts.NotNull())
			{
				object @lock = new object();
				int count = this.contexts.Length - 1;
				this.ForEachWorker(c =>
				{
					this.contexts[c + 1].Dispose();
					lock (@lock)
					{
						count--;
						System.Threading.Monitor.PulseAll(@lock);
					}
				}
				);
				lock (@lock)
					while (count > 0)
						System.Threading.Monitor.Wait(@lock);

				this.contexts[0].Dispose();

				this.contexts = null;
				base.Dispose();
			}
		}
	}
}
