// 
//  TextExtension.cs
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
using Kean.Draw.Cairo.Extension;

namespace Kean.Draw.Cairo.Extension
{
	public static class TextExtension
	{
		public static Pango.Layout CreateLayout(this Text me, global::Cairo.Context context)
		{
			Pango.Layout result = Pango.CairoHelper.CreateLayout(context);
			// Font
			result.FontDescription = me.Font.FontDescription();

			// Alignment
			result.Alignment = me.Align.ToCairo();
			// Wrapping
			if (me.Width > 0)
			{
				result.Wrap = Pango.WrapMode.WordChar;
				result.Width = (int)(me.Width * Pango.Scale.PangoScale);
			}
			else
				result.Width = -1;

			// Text
			result.SetMarkup(me.Markup);
			return result;
		}
	}
}
