//
//  Type.cs
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
using NUnit.Framework;
using Kean.Reflect.Extension;

namespace Kean.Reflect.Test
{
	[TestFixture]
	public class Type :
		Kean.Test.Fixture<Type>
	{
		string prefix = "Kean.Reflect.Test.Type.";
		[Test]
		public void NestedClass()
		{
			string parentName = "Kean.Reflect.Test.Data.ParentClass Kean.Tests";
			string name = "Kean.Reflect.Test.Data.ParentClass+NestedClass Kean.Tests";
			Reflect.Type type1 = name;
			Verify((string)type1.Parent, EqualTo(parentName), this.prefix + "NestedClass.0");
			Verify((string)type1, EqualTo(name), this.prefix + "NestedClass.1");
			Reflect.Type type0 = new Data.ParentClass.NestedClass().Type();
			Verify((string)type0.Parent, EqualTo(parentName), this.prefix + "NestedClass.2");
			Verify((string)type0, EqualTo(name), this.prefix + "NestedClass.3");
			Verify(type0, EqualTo(type1), this.prefix + "NestedClass.4");
		}
		[Test]
		public void GenericStructure()
		{
			string name = "Kean:KeyValue<string,object>";
			Reflect.Type type = name;
			Verify(type.Assembly, EqualTo("Kean"), this.prefix + "GenericStructure.0");
			Verify(type.Name, EqualTo("Kean.KeyValue"), this.prefix + "GenericStructure.1");
			//Expect(type.Arguments.Count, EqualTo(2), this.prefix + "GenericStructure.2");
			//Expect((string)type.Arguments[0], EqualTo("string"), this.prefix + "GenericStructure.3");
			//Expect((string)type.Arguments[1], EqualTo("object"), this.prefix + "GenericStructure.4");
			Verify((string)type, EqualTo(name), this.prefix + "GenericStructure.5");
			Verify(type.Category, EqualTo(TypeCategory.Structure), this.prefix + "GenericStructure.6");
		}
		protected override void Run()
		{
			this.Run(
				this.NestedClass,
				this.GenericStructure
			);
		}
	}
}
