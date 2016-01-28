// 
//  Form.cs
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

using System;

namespace Kean.Html.Dom
{
	public class Form :
	   Element
	{
		protected override string TagName { get { return "form"; } }
		public string Action { get; set; }
		public string Method { get; set; }
		public string Name { get; set; }
		public string AutoComplete { get; set; }
		public string Target { get; set; }
		public string NoValidate { get; set; }
		public string EnCodedType { get; set; }
		public string AcceptCharacterSet { get; set; }
		#region Constructor
		public Form()
		{
			this.NoLineBreaks = true;
		}
		public Form(Node content) :
			this()
		{
			this.Add(content);
		}
		public Form(params Node[] nodes) :
			this()
		{
			this.Add(nodes);
		}
		#endregion
		protected override string FormatAttributes()
		{
			return
				 base.FormatAttributes() +
				 this.FormatAttribute("action", this.Action) +
				 this.FormatAttribute("method", this.Method) +
				 this.FormatAttribute("name", this.Name) +
				 this.FormatAttribute("autocomplete", this.AutoComplete) +
				 this.FormatAttribute("target", this.Target) +
				 this.FormatAttribute("novalidate", this.NoValidate) +
				 this.FormatAttribute("enctype", this.EnCodedType) +
				 this.FormatAttribute("accept-charset", this.AcceptCharacterSet);

		}
	}
}
