// 
//  Action.cs
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

namespace Kean.Reflect
{
	public class Action : Method
	{
		public void Call() { this.Call(null); }
		internal Action(object parent, System.Type parentType, System.Reflection.MethodInfo information) : base(parent, parentType, information) { }
	}
	public class Action<T> : Method
	{
		public void Call(T argument) { this.Call(new object[] { argument }); }
		internal Action(object parent, System.Type parentType, System.Reflection.MethodInfo information) : base(parent, parentType, information) { }
	}
	public class Action<T1, T2> : Method
	{
		public void Call(T1 argument1, T2 argument2) { this.Call(new object[] { argument1, argument2 }); }
		internal Action(object parent, System.Type parentType, System.Reflection.MethodInfo information) : base(parent, parentType, information) { }
	}
	public class Action<T1, T2, T3> : Method
	{
		public void Call(T1 argument1, T2 argument2, T3 argument3) { this.Call(new object[] { argument1, argument2, argument3 }); }
		internal Action(object parent, System.Type parentType, System.Reflection.MethodInfo information) : base(parent, parentType, information) { }
	}
	public class Action<T1, T2, T3, T4> : Method
	{
		public void Call(T1 argument1, T2 argument2, T3 argument3, T4 argument4) { this.Call(new object[] { argument1, argument2, argument3, argument4 }); }
		internal Action(object parent, System.Type parentType, System.Reflection.MethodInfo information) : base(parent, parentType, information) { }
	}
	public class Action<T1, T2, T3, T4, T5> : Method
	{
		public void Call(T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5) { this.Call(new object[] { argument1, argument2, argument3, argument4, argument5 }); }
		internal Action(object parent, System.Type parentType, System.Reflection.MethodInfo information) : base(parent, parentType, information) { }
	}
	public class Action<T1, T2, T3, T4, T5, T6> : Method
	{
		public void Call(T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6) { this.Call(new object[] { argument1, argument2, argument3, argument4, argument5, argument6 }); }
		internal Action(object parent, System.Type parentType, System.Reflection.MethodInfo information) : base(parent, parentType, information) { }
	}
	public class Action<T1, T2, T3, T4, T5, T6, T7> : Method
	{
		public void Call(T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6, T7 argument7) { this.Call(new object[] { argument1, argument2, argument3, argument4, argument5, argument6, argument7 }); }
		internal Action(object parent, System.Type parentType, System.Reflection.MethodInfo information) : base(parent, parentType, information) { }
	}
	public class Action<T1, T2, T3, T4, T5, T6, T7, T8> : Method
	{
		public void Call(T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6, T7 argument7, T8 argument8) { this.Call(new object[] { argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8 }); }
		internal Action(object parent, System.Type parentType, System.Reflection.MethodInfo information) : base(parent, parentType, information) { }
	}
	public class Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> : Method
	{
		public void Call(T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6, T7 argument7, T8 argument8, T9 argument9) { this.Call(new object[] { argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9 }); }
		internal Action(object parent, System.Type parentType, System.Reflection.MethodInfo information) : base(parent, parentType, information) { }
	}
	public class Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> : Method
	{
		public void Call(T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6, T7 argument7, T8 argument8, T9 argument9, T10 argument10) { this.Call(new object[] { argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10 }); }
		internal Action(object parent, System.Type parentType, System.Reflection.MethodInfo information) : base(parent, parentType, information) { }
	}
	public class Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> : Method
	{
		public void Call(T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6, T7 argument7, T8 argument8, T9 argument9, T10 argument10, T11 argument11) { this.Call(new object[] { argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10, argument11 }); }
		internal Action(object parent, System.Type parentType, System.Reflection.MethodInfo information) : base(parent, parentType, information) { }
	}
	public class Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> : Method
	{
		public void Call(T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6, T7 argument7, T8 argument8, T9 argument9, T10 argument10, T11 argument11, T12 argument12) { this.Call(new object[] { argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10, argument11, argument12 }); }
		internal Action(object parent, System.Type parentType, System.Reflection.MethodInfo information) : base(parent, parentType, information) { }
	}
	public class Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> : Method
	{
		public void Call(T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6, T7 argument7, T8 argument8, T9 argument9, T10 argument10, T11 argument11, T12 argument12, T13 argument13) { this.Call(new object[] { argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10, argument11, argument12, argument13 }); }
		internal Action(object parent, System.Type parentType, System.Reflection.MethodInfo information) : base(parent, parentType, information) { }
	}
	public class Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> : Method
	{
		public void Call(T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6, T7 argument7, T8 argument8, T9 argument9, T10 argument10, T11 argument11, T12 argument12, T13 argument13, T14 argument14) { this.Call(new object[] { argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10, argument11, argument12, argument13, argument14 }); }
		internal Action(object parent, System.Type parentType, System.Reflection.MethodInfo information) : base(parent, parentType, information) { }
	}
	public class Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> : Method
	{
		public void Call(T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6, T7 argument7, T8 argument8, T9 argument9, T10 argument10, T11 argument11, T12 argument12, T13 argument13, T14 argument14, T15 argument15) { this.Call(new object[] { argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10, argument11, argument12, argument13, argument14, argument15 }); }
		internal Action(object parent, System.Type parentType, System.Reflection.MethodInfo information) : base(parent, parentType, information) { }
	}
	public class Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> : Method
	{
		public void Call(T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6, T7 argument7, T8 argument8, T9 argument9, T10 argument10, T11 argument11, T12 argument12, T13 argument13, T14 argument14, T15 argument15, T16 argument16) { this.Call(new object[] { argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10, argument11, argument12, argument13, argument14, argument15, argument16 }); }
		internal Action(object parent, System.Type parentType, System.Reflection.MethodInfo information) : base(parent, parentType, information) { }
	}
}
