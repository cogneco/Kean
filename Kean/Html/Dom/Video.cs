// 
//  Video.cs
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
	public class Video :
		Element
	{
		protected override string TagName { get { return "video"; } }
		public string Source { get; set; }
		public string Width { get; set; }
		public string Height { get; set; }
		public bool AutoPlay { get; set; }
		public bool Controls { get; set; }
		public bool Loop { get; set; }
		public bool Muted { get; set; }
		public string Poster { get; set; }
		public string PreLoad { get; set; }
		#region Constructor
		public Video()
		{
			this.NoLineBreaks = true;
		}
		public Video(Node content) :
			this()
		{
			this.Add(content);
		}
		public Video(params Node[] nodes) :
			this()
		{
			this.Add(nodes);
		}
		#endregion
		protected override string FormatAttributes()
		{
			return
				base.FormatAttributes() +
				this.FormatAttribute("src", this.Source) +
				this.FormatAttribute("width", this.Width) +
				this.FormatAttribute("height", this.Height) +
				this.FormatAttribute("controls", this.Controls) +
				this.FormatAttribute("loop", this.Loop) +
				this.FormatAttribute("muted", this.Muted) +
				this.FormatAttribute("poster", this.Poster) +
				this.FormatAttribute("preload", this.PreLoad);
		}
	}
}
