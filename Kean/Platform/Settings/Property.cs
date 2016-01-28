// 
//  Property.cs
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
using Kean.Reflect.Extension;
using Reflect = Kean.Reflect;
using Collection = Kean.Collection;
using Kean.Collection.Extension;

namespace Kean.Platform.Settings
{
	class Property :
		Member
	{
		class Notifier
		{
			Property property;
			Parser editor;
			public Notifier(Property property, Parser editor)
			{
				this.property = property;
				this.editor = editor;
			}
			public void Notify<T>(T value)
			{
				this.editor.Notify(this.property, this.property.parameter.AsString(value));
			}
		}
		protected override char Delimiter { get { return ' '; } }
		public string Default { get; private set; }
		Reflect.Property backend;
		Reflect.Event changed;
		Parameter.Abstract parameter;
		public string Value
		{
			get { return this.parameter.AsString(this.backend.Data); }
			set { this.backend.Data = this.parameter.FromString(value); }
		}
		public PropertyMode Mode { get; private set; }
		public Property(PropertyAttribute attribute, Reflect.Property backend, Object parent) :
			base(attribute, backend, parent)
		{
			this.Default = attribute.NotNull() ? attribute.Default : null;
			this.backend = backend;
			this.parameter = Parameter.Abstract.Create(backend);

			if (this.backend.Readable)
			{
				this.Mode |= PropertyMode.Read;
				NotifyAttribute[] attributes = this.backend.GetAttributes<NotifyAttribute>();
				if (attributes.Length == 1)
				{
					this.changed = this.Parent.GetEvent(attributes[0].Name);
					if (this.changed.NotNull())
						this.Mode |= PropertyMode.Notify;
				}
			}
			if (this.backend.Writable)
				this.Mode |= PropertyMode.Write;
		}
		public override bool Execute(Parser parser, string[] parameters)
		{
			bool parsed = true;
			bool respond = this.backend.Readable;
			if (parameters.Length > 0)
			{
				string value = string.Join(" ", parameters).Trim();
				if (value.ToLower() == "notify" && this.changed.NotNull() && respond)
				{
					this.changed.Add(Delegate.CreateDelegate(new Kean.Reflect.Type("mscorlib.dll", "System.Action", this.backend.Type), new Notifier(this, parser), typeof(Notifier).GetMethod("Notify").MakeGenericMethod(this.backend.Type)));
					respond = !parser.Asynchronous.HasFlag(Asynchronous.Notify);
				}
				else
				{
					try
					{
						this.Value = value;
					}
					catch (System.Exception)
					{
						parsed = false;
					}
					respond = !parser.Asynchronous.HasFlag(Asynchronous.Set);
				}
			}
			if (parsed)
			{
				if (respond)
					parser.Answer(this, this.Value);
			}
			else
				parser.Error(this, "Error Invalid Name:", parameters);
			return true;
		}
		public override string Complete(string[] parameters)
		{
			string result = this.backend.Writable && parameters.Length > 0 ? this.parameter.Complete(string.Join(" ", parameters)) : "";
			if (result.IsEmpty() && this.changed.NotNull() && parameters.Length > 0 && "notify".StartsWith(parameters[0]))
				result = "notify ";
			return result;
		}
		public override string Help(string[] parameters)
		{
			return parameters.Length > 0 ? this.parameter.Help(string.Join(" ", parameters)) : 
				(this.Usage.NotEmpty() ? this.Usage + "\n" : "") +
				(this.Example.NotEmpty() ? "Example: " + this.Example + "\n" : "") +
				(this.Default.NotEmpty() ? "Default: " + this.Default + "\n" : "");
		}
		public override bool RequestType(Parser editor)
		{
			editor.TypeResponse(this, "property " + this.ModeString + " : " + this.backend.Type);
			return true;
		}
		string ModeString
		{
			get
			{
				return (this.Mode.HasFlag(PropertyMode.Read) ? "r" : "-") +
					(this.Mode.HasFlag(PropertyMode.Write) ? "w" : "-") +
					(this.Mode.HasFlag(PropertyMode.Notify) ? "n" : "-");
			}
		}
	}
}
