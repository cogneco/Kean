// 
//  List.cs
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
using Collection = Kean.Collection;
using NUnit.Framework;

namespace Kean.Serialize.Test.Data
{
	public class List :
		IData
	{
		[Kean.Serialize.Parameter]
		public Collection.List<Structure> Structures { get; set; }
		[Kean.Serialize.Parameter]
		public Collection.List<Class> Classes { get; set; }
		[Kean.Serialize.Parameter]
		public Collection.List<object> Objects { get; set; }
		[Kean.Serialize.Parameter(Name = "Number")]
		public Collection.List<int> Numbers { get; set; }
		Collection.List<float> floats = new Collection.List<float>();
		[Kean.Serialize.Parameter(Name = "Float")]
		public Collection.List<float> Floats { get { return this.floats; } }
		[Kean.Serialize.Parameter]
		public Collection.List<int> Empty { get; set; }
		[Kean.Serialize.Parameter]
		public Collection.List<int> Single { get; set; }
		[Kean.Serialize.Parameter]
		public Collection.List<object> SingleObject { get; set; }

		public List()
		{
		}

		#region IData
		public  void Initialize(IFactory factory)
		{
			this.Structures = new Collection.List<Structure>(factory.Create<Structure>(), factory.Create<Structure>());
			this.Classes = new Collection.List<Class>(factory.Create<ComplexClass>(), factory.Create<Class>());
			this.Objects = new Collection.List<object>(factory.Create<ComplexClass>(), factory.Create<Class>(), factory.Create<bool>(), factory.Create<DateTime>());
			this.Numbers = new Collection.List<int>(0, 1, 2, 3, 4, 5, 6, 7, 8, 9);
			this.Floats.Add(0.1337f).Add(1.1337f).Add(2.1337f).Add(3.1337f).Add(4.1337f).Add(5.1337f).Add(6.1337f).Add(7.1337f).Add(8.1337f).Add(9.1337f);
			this.Empty = new Collection.List<int>();
			this.Single = new Collection.List<int>();
			this.Single.Add(1337);
			this.SingleObject = new Collection.List<object>(factory.Create<Class>());
		}
		public void Verify(IFactory factory, string message, params object[] arguments)
		{
			factory.Verify(this.Structures.Count, Is.EqualTo(2), message, arguments);
			factory.Verify(this.Structures[0], message, arguments);
			factory.Verify(this.Structures[1], message, arguments);

			factory.Verify(this.Classes.Count, Is.EqualTo(2), message, arguments);
			factory.Verify(this.Classes[0] as ComplexClass, message, arguments);
			factory.Verify(this.Classes[1], message, arguments);

			factory.Verify(this.Objects.Count, Is.EqualTo(4), message, arguments);
			factory.Verify(this.Objects[0] as ComplexClass, message, arguments);
			factory.Verify(this.Objects[1] as Class, message, arguments);
			factory.Verify((bool)this.Objects[2], message, arguments);
			factory.Verify((DateTime)this.Objects[3], message, arguments);

			factory.Verify(this.Numbers.Count, Is.EqualTo(10), message, arguments);
			for (int i = 0; i < 10; i++)
				factory.Verify(this.Numbers[i], Is.EqualTo(i), message, arguments);

			factory.Verify(this.Floats.Count, Is.EqualTo(10), message, arguments);
			for (int i = 0; i < 10; i++)
				factory.Verify(this.Floats[i], Is.EqualTo(i + 0.1337f), message, arguments);

			factory.Verify(this.Empty, Is.Null, message, arguments);

			factory.Verify(this.Single.Count, Is.EqualTo(1), message, arguments);
			factory.Verify(this.Single[0], Is.EqualTo(1337), message, arguments);

			factory.Verify(this.SingleObject.Count, Is.EqualTo(1), message, arguments);
			factory.Verify(this.SingleObject[0] as Class, message, arguments);
		}
		#endregion
	}
}
