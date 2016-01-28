//
//  Abstract
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2010 - 2011 Simon Mika
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
using Argument = Kean.Cli.Argument;
using Serialize = Kean.Serialize;
using Error = Kean.Error;

namespace Kean.Platform
{
	public abstract class Module :
		IDisposable
	{
		#region Properties
		internal Mode Mode { get; private set; }
		string name;
		[Serialize.Parameter("Name")]
		public string Name 
		{
			get { return this.name; }
			set
			{
				if (this.Application.NotNull())
				{
					Application application = this.Application;
					application.Unload(this.name);
					this.name = value;
					application.Load(this);
				}
				else
					this.name = value;
			}
		}
		Application application;
		public Application Application 
		{
			get { return this.application; }
			internal set 
			{
				this.application = value;
				this.loaded.Call(this);
				this.ApplicationChanged.Call(this.application);
			}
		}
		protected event Action<Application> ApplicationChanged;
		#endregion
		#region Events
		internal event Action<Module> loaded;
		#endregion
		#region Constructors
		protected Module(string name)
		{
			this.Name = name;
		}
		#endregion
		#region Implementors Interface
		internal protected virtual void AddArguments(Argument.Parser parser)
		{
		}
		internal protected virtual void Initialize() 
		{
			this.Mode = Mode.Initialized;
		}
		internal protected virtual void Start()
		{
			this.Mode = Mode.Started;
		}
		internal protected virtual void Stop()
		{
			this.Mode = Mode.Stopped;
		}
		internal protected virtual void Dispose()
		{
			this.Mode = Mode.Disposed;
		}
		#endregion
		public void WhenLoaded(Action<object> loaded)
		{
			this.WhenLoaded<object>(loaded);
		}
		public virtual void WhenLoaded<T>(Action<T> loaded) where T : class
		{
			if (this is T && !(this is Placeholder))
				loaded.Call(this as T);
			this.loaded += m =>
			{
				if (m is T && !(m is Placeholder))
					loaded.Call(m as T);
			};
		}
		internal Module Merge(Module other)
		{
			if (other.NotNull())
				this.loaded += other.loaded;
			return this;
		}
		#region IDisposable Members
		~Module()
		{
			Error.Log.Wrap((Action)(this as IDisposable).Dispose)();
		}
		void IDisposable.Dispose()
		{
			if (this.Mode != Mode.Disposed)
				this.Dispose();
		}
		#endregion
		public override string ToString()
		{
			return this.Name;
		}
	}
	public class Module<V> :
		Module
	{
		[Serialize.Parameter("Value")]
		public V Value { get; set; }

		#region Constructors
		public Module(string name, V value) :
			this(name)
		{
			this.Value = value;
		}
		public Module() :
			this("")
		{ }
		public Module(string name) :
			base(name)
		{ }
		#endregion
		public override void WhenLoaded<T>(Action<T> loaded)
		{
			if (this.Value is T)
				loaded.Call(this.Value as T);
			else if (this is T)
				loaded.Call(this as T);
			this.loaded += m =>
			{
				if (m is Module<T>)
					loaded.Call((m as Module<T>).Value);
				else if (this is T)
					loaded.Call(this as T);
			};
		}
		protected internal override void Dispose()
		{
			if (this.Value is IDisposable)
			{
				(this.Value as IDisposable).Dispose();
				this.Value = default(V);
			}
			base.Dispose();
		}
	}
}
