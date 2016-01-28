//
//  Entry
//
//  Author:
//       Simon Mika <smika@hx.se>
//
//  Copyright (c) 2010-2012 Simon Mika
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
using Error = Kean.Error;

namespace Kean.Error
{
	public class Entry :
		Error.IError
	{
		Entry()
		{
		}
		#region IError Members
		public DateTime Time { get; internal set; }
		public Error.Level Level { get; internal set; }
		public string Title { get; internal set; }
		public string Message { get; internal set; }
		public string AssemblyName { get; internal set; }
		public string AssemblyVersion { get; internal set; }
		public string Type { get; internal set; }
		public string Method { get; internal set; }
		public string Filename { get; internal set; }
		public int Line { get; internal set; }
		public int Column { get; internal set; }
		#endregion
		public override string ToString()
		{
			return Extension.ErrorExtension.AsCsv(this);
		}
		public static Error.IError Create(Error.Level level, string title, System.Exception exception)
		{
			System.Reflection.MethodBase method = exception.TargetSite ?? new System.Diagnostics.StackTrace().GetFrame(2).GetMethod();
			Type type = method.DeclaringType;
			System.Reflection.AssemblyName assembly = type.NotNull() ? type.Assembly.GetName() : null;
			return new Entry() {
				Time = DateTime.Now,
				Level = level,
				Title = title,
				Message = exception.Message,
				AssemblyName = assembly.NotNull() ? assembly.Name : "",
				AssemblyVersion = assembly.NotNull() ? assembly.Version.ToString() : "",
				Type = type.FullName,
				Method = method.Name,
			};
		}
		public static Error.IError Create(Error.Level level, string title, string message)
		{
			System.Diagnostics.StackFrame frame = new System.Diagnostics.StackTrace().GetFrame(2);
			System.Reflection.MethodBase method = frame.GetMethod();
			Type type = method.DeclaringType;
			System.Reflection.AssemblyName assembly = type.Assembly.GetName();
			return new Entry() {
				Time = DateTime.Now,
				Level = level,
				Title = title,
				Message = message,
				AssemblyName = assembly.Name,
				AssemblyVersion = assembly.Version.ToString(),
				Type = type.FullName,
				Method = method.Name,
				Filename = frame.GetFileName(),
				Line = frame.GetFileLineNumber(),
				Column = frame.GetFileColumnNumber(),
			};
		}
		public static Error.IError Parse(string error)
		{
			IError result = null;
			if (error.NotEmpty())
			{
				string[] splitted = error.SplitAt(',').ToArray();
				if (splitted.Length == 11)
					result = new Entry() {
						Time = splitted[0].Parse<DateTime>(),
						Level = splitted[1].Parse<Error.Level>(),
						Title = splitted[2],
						Message = splitted[3],
						AssemblyName = splitted[4],
						AssemblyVersion = splitted[5],
						Type = splitted[6],
						Method = splitted[7],
						Filename = splitted[8],
						Line = splitted[9].Parse<int>(),
						Column = splitted[10].Parse<int>(),
					};
			}
			return result;
		}
	}
}
