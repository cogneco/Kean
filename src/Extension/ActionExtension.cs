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
	public static class ActionExtension
	{
		#region Call
		public static void Call (this Action me)
		{
			if (me.NotNull ())
				me.Invoke ();
		}
		public static void Call<T> (this Action<T> me, T argument)
		{
			if (me.NotNull ())
				me.Invoke (argument);
		}
		public static void Call<T1, T2> (this Action<T1, T2> me, T1 argument1, T2 argument2)
		{
			if (me.NotNull ())
				me.Invoke (argument1, argument2);
		}
		public static void Call<T1, T2, T3> (this Action<T1, T2, T3> me, T1 argument1, T2 argument2, T3 argument3)
		{
			if (me.NotNull ())
				me.Invoke (argument1, argument2, argument3);
		}
		public static void Call<T1, T2, T3, T4> (this Action<T1, T2, T3, T4> me, T1 argument1, T2 argument2, T3 argument3, T4 argument4)
		{
			if (me.NotNull ())
				me.Invoke (argument1, argument2, argument3, argument4);
		}
		public static void Call<T1, T2, T3, T4, T5> (this Action<T1, T2, T3, T4, T5> me, T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5)
		{
			if (me.NotNull ())
				me.Invoke (argument1, argument2, argument3, argument4, argument5);
		}
		public static void Call<T1, T2, T3, T4, T5, T6> (this Action<T1, T2, T3, T4, T5, T6> me, T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6)
		{
			if (me.NotNull ())
				me.Invoke (argument1, argument2, argument3, argument4, argument5, argument6);
		}
		public static void Call<T1, T2, T3, T4, T5, T6, T7> (this Action<T1, T2, T3, T4, T5, T6, T7> me, T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6, T7 argument7)
		{
			if (me.NotNull ())
				me.Invoke (argument1, argument2, argument3, argument4, argument5, argument6, argument7);
		}
		public static void Call<T1, T2, T3, T4, T5, T6, T7, T8> (this Action<T1, T2, T3, T4, T5, T6, T7, T8> me, T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6, T7 argument7, T8 argument8)
		{
			if (me.NotNull ())
				me.Invoke (argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8);
		}
		public static void Call<T1, T2, T3, T4, T5, T6, T7, T8, T9> (this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> me, T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6, T7 argument7, T8 argument8, T9 argument9)
		{
			if (me.NotNull ())
				me.Invoke (argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9);
		}
		public static void Call<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> (this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> me, T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6, T7 argument7, T8 argument8, T9 argument9,
		T10 argument10)
		{
			if (me.NotNull ())
				me.Invoke (argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10);
		}
		public static void Call<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> (this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10,
		T11> me, T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6, T7 argument7, T8 argument8, T9 argument9,
		T10 argument10, T11 argument11)
		{
			if (me.NotNull ())
				me.Invoke (argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10,
				argument11);
		}
		public static void Call<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> (this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10,
		T11, T12> me, T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6, T7 argument7, T8 argument8, T9 argument9,
		T10 argument10, T11 argument11, T12 argument12)
		{
			if (me.NotNull ())
				me.Invoke (argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10,
				argument11, argument12);
		}
		public static void Call<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> (this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10,
		T11, T12, T13> me, T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6, T7 argument7, T8 argument8, T9 argument9,
		T10 argument10, T11 argument11, T12 argument12, T13 argument13)
		{
			if (me.NotNull ())
				me.Invoke (argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10,
				argument11, argument12, argument13);
		}
		public static void Call<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> (this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10,
		T11, T12, T13, T14> me, T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6, T7 argument7, T8 argument8, T9 argument9,
		T10 argument10, T11 argument11, T12 argument12, T13 argument13, T14 argument14)
		{
			if (me.NotNull ())
				me.Invoke (argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10,
				argument11, argument12, argument13, argument14);
		}
		public static void Call<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> (this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10,
		T11, T12, T13, T14, T15> me, T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6, T7 argument7, T8 argument8, T9 argument9,
		T10 argument10, T11 argument11, T12 argument12, T13 argument13, T14 argument14, T15 argument15)
		{
			if (me.NotNull ())
				me.Invoke (argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10,
				argument11, argument12, argument13, argument14, argument15);
		}
		public static void Call<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> (this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10,
		T11, T12, T13, T14, T15, T16> me, T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6, T7 argument7, T8 argument8, T9 argument9,
		T10 argument10, T11 argument11, T12 argument12, T13 argument13, T14 argument14, T15 argument15, T16 argument16)
		{
			if (me.NotNull ())
				me.Invoke (argument1, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10,
				argument11, argument12, argument13, argument14, argument15, argument16);
		}
		#endregion
		#region For
		public static void For(this Action<int> me, int count)
		{
			if (me.NotNull())
			{
				if (count > 1)
				{
					int countMinusOne = count - 1;
					IAsyncResult[] tags = new IAsyncResult[countMinusOne];
					for (int i = 0; i < countMinusOne; i++)
						tags[i] = me.BeginInvoke(i, null, null);
					me(countMinusOne);
					for (int i = 0; i < countMinusOne; i++)
						me.EndInvoke(tags[i]);
				}
				else if (count == 1)
					me(0);
			}
		}
		#endregion
	}
}
