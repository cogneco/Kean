// 
//  User.cs
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
	public class User :
        Kean.Test.Fixture<User>
	{
		protected override void Run()
		{
			this.Run(this.EqualityNull, this.Equality, this.EqualityNameOnly, this.EqualityPasswordOnly);
		}

		[Test]
		public void EqualityNull()
		{
			Target.User user = null;
			Verify(user, Is.Null);
			Verify(user == null, Is.True);
		}

		[Test]
		public void Equality()
		{
			Target.User user = "name:password";
			Verify(user, Is.Not.Null);
			Verify(user != null, Is.True);
			Verify(user.Name, Is.EqualTo("name"));
			Verify(user.Password, Is.EqualTo("password"));
			Verify((string)user, Is.EqualTo("name:password"));
			Verify(user == "name:password", Is.True);
			Verify(user != "name", Is.True);
		}

		[Test]
		public void EqualityNameOnly()
		{
			Target.User user = "name";
			Verify(user, Is.Not.Null);
			Verify(user != null, Is.True);
			Verify(user.Name, Is.EqualTo("name"));
			Verify(user.Password, Is.Null);
			Verify((string)user, Is.EqualTo("name"));
			Verify(user == "name", Is.True);
			Verify(user == "name:", Is.True);
			Verify(user != "name:password", Is.True);
		}

		[Test]
		public void EqualityPasswordOnly()
		{
			Target.User user = ":password";
			Verify(user, Is.Not.Null);
			Verify(user != null, Is.True);
			Verify(user.Name, Is.Null);
			Verify(user.Password, Is.EqualTo("password"));
			Verify((string)user, Is.EqualTo(":password"));
			Verify(user == ":password", Is.True);
			Verify(user != "name:password", Is.True);
		}
	}
}
