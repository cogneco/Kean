// 
//  ReferenceCount.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2009-2012 Simon Mika
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
using Kean.Extension;

namespace Kean
{
	public class ReferenceCounter :
		Synchronized
	{
		object target;
		int count;
		bool claimed = true;
		object claimLock = new object();
		public bool Claimed { get { lock (this.claimLock) return this.claimed; } }
		Action<object> reuse;
		public Action<object> Reuse
		{
			set
			{
				lock (this.Lock)
					if (this.reuse.NotNull())
						this.reuse = value;
			}
		}
		public ReferenceCounter(object target)
		{
			this.target = target;
		}
		public void Update(int delta)
		{
			if (delta != 0)
			{
				lock (this.Lock)
				{
					this.count += delta;
					if (this.count <= 0)
					{
						if (this.reuse.NotNull())
						{
							this.count = 0;
							this.reuse(this.target);
						}
						else if (this.target is IDisposable)
							(this.target as IDisposable).Dispose();
					}
				}
			}
		}
		public void Increase()
		{
			this.Update(1);
		}
		public void Decrease()
		{
			this.Update(-1);
		}
		public bool Claim()
		{
			lock (this.claimLock)
			{
				this.claimed = System.Threading.Monitor.Wait(this.claimLock);
				System.Threading.Monitor.PulseAll(this.claimLock);
			}
			return true;
		}
		public bool Claim(int milliseconds)
		{
			return this.Claim(new TimeSpan(0, 0, 0, 0, milliseconds));
		}
		public bool Claim(TimeSpan timeout)
		{
			bool result = false;
			lock (this.claimLock)
			{
				if (System.Threading.Monitor.Wait(this.claimLock, timeout) && this.Claimed)
					result = this.claimed = true;
				System.Threading.Monitor.PulseAll(this.claimLock);
			}
			return result;
		}
		public bool TryClaim()
		{
			bool result = false;
			lock (this.claimLock)
			{
				if (!this.Claimed)
					result = this.claimed = true;
				System.Threading.Monitor.PulseAll(this.claimLock);
			}
			return result;
		}
		public void Unclaim()
		{
			lock (this.claimLock)
			{
				this.claimed = false;
				System.Threading.Monitor.PulseAll(this.claimLock);
			}
		}
		public override string ToString()
		{
			return this.count.ToString();
		}
		public static ReferenceCounter operator ++(ReferenceCounter counter)
		{
			counter.Increase();
			return counter;
		}
		public static ReferenceCounter operator --(ReferenceCounter counter)
		{
			counter.Decrease();
			return counter;
		}
	}
}