// 
//  Misfit.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2011-2013 Simon Mika
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
using Kean.Extension;
using Uri = Kean.Uri;
using Reflect = Kean.Reflect;

namespace Kean.Serialize.Test
{
	public class Misfit<C> :
		Factory<Misfit<C>, C>
			where C : Verifier, new()
	{
		protected override void Test(Reflect.Type type)
		{
			this.VerifyLoad(type);
		}
		public override string Name(Reflect.Type type)
		{
			return base.Name(type) + "Misfit";
		}
		protected override void Run()
		{
			this.Run(
				this.Structure,
				this.Class,
				this.ComplexClass
			);
		}
		[Test]
		public void Structure()
		{
			this.Test(typeof(Data.Structure));
		}
		[Test]
		public void Class()
		{
			this.Test(typeof(Data.Class));
		}
		[Test]
		public void ComplexClass()
		{
			this.Test(typeof(Data.ComplexClass));
		}
	}
}
