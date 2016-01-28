// 
//  Object.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2011 Simon Mika
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
using Uri = Kean.Uri;

namespace Kean.Xml.Dom
{
	public abstract class Object
	{
		public Uri.Region Region { get; internal set; }

		Document document;
		public Document Document
		{
			get { return this.document; }
			internal set { this.ChangeDocument(value); }
		}
		Element parent;
		public Element Parent
		{
			get { return this.parent; }
			internal set { this.ChangeParent(value); }
		}
		protected Object()
		{
		}
		protected virtual void ChangeDocument(Document document)
		{
			this.document = document;
		}
		protected virtual void ChangeParent(Element parent)
		{
			this.parent = parent;
			this.document = parent.NotNull() ? parent.document : null;
		}
	}
}
