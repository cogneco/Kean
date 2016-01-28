// 
//  InlineFrame.cs
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
	public class InlineFrame :
		Element
	{
		protected override string TagName { get { return "iframe"; } }
		public string Height { get; set; }
		public string SandBox { get; set; }
		public string Name { get; set; }
		public string SeamLess { get; set; }
		public string Source { get; set; }
		public string Width { get; set; }
		public string SourceDocument { get; set; }
		#region Constructor
		public InlineFrame()
		{
			this.NoLineBreaks = true;
		}
		public InlineFrame(Node content) :
			this()
		{
			this.Add(content);
		}
		public InlineFrame(params Node[] nodes) :
			this()
		{
			this.Add(nodes);
		}
		#endregion
		protected override string FormatAttributes()
		{
			return
				 base.FormatAttributes() +
				 this.FormatAttribute("height", this.Height) +
				 this.FormatAttribute("sandbox", this.SandBox) +
				 this.FormatAttribute("name", this.Name) +
				 this.FormatAttribute("seamless", this.SeamLess) +
				 this.FormatAttribute("src", this.Source) +
				 this.FormatAttribute("width", this.Width) +
				 this.FormatAttribute("srcdoc", this.SourceDocument);
		}
	}
}
