// 
//  Task.cs
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
using Kean;
using Kean.Extension;

namespace Kean.Parallel
{
    internal class Task :
        ITask
    {
        public Action Action { get; set; }
        public Task() { }
        public Task(Action action) 
        {
            this.Action = action;
        }
        public void Run()
        {
            this.Action();
        }
    }
    internal class Task<T> :
        ITask
    {
        public T Argument { get; set; }
        public Action<T> Action { get; set; }
        public Task() { }
        public Task(Action<T> action, T argument)
        {
            this.Action = action;
            this.Argument = argument;
        }
        public void Run()
        {
            this.Action(this.Argument);
        }
    }
	internal class Task<T1, T2> :
		ITask
	{
		public T1 Argument1 { get; set; }
		public T2 Argument2 { get; set; }
		public Action<T1, T2> Action { get; set; }
		public Task() { }
		public Task(Action<T1, T2> action, T1 argument1, T2 argument2)
		{
			this.Action = action;
			this.Argument1 = argument1;
			this.Argument2 = argument2;
		}
		public void Run()
		{
			this.Action(this.Argument1, this.Argument2);
		}
	}
    internal class Task<T1, T2, T3> :
        ITask
    {
        public T1 Argument1 { get; set; }
        public T2 Argument2 { get; set; }
        public T3 Argument3 { get; set; }
        public Action<T1, T2, T3> Action { get; set; }
        public Task() { }
        public Task(Action<T1, T2, T3> action, T1 argument1, T2 argument2, T3 argument3)
        {
            this.Action = action;
            this.Argument1 = argument1;
            this.Argument2 = argument2;
            this.Argument3 = argument3;
        }
        public void Run()
        {
            this.Action(this.Argument1, this.Argument2, this.Argument3);
        }
    }
    internal class Task<T1, T2, T3, T4> :
        ITask
    {
        public T1 Argument1 { get; set; }
        public T2 Argument2 { get; set; }
        public T3 Argument3 { get; set; }
        public T4 Argument4 { get; set; }
        public Action<T1, T2, T3, T4> Action { get; set; }
        public Task() { }
        public Task(Action<T1, T2, T3, T4> action, T1 argument1, T2 argument2, T3 argument3, T4 argument4)
        {
            this.Action = action;
            this.Argument1 = argument1;
            this.Argument2 = argument2;
            this.Argument3 = argument3;
            this.Argument4 = argument4;
        }
        public void Run()
        {
            this.Action(this.Argument1, this.Argument2, this.Argument3, this.Argument4);
        }
    }
    internal class Task<T1, T2, T3, T4, T5> :
       ITask
    {
        public T1 Argument1 { get; set; }
        public T2 Argument2 { get; set; }
        public T3 Argument3 { get; set; }
        public T4 Argument4 { get; set; }
        public T5 Argument5 { get; set; }
        public Action<T1, T2, T3, T4, T5> Action { get; set; }
        public Task() { }
        public Task(Action<T1, T2, T3, T4, T5> action, T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5)
        {
            this.Action = action;
            this.Argument1 = argument1;
            this.Argument2 = argument2;
            this.Argument3 = argument3;
            this.Argument4 = argument4;
            this.Argument5 = argument5;
        }
        public void Run()
        {
            this.Action(this.Argument1, this.Argument2, this.Argument3, this.Argument4, this.Argument5);
        }
    }
    internal class Task<T1, T2, T3, T4, T5, T6> :
       ITask
    {
        public T1 Argument1 { get; set; }
        public T2 Argument2 { get; set; }
        public T3 Argument3 { get; set; }
        public T4 Argument4 { get; set; }
        public T5 Argument5 { get; set; }
        public T6 Argument6 { get; set; }
        public Action<T1, T2, T3, T4, T5, T6> Action { get; set; }
        public Task() { }
        public Task(Action<T1, T2, T3, T4, T5, T6> action, T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6)
        {
            this.Action = action;
            this.Argument1 = argument1;
            this.Argument2 = argument2;
            this.Argument3 = argument3;
            this.Argument4 = argument4;
            this.Argument5 = argument5;
            this.Argument6 = argument6;
        }
        public void Run()
        {
            this.Action(this.Argument1, this.Argument2, this.Argument3, this.Argument4, this.Argument5, this.Argument6);
        }
    }
    internal class Task<T1, T2, T3, T4, T5, T6, T7> :
       ITask
    {
        public T1 Argument1 { get; set; }
        public T2 Argument2 { get; set; }
        public T3 Argument3 { get; set; }
        public T4 Argument4 { get; set; }
        public T5 Argument5 { get; set; }
        public T6 Argument6 { get; set; }
        public T7 Argument7 { get; set; }
        public Action<T1, T2, T3, T4, T5, T6, T7> Action { get; set; }
        public Task() { }
        public Task(Action<T1, T2, T3, T4, T5, T6, T7> action, T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6, T7 argument7)
        {
            this.Action = action;
            this.Argument1 = argument1;
            this.Argument2 = argument2;
            this.Argument3 = argument3;
            this.Argument4 = argument4;
            this.Argument5 = argument5;
            this.Argument6 = argument6;
            this.Argument7 = argument7;
        }
        public void Run()
        {
            this.Action(this.Argument1, this.Argument2, this.Argument3, this.Argument4, this.Argument5, this.Argument6, this.Argument7);
        }
    }
    internal class Task<T1, T2, T3, T4, T5, T6, T7, T8> :
       ITask
    {
        public T1 Argument1 { get; set; }
        public T2 Argument2 { get; set; }
        public T3 Argument3 { get; set; }
        public T4 Argument4 { get; set; }
        public T5 Argument5 { get; set; }
        public T6 Argument6 { get; set; }
        public T7 Argument7 { get; set; }
        public T8 Argument8 { get; set; }
        public Action<T1, T2, T3, T4, T5, T6, T7, T8> Action { get; set; }
        public Task() { }
        public Task(Action<T1, T2, T3, T4, T5, T6, T7, T8> action, T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6, T7 argument7, T8 argument8)
        {
            this.Action = action;
            this.Argument1 = argument1;
            this.Argument2 = argument2;
            this.Argument3 = argument3;
            this.Argument4 = argument4;
            this.Argument5 = argument5;
            this.Argument6 = argument6;
            this.Argument7 = argument7;
            this.Argument8 = argument8;
        }
        public void Run()
        {
            this.Action(this.Argument1, this.Argument2, this.Argument3, this.Argument4, this.Argument5, this.Argument6, this.Argument7, this.Argument8);
        }
    }
    internal class Task<T1, T2, T3, T4, T5, T6, T7, T8, TRest> :
       ITask
    {
        public T1 Argument1 { get; set; }
        public T2 Argument2 { get; set; }
        public T3 Argument3 { get; set; }
        public T4 Argument4 { get; set; }
        public T5 Argument5 { get; set; }
        public T6 Argument6 { get; set; }
        public T7 Argument7 { get; set; }
        public T8 Argument8 { get; set; }
        public TRest ArgumentRest { get; set; }
        public Action<T1, T2, T3, T4, T5, T6, T7, T8, TRest> Action { get; set; }
        public Task() { }
        public Task(Action<T1, T2, T3, T4, T5, T6, T7, T8, TRest> action, T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6, T7 argument7, T8 argument8, TRest argumentRest)
        {
            this.Action = action;
            this.Argument1 = argument1;
            this.Argument2 = argument2;
            this.Argument3 = argument3;
            this.Argument4 = argument4;
            this.Argument5 = argument5;
            this.Argument6 = argument6;
            this.Argument7 = argument7;
            this.Argument8 = argument8;
            this.ArgumentRest = argumentRest;
        }
        public void Run()
        {
            this.Action(this.Argument1, this.Argument2, this.Argument3, this.Argument4, this.Argument5, this.Argument6, this.Argument7, this.Argument8, this.ArgumentRest);
        }
    }
}
