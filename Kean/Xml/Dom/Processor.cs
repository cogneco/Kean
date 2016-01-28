// 
//  Processor.cs
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
using Collection = Kean.Collection;
using Kean.Collection.Extension;
using IO = Kean.IO;
using Uri = Kean.Uri;
using Generic = System.Collections.Generic;

namespace Kean.Xml.Dom
{
	public abstract class Processor
	{
		protected Processor()
		{
		}
		public Document Process(Dom.Document document)
		{
			Document result = null;
			if (document.NotNull() && document.Root.NotNull())
			{
				Generic.IEnumerator<Node> root = this.Process(document.Root);
				if (root.MoveNext() && root.Current is Element)
					result = new Document(root.Current as Element) { Encoding = document.Encoding, Standalone = document.Standalone, Version = document.Version };
			}
			return result;
		}
		protected virtual Generic.IEnumerator<Node> Process(Node node)
		{
			Generic.IEnumerator<Node> result;
			if (node is Element)
				result = this.Process(node as Element);
			else if (node is ProcessingInstruction)
				result = this.Process(node as ProcessingInstruction);
			else if (node is Text)
				result = this.Process(node as Text);
			else if (node is Data)
				result = this.Process(node as Data);
			else if (node is Comment)
				result = this.Process(node as Comment);
			else
				result = Enumerator.Empty<Node>();
			return result;
		}
		protected virtual Generic.IEnumerator<Node> Process(Generic.IEnumerator<Node> nodes)
		{
			while (nodes.MoveNext())
			{
				Generic.IEnumerator<Node> result = this.Process(nodes.Current);
				while (result.MoveNext())
					yield return result.Current;
			}
		}
		protected virtual Generic.IEnumerator<Node> Process(Element element)
		{
			Element result = new Element(element.Name, this.Process(element.Attributes.GetEnumerator())) { Region = element.Region };
			foreach (Node node in element)
				this.Process(node).Apply(n =>
				{
					if (n.NotNull())
						result.Add(n);
				});
			yield return result;
		}
		protected virtual Generic.IEnumerator<Node> Process(ProcessingInstruction instruction)
		{
			yield return instruction;
		}
		protected virtual Generic.IEnumerator<Node> Process(Text text)
		{
			yield return text;
		}
		protected virtual Generic.IEnumerator<Node> Process(Data data)
		{
			yield return data;
		}
		protected virtual Generic.IEnumerator<Node> Process(Comment comment)
		{
			yield return comment;
		}
		protected virtual Generic.IEnumerator<Attribute> Process(Generic.IEnumerator<Attribute> attributes)
		{
			while (attributes.MoveNext())
			{
				Generic.IEnumerator<Attribute> result = this.Process(attributes.Current);
				while (result.MoveNext())
					if (result.Current.NotNull())
						yield return result.Current;
			}
		}
		protected virtual Generic.IEnumerator<Attribute> Process(Attribute attribute)
		{
			yield return attribute;
		}
	}
}
