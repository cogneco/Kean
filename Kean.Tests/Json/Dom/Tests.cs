// 
//  Tests.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2011-2012 Simon Mika
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
using NUnit.Framework;

namespace Kean.Json.Dom.Test
{
	[TestFixture]
	public abstract class Tests<T> :
        Factory<T>
		where T : Tests<T>, new()
	{
		[Test]
		public new void Empty ()
		{
			this.Verify ("Empty");
		}
		[Test]
		public new void Null ()
		{
			this.Verify ("Null");
		}
		[Test]
		public void BooleanFalse ()
		{
			this.Verify ("BooleanFalse");
		}
		[Test]
		public void BooleanTrue ()
		{
			this.Verify ("BooleanTrue");
		}
		[Test]
		public void Number ()
		{
			this.Verify ("Number");
		}
		[Test]
		public void String ()
		{
			this.Verify ("String");
		}
		[Test]
		public void Array ()
		{
			this.Verify ("Array");
		}
		[Test]
		public void ObjectEmpty ()
		{
			this.Verify ("ObjectEmpty");
		}
		[Test]
		public void ObjectSingle ()
		{
			this.Verify ("ObjectSingle");
		}
		[Test]
		public void Object ()
		{
			this.Verify ("Object");
		}
		[Test]
		public void ObjectNested ()
		{
			this.Verify ("ObjectNested");
		}
		protected override void Run ()
		{
			this.Run (
				this.Empty,
				this.Null,
				this.BooleanFalse,
				this.BooleanTrue,
				this.Number,
				this.String,
				this.Array,
				this.ObjectEmpty,
				this.ObjectSingle,
				this.Object,
				this.ObjectNested
			);
		}
	}
}

