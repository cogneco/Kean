// 
//  Vector.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2010 Simon Mika
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
using NUnit.Framework;

using Target = Kean.Collection.Array;

namespace Kean.Collection.Test.Array
{
	public class Vector :
		Base.Vector<Vector, Target.Vector<int>>
	{
		public Vector () :
			base("Kean.Collection.Test.Array.Vector.")
		{
			this.ZeroToNine = new Target.Vector<int>(0, 1, 2, 3, 4, 5, 6, 7, 8, 9);
		}
		public override Target.Vector<int> Create(int count)
		{
			return new Target.Vector<int>(count);
		}
	}
}

