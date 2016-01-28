//
//  Branch.cs
//
//  Author:
//       Simon Mika <smika@hx.se>
//
//  Copyright (c) 2010-2012 Simon Mika
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
using Kean.Collection.Extension;
using Kean.Reflect.Extension;
using Generic = System.Collections.Generic;
namespace Kean.Serialize.Data
{
	public class Branch :
		Node
	{
		public Kean.Collection.IList<Node> Nodes { get; private set; }
		public Branch()
		{
			this.Nodes = new Kean.Collection.List<Node>();
		}
		public Branch(Generic.IEnumerable<Node> nodes) :
			this()
		{
			this.Nodes.Add(nodes);
		}
		public Branch(params Node[] nodes) :
			this((Generic.IEnumerable<Node>)nodes)
		{
		}
		public Branch(object value, Reflect.Type type) :
			this()
		{
			Reflect.Type valueType = value.Type();
			this.Type = valueType != type ? valueType : null;
		}
		public override Node UpdateLocators(Uri.Locator resource)
		{
			base.UpdateLocators(resource);
			foreach (Node child in this.Nodes)
			{
				resource = this.Locator.Copy();
				resource.Fragment = (resource.Fragment.NotEmpty() ? resource.Fragment + "." : "") + child.Name;
				child.UpdateLocators(resource);
			}
			return this;
		}
		public bool Merge(Node node)
		{
			return (node is Branch ? this.Nodes.Add((node as Branch).Nodes) : this.Nodes.Add(node)).NotNull();
		}
	}
}
