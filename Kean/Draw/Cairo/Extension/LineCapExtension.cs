// 
//  LineCapExtension.cs
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
	public static class LineCapExtension
	{
		public static global::Cairo.LineCap ToCairo(this LineCap me)
		{
			global::Cairo.LineCap result;
			switch (me)
			{
				default:
				case LineCap.Butt: result = global::Cairo.LineCap.Butt; break;
				case LineCap.Round: result = global::Cairo.LineCap.Round; break;
				case LineCap.Square: result = global::Cairo.LineCap.Square; break;
			}
			return result;
		}
		public static LineCap FromCairo(this global::Cairo.LineCap me)
		{
			LineCap result;
			switch (me)
			{
				default:
				case global::Cairo.LineCap.Butt: result = LineCap.Butt; break;
				case global::Cairo.LineCap.Round: result = LineCap.Round; break;
				case global::Cairo.LineCap.Square: result = LineCap.Square; break;
			}
			return result;
		}
	}
}
