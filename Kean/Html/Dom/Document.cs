// 
//  Document.cs
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

using System;
using Kean;
using Kean.Extension;
using Collection = Kean.Collection;
using Kean.Collection.Extension;
using Uri = Kean.Uri;

namespace Kean.Html.Dom
{
	public class Document
	{
		public Head Head { get; set; }
		public Body Body { get; set; }
		public Document() :
			this("")
		{
		}
		public Document(string title) :
			this(new Head(title), new Body())
		{
		}
		public Document(Head head, Body body)
		{
			this.Head = head;
			this.Body = body;
		}
		public bool Save(Uri.Locator resource)
		{
			using (IO.ICharacterWriter writer = IO.CharacterWriter.Create(resource))
				return this.Save(writer);
		}
		public bool Save(IO.ICharacterWriter writer)
		{
			return writer.NotNull() && writer.Write(this.ToString());
		}
		public override string ToString()
		{
			return "<!DOCTYPE html> \n <html> \n" + this.Head.Format(1) + this.Body.Format(1) + "</html> \n";
		}
		public static implicit operator string(Document document)
		{
			return document.NotNull() ? document.ToString() : null;
		}
	}
}
