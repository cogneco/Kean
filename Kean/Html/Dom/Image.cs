// 
//  Image.cs
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
	public class Image :
		Element
	{
		protected override string TagName { get { return "img"; } }
		public string Source { get; set; }
		public string Alternate { get; set; }
		public string Height { get; set; }
		public string Width { get; set; }
		public string CrossOrigin { get; set; }
		public string IsMap { get; set; }
		public string UseMap { get; set; }
		protected override string FormatAttributes()
		{
			return
				 base.FormatAttributes() +
				 this.FormatAttribute("src", this.Source) +
				 this.FormatAttribute("alt", this.Alternate) +
				 this.FormatAttribute("height", this.Height) +
				 this.FormatAttribute("width", this.Width) +
				 this.FormatAttribute("crossorigin", this.CrossOrigin) +
				 this.FormatAttribute("ismap", this.IsMap) +
				 this.FormatAttribute("usemap", this.UseMap);
		}

	}
}
