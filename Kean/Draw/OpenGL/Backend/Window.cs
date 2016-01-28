// 
//  Window.cs
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
using Kean.Extension;
using Geometry2D = Kean.Math.Geometry2D;
using GL = OpenTK.Graphics.OpenGL.GL;
using Parallel = Kean.Parallel;
using Error = Kean.Error;

namespace Kean.Draw.OpenGL.Backend
{
	public abstract class Window :
		OpenTK.NativeWindow
	{
		public event Action Initialized;
		public event Action Draw;
		public Context Context { get; private set; }
		ThreadPool threadPool;
		public Parallel.ThreadPool ThreadPool { get { return this.threadPool; } }
		object @lock = new object();
		System.Threading.EventWaitHandle redrawSignal;
		bool makeVisible;
		public new bool Visible
		{
			get { return base.Visible; }
			set
			{
				if (value)
				{
					lock (this.@lock)
						this.makeVisible = true;
					this.Redraw();
				}
				else
					base.Visible = false;
			}
		}
		public Geometry2D.Single.Point Position
		{
			get { return new Geometry2D.Single.Point(base.Location.X, base.Location.Y); }
			set { base.Location = new System.Drawing.Point((int)value.X, (int)value.Y); }
		}
		public Geometry2D.Integer.Size Resolution
		{
			get { return new Geometry2D.Integer.Size(this.ClientSize.Width, this.ClientSize.Height); }
			set { base.ClientSize = new System.Drawing.Size(value.Width, value.Height); }
		}
		public string Clipboard
		{
			get { return System.Windows.Forms.Clipboard.GetText(); }
			set
			{
				if (value.NotNull() && value.Length > 0)
					System.Windows.Forms.Clipboard.SetText(value);
			}
		}
		bool exit;
		public bool Exit
		{
			get
			{
				lock (this.@lock)
					return this.exit;
			}
			set
			{
				lock (this.@lock)
					this.exit = true;
				this.Redraw();
			}
		}
		protected Window(Context context) :
			this()
		{
			this.Context = context;
		}
		protected Window() :
			base(1024, 768, "", OpenTK.GameWindowFlags.Default, OpenTK.Graphics.GraphicsMode.Default, OpenTK.DisplayDevice.Default)
		{
			this.threadPool = this.CreateThreadPool("OpenGL", 8);
			Backend.Context.Current = this.Context = this.Context ?? Backend.Context.Current ?? this.CreateContext();
			this.redrawSignal = new System.Threading.EventWaitHandle(true, System.Threading.EventResetMode.AutoReset);
		}
		~Window()
		{
			this.Dispose(false);
		}
		protected abstract Context CreateContext ();
		protected abstract ThreadPool CreateThreadPool (string name, int workers);
		public void Run ()
		{
			this.Initialized.Call();
			this.Runner();
		}
		void RePaint ()
		{
			if (this.WindowState != OpenTK.WindowState.Minimized && (this.Visible || this.makeVisible))
			{
				this.Draw.Call();
				if (this.threadPool.NotNull())
				{
#pragma warning disable 0618
					this.threadPool.MainContext.VSync = false; //TODO: VSync is deprecated - fix this whenever OpenTK has another solution.
#pragma warning restore
					this.threadPool.MainContext.SwapBuffers();
				}
				lock (this.@lock)
					if (this.makeVisible)
					{
						base.Visible = true;
						this.makeVisible = false;
					}
			}
		}
		protected virtual void Runner ()
		{
			this.OnResize(System.EventArgs.Empty);
			bool redraw = true;
			while (this.Exists && !this.Exit)
			{
				if (this.WindowState != OpenTK.WindowState.Minimized)
				{
					if (redraw)
						this.RePaint();
					base.ProcessEvents();
					redraw = this.redrawSignal.WaitOne(20);
				}
				//else
				//	base.WaitForNextEvent();
			}
		}
		/// <summary>
		/// If called, Draw will be called during next rendering callback.
		/// </summary>
		public void Redraw ()
		{
			this.redrawSignal.Set();
		}
		//protected override void OnPaint(EventArgs e)
		//{
		//	//base.OnPaint(e);
		//	this.RePaint();
		//}
		protected override void OnResize (EventArgs e)
		{
			base.OnResize(e);
			this.threadPool.MainContext.Update(this.WindowInfo);
			this.redrawSignal.Set();
		}
		protected override void OnFocusedChanged (EventArgs e)
		{
			base.OnFocusedChanged(e);
			this.redrawSignal.Set();
		}
		protected virtual void Dispose (bool disposing)
		{
			if (this.threadPool.NotNull())
			{
				this.threadPool.Delete();
				this.threadPool = null;
			}
			if (this.Context.NotNull())
			{
				this.Context.Dispose();
				Backend.Context.Current = this.Context = null;
			}
		}
		public sealed override void Dispose ()
		{
			this.Dispose(true);
			base.Dispose();
		}
		public static Window Create ()
		{
			// TODO: select OpenGL implementation
			return new OpenGL21.Window();
		}
	}
}
