// 
//  Root.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2011-2012 Simon Mika
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
using Collection = Kean.Collection;
using Kean.Collection.Extension;
using Generic = System.Collections.Generic;

namespace Kean.Platform.Settings
{
	public class Root : 
		Dynamic
	{
		Module module;

		internal Object Object { get; set; }
		Html.Dom.Head head = new Html.Dom.Head("");
		internal Html.Dom.Head Head { get { return this.head; } }
		internal Uri.Locator HelpFilename { get; set; }

		[Property("version", "Product version number.", "The product version number.")]
		public string Version { get { return this.module.Application.Version; } }

		string errorString;
		[Property("error", "Last error message", "The most recent error message.")]
		[Notify("OnErrorString")]
		public string ErrorString
		{
			get 
			{
				if (this.errorString.IsEmpty() && this.Error.NotNull())
					this.errorString = string.Format("{0},{1},\"{2}\",\"{3}\",\"{4}\",{5},{6},{7},{8},{9},{10}",
						this.Error.Time,
						this.Error.Level,
						this.Error.Title.Replace("\"", "\"\""),
						this.Error.Message.Replace("\"", "\"\""),
						this.Error.AssemblyName,
						this.Error.AssemblyVersion,
						this.Error.Type,
						this.Error.Method,
						this.Error.Filename,
						this.Error.Line,
						this.Error.Column);
				return this.errorString ?? ""; 
			}
		}
		public event Action<string> OnErrorString;

		public Error.IError Error { get; private set; }
		public event Action<Error.IError> OnError;

		internal Root(Module module)
		{
			this.module = module;
			Kean.Error.Log.OnAppend += this.OnErrorHelper;
		}
		void OnErrorHelper(Error.IError error)
		{
			this.OnError.Call(this.Error = error);
			this.errorString = null;
			if (this.OnErrorString.NotNull())
				this.OnErrorString(this.ErrorString);
		}
		public override void Dispose()
		{
			Kean.Error.Log.OnAppend -= this.OnErrorHelper;
			base.Dispose();
		}
		[Method("close", "Close application.", "Shut down the current application instance.")]
		public bool Close()
		{
			return this.module.Application.NotNull() && this.module.Application.Close();
		}

		[Method("help", "Open help in browser.", "Generate and open help document in browser.")]
		public void Help()
		{
			Html.Dom.Head head = new Html.Dom.Head(this.Head.Title);
			foreach(var node in this.Head)
				head.Add(node);
			head.Add(
					new Html.Dom.MetaData() { CharacterSet = "UTF-8" },
					new Html.Dom.MetaData() { Name = "generator", Content = this.module.Application.Product + " " + this.module.Application.Version },
					new Html.Dom.MetaData() { Name = "date", Content = DateTime.Today.ToShortDateString() }
					);
			new Kean.Html.Dom.Document(head, new Html.Dom.Body(this.GetHelp(this.Object))).Save(this.HelpFilename);
			System.Diagnostics.Process.Start(this.HelpFilename.PlatformPath);
		}
		Generic.IEnumerable<Html.Dom.Node> GetHelp(Object @object)
		{
			return this.GetHelp(null, @object).Prepend((Html.Dom.Node)new Html.Dom.Header(this.Head.Title));
		}
		Generic.IEnumerable<Html.Dom.Node> GetHelp(string prefix, Object @object)
		{
			Collection.IReadOnlyVector<Property> properties = @object.Properties;
			Collection.IReadOnlyVector<Method> methods = @object.Methods;
			string name = (prefix.NotEmpty() ? prefix : "") + @object.Name ;
			if (name.NotEmpty())
				prefix = name + ".";
			if (properties.Count > 0 || methods.Count > 0)
				yield return new Html.Dom.Heading1(name.NotEmpty() ? name : "&lt;root&gt;") { Identifier = name, Class = "object" };
			if (properties.Count > 0)
			{
				yield return new Html.Dom.Heading2("Properties") { Identifier = name + "_properties", Class = "properties" };
				Html.Dom.DefinitionList list = new Html.Dom.DefinitionList() { Class = "properties" };
				properties.Apply(p => this.GetHelp(prefix, p).Apply(n => list.Add(n)));
				yield return list;
			}
			if (methods.Count > 0)
			{
				yield return new Html.Dom.Heading2("Methods") { Identifier = name + "_methods", Class = "methods" };
				Html.Dom.DefinitionList list = new Html.Dom.DefinitionList() { Class = "methods" };
				methods.Apply(m => this.GetHelp(prefix, m).Apply(n => list.Add(n)));
				yield return list;
			}
			Collection.IReadOnlyVector<Object> objects = @object.Objects;
			foreach (var o in objects)
				foreach (var node in this.GetHelp(prefix, o))
					yield return node;
		}
		Generic.IEnumerable<Html.Dom.Node> GetHelp(string prefix, Method method)
		{
			string name = prefix + method.Name;
			yield return new Html.Dom.DefinitionTerm(method.Parameters.Map(p => (Html.Dom.Node)new Html.Dom.Span(p.Name) { Class = "parameter" }).Prepend(name)) { Identifier = name };
			if (method.Parameters.NotEmpty())
			{
				Html.Dom.Element parameters = new Html.Dom.DefinitionData() { Class = "parameters" };
				Html.Dom.DefinitionList parametersList = new Html.Dom.DefinitionList();
				foreach (Parameter.Abstract parameter in method.Parameters)
					if (parameter.Usage.NotEmpty() || parameter.Description.NotEmpty())
					{
						parametersList.Add(new Html.Dom.DefinitionTerm(parameter.Name));
						if (parameter.Usage.NotEmpty())
							parametersList.Add(new Html.Dom.DefinitionData(parameter.Usage) { Class = "usage" });
						if (parameter.Description.NotEmpty())
							parametersList.Add(new Html.Dom.DefinitionData(parameter.Description) { Class = "description" });
					}
				parameters.Add(parametersList);
				yield return parameters;
			}
			if (method.Usage.NotEmpty())
				yield return new Html.Dom.DefinitionData(method.Usage) { Identifier = name + "_usage", Class = "usage" };
			if (method.Example.NotEmpty())
				yield return new Html.Dom.DefinitionData(name + " " + method.Example) { Identifier = name + "_example", Class = "example" };
			if (method.Default.NotEmpty())
				yield return new Html.Dom.DefinitionData(method.Default) { Identifier = name + "_default", Class = "default" };
		}
		Generic.IEnumerable<Html.Dom.Node> GetHelp(string prefix, Property property)
		{
			string name = prefix + property.Name;
			string classes = string.Join(" ", property.Mode.ToString().Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries)).ToLowerInvariant();
			yield return new Html.Dom.DefinitionTerm(name) { Identifier = name, Class = classes };
			if (property.Usage.NotEmpty())
				yield return new Html.Dom.DefinitionData(property.Usage) { Identifier = name + "_usage", Class = classes + " usage" };
			if (property.Example.NotEmpty())
				yield return new Html.Dom.DefinitionData(name + " " + property.Example) { Identifier = name + "_example", Class = "example" };
			if (property.Default.NotEmpty())
				yield return new Html.Dom.DefinitionData(property.Default) { Identifier = name + "_default", Class = "default" };
		}
	}
}
