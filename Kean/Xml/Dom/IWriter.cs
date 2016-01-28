// 
//  IWriter.cs
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
using Collection = Kean.Collection;

namespace Kean.Xml.Dom
{
	public interface IWriter
	{
		bool Write(Element element);
		bool Write(Data data);
		bool Write(Text text);
		bool Write(ProcessingInstruction processingInstruction);
		bool Write(Node node);
		bool Write(Document document);
		bool Write(Fragment fragment);
		bool Write(Comment comment);
		bool Write(Attribute attribute);
	}
}
