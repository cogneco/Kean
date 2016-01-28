//
//  Exception.cs
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

namespace Kean.Error
{
	public abstract class Exception :
		System.ApplicationException,
		IError
	{
		static Level threshold = Level.Warning;
		public static Level Threshold
		{
			get { return Exception.threshold; }
			set
			{
				if (value < Level.Recoverable)
					Exception.threshold = value;
			}
		}
		public DateTime Time { get; private set; }
		public Level Level { get; private set; }
		public string Title { get; private set; }
		public string AssemblyName { get; private set; }
		public string AssemblyVersion { get; private set; }
		public string Type { get; private set; }
		public string Method { get; private set; }
		public string Filename { get; private set; }
		public int Line { get; private set; }
		public int Column { get; private set; }
		protected Exception(Level level, string title, string message, params object[] arguments) : this(null, level, title, message, arguments)
		{
		}
		protected Exception(System.Exception exception, Level level, string title, string message, params object[] arguments) : 
			base(System.String.Format(message, arguments), exception)
		{
			this.Time = DateTime.Now;
			this.Level = level;
			this.Title = title;
			System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace(0, true);
			int depth = 0;
			while (trace.GetFrame(depth).GetMethod().DeclaringType.IsInstanceOfType(this))
				depth++;
			System.Diagnostics.StackFrame frame = trace.GetFrame(depth);
			System.Reflection.MethodBase method = frame.GetMethod();
			Type type = method.DeclaringType;
			System.Reflection.AssemblyName assembly = type.Assembly.GetName();
			this.AssemblyName = assembly.Name;
			this.AssemblyVersion = assembly.Version.ToString();
			this.Type = type.FullName;
			this.Method = method.Name;
			this.Filename = frame.GetFileName();
			this.Line = frame.GetFileLineNumber();
			this.Column = frame.GetFileColumnNumber();
			Log.Append(this);
		}
		public void Throw()
		{
			if (this.Level >= Exception.Threshold)
				throw this;
		}
	}
}
