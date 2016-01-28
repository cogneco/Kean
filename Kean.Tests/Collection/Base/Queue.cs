// 
//  Queue.cs
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
using Kean.Collection.Extension;

namespace Kean.Collection.Test.Base
{
	public abstract class Queue<T, Q> :
		Kean.Test.Fixture<T>
        where T : Kean.Test.Fixture<T>, new()
        where Q : Target.IQueue<int>, new()
	{
		protected Queue ()
		{
		}
		protected Queue (string prefix) :
			base(prefix)
		{
		}

		protected override void Run ()
		{
			this.Run (
            this.EmptyQueue,
            this.DequeueEmpty,
            this.PeekEmpty,
            this.EnqueuePeekDequeue,
            this.EnqueuePeekDequeueTen,
            this.EnqueueDequeueEnqueue,
            this.AddAfterEmptyQueue
			);
		}
		[Test]
		public void AddAfterEmptyQueue ()
		{
			Q target = new Q ();
			for (int i = 0; i < 10; i++)
				target.Enqueue (i);
			target.Clear ();
			for (int i = 0; i < 10; i++)
				target.Enqueue (i);
			Verify (target.Count, EqualTo (10));
		}
		[Test]
		public void EmptyQueue ()
		{
			Q target = new Q ();
			Verify (target.Empty, Is.True);
			Verify (target.Count, EqualTo (0));
		}
		[Test]
		public void DequeueEmpty ()
		{
			Q target = new Q ();
			Verify (target.Empty, Is.True);
			Verify (target.Dequeue (), EqualTo (0));
		}
		[Test]
		public void PeekEmpty ()
		{
			Q target = new Q ();
			Verify (target.Empty, Is.True);
			Verify (target.Peek (), EqualTo (0));
		}
		[Test]
		public void EnqueuePeekDequeue ()
		{
			Q target = new Q ();
			Verify (target.Empty, Is.True);
			Verify (target.Count, EqualTo (0));
			target.Enqueue (42);
			Verify (target.Count, EqualTo (1));
			Verify (target.Peek (), EqualTo (42));
			Verify (target.Count, EqualTo (1));
			Verify (target.Dequeue (), EqualTo (42));
			Verify (target.Count, EqualTo (0));
			Verify (target.Empty, Is.True);
		}
		[Test]
		public void EnqueuePeekDequeueTen ()
		{
			Q target = new Q ();
			Verify (target.Empty, Is.True);
			Verify (target.Count, EqualTo (0));
			for (int i = 0; i < 10; i++) {
				target.Enqueue (i);
				Verify (target.Peek (), EqualTo (0));
				Verify (target.Count, EqualTo (i + 1));
			}
			for (int i = 0; i < 10; i++) {
				Verify (target.Peek (), EqualTo (i));
				Verify (target.Count, EqualTo (10 - i));
				Verify (target.Dequeue (), EqualTo (i));
				Verify (target.Count, EqualTo (10 - i - 1));
			}
			Verify (target.Empty, Is.True);
		}
		[Test]
		public void EnqueueDequeueEnqueue ()
		{
			Q target = new Q ();
			Verify (target.Empty, Is.True);
			for (int i = 0; i < 10; i++) {
				target.Enqueue (i);
				Verify (target.Peek (), EqualTo (0));
				Verify (target.Count, EqualTo (i + 1));
			}
			for (int i = 0; i < 5; i++) {
				Verify (target.Count, EqualTo (10 - i));
				Verify (target.Peek (), EqualTo (i));
				Verify (target.Count, EqualTo (10 - i));
				Verify (target.Dequeue (), EqualTo (i));
				Verify (target.Count, EqualTo (10 - i - 1));
			}
			for (int i = 0; i < 5; i++) {
				target.Enqueue (i + 10);
				Verify (target.Count, EqualTo (5 + 1 + i));
				Verify (target.Peek (), EqualTo (5));
				Verify (target.Count, EqualTo (5 + 1 + i));
			}
			for (int i = 0; i < 10; i++) {
				Verify (target.Count, EqualTo (10 - i));
				Verify (target.Peek (), EqualTo (i + 5));
				Verify (target.Count, EqualTo (10 - i));
				Verify (target.Dequeue (), EqualTo (i + 5));
				Verify (target.Count, EqualTo (10 - i - 1));
			}
			Verify (target.Empty, Is.True);
			Verify (target.Count, Is.EqualTo (0));
		}
	}
}

