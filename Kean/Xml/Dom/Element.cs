// 
//  Element.cs
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
using Kean;
using Kean.Extension;
using Collection = Kean.Collection;
using Kean.Collection.Extension;
using Uri = Kean.Uri;
using Generic = System.Collections.Generic;

namespace Kean.Xml.Dom
{
	public class Element:
		Node,
		Collection.IList<Node>,
		IEquatable<Element>
	{
		readonly Collection.IList<Node> childNodes = new Collection.Linked.List<Node>();
		public string Name { get; set; }
		public Generic.IEnumerable<Node> this [params string[] path] { get { return this[(Generic.IEnumerable<string>)path]; } }
		public Generic.IEnumerable<Node> this [Generic.IEnumerable<string> path] { get { return this[path.GetEnumerator()]; } }
		public Generic.IEnumerable<Node> this [Generic.IEnumerator<string> path]
		{
			get
			{
				Generic.IEnumerable<Node> result;
				if (path.MoveNext())
				{
					var next = this.Find(n => n is Element && (n as Element).Name == path.Current) as Element;
					result = next.NotNull() ? next[path] : null;
				}
				else
					result = this;
				return result;
			}
		}
		public Collection.IList<Attribute> Attributes { get; private set; }
		#region Constructors
		public Element()
		{
			Collection.Hooked.List<Attribute> attributes = new Collection.Hooked.List<Attribute>();
			attributes.Added += (index, attribute) => attribute.Parent = this;
			attributes.Removed += (index, attribute) => attribute.Parent = null;
			attributes.Replaced += (index, oldAttribute, newAttribute) =>
			{
				oldAttribute.Parent = null;
				newAttribute.Parent = this;
			};
			this.Attributes = new Collection.Wrap.List<Attribute>(attributes);
		}
		public Element(string name) :
			this()
		{
			this.Name = name;
		}
		public Element(string name, string text) :
			this(name, new Xml.Dom.Text(text))
		{
		}
		internal Element(string name, System.Collections.Generic.IEnumerator<Attribute> attributes) :
			this(name)
		{
			attributes.Apply(attribute => this.Attributes.Add(new Attribute(attribute.Name, attribute.Value) { Region = attribute.Region }));
		}
		public Element(string name, System.Collections.Generic.IEnumerable<Tuple<string, string, Uri.Region>> attributes) :
			this(name)
		{
			foreach (Tuple<string, string, Uri.Region> attribute in attributes)
				this.Attributes.Add(new Attribute(attribute.Item1, attribute.Item2) { Region = attribute.Item3 });
		}
		public Element(string name, params KeyValue<string, string>[] attributes) :
			this(name, (System.Collections.Generic.IEnumerable<Tuple<string, string, Uri.Region>>)attributes.Map(a => Tuple.Create(a.Key, a.Value, (Uri.Region)null)))
		{
		}
		public Element(string name, KeyValue<string, string> attribute) :
			this(name, new KeyValue<string, string>[] { attribute })
		{
		}
		public Element(string name, System.Collections.Generic.IEnumerable<Node> nodes) :
			this(name)
		{
			foreach (Node node in nodes)
				this.Add(node);
		}
		public Element(string name, params Node[] nodes) :
			this(name, (System.Collections.Generic.IEnumerable<Node>)nodes)
		{
		}
		public Element(string name, Node node) :
			this(name)
		{
			this.Add(node);
		}
		public Element(string name, Node node, params KeyValue<string, string>[] attributes) :
			this(name, attributes)
		{
			this.Add(node);
		}
		public Element(string name, string text, params KeyValue<string, string>[] attributes) :
			this(name, new Xml.Dom.Text(text), attributes)
		{
		}
		public Element(string name, Node node, KeyValue<string, string> attribute) :
			this(name, node, new KeyValue<string, string>[] { attribute })
		{
		}
		#endregion
		protected override void ChangeDocument(Document document)
		{
			base.ChangeDocument(document);
			this.Attributes.Apply(attribute =>
			{
				attribute.Document = document;
			});
			this.Apply(node =>
			{
				node.Document = document;
			});
		}
		protected override void ChangeParent(Element parent)
		{
			base.ChangeParent(parent);
			this.Attributes.Apply(attribute =>
			{
				attribute.Parent = parent;
			});
			this.Apply(node =>
			{
				node.Parent = parent;
			});
		}
		public Element AddAttribute(string name, string value)
		{
			this.Attributes.Add(new Attribute(name, value));
			return this;
		}
		#region IList<Node> Members
		public Collection.IList<Node> Add(Node item)
		{
			if (item.NotNull())
			{
				item.Parent = this;
				this.childNodes.Add(item);
			}
			return this;
		}
		public Node Remove()
		{ 
			Node result = this.childNodes.Remove();
			result.Parent = null;
			return result;
		}
		public Collection.IList<Node> Insert(int index, Node item)
		{
			item.Parent = this;
			return this.childNodes.Insert(index, item); 
		}
		public Node Remove(int index)
		{
			Node result = this.childNodes.Remove(index);
			result.Parent = null;
			return result;
		}
		#endregion
		#region IVector<Node> Members
		public int Count { get { return this.childNodes.Count; } }
		public Node this [int index]
		{
			get { return this.childNodes[index]; }
			set
			{
				this.childNodes[index].Parent = null;
				this.childNodes[index] = value;
			}
		}
		#endregion
		#region IEnumerable<Node> Members
		public System.Collections.Generic.IEnumerator<Node> GetEnumerator()
		{
			return this.childNodes.GetEnumerator();
		}
		#endregion
		#region IEnumerable Members
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return (this.childNodes as System.Collections.IEnumerable).GetEnumerator();
		}
		#endregion
		#region IEquatable<IVector<Node>> Members
		public bool Equals(Kean.Collection.IVector<Node> other)
		{
			return this.childNodes.Equals(other);
		}
		#endregion
		#region Object Overrides
		public override bool Equals(object other)
		{
			return this.Equals(other as Element);
		}
		public override int GetHashCode()
		{
			return this.Name.Hash() ^
			this.Attributes.GetHashCode() ^
			this.childNodes.GetHashCode();
		}
		public override string ToString()
		{
			return base.ToString();
		}
		#endregion
		#region IEquatable<Element> Members
		public bool Equals(Element other)
		{
			return other.NotNull() &&
			this.Name == other.Name &&
			this.Attributes.Equals(other.Attributes) &&
			this.childNodes.Equals(other.childNodes);
		}
		#endregion
		#region Equality Operators
		public static bool operator ==(Element left, Element right)
		{
			return left.Same(right) || left.NotNull() && left.Equals(right);
		}
		public static bool operator !=(Element left, Element right)
		{
			return !(left == right);
		}
		#endregion
		#region Static Methods
		public static Element Open(Sax.Parser parser)
		{
			Element result = null;
			Element current = null;
			parser.OnElementStart += (name, attributes, region) =>
			{ 
				Element next = new Element(name, attributes) { Region = region };
				if (result.IsNull())
					result = next;
				else
					current.Add(next);
				current = next;
			};
			parser.OnElementEnd += (name, region) =>
			{
				if (current.Name != name)
					throw new Exception.EndTagUnmatched(current.Name, current.Region, name, region);
				if (current.Region.NotNull() && region.NotNull())
					current.Region = new Uri.Region(current.Region.Resource, current.Region.Start, region.End); 
				current = current.Parent;
			};
			parser.OnText += (value, region) =>
			{
				current.Add(new Text(value) { Region = region });
			};
			parser.OnData += (value, region) =>
			{
				current.Add(new Data(value) { Region = region });
			};
			parser.OnComment += (value, region) =>
			{
				current.Add(new Comment(value) { Region = region });
			};
			parser.OnProccessingInstruction += (target, value, region) =>
			{
				current.Add(new ProcessingInstruction(target, value) { Region = region });
			};

			return parser.Parse() ? result : null;
		}
		#endregion
	}
}
