//
//  Collection.cs
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
namespace Kean.Serialize.Data
{
    public class Collection :
		Branch
    {
        public override Uri.Locator Locator
        {
            get { return base.Locator; }
            set
            {
                base.Locator = value;
                foreach (Node child in this.Nodes)
                    child.Locator = value;
            }
        }
        public override ParameterAttribute Attribute
        {
            get { return base.Attribute; }
            set
            {
                base.Attribute = value;
                foreach (Node child in this.Nodes)
                    child.Attribute = value;
            }
        }
        public override string Name
        {
            get { return base.Name; }
            set
            {
                base.Name = value;
                foreach (Node child in this.Nodes)
                    child.Name = value;
            }
        }
        public Collection()
        {
        }
        public Collection(object value, Reflect.Type type) :
			this()
        {
            Reflect.Type valueType = value.Type();
            this.Type = valueType != type ? valueType : null;
        }
        public Collection(System.Collections.Generic.IEnumerable<Node> nodes) :
			base(nodes)
        {
        }
        public Collection(params Node[] nodes) :
			this((System.Collections.Generic.IEnumerable<Node>)nodes)
        {
        }
    }
}
