//
//  Formatting.cs
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
using Kean;
using Kean.Extension;
using IO = Kean.IO;
using Collection = Kean.Collection;
using Kean.Collection.Extension;

namespace Kean.Json.Dom.Writer
{
	public class Formatting : 
		IWriter
	{
		IO.Text.Indenter writer;
		public bool Format
		{ 
			get { return this.writer.Format; }
			set { this.writer.Format = value; } 
		}
		public Formatting(IO.ICharacterWriter backend)
		{
			this.writer = new IO.Text.Indenter(backend);
			this.Format = true;
		}
		bool Write(Label label)
		{
			return this.writer.Write("\"" + label + "\": ");
		}
		#region IWriter implementation
		public bool Write(Item value)
		{
			return value is Object && this.Write(value as Object) ||
				value is Array && this.Write(value as Array) ||
				(value is Null || value.IsNull()) && this.Write(value as Null) ||
				value is Boolean && this.Write(value as Boolean) ||
				value is Number && this.Write(value as Number) ||
				value is String && this.Write(value as String);
		}
		public bool Write(Object value)
		{
			return this.writer.WriteLine("{ ") && this.writer.AddIndent() && value.All((element, last) => this.Write(element.Key) && this.Write(element.Value) && (last || this.writer.Write(",")) && this.writer.WriteLine(" ")) && this.writer.RemoveIndent() && this.writer.Write("}");
		}
		public bool Write(Array value)
		{
			return this.writer.WriteLine("[ ") && this.writer.AddIndent() && value.All((node, last) => this.Write(node) && (last || this.writer.Write(",")) && this.writer.WriteLine(" ")) && this.writer.RemoveIndent() && this.writer.Write("]");
		}
		public bool Write(Null value)
		{
			return this.writer.Write("null");
		}
		public bool Write(Boolean value)
		{
			return this.writer.Write(value.Value ? "true" : "false");
		}
		public bool Write(Number value)
		{
			return this.writer.Write(value.Value.AsString());
		}
		public bool Write(String value)
		{
			return this.writer.Write("\"" + value.Value + "\"");
		}
		#endregion
	}
}

