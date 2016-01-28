// 
//  TextAlignExtension.cs
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

namespace Kean.Draw.Cairo.Extension
{
	public static class TextAlignExtension
	{
		public static Pango.Alignment ToCairo(this TextAlign me)
		{
			Pango.Alignment result;
			switch (me)
			{
				default:
				case TextAlign.Left: result = Pango.Alignment.Left; break;
				case TextAlign.Center: result = Pango.Alignment.Center; break;
				case TextAlign.Right: result = Pango.Alignment.Right; break;
			}
			return result;
		}
		public static TextAlign FromCairo(this Pango.Alignment me)
		{
			TextAlign result;
			switch (me)
			{
				default:
				case Pango.Alignment.Left: result = TextAlign.Left; break;
				case Pango.Alignment.Center: result = TextAlign.Center; break;
				case Pango.Alignment.Right: result = TextAlign.Right; break;
			}
			return result;
		}
	}
}
