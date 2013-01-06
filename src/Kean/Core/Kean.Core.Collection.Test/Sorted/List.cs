﻿// 
//  List.cs
//  
//  Author:
//       Anders Frisk <>
//  
//  Copyright (c) 2011 Anders Frisk
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

using Target = Kean.Core.Collection;
namespace Kean.Core.Collection.Test.Sorted
{
    public class List :
        Kean.Test.Fixture<List>
    {
        public List() :
			base("Kean.Core.Collection.Test.Sorted.List.")
        { }
        protected override void  Run()
        {
            this.Run(
                this.Add,
                this.AddNonUnique,
                this.Remove
            );
        }
        [Test]
        public void Add()
        {
            int subtest = 0;
            Target.Sorted.List<int> list = new Target.Sorted.List<int>();
            Expect(list.Count, EqualTo(0), this.Prefix + "Add." + subtest++);
            list.Add(5);
            Expect(list.Count, EqualTo(1), this.Prefix + "Add." + subtest++);
            Expect(list[0], EqualTo(5), this.Prefix + "Add." + subtest++);
            list.Add(3);
            Expect(list.Count, EqualTo(2), this.Prefix + "Add." + subtest++);
            Expect(list[0], EqualTo(3), this.Prefix + "Add." + subtest++);
            Expect(list[1], EqualTo(5), this.Prefix + "Add." + subtest++);
            list.Add(2);
            Expect(list.Count, EqualTo(3), this.Prefix + "Add." + subtest++);
            Expect(list[0], EqualTo(2), this.Prefix + "Add." + subtest++);
            Expect(list[1], EqualTo(3), this.Prefix + "Add." + subtest++);
            Expect(list[2], EqualTo(5), this.Prefix + "Add." + subtest++);
            list.Add(7);
            Expect(list.Count, EqualTo(4), this.Prefix + "Add." + subtest++);
            Expect(list[0], EqualTo(2), this.Prefix + "Add." + subtest++);
            Expect(list[1], EqualTo(3), this.Prefix + "Add." + subtest++);
            Expect(list[2], EqualTo(5), this.Prefix + "Add." + subtest++);
            Expect(list[3], EqualTo(7), this.Prefix + "Add." + subtest++);
            list.Add(1);
            Expect(list.Count, EqualTo(5), this.Prefix + "Add." + subtest++);
            Expect(list[0], EqualTo(1), this.Prefix + "Add." + subtest++);
            Expect(list[1], EqualTo(2), this.Prefix + "Add." + subtest++);
            Expect(list[2], EqualTo(3), this.Prefix + "Add." + subtest++);
            Expect(list[3], EqualTo(5), this.Prefix + "Add." + subtest++);
            Expect(list[4], EqualTo(7), this.Prefix + "Add." + subtest++);
            list.Add(8);
            Expect(list.Count, EqualTo(6), this.Prefix + "Add." + subtest++);
            Expect(list[0], EqualTo(1), this.Prefix + "Add." + subtest++);
            Expect(list[1], EqualTo(2), this.Prefix + "Add." + subtest++);
            Expect(list[2], EqualTo(3), this.Prefix + "Add." + subtest++);
            Expect(list[3], EqualTo(5), this.Prefix + "Add." + subtest++);
            Expect(list[4], EqualTo(7), this.Prefix + "Add." + subtest++);
            Expect(list[5], EqualTo(8), this.Prefix + "Add." + subtest++);
            list.Add(4);
            Expect(list.Count, EqualTo(7), this.Prefix + "Add." + subtest++);
            Expect(list[0], EqualTo(1), this.Prefix + "Add." + subtest++);
            Expect(list[1], EqualTo(2), this.Prefix + "Add." + subtest++);
            Expect(list[2], EqualTo(3), this.Prefix + "Add." + subtest++);
            Expect(list[3], EqualTo(4), this.Prefix + "Add." + subtest++);
            Expect(list[4], EqualTo(5), this.Prefix + "Add." + subtest++);
            Expect(list[5], EqualTo(7), this.Prefix + "Add." + subtest++);
            Expect(list[6], EqualTo(8), this.Prefix + "Add." + subtest++);
        }
        [Test]
        public void AddNonUnique()
        {
            int subtest = 0;
            Target.Sorted.List<int> list = new Target.Sorted.List<int>();
            Expect(list.Count, EqualTo(0), this.Prefix + "AddNonUnique." + subtest++);
            list.Add(5);
            Expect(list.Count, EqualTo(1), this.Prefix + "AddNonUnique." + subtest++);
            Expect(list[0], EqualTo(5), this.Prefix + "AddNonUnique." + subtest++);
            list.Add(3);
            Expect(list.Count, EqualTo(2), this.Prefix + "AddNonUnique." + subtest++);
            Expect(list[0], EqualTo(3), this.Prefix + "AddNonUnique." + subtest++);
            Expect(list[1], EqualTo(5), this.Prefix + "AddNonUnique." + subtest++);
            list.Add(2);
            Expect(list.Count, EqualTo(3), this.Prefix + "AddNonUnique." + subtest++);
            Expect(list[0], EqualTo(2), this.Prefix + "AddNonUnique." + subtest++);
            Expect(list[1], EqualTo(3), this.Prefix + "AddNonUnique." + subtest++);
            Expect(list[2], EqualTo(5), this.Prefix + "AddNonUnique." + subtest++);
            list.Add(2);
            Expect(list.Count, EqualTo(4), this.Prefix + "AddNonUnique." + subtest++);
            Expect(list[0], EqualTo(2), this.Prefix + "AddNonUnique." + subtest++);
            Expect(list[1], EqualTo(2), this.Prefix + "AddNonUnique." + subtest++);
            Expect(list[2], EqualTo(3), this.Prefix + "AddNonUnique." + subtest++);
            Expect(list[3], EqualTo(5), this.Prefix + "AddNonUnique." + subtest++);
            list.Add(1);
            Expect(list.Count, EqualTo(5), this.Prefix + "AddNonUnique." + subtest++);
            Expect(list[0], EqualTo(1), this.Prefix + "AddNonUnique." + subtest++);
            Expect(list[1], EqualTo(2), this.Prefix + "AddNonUnique." + subtest++);
            Expect(list[2], EqualTo(2), this.Prefix + "AddNonUnique." + subtest++);
            Expect(list[3], EqualTo(3), this.Prefix + "AddNonUnique." + subtest++);
            Expect(list[4], EqualTo(5), this.Prefix + "AddNonUnique." + subtest++);
        }
        [Test]
        public void Remove()
        {
            int subtest = 0;
            Target.Sorted.List<int> list = new Target.Sorted.List<int>();
            list.Add(5);
            list.Add(3);
            list.Add(2);
            list.Add(7);
            list.Add(1);
            list.Add(4);
            list.Add(6);
            Expect(list.Count, EqualTo(7), this.Prefix + "Remove." + subtest++);
            Expect(list[0], EqualTo(1), this.Prefix + "Remove." + subtest++);
            Expect(list[1], EqualTo(2), this.Prefix + "Remove." + subtest++);
            Expect(list[2], EqualTo(3), this.Prefix + "Remove." + subtest++);
            Expect(list[3], EqualTo(4), this.Prefix + "Remove." + subtest++);
            Expect(list[4], EqualTo(5), this.Prefix + "Remove." + subtest++);
            Expect(list[5], EqualTo(6), this.Prefix + "Remove." + subtest++);
            Expect(list[6], EqualTo(7), this.Prefix + "Remove." + subtest++);

            Expect(list.Remove(list.Count / 2), EqualTo(4), this.Prefix + "Remove." + subtest++);
            Expect(list.Count, EqualTo(6), this.Prefix + "Remove." + subtest++);
            Expect(list[0], EqualTo(1), this.Prefix + "Remove." + subtest++);
            Expect(list[1], EqualTo(2), this.Prefix + "Remove." + subtest++);
            Expect(list[2], EqualTo(3), this.Prefix + "Remove." + subtest++);
            Expect(list[3], EqualTo(5), this.Prefix + "Remove." + subtest++);
            Expect(list[4], EqualTo(6), this.Prefix + "Remove." + subtest++);
            Expect(list[5], EqualTo(7), this.Prefix + "Remove." + subtest++);

            Expect(list.Remove(), EqualTo(7), this.Prefix + "Remove." + subtest++);
            Expect(list.Count, EqualTo(5), this.Prefix + "Remove." + subtest++);
            Expect(list[0], EqualTo(1), this.Prefix + "Remove." + subtest++);
            Expect(list[1], EqualTo(2), this.Prefix + "Remove." + subtest++);
            Expect(list[2], EqualTo(3), this.Prefix + "Remove." + subtest++);
            Expect(list[3], EqualTo(5), this.Prefix + "Remove." + subtest++);
            Expect(list[4], EqualTo(6), this.Prefix + "Remove." + subtest++);
        }
    }
}
