// 
//  Test.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2009 Simon Mika
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

using Target = Kean.Collection;

namespace Kean.Collection.Test
{
	[TestFixture]
	public class Vector :
		Base.Vector<Vector, Target.Vector<int>>
	{
		public Vector() :
			base("Kean.Collection.Test.Vector.")
		{
			this.ZeroToNine = new Target.Vector<int>(0, 1, 2, 3, 4, 5, 6, 7, 8, 9);			
		}
		public override Target.Vector<int> Create(int count)
		{
			return new Target.Vector<int>(count);
		}
		
		protected override void Run()
		{
			base.Run();
			this.Run(
				this.ConstructorParameter,
				this.ConstructorArray,
				this.ConstructorCount
				);
		}

		[Test]
		public void ConstructorParameter()
		{
			Target.Vector<int> data = new Target.Vector<int>(0, 1, 2, 3, 4, 5, 6, 7, 8, 9);
			Verify(data.Count, EqualTo(10), this.Prefix + "ConstructorParameter.0");
			for (int i = 0; i < 10; i++)
				Verify(data[i], EqualTo(i), this.Prefix + "ConstructorParameter." + (i + 1).ToString());
		}
		[Test]
		public void ConstructorArray()
		{
			int[] original = new int[] { -3, 33, 23, 9, 1223, -52, 3, 5, 5, 72 };
			Target.Vector<int> data = new Target.Vector<int>(original);
			Verify(data.Count, EqualTo(10), this.Prefix + "ConstructorArray.0");
			for (int i = 0; i < 10; i++)
				Verify(data[i], EqualTo(original[i]), this.Prefix + "ConstructorArray." + (i + 1).ToString());
		}
		[Test]
		public void ConstructorCount()
		{
			Target.Vector<int> data = new Target.Vector<int>(10);
			Verify(data.Count, EqualTo(10), this.Prefix + "ConstructorCount0");
			for (int i = 0; i < 10; i++)
				Verify(data[i], EqualTo(0), this.Prefix + "ConstructorCount." + (i + 1).ToString());
		}
	}
}
