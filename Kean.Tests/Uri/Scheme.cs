// 
//  Scheme.cs
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
	public class Scheme :
        Kean.Test.Fixture<Scheme>
	{
		protected override void Run()
		{
			this.Run(this.EqualityNull, this.Equality);
		}

		[Test]
		public void EqualityNull()
		{
			Target.Scheme scheme = null;
			Verify(scheme, Is.Null);
			Verify(scheme == null, Is.True);
		}

		[Test]
		public void Equality()
		{
			Target.Scheme scheme = "svn+ssh";
			Verify(scheme, Is.Not.Null);
			Verify(scheme != null, Is.True);
			Verify((string)scheme, Is.EqualTo("svn+ssh"));
			Verify(scheme == "svn+ssh", Is.True);
			Verify(scheme.Head, Is.EqualTo("svn"));
			Verify((string)scheme.Tail, Is.EqualTo("ssh"));
			Verify(scheme.Tail == "ssh", Is.True);
			Verify(scheme.Tail.Head, Is.EqualTo("ssh"));
			Verify(scheme.Tail.Tail, Is.Null);
		}
	}
}
