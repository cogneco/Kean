// 
//  Link.cs
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
	public class Link :
	  Element
	{
		protected override string TagName { get { return "link"; } }
		public string Destination { get; set; }
		public string DestinationLanguage { get; set; }
		public string Relation { get; set; }
		public string Media { get; set; }
		public string Size { get; set; }
		public string Type { get; set; }
		#region Constructor
		public Link()
		{
			this.NoLineBreaks = true;
		}
		public Link(Node content) :
			this()
		{
			this.Add(content);
		}
		public Link(params Node[] nodes) :
			this()
		{
			this.Add(nodes);
		}
		#endregion
		protected override string FormatAttributes()
		{
			return
				base.FormatAttributes() +
				this.FormatAttribute("href", this.Destination) +
				this.FormatAttribute("hreflang", this.DestinationLanguage) +
				this.FormatAttribute("rel", this.Relation) +
				this.FormatAttribute("media", this.Media) +
				this.FormatAttribute("size", this.Size) +
				this.FormatAttribute("type", this.Type);
		}
	}
}
