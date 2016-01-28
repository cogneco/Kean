//
//  Storage.cs
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
using Kean;
using Kean.Extension;
using Collection = Kean.Collection;
using Kean.Collection.Extension;
using IO = Kean.IO;
using Uri = Kean.Uri;

namespace Kean.Xml.Serialize
{
	public class Storage : 
		Kean.Serialize.Storage
	{
		public override string ContentType { get { return "application/xml"; } }
		Preprocessor preprocessor;
		public Storage() :
			this(null, null)
		{
		}
		public Storage(Kean.Serialize.IRebuilder rebuilder, params Kean.Serialize.ISerializer[] serializers) :
			base(null, rebuilder ?? new Kean.Serialize.Rebuilder.Default(), serializers)
		{
			this.preprocessor = new Serialize.Preprocessor();
		}
		protected override Kean.Serialize.Data.Node LoadImplementation(IO.IByteInDevice device)
		{
			Dom.Document document = Dom.Document.Open(device);
			return document.NotNull() && document.Root.NotNull() ? this.Convert(this.preprocessor.Process(document).Root) : null;
		}
		string GetTextContent(Dom.Element element)
		{
			return element.Fold((n, s) => s + (n is Dom.Text ? (n as Dom.Text).Value : n is Dom.Data ? (n as Dom.Data).Value : ""), "");
		}
		Kean.Serialize.Data.Node Convert(Dom.Element element)
		{
			Kean.Serialize.Data.Node result = null;
			if (element.NotNull())
			{
				Dom.Attribute type = element.Attributes.Find(a => a.Name == "type");
				if (type.NotNull() && type.Value == "link")
					result = new Kean.Serialize.Data.Link(this.GetTextContent(element)) { Name = element.Name, Region = element.Region };
				else if (element.All(n => !(n is Dom.Element)))
					result = new Kean.Serialize.Data.String(this.GetTextContent(element)) { Name = element.Name, Region = element.Region };
				else
				{
					result = new Kean.Serialize.Data.Branch() { Name = element.Name, Region = element.Region };
					foreach (Dom.Node node in element)
						if (node is Dom.Element)
							(result as Kean.Serialize.Data.Branch).Nodes.Add(this.Convert(node as Dom.Element));
				}
				if (type.NotNull())
					result.Type = type.Value;
			}
			return result;
		}
		protected override bool StoreImplementation(Kean.Serialize.Data.Node value, IO.IByteOutDevice device)
		{
			return new Dom.Document() { Root = this.Convert(value) }.Save(device);
		}
		Dom.Element Convert(Kean.Serialize.Data.Node element)
		{
			Dom.Element result = null;
			if (element.NotNull())
			{
				result = new Dom.Element(element.Name ?? "node");
				if (element is Kean.Serialize.Data.Link)
				{
					result.Attributes.Add(new Kean.Xml.Dom.Attribute("type", "link"));
					result.Add(new Dom.Text((element as Kean.Serialize.Data.Link).Relative));
				}
				else
				{
					if (element.Type.NotNull())
						result.Attributes.Add(new Kean.Xml.Dom.Attribute("type", element.Type));
					if (element is Kean.Serialize.Data.Leaf)
						result.Add(new Dom.Text((element as Kean.Serialize.Data.Leaf).Text));
					else if (element is Kean.Serialize.Data.Branch)
						(element as Kean.Serialize.Data.Branch).Nodes.Apply(e => result.Add(this.Convert(e)));
				}
			}
			return result;
		}
		public static T Open<T>(Uri.Locator resource)
		{
			return new Storage().Load<T>(resource);
		}
		public static bool Save<T>(T value, Uri.Locator resource)
		{
			return new Storage().Store<T>(value, resource);
		}
		public static bool Save<T>(T value, Uri.Locator resource, string name)
		{
			return new Storage().Store<T>(value, resource, name);
		}
	}
}
