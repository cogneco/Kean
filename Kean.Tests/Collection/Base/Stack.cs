// 
//  Stack.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2010 Simon Mika
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
	public abstract class Stack<T, S> :
		Kean.Test.Fixture<T>
        where T : Kean.Test.Fixture<T>, new()
		where S : Target.IStack<int>, new()
	{
		protected Stack ()
		{ }
		protected Stack (string prefix) :
			base(prefix)
		{ }
		protected override void Run()
		{
            this.Run(
    			(Action)this.EmptyStack,
                Kean.Test.Test<Target.Exception.Empty>.Create(this.PopEmpty, this.Prefix + "PopEmpty.0"),
                Kean.Test.Test<Target.Exception.Empty>.Create(this.PeekEmpty, this.Prefix + "PeekEmpty.0"),
                (Action)this.PushPeekPop,
                (Action)this.PushPeekPopTen,
                (Action)this.PushPopPush
            );
		}
		[Test]
		public void EmptyStack()
		{
			S target = new S();
			Verify(target.Empty, Is.True);
		}
		[Test]
		[ExpectedException(typeof(Target.Exception.Empty))]
		public void PopEmpty()
		{
			S target = new S();
			Verify(target.Empty, Is.True);
			target.Pop();
		}
		[Test]
		[ExpectedException(typeof(Target.Exception.Empty))]
		public void PeekEmpty()
		{
			S target = new S();
			Verify(target.Empty, Is.True);
			target.Peek();
		}
		[Test]
		public void PushPeekPop()
		{
			S target = new S();
			Verify(target.Empty,Is.True);
			target.Push(42);
			Verify(target.Peek(), EqualTo(42), this.Prefix + "PushPeekPop.1");
			Verify(target.Pop(), EqualTo(42), this.Prefix + "PushPeekPop.2");
			Verify(target.Empty, Is.True);
		}
		[Test]
		public void PushPeekPopTen()
		{
			S target = new S();
			Verify(target.Empty, Is.True);
			for (int i = 0; i < 10; i++)
			{
				target.Push(i);
				Verify(target.Peek(), EqualTo(i), this.Prefix + "PushPeekPopTen." + (1 + i));
			}
			for (int i = 0; i < 10; i++)
			{
				Verify(target.Peek(), EqualTo(9 - i), this.Prefix + "PushPeekPopTen." + (12 + i * 2));
				Verify(target.Pop(), EqualTo(9 - i), this.Prefix + "PushPeekPopTen." + (13 + i * 2));
			}
			Verify(target.Empty, Is.True);
		}
		[Test]
		public void PushPopPush()
		{
			S target = new S();
			Verify(target.Empty, Is.True);
			for (int i = 0; i < 10; i++)
			{
				target.Push(i);
				Verify(target.Peek(), EqualTo(i), this.Prefix + "PushPeekPopTen." + (1 + i));
			}
			for (int i = 0; i < 5; i++)
			{
				Verify(target.Peek(), EqualTo(9 - i), this.Prefix + "PushPeekPopTen." + (16 + i * 2));
				Verify(target.Pop(), EqualTo(9 - i), this.Prefix + "PushPeekPopTen." + (17 + i * 2));
			}
			for (int i = 0; i < 5; i++)
			{
				target.Push(i + 5);
				Verify(target.Peek(), EqualTo(i + 5), this.Prefix + "PushPeekPopTen." + (28 + i));
			}
			for (int i = 0; i < 10; i++)
			{
				Verify(target.Peek(), EqualTo(9 - i), this.Prefix + "PushPeekPopTen." + (34 + i * 2));
				Verify(target.Pop(), EqualTo(9 - i), this.Prefix + "PushPeekPopTen." + (35 + i * 2));
			}
			Verify(target.Empty, Is.True);
		}
	}
}

