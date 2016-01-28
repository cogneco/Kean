// 
//  Text.cs
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
using Geometry2D = Kean.Math.Geometry2D;

namespace Kean.Draw
{
	public class Text
	{
		public string Markup { get; set; }
		public string Raw { get; set; }
		public Font Font { get; set; }
		public TextAlign Align { get; set; }
		public bool BaseLinePosition { get; set; }
		public float Width { get; set; }
		public Text()
		{
			this.Width = -1;
		}
		public Text(string markup) :
			this(markup, null)
		{ }
		public Text(string markup, Font font) :
			this()
		{
			this.Markup = markup;
			this.Font = font ?? new Font("Verdana", 12, FontWeight.Normal, FontSlant.Normal);
		}
		public static implicit operator Text(string markup)
		{
			return new Text(markup);
		}
	}
}
