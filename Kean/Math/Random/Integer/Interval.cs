// 
//  Interval.cs
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

namespace Kean.Math.Random.Integer
{
	public class Interval :
		Interval<int>
	{
		public Interval()
		{
		}
		public override int Generate()
		{
			uint result;
			uint length;
			if (this.Floor < 0 && this.Ceiling > 0)
				length = unchecked((uint)this.Ceiling + (uint)(-this.Floor));
			else
				length = (uint)(this.Ceiling - this.Floor);
            int bits = Kean.Math.Integer.Ceiling(Kean.Math.Double.Logarithm(length + 1, 2));
			do
				result = (uint)this.Next(bits);
			while (result > length);
			return (int)(this.Floor < 0 ? result - (uint)(-this.Floor) : result + this.Floor);		}
	}
}

