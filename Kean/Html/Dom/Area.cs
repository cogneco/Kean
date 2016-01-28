// 
//  Area.cs
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
	public class Area :
	   Element
	{
		protected override string TagName { get { return "area"; } }
		public string Alternate { get; set; }
		public string Coordinate { get; set; }
		public string Destination { get; set; }
		public string Media { get; set; }
		public string Relation { get; set; }
		public string Shape { get; set; }
		public string Target { get; set; }
		public string Type { get; set; }
		protected override string FormatAttributes()
		{
			return
				 base.FormatAttributes() +
				 this.FormatAttribute("alt", this.Alternate) +
				 this.FormatAttribute("coords", this.Coordinate) +
				 this.FormatAttribute("href", this.Destination) +
				 this.FormatAttribute("media", this.Media) +
				 this.FormatAttribute("rel", this.Relation) +
				 this.FormatAttribute("shape", this.Shape) +
				 this.FormatAttribute("target", this.Target) +
				 this.FormatAttribute("type", this.Type);
		}
	}
}
