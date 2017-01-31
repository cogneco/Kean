// 
//  Query.cs
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
using Kean.Collection.Linked.Extension;
using Kean;

namespace Kean.Uri.Test
{
	[TestFixture]
	public class Query :
		Kean.Test.Fixture<Query>
	{
		protected override void Run()
		{
			this.Run(this.EqualityNull, this.Equality, this.RemoveList, this.RemoveQuery, this.AddQuery);
		}

		[Test]
		public void EqualityNull()
		{
			Target.Query query = null;
			Verify(query, Is.Null);
			Verify(query == null, Is.True);
		}

		[Test]
		public void Equality()
		{
			Target.Query query = "keyA=valueA&keyB=valueB&key+c=value+c";
			Verify(query, Is.Not.Null);
			Verify(query != null, Is.True);
			Verify((string)query, Is.EqualTo("keyA=valueA&keyB=valueB&key+c=value+c"));
			Verify(query == "keyA=valueA&keyB=valueB&key+c=value+c", Is.True);
			Verify(query["keyA"], Is.EqualTo("valueA"));
			Verify(query["keyB"], Is.EqualTo("valueB"));
			Verify(query["key+c"], Is.EqualTo("value+c"));
		}

		[Test]
		public void Enums()
		{
			Target.Query query = "consoleColor=Green";
			Verify(query.GetEnumeration<ConsoleColor>("consoleColor", ConsoleColor.Black), Is.EqualTo(ConsoleColor.Green));
			query = "consoleColor=green";
			Verify(query.GetEnumeration<ConsoleColor>("consoleColor", ConsoleColor.Black), Is.EqualTo(ConsoleColor.Green));
		}

		[Test]
		public void RemoveList()
		{
			Target.Query query = "keyA=valueA&keyB=valueB&keyC=valueC&keyD=valueD&keyE=valueE&keyA=valueAA";
			query.Remove("keyA");
			Verify(query, Is.EqualTo((Target.Query)"keyB=valueB&keyC=valueC&keyD=valueD&keyE=valueE"));
			query.Remove("keyE");
			Verify(query, Is.EqualTo((Target.Query)"keyB=valueB&keyC=valueC&keyD=valueD"));
			query.Remove("keyC");
			Verify(query, Is.EqualTo((Target.Query)"keyB=valueB&keyD=valueD"));
			query.Remove("keyF");
			Verify(query, Is.EqualTo((Target.Query)"keyB=valueB&keyD=valueD"));
			query.Remove("keyB");
			Verify(query, Is.EqualTo((Target.Query)"keyD=valueD"));
			query.Remove("keyD");
			Verify(query, Is.EqualTo((Target.Query)""));
		}

		[Test]
		public void RemoveQuery()
		{
			Target.Query query = "keyA=valueA&keyB=valueB&keyC=valueC&keyD=valueD&keyE=valueE&keyA=valueAA";
			query.Remove("keyA", "keyE");
			Verify(query, Is.EqualTo((Target.Query)"keyB=valueB&keyC=valueC&keyD=valueD"));
			query.Keep("keyC");
			Verify(query, Is.EqualTo((Target.Query)"keyC=valueC"));
			query.Remove("keyC", "keyC");
			Verify(query, Is.EqualTo((Target.Query)""));
		}

		[Test]
		public void AddQuery()
		{
			Target.Query query = "keyA=valueA";
			query["keyB"] = "valueB";
			Verify((string)query, Is.EqualTo("keyA=valueA&keyB=valueB"));
			query = new Target.Query();
			query.ToString();
			query["keyB"] = "valueB";
			query["keyC"] = "valueC";
			Verify((string)query, Is.EqualTo("keyB=valueB&keyC=valueC"));
		}
	}
}
