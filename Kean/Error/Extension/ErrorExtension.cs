//
//  ErrorExtension
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2013 Simon Mika
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

namespace Kean.Error.Extension
{
	public static class ErrorExtension
	{
		public static string AsCsv(this IError me)
		{
			return me.IsNull() ? "" : string.Format("{0},{1},\"{2}\",\"{3}\",\"{4}\",{5},{6},{7},{8},{9},{10}",
						me.Time,
						me.Level,
						me.Title.Replace("\"", "\"\""),
						me.Message.Replace("\"", "\"\""),
						me.AssemblyName,
						me.AssemblyVersion,
						me.Type,
						me.Method,
						me.Filename,
						me.Line,
						me.Column);
		}
	}
}
