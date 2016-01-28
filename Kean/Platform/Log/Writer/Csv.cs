//
//  Csv
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
using Kean;
using Kean.Extension;
using Error = Kean.Error;

namespace Kean.Platform.Log.Writer
{
	public class Csv :
		Abstract
	{
		System.IO.TextWriter writer;
		public string Filename { get; set; }
		protected override Func<Error.IError, bool> OpenHelper()
		{
			if (this.writer.IsNull())
			{
				string filename = this.Filename;
				for (int i = 0; i < 10; i++)
				{
					try
					{
						this.writer = new System.IO.StreamWriter(filename);
					}
					catch (System.IO.IOException)
					{
						filename = System.IO.Path.GetFileNameWithoutExtension(this.Filename) + i + System.IO.Path.GetExtension(this.Filename);
					}
				}
				if (this.writer.NotNull())
					this.writer.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}", "Time", "Level", "Title", "Message", "Assembly", "Version", "Class", "Method", "Source", "Line", "Column");
			}
			return (Error.IError entry) =>
			{
				bool result;
				if (result = this.writer.NotNull())
					this.writer.WriteLine("{0},{1},\"{2}\",\"{3}\",\"{4}\",{5},{6},{7},{8},{9},{10}",
					entry.Time,
					entry.Level,
					entry.Title.NotEmpty() ? entry.Title.Replace("\"", "\"\"") : entry.Title,
					entry.Message.NotEmpty() ? entry.Message.Replace("\"", "\"\"") : entry.Message,
					entry.AssemblyName,
					entry.AssemblyVersion,
					entry.Type,
					entry.Method,
					entry.Filename,
					entry.Line,
					entry.Column);
				return result;
			};
		}
		protected override bool CloseHelper()
		{
			bool result;
			if (result = this.writer.NotNull())
			{
				this.writer.Close();
				this.writer = null;
			}
			return result;
		}
		protected override bool FlushHelper()
		{
			bool result;
			if (result = this.writer.NotNull())
				this.writer.Flush();
			return result;
		}
	}
}
