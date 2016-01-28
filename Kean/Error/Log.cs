//
//  Log.cs
//
//  Author:
//       Simon Mika <smika@hx.se>
//
//  Copyright (c) 2012 Simon Mika
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

namespace Kean.Error
{
	public static class Log
	{
		static bool catchErrors = true;
		public static bool CatchErrors
		{ 
			get { return Log.catchErrors; }
			set
			{
				if (Log.catchErrors != value)
					Log.CatchErrorsChanged.Call(Log.catchErrors = value);
			}
		}
		public static event Action<bool> CatchErrorsChanged;
		public static event Action<IError> OnAppend;
		static Log()
		{
			Log.catchErrors = !System.Environment.GetCommandLineArgs().Contains("-d", "--debug");
		}
		public static void Append(IError entry)
		{
			Action<IError> onAppend = Log.OnAppend;
			if (onAppend.NotNull())
			{
				if (Log.CatchErrors)
				{
					foreach (Action<IError> action in onAppend.GetInvocationList().Map(s => s as Action<IError>))
						if (action.NotNull())
							try
							{
								action(entry);
							}
							catch (System.Exception)
							{
							}
				}
				else
					onAppend(entry);
			}
		}
		public static void Free()
		{
			Log.CatchErrorsChanged = null;
			Log.OnAppend = null;
		}
		public static void Append(Level level, string title, System.Exception exception)
		{
			Log.Append(Entry.Create(level, title, exception));
		}
		public static void Append(Level level, string title, string message)
		{
			Log.Append(Entry.Create(level, title, message));
		}
		public static void Append(Level level, string title, string message, params object[] arguments)
		{
			Log.Append(Entry.Create(level, title, message.Format(arguments)));
		}

		#region Wrap
		#region Func<bool>
		public static Func<bool> Wrap(Func<bool> task)
		{
			return Log.Wrap("Task failed", task);
		}
		public static Func<bool> Wrap(string title, Func<bool> task)
		{
			Func<bool> result = task;
			if (Log.CatchErrors)
				result = () =>
				{
					bool r = false;
					try
					{ 
						if (!(r = task()))
							Log.Append(Error.Level.Notification, title, "Wrapped task returned false."); 
					}
					catch (System.Threading.ThreadInterruptedException)
					{
						throw;
					}
					catch (Exception e)
					{
						Log.Append(e);
					}
					catch (System.Exception e)
					{
						Log.Append(Error.Level.Recoverable, title, e);
					}
					return r;
				};
			return result;
		}
		#endregion

		#region Action
		public static Action Wrap(Action task)
		{
			return Log.Wrap("Task failed", task);
		}
		public static Action<T> Wrap<T>(Action<T> task)
		{
			return Log.Wrap("Task failed", task);
		}
		public static Action<T1, T2> Wrap<T1, T2>(Action<T1, T2> task)
		{
			return Log.Wrap("Task failed", task);
		}
		#endregion

		#region Action with title
		public static Action Wrap(string title, Action task)
		{
			return Log.Wrap<System.Exception>(title, task, null);
		}
		public static Action<T> Wrap<T>(string title, Action<T> task)
		{
			return Log.Wrap<System.Exception, T>(title, task, null);
		}
		public static Action<T1, T2> Wrap<T1, T2>(string title, Action<T1, T2> task)
		{
			return Log.Wrap<System.Exception, T1, T2>(title, task, null);
		}
		#endregion

		#region Action with title and onCatch
		public static Action Wrap<E>(string title, Action task, Action<E> onCatch) where E : System.Exception
		{
			Action result;
			if (Log.CatchErrors)
				result = () =>
				{
					try
					{
						task.Call();
					}
					catch (System.Threading.ThreadInterruptedException)
					{
						throw;
					}
					catch (E e)
					{
						onCatch.Call(e);
						if (e is Exception)
							Log.Append(e as Exception);
						else
							Log.Append(Level.Recoverable, title, e);
					}
				};
			else
				result = task.Call;
			return result;
		}
		public static Action<T> Wrap<E, T>(string title, Action<T> task, Action<E> onCatch) where E : System.Exception
		{
			Action<T> result;
			if (Log.CatchErrors)
				result = argument =>
				{
					try
					{
						task.Call(argument);
					}
					catch (System.Threading.ThreadInterruptedException)
					{
						throw;
					}
					catch (E e)
					{
						onCatch.Call(e);
						if (e is Exception)
							Log.Append(e as Exception);
						else
							Log.Append(Level.Recoverable, title, e);
					}
				};
			else
				result = task.Call;
			return result;
		}
		public static Action<T1, T2> Wrap<E, T1, T2>(string title, Action<T1, T2> task, Action<E> onCatch) where E : System.Exception
		{
			Action<T1, T2> result;
			if (Log.CatchErrors)
				result = (argument1, argument2) =>
				{
					try
					{
						task.Call(argument1, argument2);
					}
					catch (System.Threading.ThreadInterruptedException)
					{
						throw;
					}
					catch (E e)
					{
						onCatch.Call(e);
						if (e is Exception)
							Log.Append(e as Exception);
						else
							Log.Append(Level.Recoverable, title, e);
					}
				};
			else
				result = task.Call;
			return result;
		}
		#endregion

		#endregion

		#region Call
		#region Action
		public static bool Call(Action task)
		{
			return Log.Call("Call Failed", task);
		}
		public static bool Call<T>(Action<T> task, T argument)
		{
			return Log.Call("Call Failed", task, argument);
		}
		public static bool Call<T1, T2>(Action<T1, T2> task, T1 argument1, T2 argument2)
		{
			return Log.Call("Call Failed", task, argument1, argument2);
		}
		#endregion

		#region Action with title
		public static bool Call(string title, Action task)
		{
			bool result = true;
			Log.Call<System.Exception>(title, task, (e) => result = false);
			return result;
		}
		public static bool Call<T>(string title, Action<T> task, T argument)
		{
			bool result = true;
			Log.Call<System.Exception, T>(title, task, argument, (e) => result = false);
			return result;
		}
		public static bool Call<T1, T2>(string title, Action<T1, T2> task, T1 argument1, T2 argument2)
		{
			bool result = true;
			Log.Call<System.Exception, T1, T2>(title, task, argument1, argument2, (e) => result = false);
			return result;
		}
		#endregion

		#region Action with onCatch
		public static void Call<E>(Action task, Action<E> onCatch) where E : System.Exception
		{
			Log.Call<E>("Call Failed", task, onCatch);
		}
		public static void Call<E, T>(Action<T> task, T argument, Action<E> onCatch) where E : System.Exception
		{
			Log.Call<E, T>("Call Failed", task, argument, onCatch);
		}
		public static void Call<E, T1, T2>(Action<T1, T2> task, T1 argument1, T2 argument2, Action<E> onCatch) where E : System.Exception
		{
			Log.Call<E, T1, T2>("Call Failed", task, argument1, argument2, onCatch);
		}
		#endregion

		#region Action with title and onCatch
		public static void Call<E>(string title, Action task, Action<E> onCatch) where E : System.Exception
		{
			Log.Wrap<E>(title, task, onCatch).Call();
		}
		public static void Call<E, T>(string title, Action<T> task, T argument, Action<E> onCatch) where E : System.Exception
		{
			Log.Wrap<E, T>(title, task, onCatch).Call(argument);
		}
		public static void Call<E, T1, T2>(string title, Action<T1, T2> task, T1 argument1, T2 argument2, Action<E> onCatch) where E : System.Exception
		{
			Log.Wrap<E, T1, T2>(title, task, onCatch).Call(argument1, argument2);
		}
		#endregion

		#endregion

	}
}
