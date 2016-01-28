// 
//  Member.cs
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
using Reflect = Kean.Reflect;

namespace Kean.Platform.Settings
{
	abstract class Member :
		Kean.IComparable<Member>
	{
		public string Name { get; private set; }
		public string Description { get; private set; }
		public string Usage { get; private set; }
		public string Example { get; private set; }
		Reflect.Member backend;
		public Object Parent { get; private set; }
		protected abstract char Delimiter { get; }
		protected Member(MemberAttribute attribute, Reflect.Member backend, Object parent)
		{
			this.Name = attribute.NotNull() ? attribute.Name : null;
			this.Description = attribute.NotNull() ? attribute.Description : null;
			this.Usage = attribute.NotNull() ? attribute.Usage ?? this.Description : null;
			this.Example = attribute.NotNull() ? attribute.Example : null;
			this.backend = backend;
			this.Parent = parent;
		}
		public abstract bool Execute(Parser editor, string[] parameters);
		public abstract string Complete(string[] parameters);
		public abstract string Help(string[] parameters);
		public abstract bool RequestType(Parser editor);

		#region IComparable<Member> Members
		public Order Compare(Member other)
		{
			return this.Name.CompareWith(other.Name);
		}
		#endregion
		public string NameRelative(Member current)
		{
			string result;
			if (this == current || this.Name.IsEmpty())
				result = "";
			else
			{
				result = this.Name + this.Delimiter;
				if (this.Parent.NotNull())
				{
					string parent = this.Parent.NameRelative(current);
					if (parent.NotEmpty())
						result = parent + result;
				}
			}
			return result;
		}
		public override string ToString()
		{
			return this.NameRelative(null).TrimEnd(this.Delimiter);
		}
	}
}
