// 
//  Fragment.cs
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
using Uri = Kean.Uri;

namespace Kean.Xml.Dom
{
	public class Fragment :
		Collection.IList<Node>,
		IEquatable<Fragment>
	{
		Collection.IList<Node> childNodes = new Collection.Linked.List<Node>();
		public Fragment()
		{ }
		public Fragment(params Node[] nodes)
		{ 
		}
		public bool Save(Uri.Locator resource)
		{
			using (IO.ICharacterWriter writer = IO.CharacterWriter.Create(resource))
				return this.Save(writer);
		}
		public bool Save(IO.ICharacterWriter writer)
		{
			return writer.NotNull() && this.Save(new Writer.Formatting(writer));
		}
		public bool Save(IWriter writer)
		{
			return writer.Write(this);
		}
		#region IList<Node> Members
		public Collection.IList<Node> Add(Node item)
		{
			return this.childNodes.Add(item);
		}
		public Node Remove()
		{
			return this.childNodes.Remove();
		}
		public Collection.IList<Node> Insert(int index, Node item)
		{
			return this.childNodes.Insert(index, item);
		}
		public Node Remove(int index)
		{
			return this.childNodes.Remove(index);
		}
		#endregion
		#region IVector<Node> Members
		public int Count { get { return this.childNodes.Count; } }
		public Node this[int index]
		{
			get { return this.childNodes[index]; }
			set { this.childNodes[index] = value; }
		}
		#endregion
		#region IEnumerable<Node> Members
		public System.Collections.Generic.IEnumerator<Node> GetEnumerator() { return this.childNodes.GetEnumerator(); }
		#endregion
		#region IEnumerable Members
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { return (this.childNodes as System.Collections.IEnumerable).GetEnumerator(); }
		#endregion
		#region IEquatable<IVector<Node>> Members
		public bool Equals(Kean.Collection.IVector<Node> other) { return this.childNodes.Equals(other); }
		#endregion
		#region Object Overrides
		public override bool Equals(object other)
		{
			return this.Equals(other as Fragment);
		}
		public override int GetHashCode()
		{
			return this.childNodes.GetHashCode();
		}
		public override string ToString()
		{
			IO.Text.Writer result = new IO.Text.Writer();
			new Writer.Formatting(result).Write(this);
			return result;
		}
		#endregion
		#region IEquatable<Fragment> Members
		public bool Equals(Fragment other)
		{
			return other.NotNull() &&
				this.childNodes.Equals(other.childNodes);
		}
		#endregion
		#region Operators
		public static bool operator ==(Fragment left, Fragment right)
		{
			return left.Same(right) || left.NotNull() && left.Equals(right);
		}
		public static bool operator !=(Fragment left, Fragment right)
		{
			return !(left == right);
		}
		public static explicit operator string(Fragment fragment)
		{
			return fragment.ToString();
		}
		public static explicit operator Fragment(string data)
		{
			return Fragment.Open(Sax.Parser.Open(new IO.Text.Reader(data)));
		}
		#endregion
		#region Static Open
		public static Fragment Open(Sax.Parser parser)
		{
			Fragment result = new Fragment();
			Collection.IList<Node> current = result;
			parser.OnElementStart += (name, attributes, region) =>
			{
				Element next = new Element(name, attributes) { Region = region };
				current.Add(next);
				current = next;
			};
			parser.OnElementEnd += (name, region) =>
			{
				if (!(current is Element))
					throw new Exception.StartTagMissing(name, region);
				else if ((current as Element).Name != name)
					throw new Exception.EndTagUnmatched((current as Element).Name, (current as Element).Region, name, region);
				if ((current as Element).Region.NotNull() && region.NotNull())
					(current as Element).Region = new Uri.Region((current as Element).Region.Resource, (current as Element).Region.Start, region.End);
				current = (current as Element).Parent as Collection.IList<Node> ?? result;
			};
			parser.OnText += (value, region) => { current.Add(new Text(value) { Region = region }); };
			parser.OnData += (value, region) => { current.Add(new Data(value) { Region = region }); };
			parser.OnComment += (value, region) => { current.Add(new Comment(value) { Region = region }); };
			parser.OnProccessingInstruction += (target, value, region) => { current.Add(new ProcessingInstruction(target, value) { Region = region }); };

			return parser.Parse() ? result : null;
		}
		public static Fragment OpenResource(System.Reflection.Assembly assembly, Uri.Path resource)
		{
			return Fragment.Open(Sax.Parser.Open(assembly, resource));
		}
		public static Fragment OpenResource(Uri.Path resource)
		{
			return Fragment.OpenResource(System.Reflection.Assembly.GetCallingAssembly(), resource);
		}
		public static Fragment Open(string filename)
		{
			Fragment result;
			try
			{
				using (System.IO.Stream stream = new System.IO.StreamReader(filename).BaseStream)
					result = Fragment.Open(stream);
			}
			catch (System.IO.IOException)
			{
				result = null;
			}
			return result;
		}
		public static Fragment Open(System.IO.Stream stream)
		{
			return Fragment.Open(Sax.Parser.Open(stream));
		}
		public static Fragment Open(Uri.Locator resource)
		{
			return Fragment.Open(Sax.Parser.Open(resource));
		}
		#endregion
	}
}
