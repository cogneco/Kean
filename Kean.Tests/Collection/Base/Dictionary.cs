// 
//  Dictionary.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2011 Simon Mika
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
	public abstract class Dictionary<T, D> :
		Kean.Test.Fixture<T>
		where T : Kean.Test.Fixture<T>, new()
		where D : Target.IDictionary<string, int>
	{
		public string[] Correct { get; private set; }
		protected Dictionary()
		{
			this.Initialize();
		}
		public Dictionary (string prefix) :
			base(prefix)
		{
			this.Initialize();
		}

		void Initialize()
		{
			this.Correct = new string[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen" };
		}

		protected abstract D Create(int size);

		protected override void Run()
		{
			this.Run(
				this.SingleElement,
				this.TwoElements,
				this.TwelveElements,
				this.Equality,
				this.NullValue
			);
		}
		
		[Test]
		public void SingleElement()
		{
			D target = this.Create(10);
			Verify(target.Contains(this.Correct[1]), Is.False);
			Verify(target[this.Correct[1]], Is.EqualTo(0));
			Verify(target.Remove(this.Correct[1]), Is.False);
			target[this.Correct[1]] = 1;
			Verify(target.Contains(this.Correct[2]), Is.False);
			Verify(target[this.Correct[2]], Is.EqualTo(0));
			Verify(target.Remove(this.Correct[2]), Is.False);
			Verify(target.Contains(this.Correct[1]), Is.True);
			Verify(target[this.Correct[1]], Is.EqualTo(1));
			Verify(target.Remove(this.Correct[1]), Is.True);
			Verify(target.Contains(this.Correct[1]), Is.False);
			Verify(target[this.Correct[1]], Is.EqualTo(0));
			Verify(target.Remove(this.Correct[1]), Is.False);
		}
		[Test]
		public void TwoElements()
		{
			D target = this.Create(10);
			target[this.Correct[1]] = 1;
			target[this.Correct[2]] = 2;
			Verify(target.Contains(this.Correct[3]), Is.False);
			Verify(target[this.Correct[3]], Is.EqualTo(0));
			Verify(target.Remove(this.Correct[3]), Is.False);
			Verify(target.Contains(this.Correct[1]), Is.True);
			Verify(target.Contains(this.Correct[2]), Is.True);
			Verify(target[this.Correct[1]], Is.EqualTo(1));
			Verify(target[this.Correct[2]], Is.EqualTo(2));
			Verify(target.Remove(this.Correct[1]), Is.True);
			Verify(target.Remove(this.Correct[2]), Is.True);
			Verify(target.Contains(this.Correct[1]), Is.False);
			Verify(target[this.Correct[1]], Is.EqualTo(0));
			Verify(target.Remove(this.Correct[1]), Is.False);
			Verify(target.Contains(this.Correct[2]), Is.False);
			Verify(target[this.Correct[2]], Is.EqualTo(0));
			Verify(target.Remove(this.Correct[2]), Is.False);
		}
		[Test]
		public void TwelveElements()
		{
			D target = this.Create(10);
			for (int i = 1; i < 13; i++)
				Verify(target.Contains(this.Correct[i]), Is.False);
			for (int i = 1; i < 13; i++)
				Verify(target[this.Correct[i]], Is.EqualTo(0));
			for (int i = 1; i < 13; i++)
				Verify(target.Remove(this.Correct[i]), Is.False);
			for (int i = 1; i < 13; i++)
				target[this.Correct[i]] = i;
			Verify(target.Contains(this.Correct[13]), Is.False);
			Verify(target[this.Correct[13]], Is.EqualTo(0));
			Verify(target.Remove(this.Correct[13]), Is.False);
			for (int i = 1; i < 13; i++)
				Verify(target.Contains(this.Correct[i]), Is.True);
			for (int i = 1; i < 13; i++)
				Verify(target[this.Correct[i]], Is.EqualTo(i));
			for (int i = 1; i < 13; i++)
				Verify(target.Remove(this.Correct[i]), Is.True);
			for (int i = 1; i < 13; i++)
				Verify(target.Contains(this.Correct[i]), Is.False);
			for (int i = 1; i < 13; i++)
				Verify(target[this.Correct[i]], Is.EqualTo(0));
			for (int i = 1; i < 13; i++)
				Verify(target.Remove(this.Correct[i]), Is.False);

		}
		[Test]
		public void Equality()
		{
			D a = this.Create(100);
			D b = this.Create(100);
			for (int i = 0; i < 100; i++)
			{
				a["index" + i] = i;
				b["index" + (99 - i)] = 99 - i;
			}
			Verify(a.Equals(b), Is.True);
			Verify(b.Equals(a), Is.True);
			b["index100"] = 100;
			Verify(a.Equals(b), Is.False);
			Verify(b.Equals(a), Is.False);
			b.Remove("index100");
			Verify(a.Equals(b), Is.True);
			Verify(b.Equals(a), Is.True);
			b.Remove("index50");
			Verify(a.Equals(b), Is.False);
			Verify(b.Equals(a), Is.False);
		}
		[Test]
		public void NullValue()
		{
			Collection.IDictionary<string, string> d = new Collection.Dictionary<string, string>();
			d["hello"] = "goodbye";
			d["kean"] = null;
			Verify(d.Contains("hello"), Is.True);
			Verify(d.Contains("kean"), Is.True);
			Verify(d.Contains("kean"), Is.True);
			Verify(d.Remove("kean"), Is.True);
			Verify(d.Contains("kean"), Is.False);
			Verify(d["kean"], Is.Null);
		}
	}
}

