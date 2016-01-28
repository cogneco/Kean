//
//  Preprocessor.cs
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
using Kean;
using Kean.Extension;
using Uri = Kean.Uri;
using Reflect = Kean.Reflect;
using NUnit.Framework;

namespace Kean.Xml.Serialize.Test
{
	public class Preprocessor :
		Kean.Serialize.Test.Factory<Preprocessor, Preprocessor.Verifier>
	{
		public class Verifier :
			Test.Verifier
		{
			protected override Uri.Locator CorrectBase { get { return "file:///./Xml/Serialize/Xml"; } }
			public Verifier()
			{
			}
		}
		protected override void Test(Reflect.Type type)
		{
			this.VerifyLoad(type);
		}
		protected override void Run()
		{
			this.Run(
				this.Include
			);
		}
		[Test]
		public void Include()
		{
			this.Test(typeof(Kean.Serialize.Test.Data.Include));
		}
	}
}
