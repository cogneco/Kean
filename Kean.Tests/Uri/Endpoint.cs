// 
//  Endpoint.cs
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
	public class Endpoint :
        Kean.Test.Fixture<Endpoint>
	{
		protected override void Run()
		{
			this.Run(
				this.EqualityNull, 
				this.Equality, 
				this.EqualityHostOnly,
				this.EqualityPortOnly);
		}

		[Test]
		public void EqualityNull()
		{
			Target.Endpoint endpoint = null;
			Verify(endpoint, Is.Null);
			Verify(endpoint == null, Is.True);
		}

		[Test]
		public void Equality()
		{
			Target.Endpoint endpoint = "www.example.com:80";
			Verify(endpoint, Is.Not.Null);
			Verify(endpoint != null, Is.True);
			Verify((string)endpoint.Host, Is.EqualTo("www.example.com"));
			Verify(endpoint.Port, Is.EqualTo(80));
			Verify((string)endpoint, Is.EqualTo("www.example.com:80"));
			Verify(endpoint == "www.example.com:80", Is.True);
			Verify(endpoint != "www.example.com", Is.True);
		}

		[Test]
		public void EqualityHostOnly()
		{
			Target.Endpoint endpoint = "www.example.com";
			Verify(endpoint, Is.Not.Null);
			Verify(endpoint != null, Is.True);
			Verify((string)endpoint.Host, Is.EqualTo("www.example.com"));
			Verify(endpoint.Port, Is.Null);
			Verify((string)endpoint, Is.EqualTo("www.example.com"));
			Verify(endpoint == "www.example.com", Is.True);
			Verify(endpoint == "www.example.com:", Is.True);
			Verify(endpoint != "www.example.com:80", Is.True);
		}

		[Test]
		public void EqualityPortOnly()
		{
			Target.Endpoint endpoint = ":80";
			Verify(endpoint, Is.Not.Null);
			Verify(endpoint != null, Is.True);
			Verify((string)endpoint.Host, Is.EqualTo(""));
			Verify(endpoint.Port, Is.EqualTo(80));
			Verify((string)endpoint, Is.EqualTo(":80"));
			Verify(endpoint == ":80", Is.True);
			Verify(endpoint != "wwww.example.com:80", Is.True);
		}
	}
}
