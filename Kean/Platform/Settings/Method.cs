// 
//  Method.cs
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
	class Method :
		Member
	{
		Parameter.Abstract[] parameters;
		public Parameter.Abstract[] Parameters
		{
			get 
			{
				if (this.parameters.IsNull())
					this.parameters = this.backend.Parameters.Map(parameter => Parameter.Abstract.Create(parameter));
				return this.parameters; 
			}
		}
		Reflect.Type returnType;
		public Reflect.Type ReturnType
		{
			get
			{
				if (this.returnType.IsNull())
					this.returnType = this.backend.ReturnType;
				return this.returnType;
			}
		}
		protected override char Delimiter { get { return ' '; } }
		public string Default { get; private set; }
		Reflect.Method backend;
		public Method(MethodAttribute attribute, Reflect.Method backend, Object parent) :
			base(attribute, backend, parent)
		{
			this.Default = attribute.NotNull() ? attribute.Default : null;
			this.backend = backend;
		}
		public override bool Execute(Parser editor, string[] parameters)
		{
			bool result;
			object methodResult = null;
			if (result = this.Parameters.Length == 0 && parameters.All(parameter => parameter.IsEmpty()))
				methodResult = this.backend.Call();
			else if (result = parameters.Length == this.Parameters.Length && parameters.All(parameter => parameter.NotEmpty()) || parameters.Length - 1 == this.Parameters.Length && parameters[parameters.Length - 1].IsEmpty())
			{
				object[] p = new object[this.Parameters.Length];
				for (int i = 0; i < p.Length; i++)
					p[i] = this.Parameters[i].FromString(parameters[i]);
				methodResult = this.backend.Call(p);
			}
			if (methodResult.NotNull())
			{
				if (methodResult is bool)
				{
					if (!(bool)methodResult)
						editor.Answer(this, "failed");
				}
				else if (methodResult is string)
					editor.Answer(this, methodResult as string);
				else
					editor.Answer(this, methodResult.ToString());
			}
			return result;
		}
		public override string Complete(string[] parameters)
		{
			string result = "";
			if (this.Parameters.Length > 0)
			{
				if (parameters.Length <= this.Parameters.Length)
				{
					if (parameters.Length > 0)
					{
						if (parameters.Length > 1)
							result = string.Join(" ", parameters, 0, parameters.Length - 1) + " ";
						result += this.Parameters[parameters.Length - 1].Complete(parameters[parameters.Length - 1]);
					}
					else
						result = this.Parameters[0].Complete("");
				}
				else
					result = string.Join(" ", parameters, 0, this.Parameters.Length).TrimStart();
			}
			return result;
		}
		public override bool RequestType(Parser editor)
		{
			Collection.List<string> results = new Collection.List<string>();
			foreach (Parameter.Abstract parameter in this.Parameters)
				results.Add(parameter.Name + " : " + parameter.Type);
			editor.TypeResponse(this, "method : (" + results.Join(" * ") + ") => " + this.ReturnType);
			return true;
		}

		public override string Help(string[] parameters)
		{
			return (this.Parameters.Length > 0 && parameters.Length > 0 && parameters.Length <= this.Parameters.Length) ?
				this.Parameters[parameters.Length - 1].Help(parameters[parameters.Length - 1]) :
				(this.Usage.NotEmpty() ? this.Usage + "\n" : "") +
				(this.Example.NotEmpty() ? "Example: " + this.Example + "\n" : "") +
				(this.Default.NotEmpty() ? "Default: " + this.Default + "\n" : "");
		}
	}
}
