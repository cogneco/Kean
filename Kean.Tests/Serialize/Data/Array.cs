// 
//  Array.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2012 Simon Mika
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

namespace Kean.Serialize.Test.Data
{
	public class Array :
		IData
	{
		[Kean.Serialize.Parameter]
		public Structure[] Structures { get; set; }
		[Kean.Serialize.Parameter]
		public Class[] Classes { get; set; }
		[Kean.Serialize.Parameter]
		public object[] Objects { get; set; }
		[Kean.Serialize.Parameter(Name = "Number")]
		public int[] Numbers { get; set; }
		[Kean.Serialize.Parameter]
		public int[] Empty { get; set; }
		#region IData
		public virtual void Initialize(IFactory factory)
		{
			this.Structures = new Structure[] { factory.Create<Structure>(), factory.Create<Structure>() };
			this.Classes = new Class[] { factory.Create<ComplexClass>(), factory.Create<Class>() };
			this.Objects = new object[] { factory.Create<ComplexClass>(), factory.Create<Class>(), factory.Create<bool>(), factory.Create<DateTime>() };
			this.Numbers = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
			this.Empty = new int[0];
		}
		public virtual void Verify(IFactory factory, string message, params object[] arguments)
		{
			factory.Verify(this.Structures.Length, Is.EqualTo(2), message, arguments);
			factory.Verify(this.Structures[0], message, arguments);
			factory.Verify(this.Structures[1], message, arguments);

			factory.Verify(this.Classes.Length, Is.EqualTo(2), message, arguments);
			factory.Verify(this.Classes[0] as ComplexClass, message, arguments);
			factory.Verify(this.Classes[1], message, arguments);

			factory.Verify(this.Objects.Length, Is.EqualTo(4), message, arguments);
			factory.Verify(this.Objects[0] as ComplexClass, message, arguments);
			factory.Verify(this.Objects[1] as Class, message, arguments);
			factory.Verify((bool)this.Objects[2], message, arguments);
			factory.Verify((DateTime)this.Objects[3], message, arguments);

			factory.Verify(this.Numbers.Length, Is.EqualTo(10), message, arguments);
			for (int i = 0; i < 10; i++)
				factory.Verify(this.Numbers[i], Is.EqualTo(i), message, arguments);

			factory.Verify(this.Empty, Is.Null, message, arguments);
		}
		#endregion
	}
}
