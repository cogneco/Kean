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
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.using System;

using Kean;
using Kean.Extension;
using Kean.Reflect.Extension;
using Kean.Collection.Extension;

namespace Kean.Reflect
{
	public abstract class Member :
		IComparable<Member>
	{
		protected Type ParentType { get; private set; }
		System.Reflection.MemberInfo information;
		public string Name { get; private set; }
		private System.Attribute[] attributes;
		public System.Attribute[] Attributes
		{
			get
			{
                if (this.attributes.IsNull())
                    this.attributes = System.Attribute.GetCustomAttributes(this.information, true).Map(attribute => attribute as System.Attribute);
				return this.attributes;
			}
		}
		internal Member(Type parentType, System.Reflection.MemberInfo information)
		{
			this.ParentType = parentType;
			this.information = information;
			this.Name = information.Name;
		}
		public T[] GetAttributes<T>()
			where T : System.Attribute
		{
            return System.Attribute.GetCustomAttributes(this.information, typeof(T), true).Map(attribute => attribute as T);
		}

		#region IComparable<Member> Members
		public Order Compare(Member other)
		{
			return this.Name.CompareWith(other.NotNull() ? other.Name : null);
		}
		#endregion
		internal static Member Create(object parent, Type parentType, System.Reflection.MemberInfo information)
		{
			return Member.Create(parent, parentType, information, MemberFilter.Field | MemberFilter.Property | MemberFilter.Method);
		}
		internal static Member Create(object parent, Type parentType, System.Reflection.MemberInfo information, MemberFilter filter)
		{
			Member result = null;
			if (information is System.Reflection.PropertyInfo && filter.Contains(MemberFilter.Property))
				result = Property.Create(parent, parentType, information as System.Reflection.PropertyInfo);
			else if (information is System.Reflection.FieldInfo && filter.Contains(MemberFilter.Property))
				result = Field.Create(parent, parentType, information as System.Reflection.FieldInfo);
			else if (information is System.Reflection.MethodInfo && filter.Contains(MemberFilter.Method))
				result = Method.Create(parent, parentType, information as System.Reflection.MethodInfo);
			return result;
		}

		internal static Member Create(object parent, Type parentType, string name)
		{
			System.Reflection.MemberInfo[] informations = ((System.Type)parentType).GetMember(name);
			return informations.Length > 0 ? Member.Create(parent, parentType, name) : null;
		}
		internal static Member[] Create(object parent, Type parentType, string name, MemberFilter filter)
		{
			System.Type type = parentType;
			Collection.IList<Member> result = new Collection.Sorted.List<Member>();
			foreach (System.Reflection.MemberInfo member in type.GetMember(name, filter.AsBindingFlags()))
			{
				Member m = Member.Create(parent, type, member, filter);
				if (m.NotNull())
					result.Add(m);
			}
			return result.ToArray();
		}
	}
}
