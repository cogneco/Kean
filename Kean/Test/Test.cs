// 
//  Test.cs
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
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.using System;
using System;
using Kean.Extension;
using NUnit.Framework;

namespace Kean.Test
{
	public class Test
	{
		public string Name { get; private set; }
		protected Action Method { get; set; }
		internal Test(Action method, string name)
		{
			this.Method = method;
			this.Name = name;
		}
		Test(Action method) :
			this(method, method.Method.Name)
		{
		}
		internal virtual void Run(Action pre, Action post)
		{
			pre.Call();
			if (Kean.Error.Log.CatchErrors)
				try
				{
					this.Method.Call();
				}
				catch (Exception)
				{
					Console.Write("F");
				}
			else
				this.Method.Call();
			post.Call();
		}
		public static implicit operator Test(Action method)
		{
			return new Test(method);
		}
	}

	public class Test<T> :
		Test
		where T : Exception
	{
		internal Test(Action method, string name) :
			base(method, name)
		{
		}
		internal override void Run(Action pre, Action post)
		{
			bool catched = false;
			pre.Call();
			try
			{
				this.Method.Call();
			}
			catch (T)
			{
				catched = true;
			}
			finally
			{
				if (!catched)
					Assert.Fail("Expected exception \"{0}\" in test \"{1}\" but it was not received.", typeof(T).FullName, this.Name);
				post.Call();
			}
		}
		public static Test<T> Create(Action method, string name)
		{
			return new Test<T>(method, name);
		}
	}
}
