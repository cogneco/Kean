//
//  Application
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2010 - 2012 Simon Mika
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
using Error = Kean.Error;
using Argument = Kean.Cli.Argument;
using Serialize = Kean.Serialize;
using Kean.Reflect.Extension;
using Uri = Kean.Uri;
using Parallel = Kean.Parallel;
using Kean.Collection;

namespace Kean.Platform
{
	public class Application :
		IDisposable
	{
		#region Modules
		[Serialize.Parameter("Module")]
		protected Collection.IList<Module> Modules { get; private set; }
		public Module this[string name]
		{
			get
			{
				Module result = this.Modules.Find(item => item.Name == name);
				if (result.IsNull())
					this.Modules.Add(result = new Placeholder(name));
				return result;
			}
		}
		#endregion
		#region Properties
		public string Product { get; private set; }
		public string Version { get; private set; }
		public string Company { get; private set; }
		public string Copyright { get; private set; }
		public string Description { get; private set; }
		public System.Drawing.Icon Icon { get; private set; }

		public string[] CommandLine { get { return System.Environment.GetCommandLineArgs(); } }
		public string Executable { get { return System.IO.Path.GetFullPath(this.CommandLine[0]); } }
		public string ExecutablePath { get { return System.IO.Path.GetDirectoryName(this.Executable); } }
		public IRunner Runner { get; set; }
		[Serialize.Parameter]
		public bool CatchErrors { get { return Error.Log.CatchErrors; } set { Error.Log.CatchErrors = value; } }
		public Environment Environment { get { return Environment.Current; } }
		#region State
		Mode mode;
		public Mode Mode
		{
			get { return this.mode; }
			private set
			{
				switch (this.mode)
				{
					case Mode.Created:
						if (value == Mode.Initialized)
						{
							this.mode = Mode.Initialized;
							lock (this.onInitializedLock)
								this.onInitialized.Call();
						}
						else if (value == Mode.Disposed)
							this.mode = Mode.Disposed;
						break;
					case Mode.Initialized:
						if (value == Mode.Started)
						{
							this.mode = Mode.Started;
							lock (this.onStartedLock)
								this.onStarted.Call();
						}
						else if (value == Mode.Disposed)
							this.mode = Mode.Disposed;
						break;
					case Mode.Started:
						if (value == Mode.Stopped)
						{
							this.mode = Mode.Stopped;
							lock (this.onStoppedLock)
								this.onStopped.Call();
						}
						break;
					case Mode.Stopped:
						if (value == Mode.Disposed)
							this.mode = Mode.Disposed;
						break;
					case Mode.Disposed:
						break;
				}
			}
		}
		#endregion
		#endregion
		#region Events
		object onIdleLock = new object();
		Action onIdle;
		public event Action OnIdle
		{
			add { lock (this.onIdleLock) this.onIdle += value; }
			remove { lock (this.onIdleLock) this.onIdle -= value; }
		}
		object onNextIdleLock = new object();
		Action onNextIdle;
		public event Action OnNextIdle
		{
			add { lock (this.onNextIdleLock) this.onNextIdle += value; }
			remove { lock (this.onNextIdleLock) this.onNextIdle -= value; }
		}
		object onInitializedLock = new object();
		Action onInitialized;
		public event Action OnInitialized
		{
			add { lock (this.onInitializedLock) this.onInitialized += value; }
			remove { lock (this.onInitializedLock) this.onInitialized -= value; }
		}
		object onStartedLock = new object();
		Action onStarted;
		public event Action OnStarted
		{
			add { lock (this.onStartedLock) this.onStarted += value; }
			remove { lock (this.onStartedLock) this.onStarted -= value; }
		}
		object onStoppedLock = new object();
		Action onStopped;
		public event Action OnStopped
		{
			add { lock (this.onStoppedLock) this.onStopped += value; }
			remove { lock (this.onStoppedLock) this.onStopped -= value; }
		}
		object onClosedLock = new object();
		Action onClosed;
		public event Action OnClosed
		{
			add { lock (this.onClosedLock) this.onClosed += value; }
			remove { lock (this.onClosedLock) this.onClosed -= value; }
		}
		public event Action<Error.IError> OnError
		{
			add { Error.Log.OnAppend += value; }
			remove { Error.Log.OnAppend -= value; }
		}
		#endregion
		#region Constructors
		public Application()
		{
			Collection.Hooked.List<Module> modules = new Collection.Hooked.List<Module>();
			modules.Added += (index, module) => module.Application = this;
			modules.Replaced += (index, old, @new) =>
			{
				old.Application = null;
				@new.Application = this;
			};
			modules.Removed += (index, module) => module.Application = null;
			this.Modules = modules;
			#region Initialize Properties
			System.Reflection.Assembly assembly = System.Reflection.Assembly.GetEntryAssembly() ?? System.Reflection.Assembly.GetCallingAssembly();
			// Product
			System.Reflection.AssemblyProductAttribute[] productAttribute = (System.Reflection.AssemblyProductAttribute[])assembly.GetCustomAttributes(typeof(System.Reflection.AssemblyProductAttribute), true);
			if (productAttribute != null && productAttribute.Length > 0)
				this.Product = productAttribute[0].Product;
			// Version
			System.Reflection.AssemblyFileVersionAttribute version = Attribute.GetCustomAttribute(assembly, typeof(System.Reflection.AssemblyFileVersionAttribute)) as System.Reflection.AssemblyFileVersionAttribute;
			if (version != null)
				this.Version = version.Version;
			// Company
			System.Reflection.AssemblyCompanyAttribute[] companyAttribute = (System.Reflection.AssemblyCompanyAttribute[]) assembly.GetCustomAttributes(typeof(System.Reflection.AssemblyCompanyAttribute), true);
			if (companyAttribute != null && companyAttribute.Length > 0)
				this.Company = companyAttribute[0].Company;
			// Copyright
			System.Reflection.AssemblyCopyrightAttribute copyright = Attribute.GetCustomAttribute(assembly, typeof(System.Reflection.AssemblyCopyrightAttribute)) as System.Reflection.AssemblyCopyrightAttribute;
			if (copyright != null)
				this.Copyright = copyright.Copyright;
			// Description
			System.Reflection.AssemblyDescriptionAttribute description = Attribute.GetCustomAttribute(assembly, typeof(System.Reflection.AssemblyDescriptionAttribute)) as System.Reflection.AssemblyDescriptionAttribute;
			if (description.NotNull())
				this.Description = description.Description;
			// Icon
			this.Icon = System.Drawing.Icon.ExtractAssociatedIcon(this.Executable);
			#endregion

		}
		#endregion
		#region Public Methods
		public void Load(Module module)
		{
			Module old = this.Modules.Find(m => m.Name == module.Name);
			if (old.NotNull())
				this.Modules.Remove(m => m.Name == module.Name);
			this.Modules.Add(module.Merge(old));
		}
		public void Load<T>(string name, T value)
		{
			if (value is Module)
			{
				(value as Module).Name = name;
				this.Load(value as Module);
			}
			else
				this.Load(new Module<T>(name, value));
		}
		public void Unload(string name)
		{
			this.Modules.Remove(module => module.Name == name);
		}
		public T GetValue<T>(string name) where T : class
		{
			Module<T> module = this[name] as Module<T>;
			return module.NotNull() ? module.Value : null;
		}
		public bool Close()
		{
			return this.Mode == Mode.Started && (this.Runner.NotNull() ? this.Runner.Close() : Parallel.Thread.Start("Close Thread", () => this.Stop()).NotNull());
		}
		public bool Start()
		{
			bool result;
			if (result = this.Mode == Mode.Created)
				if (this.CatchErrors)
				{
					try
					{
						this.Starter();
					}
					catch (Error.Exception e)
					{
						Error.Log.Append(e);
						result = false;
					}
					catch (System.Exception e)
					{
						Error.Log.Append(Error.Level.Critical, string.Format("Unhandled Error {0} when Starting.", e.Type().Name), e);
						result = false;
					}
				}
				else
					this.Starter();
			return result;
		}
		void Starter()
		{
			Argument.Parser parser = new Argument.Parser();
			parser.Add('v', "version", () =>
			{
				Console.WriteLine("Product: " + this.Product);
				Console.WriteLine("Version: " + this.Version);
				Console.WriteLine("Company: " + this.Company);
				Console.WriteLine("Copyright: " + this.Copyright);
				Console.WriteLine("Description: " + this.Description);
			});
			foreach (Module module in this.Modules)
				module.AddArguments(parser);
			parser.Parse(this.CommandLine);

			foreach (Module module in this.Modules)
				if (module.Mode == Mode.Created)
					this.Run(module.Initialize, () => Error.Log.Append(Error.Level.Message, "Module " + module.Name + " initialized.", "Module "  + module.Name + " is initialized."),	exception => Error.Log.Append(Error.Level.Recoverable, "Module " + module.Name + " failed to initialize.", "Module " + module.Name + " failed to initialization with exception: " + exception.GetType() + " and message: " +  exception.Message));
			this.Mode = Mode.Initialized;
			foreach (Module module in this.Modules)
				if (module.Mode == Mode.Initialized)
					this.Run(module.Start, () => Error.Log.Append(Error.Level.Message, "Module " + module.Name + " started.", "Module " + module.Name + " is started."), exception => Error.Log.Append(Error.Level.Recoverable, "Module " + module.Name + " failed to start.", "Module " + module.Name + " failed to start with exception: " + exception.GetType() + " and message: " + exception.Message));
			this.Mode = Mode.Started;
		}
		bool Stop()
		{
			bool result;
			if (result = this.Mode == Mode.Started)
				if (this.CatchErrors)
					try
					{
						this.Stopper();
					}
					catch (Error.Exception e)
					{
						Error.Log.Append(e);
						result = false;
					}
					catch (System.Exception e)
					{
						Error.Log.Append(Error.Level.Critical, string.Format("Unhandled Error {0} when Stopping", e.Type().Name), e);
						result = false;
					}
					finally
					{
						lock (this.onClosedLock)
							this.onClosed.Call();
					}
				else
				{
					this.Stopper();
					lock (this.onClosedLock)
						this.onClosed.Call();
				}
			return result;
		}
		void Stopper()
		{
			this.Mode = Mode.Stopped;
			foreach (Module module in this.Modules)
				if (module.Mode == Mode.Started)
					this.Run(module.Stop, () => Error.Log.Append(Error.Level.Message, "Module " + module.Name + " stopped.", "Module " + module.Name + " is stopped."), exception => Error.Log.Append(Error.Level.Recoverable, "Module " + module.Name + " failed to stop.", "Module " + module.Name + " failed to stop with exception: " + exception.GetType() + " and message: " + exception.Message));
			this.Mode = Mode.Disposed;
			foreach (Module module in this.Modules)
				if (module.Mode == Mode.Stopped)
					this.Run(module.Dispose, () => Error.Log.Append(Error.Level.Message, "Module " + module.Name + " disposed.", "Module " + module.Name + " is disposed."), exception => Error.Log.Append(Error.Level.Recoverable, "Module " + module.Name + " failed to dispose.", "Module " + module.Name + " failed to dispose with exception: " + exception.GetType() + " and message: " + exception.Message));
		}
		void Run(Action action, Action success, Action<System.Exception> failed)
		{
			if (Error.Log.CatchErrors)
			{
				try
				{
					action();
					success.Call();
				}
				catch (System.Exception e) { failed.Call(e); }
			}
			else
			{
				action();
				success.Call();
			}
		}
		void Executer()
		{
			this.Starter();
			if (this.Runner.IsNull())
				this.Runner = new Waiter();
			this.Runner.Run();
			this.Stopper();
		}
		public bool Execute()
		{
			bool result;
			if (result = this.Mode == Mode.Created)
				if (this.CatchErrors)
				{
					try
					{
						this.Executer();
					}
					catch (Error.Exception e)
					{
						Error.Log.Append(e);
						result = false;
					}
					catch (System.Exception e)
					{
						Error.Log.Append(Error.Level.Critical, string.Format("Unhandled Error {0} when Executing.", e.Type().Name), e);
						result = false;
					}
					finally
					{
						lock (this.onClosedLock)
							this.onClosed.Call();
					}
				}
				else
				{
					this.Executer();
					lock (this.onClosedLock)
						this.onClosed.Call();
				}
			return result;
		}
		public void Idle()
		{
			Action onNextIdle;
			lock (this.onNextIdleLock)
			{
				onNextIdle = this.onNextIdle;
				this.onNextIdle = null;
			}
			onNextIdle.Call();
			lock (this.onIdleLock)
				this.onIdle.Call();
		}
		#endregion
		#region IDisposable Members
		~Application()
		{
			Error.Log.Wrap((Action)(this as IDisposable).Dispose)();
		}
		void  IDisposable.Dispose()
		{
			if (this.Modules.NotNull())
			{
				foreach (Module module in this.Modules)
					if (module.Mode != Mode.Disposed)
						module.Dispose();
			}
		}
		#endregion
		#region Static Load
		public static Application Load()
		{
			Application result = new Application();
			Xml.Serialize.Storage storage = new Xml.Serialize.Storage();
			result.Load("Storage", storage);
			Collection.Dictionary<string, Uri.Locator> modules = new Collection.Dictionary<string, Uri.Locator>();
			// Load from "Modules" folder and from "Modules/{executable name}" folder
			foreach (string path in new string[] { System.IO.Path.Combine(result.ExecutablePath, "Modules"), System.IO.Path.Combine(result.ExecutablePath, "Modules", System.IO.Path.GetFileNameWithoutExtension(result.Executable).Replace(".vshost", "")) })
				if (System.IO.Directory.Exists(path))
					foreach (string file in System.IO.Directory.GetFiles(path, "*.xml", System.IO.SearchOption.TopDirectoryOnly))
					{
						Uri.Locator locator = Uri.Locator.FromPlatformPath(file);
						modules[locator.Path.Stem] = locator;
					}
			foreach (Tuple<string, Uri.Locator> module in modules)
				Error.Log.Call(() => result.Load(module.Item1, storage.Load<Module>(module.Item2)));
			return result;
		}
		#endregion
	}
}
