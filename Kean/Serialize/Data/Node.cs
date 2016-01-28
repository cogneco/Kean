// 
//  Node.cs
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

namespace Kean.Serialize.Data
{
	public abstract class Node
	{
		public System.Diagnostics.StackTrace created;
		public Uri.Region Region { get; set; } 
		public virtual Uri.Locator Locator { get; set; }
		string name;
		public virtual string Name 
		{
			get { return (this.Attribute.NotNull() ? this.Attribute.Name : null) ?? this.name ?? (this.Type.NotNull() ? this.Type.ShortName : null); }
			set { this.name = value; } 
		}
		public virtual ParameterAttribute Attribute { get; set; }
		public Reflect.Type Type { get; set; }
		public Reflect.Type OriginalType { get; set; }
		protected Node()
		{
			this.created = new System.Diagnostics.StackTrace();
		}
		public Node UpdateName(string name)
		{
			this.Name = name;
			return this;
		}
		public Node UpdateAttribute(ParameterAttribute attribute)
		{
			this.Attribute = attribute;
			return this;
		}
		public Node DefaultType(Reflect.Type type)
		{
			this.OriginalType = this.Type;
			if (type.Name == "System.Nullable")
			{
				if (this.Type.IsNull())
					this.Type = type.Arguments[0];
				else if (!this.Type.Implements(type.Arguments[0]))
					this.Type = type;
			}
			else if (this.Type.IsNull() || type != typeof(object) && !this.Type.Implements(type))
				this.Type = type;
			return this;
		}
		public virtual Node UpdateLocators(Uri.Locator locator)
		{
			return this.UpdateLocator(locator);
		}
		public Node UpdateLocator(Uri.Locator locator)
		{
			this.Locator = locator;
			return this;
		}
	}
}
