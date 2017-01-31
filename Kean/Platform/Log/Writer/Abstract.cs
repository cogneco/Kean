//
//  Abstract
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
using Kean;
using Kean.Extension;
using Error = Kean.Error;

namespace Kean.Platform.Log.Writer
{
	public abstract class Abstract :
		Synchronized,
		IDisposable
	{
		Func<Error.IError, bool> append;
		public bool Opened { get { return this.append.NotNull(); } }
		protected Abstract()
		{
		}
		~Abstract()
		{
			Error.Log.Wrap((Action)(this as IDisposable).Dispose)();
		}
		#region IDisposable Members
		void IDisposable.Dispose()
		{
			this.Close();
		}
		#endregion
		public bool Open()
		{
			return !this.Opened && (this.append = Abstract.Call<Func<Error.IError, bool>>(this.OpenHelper)).NotNull();
		}
		protected abstract Func<Error.IError, bool> OpenHelper();
		public bool Close()
		{
			lock (this.Lock)
			{
				bool result = this.Opened && Abstract.Call<bool>(this.CloseHelper);
				if (result)
					this.append = null;
				return result;
			}
		}
		protected abstract bool CloseHelper();
		public bool Append(Error.IError error)
		{
			return this.Opened ? Abstract.Call<bool>(() => this.append(error)) : this.Open() && Abstract.Call(() => this.append(error));
		}
		public bool Flush()
		{
			return Abstract.Call<bool>(this.FlushHelper);
		}
		protected abstract bool FlushHelper();
		static void Call(Action action)
		{
			if (Error.Log.CatchErrors)
				try
				{
					action();
				}
				catch
				{
				}
			else
				action();
		}
		static T Call<T>(Func<T> call)
		{
			T result;
			if (Error.Log.CatchErrors)
				try
				{
					result = call();
				}
				catch (System.Exception)
				{
					result = default(T);
				}
			else
				result = call();
			return result;
		}
	}
}
