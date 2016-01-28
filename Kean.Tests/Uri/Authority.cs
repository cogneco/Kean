// 
//  Authority.cs
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
	public class Authority : 
        Kean.Test.Fixture<Authority>
	{
		protected override void Run()
		{
			this.Run(
				this.EqualityNull,
				this.Equality,
				this.EqualityUserOnly,
				this.EqualityEndpointOnly);
		}

		[Test]
		public void EqualityNull()
		{
			Target.Authority authority = null;
			Verify(authority, Is.Null);
			Verify(authority == null, Is.True);
		}

		[Test]
		public void Equality()
		{
			Target.Authority authority = "name:password@example.com:80";
			Verify(authority, Is.Not.Null);
			Verify(authority != null, Is.True);
			Verify((string)authority.User, Is.EqualTo("name:password"));
			Verify((string)authority.Endpoint, Is.EqualTo("example.com:80"));
			Verify((string)authority, Is.EqualTo("name:password@example.com:80"));
			Verify(authority == "name:password@example.com:80", Is.True);
			Verify(authority != "name:password", Is.True);
			Verify(authority != "example.com:80", Is.True);
		}

		[Test]
		public void EqualityUserOnly()
		{
			Target.Authority authority = "name:password@";
			Verify(authority, Is.Not.Null);
			Verify(authority, Is.Not.Null);
			Verify((string)authority.User, Is.EqualTo("name:password"));
			Verify(authority.Endpoint, Is.Null);
			Verify((string)authority, Is.EqualTo("name:password@"));
			Verify((string)authority == "name:password@", Is.True);
			Verify((string)authority != "name:password", Is.True);
			Verify(authority != "", Is.True);
		}

		[Test]
		public void EqualityEndpointOnly()
		{
			Target.Authority authority = "example.com:80";
			Verify(authority, Is.Not.Null);
			Verify(authority != null, Is.True);
			Verify(authority.User, Is.Null);
			Verify((string)authority.Endpoint, Is.EqualTo("example.com:80"));
			Verify((string)authority, Is.EqualTo("example.com:80"));
			Verify(authority == "@example.com:80", Is.True);
			Verify(authority != "name:password@example.com:80", Is.True);
		}
	}
}
