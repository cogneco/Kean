// 
//  LineJoinExtension.cs
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
	public static class LineJoinExtension
	{
		public static global::Cairo.LineJoin ToCairo(this LineJoin me)
		{
			global::Cairo.LineJoin result;
			switch (me)
			{
				case LineJoin.Bevel: result = global::Cairo.LineJoin.Bevel; break;
				default:
				case LineJoin.Miter: result = global::Cairo.LineJoin.Miter; break;
				case LineJoin.Round: result = global::Cairo.LineJoin.Round; break;
			}
			return result;
		}
		public static LineJoin FromCairo(this global::Cairo.LineJoin me)
		{
			LineJoin result;
			switch (me)
			{
				case global::Cairo.LineJoin.Bevel: result = LineJoin.Bevel; break;
				default:
				case global::Cairo.LineJoin.Miter: result = LineJoin.Miter; break;
				case global::Cairo.LineJoin.Round: result = LineJoin.Round; break;
			}
			return result;
		}
	}
}
