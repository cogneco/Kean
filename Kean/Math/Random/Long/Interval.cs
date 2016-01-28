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

namespace Kean.Math.Random.Long
{
	public class Interval :
		Interval<long>
	{
		public Interval()
		{
		}
		public override long Generate()
		{
			ulong result;
			ulong length;
			if (this.Floor < 0 && this.Ceiling > 0)
				length = unchecked((ulong)this.Ceiling + (ulong)(-this.Floor));
			else
				length = (ulong)(this.Ceiling - this.Floor);
            int bits = Kean.Math.Integer.Ceiling(Kean.Math.Double.Logarithm(length + 1, 2));
			do
				result = this.Next(bits);
			while (result > length);
			return (long)(this.Floor < 0 ? result - (ulong)(-this.Floor) : result + (ulong)this.Floor);
		}
	}
}

