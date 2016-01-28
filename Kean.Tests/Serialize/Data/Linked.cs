// 
//  Linked.cs
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
	public class Linked :
		IData
	{
		[Kean.Serialize.Parameter]
		public Structure Structure0 { get; set; }
		[Kean.Serialize.Parameter]
		public Structure Structure1 { get; set; }
		[Kean.Serialize.Parameter]
		public ComplexClass ComplexClass { get; set; }
		[Kean.Serialize.Parameter]
		public Class Class0 { get; set; }
		[Kean.Serialize.Parameter]
		public Class Class1 { get; set; }
		[Kean.Serialize.Parameter]
		public Class OtherClass { get; set; }

		#region IData
		public  void Initialize(IFactory factory)
		{
			this.Structure0 = this.Structure1 = factory.Create<Structure>();
			this.ComplexClass = factory.Create<ComplexClass>();
			this.Class0 = this.Class1 = this.ComplexClass.Class;
			this.OtherClass = factory.Create<Class>();
		}
		public void Verify(IFactory factory, string message, params object[] arguments)
		{
			factory.Verify(this.Structure0, Is.Not.SameAs(this.Structure1), message, arguments);
			factory.Verify(this.Structure0, message, arguments);
			factory.Verify(this.Structure1, message, arguments);
			factory.Verify(this.ComplexClass.Class, Is.SameAs(this.Class0), message, arguments);
			factory.Verify(this.ComplexClass.Class, Is.SameAs(this.Class1), message, arguments);
			factory.Verify(this.ComplexClass.Class, Is.Not.SameAs(this.OtherClass), message, arguments);
			factory.Verify(this.ComplexClass, message, arguments);
			factory.Verify(this.Class0, message, arguments);
			factory.Verify(this.Class1, message, arguments);
			factory.Verify(this.OtherClass, message, arguments);
		}
		#endregion
	}
}
