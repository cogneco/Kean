// 
//  Function.cs
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
	public class Function<TResult> : Method
	{
		public TResult Call() { return (TResult)this.Call(null); }
		internal Function(object parent, System.Type parentType, System.Reflection.MethodInfo information) : base(parent, parentType, information) { }
	}
	public class Function<T, TResult> : Method
	{
		public TResult Call(T argument) { return (TResult)this.Call(new object[] { argument }); }
		internal Function(object parent, System.Type parentType, System.Reflection.MethodInfo information) : base(parent, parentType, information) { }
	}
	public class Function<T1, T2, TResult> : Method
	{
		public TResult Call(T1 argument1, T2 argument2) { return (TResult)this.Call(new object[] { argument1, argument2 }); }
		internal Function(object parent, System.Type parentType, System.Reflection.MethodInfo information) : base(parent, parentType, information) { }
	}
	public class Function<T1, T2, T3, TResult> : Method
	{
		public TResult Call(T1 argument1, T2 argument2, T3 argument3) { return (TResult)this.Call(new object[] { argument1, argument2, argument3 }); }
		internal Function(object parent, System.Type parentType, System.Reflection.MethodInfo information) : base(parent, parentType, information) { }
	}
	public class Function<T1, T2, T3, T4, TResult> : Method
	{
		public TResult Call(T1 argument1, T2 argument2, T3 argument3, T4 argument4) { return (TResult)this.Call(new object[] { argument1, argument2, argument3, argument4 }); }
		internal Function(object parent, System.Type parentType, System.Reflection.MethodInfo information) : base(parent, parentType, information) { }
	}
	public class Function<T1, T2, T3, T4, T5, TResult> : Method
	{
		public TResult Call(T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5) { return (TResult)this.Call(new object[] { argument1, argument2, argument3, argument4, argument5 }); }
		internal Function(object parent, System.Type parentType, System.Reflection.MethodInfo information) : base(parent, parentType, information) { }
	}
	public class Function<T1, T2, T3, T4, T5, T6, TResult> : Method
	{
		public TResult Call(T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6) { return (TResult)this.Call(new object[] { argument1, argument2, argument3, argument4, argument5, argument6 }); }
		internal Function(object parent, System.Type parentType, System.Reflection.MethodInfo information) : base(parent, parentType, information) { }
	}
	public class Function<T1, T2, T3, T4, T5, T6, T7, TResult> : Method
	{
		public TResult Call(T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6, T7 argument7) { return (TResult)this.Call(new object[] { argument1, argument2, argument3, argument4, argument5, argument6, argument7 }); }
		internal Function(object parent, System.Type parentType, System.Reflection.MethodInfo information) : base(parent, parentType, information) { }
	}
	public class Function<T1, T2, T3, T4, T5, T6, T7, T8, TResult> : Method
	{
		public TResult Call(T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6, T7 argument7, T8 argument8) { return (TResult)this.Call(new object[] { argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8 }); }
		internal Function(object parent, System.Type parentType, System.Reflection.MethodInfo information) : base(parent, parentType, information) { }
	}
	public class Function<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> : Method
	{
		public TResult Call(T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6, T7 argument7, T8 argument8, T9 argument9) { return (TResult)this.Call(new object[] { argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9 }); }
		internal Function(object parent, System.Type parentType, System.Reflection.MethodInfo information) : base(parent, parentType, information) { }
	}
	public class Function<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> : Method
	{
		public TResult Call(T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6, T7 argument7, T8 argument8, T9 argument9, T10 argument10) { return (TResult)this.Call(new object[] { argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10 }); }
		internal Function(object parent, System.Type parentType, System.Reflection.MethodInfo information) : base(parent, parentType, information) { }
	}
	public class Function<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> : Method
	{
		public TResult Call(T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6, T7 argument7, T8 argument8, T9 argument9, T10 argument10, T11 argument11) { return (TResult)this.Call(new object[] { argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10, argument11 }); }
		internal Function(object parent, System.Type parentType, System.Reflection.MethodInfo information) : base(parent, parentType, information) { }
	}
	public class Function<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> : Method
	{
		public TResult Call(T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6, T7 argument7, T8 argument8, T9 argument9, T10 argument10, T11 argument11, T12 argument12) { return (TResult)this.Call(new object[] { argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10, argument11, argument12 }); }
		internal Function(object parent, System.Type parentType, System.Reflection.MethodInfo information) : base(parent, parentType, information) { }
	}
	public class Function<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> : Method
	{
		public TResult Call(T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6, T7 argument7, T8 argument8, T9 argument9, T10 argument10, T11 argument11, T12 argument12, T13 argument13) { return (TResult)this.Call(new object[] { argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10, argument11, argument12, argument13 }); }
		internal Function(object parent, System.Type parentType, System.Reflection.MethodInfo information) : base(parent, parentType, information) { }
	}
	public class Function<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> : Method
	{
		public TResult Call(T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6, T7 argument7, T8 argument8, T9 argument9, T10 argument10, T11 argument11, T12 argument12, T13 argument13, T14 argument14) { return (TResult)this.Call(new object[] { argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10, argument11, argument12, argument13, argument14 }); }
		internal Function(object parent, System.Type parentType, System.Reflection.MethodInfo information) : base(parent, parentType, information) { }
	}
	public class Function<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> : Method
	{
		public TResult Call(T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6, T7 argument7, T8 argument8, T9 argument9, T10 argument10, T11 argument11, T12 argument12, T13 argument13, T14 argument14, T15 argument15) { return (TResult)this.Call(new object[] { argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10, argument11, argument12, argument13, argument14, argument15 }); }
		internal Function(object parent, System.Type parentType, System.Reflection.MethodInfo information) : base(parent, parentType, information) { }
	}
	public class Function<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> : Method
	{
		public TResult Call(T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6, T7 argument7, T8 argument8, T9 argument9, T10 argument10, T11 argument11, T12 argument12, T13 argument13, T14 argument14, T15 argument15, T16 argument16) { return (TResult)this.Call(new object[] { argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10, argument11, argument12, argument13, argument14, argument15, argument16 }); }
		internal Function(object parent, System.Type parentType, System.Reflection.MethodInfo information) : base(parent, parentType, information) { }
	}
}
