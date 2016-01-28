//
//  Factory.cs
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

namespace Kean.Xml.Dom.Test
{
	public abstract class Factory<T> :
		Kean.Test.Fixture<T>
		where T : Factory<T>, new()
	{
		protected abstract void Verify(string name);
		protected Document Open(string name)
		{
			return Document.OpenResource("Xml/Dom/Data/" + name + ".xml");
		}
		protected Document Create(string name)
		{
			Document result = null;
			switch (name)
			{
				case "valid001": 
					result = new Document() { Root = new Element("empty") };
					break;
				case "valid002":
					{
						result = new Document() { Root = new Element("text") };
						result.Root.Add(new Text("Text"));
					}
					break;
				case "valid003":
					{
						result = new Document() { Root = new Element("library") };
						Element book = new Element("book");
						Element title = new Element("title");
						title.Add(new Text("Write with XML"));
						book.Add(title);
						result.Root.Add(book);
					}
					break;
				case "valid004":
					{
						result = new Document() { Root = new Element("library") };
						Element book = new Element("book");
						book.Attributes.Add(new Attribute("language", "english"));
						Element title = new Element("title");
						title.Attributes.Add(new Attribute("location", "office"));
						title.Add(new Text("Write with XML"));
						book.Add(title);
						result.Root.Add(book);
					}
					break;
				case "valid005":
					{
						result = new Document() { Root = new Element("library") };
						Comment comment = new Comment("New Book");
						result.Root.Add(comment);
						Element book = new Element("book");
						book.Add(new Text("Write with XML"));
						result.Root.Add(book);
					}
					break;
				case "valid006":
					{
						result = new Document() { Root = new Element("data") };
						Data data = new Data("Example");
						result.Root.Add(data);

					} 
					break;
				case "valid007":
					{
						result = new Document() { Root = new Element("instruction") };
						ProcessingInstruction instruction = new ProcessingInstruction("target", "Try this!");
						result.Root.Add(instruction);
					}
					break;
				case "valid008":
					{
						result = new Document() { Root = new Element("empty") };
						result.Root.AddAttribute("key", "value");
					}
					break;
				case "valid009":
					{
						result = new Document() { Root = new Element("empty") };
						result.Root.AddAttribute("keyA", "valueA");
						result.Root.AddAttribute("keyB", "valueB");
					}
					break;
			}
			return result;
		}
	}
}
