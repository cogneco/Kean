// 
//  Select.cs
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
	public class Select :
		Element
	{
		protected override string TagName { get { return "select"; } }
		public bool AutoFocus { get; set; }
		public bool Disabled { get; set; }
		public bool MultipleOption { get; set; }
		public string FormIdentifier { get; set; }
		public string NameOfList { get; set; }
		public bool Required { get; set; }
		public string Size { get; set; }
		#region Constructor
		public Select()
		{
			this.NoLineBreaks = true;
		}
		public Select(Node content) :
			this()
		{
			this.Add(content);
		}
		public Select(params Node[] nodes) :
			this()
		{
			this.Add(nodes);
		}
		#endregion
		protected override string FormatAttributes()
		{
			return
				base.FormatAttributes() +
				this.FormatAttribute("form", this.FormIdentifier) +
				this.FormatAttribute("disabled", this.Disabled) +
				this.FormatAttribute("required", this.Required) +
				this.FormatAttribute("name", this.NameOfList) +
				this.FormatAttribute("multiple", this.MultipleOption) +
				this.FormatAttribute("size", this.Size) +
				this.FormatAttribute("autofocus", this.AutoFocus);
		}
	}
}
