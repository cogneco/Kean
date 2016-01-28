// 
//  Object.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2011 Simon Mika
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
using Uri = Kean.Uri;
using Serialize = Kean.Serialize;
using Argument = Kean.Cli.Argument;
using Collection = Kean.Collection;

namespace Kean.Platform.Settings
{
	public class Module :
		Platform.Module
	{
		Collection.IList<IDisposable> editors = new Collection.List<IDisposable>();
		Collection.IList<Uri.Locator> configurations = new Collection.List<Kean.Uri.Locator>();
		Collection.IList<string> settings = new Collection.List<string>();
		Collection.IList<Uri.Locator> remotes = new Collection.List<Kean.Uri.Locator>();

		Root root;

		[Serialize.Parameter]
		public string Title { get { return this.root.Head.Title; } set { this.root.Head.Title = value; } }
		public Html.Dom.Head Head { get { return this.root.Head; } }

		public object this[string name] { get { return (this.root as IDynamic)[name]; } set { (this.root as IDynamic)[name] = value; } }

		public Module() :
			base("Settings")
		{
			this.root = new Root(this);
		}
		protected internal override void Initialize()
		{
			base.Initialize();
			this.root.HelpFilename = Uri.Locator.FromPlatformPath(this.Application.ExecutablePath);
			this.root.HelpFilename.Path.Add("settings.html");
		}

		public void Load(string name, object value)
		{
			this.Load(name, null, value);
		}
		public void Load(string name, string description, object value)
		{
			this.Load(name, description, null, value);
		}
		public void Load(string name, string description, string usage, object value)
		{
			this.root.Load(name, description, usage, value);
		}
		public void Unload(string name)
		{
			this.root.Unload(name);
		}

		protected override internal void AddArguments(Argument.Parser parser)
		{
			parser.Add('c', "config", argument => this.configurations.Add(argument));
			parser.Add('s', "setting", argument => this.settings.Add(argument));
			parser.Add('r', "remote", argument => this.remotes.Add(argument));
			base.AddArguments(parser);
		}
		protected override internal void Start()
		{
			this.Application.OnStarted += () => Parallel.ThreadPool.Global.Enqueue(() =>
			{
					System.Threading.Thread.Sleep(100);
				Settings.Parser.Listen(this.root, Uri.Locator.FromPlatformPath(this.Application.ExecutablePath + "/" + System.IO.Path.GetFileNameWithoutExtension(this.Application.Executable).Replace(".vshost", "") + ".conf")).TryDispose();
				if (System.IO.Directory.Exists(this.Application.ExecutablePath + "/Settings/"))
					foreach (string file in System.IO.Directory.GetFiles(this.Application.ExecutablePath + "/Settings/", "*.conf", System.IO.SearchOption.AllDirectories))
						Settings.Parser.Listen(this.root, Uri.Locator.FromPlatformPath(file)).TryDispose();
				Settings.Parser.Listen(this.root, Uri.Locator.FromPlatformPath(this.Application.ExecutablePath + "/settings.conf")).TryDispose();
				foreach (Uri.Locator locator in this.configurations)
					Settings.Parser.Listen(this.root, locator).TryDispose();
				if (this.settings.NotNull() && this.settings.Count > 0)
					Settings.Parser.Read(this.root, new IO.Text.CharacterInDevice(this.settings.Join("\n"))).TryDispose();
				if (this.remotes.NotNull())
					foreach (Uri.Locator locator in this.remotes)
					{
						IDisposable editor = Settings.Parser.Listen(this.root, locator);
						this.editors.Add(editor);
					}
				});
			base.Start();
		}
		protected override internal void Stop()
		{
			base.Stop();
		}
		protected override internal void Dispose()
		{
			if (this.editors.NotNull())
			{
				this.editors.Apply(editor => editor.Dispose());
				this.editors = null;
			}
			if (this.root.NotNull())
			{
				this.root.Dispose();
				this.root = null;
			}
			base.Dispose();
		}
	}
}
