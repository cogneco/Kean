// Copyright (C) 2010-2011  Simon Mika <simon@mika.se>
//
// This file is part of Kean.
//
// Kean is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Kean is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with Kean.  If not, see <http://www.gnu.org/licenses/>.
//

using System;

namespace Kean.Extension
{
	public static class FuncExtension
	{
		#region AllTrue
		public static bool AllTrue(this Func<bool> me)
		{
			bool result = true;
			if (me.NotNull())
			{
				Delegate[] functions = me.GetInvocationList();
				for (int i = 0; result && i < functions.Length; i++)
					result &= (functions[i] as Func<bool>).Invoke();
			}
			return result;
		}
		public static bool AllTrue<T>(this Func<T, bool> me, T argument)
		{
			bool result = true;
			if (me.NotNull())
			{
				Delegate[] functions = me.GetInvocationList();
				for (int i = 0; result && i < functions.Length; i++)
					result &= (functions[i] as Func<T, bool>).Invoke(argument);
			}
			return result;
		}
		public static bool AllTrue<T1, T2>(this Func<T1, T2, bool> me, T1 argument1, T2 argument2)
		{
			bool result = true;
			if (me.NotNull())
			{
				Delegate[] functions = me.GetInvocationList();
				for (int i = 0; result && i < functions.Length; i++)
					result &= (functions[i] as Func<T1, T2, bool>).Invoke(argument1, argument2);
			}
			return result;
		}
		public static bool AllTrue<T1, T2, T3>(this Func<T1, T2, T3, bool> me, T1 argument1, T2 argument2, T3 argument3)
		{
			bool result = true;
			if (me.NotNull())
			{
				Delegate[] functions = me.GetInvocationList();
				for (int i = 0; result && i < functions.Length; i++)
					result &= (functions[i] as Func<T1, T2, T3, bool>).Invoke(argument1, argument2, argument3);
			}
			return result;
		}
		public static bool AllTrue<T1, T2, T3, T4>(this Func<T1, T2, T3, T4, bool> me, T1 argument1, T2 argument2, T3 argument3, T4 argument4)
		{
			bool result = true;
			if (me.NotNull())
			{
				Delegate[] functions = me.GetInvocationList();
				for (int i = 0; result && i < functions.Length; i++)
					result &= (functions[i] as Func<T1, T2, T3, T4, bool>).Invoke(argument1, argument2, argument3, argument4);
			}
			return result;
		}
		public static bool AllTrue<T1, T2, T3, T4, T5>(this Func<T1, T2, T3, T4, T5, bool> me, T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5)
		{
			bool result = true;
			if (me.NotNull())
			{
				Delegate[] functions = me.GetInvocationList();
				for (int i = 0; result && i < functions.Length; i++)
					result &= (functions[i] as Func<T1, T2, T3, T4, T5, bool>).Invoke(argument1, argument2, argument3, argument4, argument5);
			}
			return result;
		}
		public static bool AllTrue<T1, T2, T3, T4, T5, T6>(this Func<T1, T2, T3, T4, T5, T6, bool> me, T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6)
		{
			bool result = true;
			if (me.NotNull())
			{
				Delegate[] functions = me.GetInvocationList();
				for (int i = 0; result && i < functions.Length; i++)
					result &= (functions[i] as Func<T1, T2, T3, T4, T5, T6, bool>).Invoke(argument1, argument2, argument3, argument4, argument5, argument6);
			}
			return result;
		}
		public static bool AllTrue<T1, T2, T3, T4, T5, T6, T7>(this Func<T1, T2, T3, T4, T5, T6, T7, bool> me, T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6, T7 argument7)
		{
			bool result = true;
			if (me.NotNull())
			{
				Delegate[] functions = me.GetInvocationList();
				for (int i = 0; result && i < functions.Length; i++)
					result &= (functions[i] as Func<T1, T2, T3, T4, T5, T6, T7, bool>).Invoke(argument1, argument2, argument3, argument4, argument5, argument6, argument7);
			}
			return result;
		}
		public static bool AllTrue<T1, T2, T3, T4, T5, T6, T7, T8>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, bool> me, T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6, T7 argument7, T8 argument8)
		{
			bool result = true;
			if (me.NotNull())
			{
				Delegate[] functions = me.GetInvocationList();
				for (int i = 0; result && i < functions.Length; i++)
					result &= (functions[i] as Func<T1, T2, T3, T4, T5, T6, T7, T8, bool>).Invoke(argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8);
			}
			return result;
		}
		public static bool AllTrue<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, bool> me, T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6, T7 argument7, T8 argument8, T9 argument9)
		{
			bool result = true;
			if (me.NotNull())
			{
				Delegate[] functions = me.GetInvocationList();
				for (int i = 0; result && i < functions.Length; i++)
					result &= (functions[i] as Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>).Invoke(argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9);
			}
			return result;
		}
		public static bool AllTrue<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, bool> me, T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6, T7 argument7, T8 argument8, T9 argument9, T10 argument10)
		{
			bool result = true;
			if (me.NotNull())
			{
				Delegate[] functions = me.GetInvocationList();
				for (int i = 0; result && i < functions.Length; i++)
					result &= (functions[i] as Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, bool>).Invoke(argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10);
			}
			return result;
		}
		public static bool AllTrue<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, bool> me, T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6, T7 argument7, T8 argument8, T9 argument9, T10 argument10, T11 argument11)
		{
			bool result = true;
			if (me.NotNull())
			{
				Delegate[] functions = me.GetInvocationList();
				for (int i = 0; result && i < functions.Length; i++)
					result &= (functions[i] as Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, bool>).Invoke(argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10, argument11);
			}
			return result;
		}
		public static bool AllTrue<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, bool> me, T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6, T7 argument7, T8 argument8, T9 argument9, T10 argument10, T11 argument11, T12 argument12)
		{
			bool result = true;
			if (me.NotNull())
			{
				Delegate[] functions = me.GetInvocationList();
				for (int i = 0; result && i < functions.Length; i++)
					result &= (functions[i] as Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, bool>).Invoke(argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10, argument11, argument12);
			}
			return result;
		}
		public static bool AllTrue<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, bool> me, T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6, T7 argument7, T8 argument8, T9 argument9, T10 argument10, T11 argument11, T12 argument12, T13 argument13)
		{
			bool result = true;
			if (me.NotNull())
			{
				Delegate[] functions = me.GetInvocationList();
				for (int i = 0; result && i < functions.Length; i++)
					result &= (functions[i] as Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, bool>).Invoke(argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10, argument11, argument12, argument13);
			}
			return result;
		}
		public static bool AllTrue<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, bool> me, T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6, T7 argument7, T8 argument8, T9 argument9, T10 argument10, T11 argument11, T12 argument12, T13 argument13, T14 argument14)
		{
			bool result = true;
			if (me.NotNull())
			{
				Delegate[] functions = me.GetInvocationList();
				for (int i = 0; result && i < functions.Length; i++)
					result &= (functions[i] as Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, bool>).Invoke(argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10, argument11, argument12, argument13, argument14);
			}
			return result;
		}
		public static bool AllTrue<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, bool> me, T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6, T7 argument7, T8 argument8, T9 argument9, T10 argument10, T11 argument11, T12 argument12, T13 argument13, T14 argument14, T15 argument15)
		{
			bool result = true;
			if (me.NotNull())
			{
				Delegate[] functions = me.GetInvocationList();
				for (int i = 0; result && i < functions.Length; i++)
					result &= (functions[i] as Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, bool>).Invoke(argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10, argument11, argument12, argument13, argument14, argument15);
			}
			return result;
		}
		public static bool AllTrue<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, bool> me, T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6, T7 argument7, T8 argument8, T9 argument9, T10 argument10, T11 argument11, T12 argument12, T13 argument13, T14 argument14, T15 argument15, T16 argument16)
		{
			bool result = true;
			if (me.NotNull())
			{
				Delegate[] functions = me.GetInvocationList();
				for (int i = 0; result && i < functions.Length; i++)
					result &= (functions[i] as Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, bool>).Invoke(argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10, argument11, argument12, argument13, argument14, argument15, argument16);
			}
			return result;
		}
		#endregion
		#region TrueExists
		public static bool TrueExists(this Func<bool> me)
		{
			bool result = false;
			if (me.NotNull())
			{
				Delegate[] functions = me.GetInvocationList();
				for (int i = 0; !result && i < functions.Length; i++)
					result |= (functions[i] as Func<bool>).Invoke();
			}
			return result;
		}
		public static bool TrueExists<T>(this Func<T, bool> me, T argument)
		{
			bool result = false;
			if (me.NotNull())
			{
				Delegate[] functions = me.GetInvocationList();
				for (int i = 0; !result && i < functions.Length; i++)
					result |= (functions[i] as Func<T, bool>).Invoke(argument);
			}
			return result;
		}
		public static bool TrueExists<T1, T2>(this Func<T1, T2, bool> me, T1 argument1, T2 argument2)
		{
			bool result = false;
			if (me.NotNull())
			{
				Delegate[] functions = me.GetInvocationList();
				for (int i = 0; !result && i < functions.Length; i++)
					result |= (functions[i] as Func<T1, T2, bool>).Invoke(argument1, argument2);
			}
			return result;
		}
		public static bool TrueExists<T1, T2, T3>(this Func<T1, T2, T3, bool> me, T1 argument1, T2 argument2, T3 argument3)
		{
			bool result = false;
			if (me.NotNull())
			{
				Delegate[] functions = me.GetInvocationList();
				for (int i = 0; !result && i < functions.Length; i++)
					result |= (functions[i] as Func<T1, T2, T3, bool>).Invoke(argument1, argument2, argument3);
			}
			return result;
		}
		public static bool TrueExists<T1, T2, T3, T4>(this Func<T1, T2, T3, T4, bool> me, T1 argument1, T2 argument2, T3 argument3, T4 argument4)
		{
			bool result = false;
			if (me.NotNull())
			{
				Delegate[] functions = me.GetInvocationList();
				for (int i = 0; !result && i < functions.Length; i++)
					result |= (functions[i] as Func<T1, T2, T3, T4, bool>).Invoke(argument1, argument2, argument3, argument4);
			}
			return result;
		}
		public static bool TrueExists<T1, T2, T3, T4, T5>(this Func<T1, T2, T3, T4, T5, bool> me, T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5)
		{
			bool result = false;
			if (me.NotNull())
			{
				Delegate[] functions = me.GetInvocationList();
				for (int i = 0; !result && i < functions.Length; i++)
					result |= (functions[i] as Func<T1, T2, T3, T4, T5, bool>).Invoke(argument1, argument2, argument3, argument4, argument5);
			}
			return result;
		}
		public static bool TrueExists<T1, T2, T3, T4, T5, T6>(this Func<T1, T2, T3, T4, T5, T6, bool> me, T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6)
		{
			bool result = false;
			if (me.NotNull())
			{
				Delegate[] functions = me.GetInvocationList();
				for (int i = 0; !result && i < functions.Length; i++)
					result |= (functions[i] as Func<T1, T2, T3, T4, T5, T6, bool>).Invoke(argument1, argument2, argument3, argument4, argument5, argument6);
			}
			return result;
		}
		public static bool TrueExists<T1, T2, T3, T4, T5, T6, T7>(this Func<T1, T2, T3, T4, T5, T6, T7, bool> me, T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6, T7 argument7)
		{
			bool result = false;
			if (me.NotNull())
			{
				Delegate[] functions = me.GetInvocationList();
				for (int i = 0; !result && i < functions.Length; i++)
					result |= (functions[i] as Func<T1, T2, T3, T4, T5, T6, T7, bool>).Invoke(argument1, argument2, argument3, argument4, argument5, argument6, argument7);
			}
			return result;
		}
		public static bool TrueExists<T1, T2, T3, T4, T5, T6, T7, T8>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, bool> me, T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6, T7 argument7, T8 argument8)
		{
			bool result = false;
			if (me.NotNull())
			{
				Delegate[] functions = me.GetInvocationList();
				for (int i = 0; !result && i < functions.Length; i++)
					result |= (functions[i] as Func<T1, T2, T3, T4, T5, T6, T7, T8, bool>).Invoke(argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8);
			}
			return result;
		}
		public static bool TrueExists<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, bool> me, T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6, T7 argument7, T8 argument8, T9 argument9)
		{
			bool result = false;
			if (me.NotNull())
			{
				Delegate[] functions = me.GetInvocationList();
				for (int i = 0; !result && i < functions.Length; i++)
					result |= (functions[i] as Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>).Invoke(argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9);
			}
			return result;
		}
		public static bool TrueExists<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, bool> me, T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6, T7 argument7, T8 argument8, T9 argument9, T10 argument10)
		{
			bool result = false;
			if (me.NotNull())
			{
				Delegate[] functions = me.GetInvocationList();
				for (int i = 0; !result && i < functions.Length; i++)
					result |= (functions[i] as Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, bool>).Invoke(argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10);
			}
			return result;
		}
		public static bool TrueExists<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, bool> me, T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6, T7 argument7, T8 argument8, T9 argument9, T10 argument10, T11 argument11)
		{
			bool result = false;
			if (me.NotNull())
			{
				Delegate[] functions = me.GetInvocationList();
				for (int i = 0; !result && i < functions.Length; i++)
					result |= (functions[i] as Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, bool>).Invoke(argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10, argument11);
			}
			return result;
		}
		public static bool TrueExists<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, bool> me, T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6, T7 argument7, T8 argument8, T9 argument9, T10 argument10, T11 argument11, T12 argument12)
		{
			bool result = false;
			if (me.NotNull())
			{
				Delegate[] functions = me.GetInvocationList();
				for (int i = 0; !result && i < functions.Length; i++)
					result |= (functions[i] as Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, bool>).Invoke(argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10, argument11, argument12);
			}
			return result;
		}
		public static bool TrueExists<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, bool> me, T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6, T7 argument7, T8 argument8, T9 argument9, T10 argument10, T11 argument11, T12 argument12, T13 argument13)
		{
			bool result = false;
			if (me.NotNull())
			{
				Delegate[] functions = me.GetInvocationList();
				for (int i = 0; !result && i < functions.Length; i++)
					result |= (functions[i] as Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, bool>).Invoke(argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10, argument11, argument12, argument13);
			}
			return result;
		}
		public static bool TrueExists<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, bool> me, T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6, T7 argument7, T8 argument8, T9 argument9, T10 argument10, T11 argument11, T12 argument12, T13 argument13, T14 argument14)
		{
			bool result = false;
			if (me.NotNull())
			{
				Delegate[] functions = me.GetInvocationList();
				for (int i = 0; !result && i < functions.Length; i++)
					result |= (functions[i] as Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, bool>).Invoke(argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10, argument11, argument12, argument13, argument14);
			}
			return result;
		}
		public static bool TrueExists<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, bool> me, T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6, T7 argument7, T8 argument8, T9 argument9, T10 argument10, T11 argument11, T12 argument12, T13 argument13, T14 argument14, T15 argument15)
		{
			bool result = false;
			if (me.NotNull())
			{
				Delegate[] functions = me.GetInvocationList();
				for (int i = 0; !result && i < functions.Length; i++)
					result |= (functions[i] as Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, bool>).Invoke(argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10, argument11, argument12, argument13, argument14, argument15);
			}
			return result;
		}
		public static bool TrueExists<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, bool> me, T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6, T7 argument7, T8 argument8, T9 argument9, T10 argument10, T11 argument11, T12 argument12, T13 argument13, T14 argument14, T15 argument15, T16 argument16)
		{
			bool result = false;
			if (me.NotNull())
			{
				Delegate[] functions = me.GetInvocationList();
				for (int i = 0; !result && i < functions.Length; i++)
					result |= (functions[i] as Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, bool>).Invoke(argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10, argument11, argument12, argument13, argument14, argument15, argument16);
			}
			return result;
		}
		#endregion
	}
}
