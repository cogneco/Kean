// 
//  List.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2009 Simon Mika
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

using Target = Kean.Collection;

namespace Kean.Collection.Test.Base
{
	public abstract class List<T, L> :
		Vector<T, L>
		where T : Kean.Test.Fixture<T>, new()
		where L : Target.IList<int>, new()
	{
		protected List ()
		{ }
		protected List (string prefix) :
			base(prefix)
		{ }

		protected override void Run()
		{
			base.Run();
			this.Run(
				this.Equality,
				this.AddReplaceRemove,
				this.AddReplaceRemoveTen,
				this.InsertReplaceRemove,
				this.BoundaryCases
				);
		}
        [Test]
        public void EqualityList()
        {
            L a = new L();
            L b = new L();
            for (int i = 0; i < 10; i++)
            {
                a.Add(i);
                b.Add(i);
            }
           Verify(a, EqualTo(b), this.Prefix + "Equality.0");
           Verify(a, EqualTo(b as object), this.Prefix + "Equality.1");
           Verify(b, EqualTo(a), this.Prefix + "Equality.2");
           Verify(b, EqualTo(a as object), this.Prefix + "Equality.3");
            b.Add(10);
           Verify(a, Is.Not.EqualTo(b as object), this.Prefix + "Equality.4");
           Verify(b, Is.Not.EqualTo(a as object), this.Prefix + "Equality.5");
        }
        [Test]
		public void AddReplaceRemove()
		{
			L target = new L();
			Verify(target.Count, EqualTo(0), this.Prefix + "AddReplaceRemove.0");
			target.Add(42);
			Verify(target.Count, EqualTo(1), this.Prefix + "AddReplaceRemove.1");
			Verify(target[0], EqualTo(42), this.Prefix + "AddReplaceRemove.2");
			target[0] = 1337;
			Verify(target[0], EqualTo(1337), this.Prefix + "AddReplaceRemove.3");
			Verify(target.Remove(), EqualTo(1337), this.Prefix + "AddReplaceRemove.4");
			Verify(target.Count, EqualTo(0), this.Prefix + "AddReplaceRemove.5");
		}
		[Test]
		public void AddReplaceRemoveTen()
		{
			L target = new L();
			for (int i = 0; i < 10; i++)
			{
				Verify(target.Count, EqualTo(i), this.Prefix + "AddReplaceRemoveTen." + i);
				target.Add(42 + i);
			}
			Verify(target.Count, EqualTo(10), this.Prefix + "AddReplaceRemoveTen.10");
			for (int i = 0; i < 10; i++)
			{
				Verify(target[i], EqualTo(42 + i), this.Prefix + "AddReplaceRemoveTen." + (11 + 2 * i));
				target[i] = 1337 + i;
				Verify(target[i], EqualTo(1337 + i), this.Prefix + "AddReplaceRemoveTen." + (12 + 2 * i));
			}
			for (int i = 0; i < 10; i++)
			{
				Verify(target.Remove(), EqualTo(1337 + (9 - i)), this.Prefix + "AddReplaceRemoveTen." + (30 + 2 * i));
				Verify(target.Count, EqualTo(9 - i), this.Prefix + "AddReplaceRemoveTen." + (31 + 2 * i));
			}
		}
		[Test]
		public void InsertReplaceRemove()
		{
			L target = new L();
			for (int i = 0; i < 10; i++)
			{
				Verify(target.Count, EqualTo(i), this.Prefix + "InsertReplaceRemove." + i);
				target.Add(i);
			}
			Verify(target[6], EqualTo(6), this.Prefix + "InsertReplaceRemove.10");
			target.Insert(6, 42);
			Verify(target.Count, EqualTo(11), this.Prefix + "InsertReplaceRemove.11");
			for (int i = 0; i < 6; i++)
				Verify(target[i], EqualTo(i), this.Prefix + "InsertReplaceRemove." + (12 + i));
			Verify(target[6], EqualTo(42), this.Prefix + "InsertReplaceRemove.18");
			for (int i = 7; i < 11; i++)
				Verify(target[i], EqualTo(i - 1), this.Prefix + "InsertReplaceRemove." + (19 - 7 + i));
			target[6] = 1337;
			Verify(target[6], EqualTo(1337), this.Prefix + "InsertReplaceRemove.23");
			Verify(target.Remove(6), EqualTo(1337), this.Prefix + "AddReplaceRemoveTen.24");
			Verify(target.Count, EqualTo(10), this.Prefix + "InsertReplaceRemove.15");
			for (int i = 0; i < 10; i++)
				Verify(target[i], EqualTo(i), this.Prefix + "InsertReplaceRemove." + (16 + i));
		}
        [Test]
        public void BoundaryCases()
        {
            L target = new L();
            for (int i = 0; i < 10; i++)
                target.Add(i);
            target.Insert(10, 1111);
           Verify(target[10], EqualTo(1111), this.Prefix + "BoundaryCases.15");
            target.Insert(0, 2222);
           Verify(target[0], EqualTo(2222), this.Prefix + "BoundaryCases.15");
        }
	
	}
}
