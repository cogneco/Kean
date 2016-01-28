// 
//  Formatting.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2011-2012 Simon Mika
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
using Text = System.Text;
using Collection = Kean.Collection;
using Kean;
using Kean.Extension;

namespace Kean.Xml.Dom.Writer
{
	public class Formatting : 
		IWriter
	{
		IO.Text.Indenter writer;
		public bool Indent
		{ 
			get { return this.writer.Format; }
			set { this.writer.Format = value; } 
		}
		public Formatting(IO.ICharacterWriter backend)
		{
			this.writer = new IO.Text.Indenter(backend);
			this.Indent = true;
		}
		#region IWriter Members
		public bool Write(Document document)
		{
			return this.writer.WriteLine("<?xml version=\"{0}\" encoding=\"{1}\" ?>", document.Version.ToString("#.0", System.Globalization.CultureInfo.InvariantCulture), document.Encoding) && this.Write(document.Root);
		}
		public bool Write(Node node)
		{
			return node is Element ? this.Write(node as Element) :
				node is Text ? this.Write(node as Text) :
				node is Data ? this.Write(node as Data) :
				node is Comment ? this.Write(node as Comment) :
				node is ProcessingInstruction ? this.Write(node as ProcessingInstruction) :
				false;
		}
		public bool Write(Attribute attribute)
		{
			return this.writer.Write(" {0}=\"{1}\"", attribute.Name, attribute.Value);
		}
		public bool Write(Comment comment)
		{
			return this.writer.Write("<!--") && this.writer.Write(comment.Value) && this.writer.Write("-->");
		}
		public bool Write(Data data)
		{
			return this.writer.Write("<![CDATA[") && this.writer.Write(data.Value) && this.writer.Write("]]>");
		}
		public bool Write(Element element)
		{
			return element.IsNull() || (this.writer.Write("<") && this.writer.Write(element.Name) && 
				element.Attributes.Fold((attribute, result) => result && this.Write(attribute), true) &&
				element.Count > 0 ?
				this.writer.Write(">") && (!element.Exists(n => n is Element) || this.writer.WriteLine()) && this.writer.AddIndent() && element.Fold((child, result) => result && this.Write(child), true) && this.writer.RemoveIndent() && this.writer.WriteLine("</{0}>", element.Name) : 
				this.writer.WriteLine(" />"));
		}
		public bool Write(Fragment fragment)
		{
			return fragment.IsNull() || fragment.Count == 0 || fragment.Fold((child, result) => result && this.Write(child), true);
		}
		public bool Write(ProcessingInstruction processingInstruction)
		{
			return this.writer.Write("<?") && this.writer.Write(processingInstruction.Target) && this.writer.Write(" ") && this.writer.AddIndent() &&
				this.writer.Write(processingInstruction.Value) && this.writer.RemoveIndent() && this.writer.WriteLine(" ?>");
		}
		public bool Write(Text text)
		{
			return this.writer.Write(text.Value);
		}
		#endregion
	}
}
