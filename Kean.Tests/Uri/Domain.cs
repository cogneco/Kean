// 
//  Domain.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2010-2011 Simon Mika
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
using Target = Kean.Uri;

namespace Kean.Uri.Test
{
	[TestFixture]
	public class Domain :
        Kean.Test.Fixture<Domain>
	{
		protected override void Run()
		{
			this.Run(this.EqualityNull, this.Equality);
		}

		[Test]
		public void EqualityNull()
		{
			Target.Domain domain = null;
			Verify(domain, Is.Null);
			Verify(domain == null, Is.True);
		}

		[Test]
		public void Equality()
		{
			Target.Domain domain = "www.example.com";
			Verify(domain, Is.Not.Null);
			Verify(domain != null, Is.True);
			Verify((string)domain, Is.EqualTo("www.example.com"));
			Verify(domain == "www.example.com", Is.True);
			Verify(domain.Head, Is.EqualTo("www"));
			Verify((string)domain.Tail, Is.EqualTo("example.com"));
			Verify(domain.Tail == "example.com", Is.True);
			Verify(domain.Tail.Head, Is.EqualTo("example"));
			Verify((string)domain.Tail.Tail, Is.EqualTo("com"));
			Verify(domain.Tail.Tail.Tail, Is.Null);
		}
	}
}
