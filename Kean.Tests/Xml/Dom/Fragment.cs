//
//  Fragment.cs
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
using NUnit.Framework;
using Uri = Kean.Uri;

namespace Kean.Xml.Dom.Test
{
	[TestFixture]
	public class Fragment :
		Kean.Test.Fixture<Fragment>
	{
		Dom.Fragment Open(string name)
		{
			return Dom.Fragment.OpenResource("Xml/Dom/Data/" + name + ".xml");
		}
		protected Dom.Fragment Create(string name)
		{
			Dom.Fragment result = new Dom.Fragment();
			switch (name)
			{
				case "fragment001":
					Element book = new Element("book");
					book.Attributes.Add(new Attribute("language", "english"));
					Element title = new Element("title");
					title.Attributes.Add(new Attribute("location", "office"));
					title.Add(new Text("Write with XML"));
					book.Add(title);
					result.Add(book);
					Comment comment = new Comment("New Book");
					result.Add(comment);
					book = new Element("book");
					book.Add(new Text("Write with XML 2"));
					result.Add(book);
					break;
			}
			return result;
		}
		[Test]
		public void Open()
		{
			Expect(this.Open("fragment001"), Is.EqualTo(this.Create("fragment001")));
		}
		[Test]
		public void Save()
		{
			this.Create("fragment001").Save(Uri.Locator.FromPlatformPath(System.IO.Path.GetFullPath("fragment001.xml")));
		}
		protected override void Run()
		{
			this.Run(
				this.Save,
				this.Open
			);
		}
	}
}
