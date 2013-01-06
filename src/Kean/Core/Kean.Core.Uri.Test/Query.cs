﻿// 
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

using Target = Kean.Core.Uri;
using Kean.Core.Collection.Linked.Extension;
using Kean.Core;

namespace Kean.Core.Uri.Test
{
    [TestFixture]
    public class Query :
        Kean.Test.Fixture<Query>
    {
        string prefix = "Kean.Core.Uri.Test.Query.";
        protected override void Run()
        {
            this.Run(this.EqualityNull, this.Equality, this.RemoveList, this.RemoveQuery, this.AddQuery);
        }
        [Test]
        public void EqualityNull()
        {
            Target.Query query = null;
            Expect(query, Is.EqualTo(null), this.prefix + "EqualityNull.0");
            Expect(query == null, Is.True, this.prefix + "EqualityNull.1");
        }
        [Test]
        public void Equality()
        {
            Target.Query query = "keyA=valueA&keyB=valueB&key+c=value+c";
            Expect(query, Is.Not.EqualTo(null), this.prefix + "Equality.0");
            Expect(query != null, "query != null", this.prefix + "Equality.1");
            Expect((string)query, Is.EqualTo("keyA=valueA&keyB=valueB&key+c=value+c"), this.prefix + "Equality.2");
            Expect(query == "keyA=valueA&keyB=valueB&key+c=value+c", "query == \"keyA=valueA&keyB=valueB&key+c=value+c\"", this.prefix + "Equality.3");
            Expect(query["keyA"], Is.EqualTo("valueA"), this.prefix + "Equality.4");
			Expect(query["keyB"], Is.EqualTo("valueB"), this.prefix + "Equality.5");
			Expect(query["key c"], Is.EqualTo("value c"), this.prefix + "Equality.6");
		}
		[Test]
		public void RemoveList()
		{
			Target.Query query = "keyA=valueA&keyB=valueB&keyC=valueC&keyD=valueD&keyE=valueE&keyA=valueAA";
			query.Remove("keyA");
			Expect(query, Is.EqualTo((Target.Query)"keyB=valueB&keyC=valueC&keyD=valueD&keyE=valueE"), this.prefix + "RemoveList.0");
			query.Remove("keyE");
			Expect(query, Is.EqualTo((Target.Query)"keyB=valueB&keyC=valueC&keyD=valueD"), this.prefix + "RemoveList.1");
			query.Remove("keyC");
			Expect(query, Is.EqualTo((Target.Query)"keyB=valueB&keyD=valueD"), this.prefix + "RemoveList.2");
			query.Remove("keyF");
			Expect(query, Is.EqualTo((Target.Query)"keyB=valueB&keyD=valueD"), this.prefix + "RemoveList.3");
			query.Remove("keyB");
			Expect(query, Is.EqualTo((Target.Query)"keyD=valueD"), this.prefix + "RemoveList.4");
			query.Remove("keyD");
			Expect(query, Is.EqualTo((Target.Query)""), this.prefix + "RemoveList.5");
		}
		[Test]
		public void RemoveQuery()
		{
			Target.Query query = "keyA=valueA&keyB=valueB&keyC=valueC&keyD=valueD&keyE=valueE&keyA=valueAA";
			query.Remove("keyA", "keyE");
			Expect(query, Is.EqualTo((Target.Query)"keyB=valueB&keyC=valueC&keyD=valueD"), this.prefix + "RemoveQuery.0");
			query.Keep("keyC");
			Expect(query, Is.EqualTo((Target.Query)"keyC=valueC"), this.prefix + "RemoveQuery.1");
			query.Remove("keyC", "keyC");
			Expect(query, Is.EqualTo((Target.Query)""), this.prefix + "RemoveQuery.2");
		}
		[Test]
		public void AddQuery()
		{
			Target.Query query = "keyA=valueA";
			query["keyB"] = "valueB";
			Expect((string)query, Is.EqualTo("keyB=valueB&keyA=valueA"), this.prefix + "AddQuery.0");
			query = new Target.Query();
			query.ToString();
			query["keyB"] = "valueB";
			query["keyC"] = "valueC";
			Expect((string)query, Is.EqualTo("keyC=valueC&keyB=valueB"), this.prefix + "AddQuery.1");
		}
    }
}
