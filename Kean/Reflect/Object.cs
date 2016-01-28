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
	public class Object :
		System.IEquatable<Object>
	{
		public object Backend { get; private set; }
		#region Properties
		Type type;
		public Type Type
		{
			get
			{
				if (this.type.IsNull())
					this.type = this.Backend.GetType();
				return this.type;
			}
		}
		Field[] fields;
		public Field[] Fields
		{
			get
			{
				if (this.fields.IsNull())
					this.fields = ((System.Type)this.Type).GetFields().Map(field => Field.Create(this, this.Type, field));
				return this.fields;
			}
		}
		Property[] properties;
		public Property[] Properties
		{
			get
			{
				if (this.properties.IsNull())
					this.properties = ((System.Type)this.Type).GetProperties().Map(property => Property.Create(this, this.Type, property));
				return this.properties;
			}
		}
		Event[] events;
		public Event[] Events
		{
			get
			{
				if (this.events.IsNull())
					this.events = ((System.Type)this.Type).GetEvents().Map(@event => Event.Create(this, this.Type, @event));
				return this.events;
			}
		}
		Method[] methods;
		public Method[] Methods
		{
			get
			{
				if (this.methods.IsNull())
					this.methods = ((System.Type)this.Type).GetMethods().Map(method => Method.Create(this, this.Type, method));
				return this.methods;
			}
		}

		#endregion
		#region Constructors
		public Object(object backend)
		{
			this.Backend = backend;
		}
		#endregion
		#region Get/Set Properties
		public void Set<T>(string property, T value)
		{
			((System.Type)this.Type).GetProperty(property).SetValue(this, value, null);
		}
		public T Get<T>(string property)
		{
			return (T)((System.Type)this.Type).GetProperty(property).GetValue(this, null);
		}
		#endregion
		#region Invoke Methods
		public object Invoke(string method, params object[] arguments)
		{
			return ((System.Type)this.Type).GetMethod(method).Invoke(this, arguments);
		}
		#endregion
		#region Get Members
		public Member GetMember(string name)
		{
			return Member.Create(this, this.Type, name);
		}
		public Member[] GetMember(string name, MemberFilter filter)
		{
			System.Type type = ((System.Type)this.Type);
			Collection.IList<Member> result = new Collection.Sorted.List<Member>();
			foreach (System.Reflection.MemberInfo member in type.GetMember(name, filter.AsBindingFlags()))
			{
				Member m = Member.Create(this, type, member, filter);
				if (m.NotNull())
					result.Add(m);
			}
			return result.ToArray();
		}
		public Member[] GetMembers()
		{
			System.Type type = ((System.Type)this.Type);
			Collection.IList<Member> result = new Collection.Sorted.List<Member>();
			foreach (System.Reflection.MemberInfo member in type.GetMembers())
			{
				Member m = Member.Create(this, type, member);
				if (m.NotNull())
					result.Add(m);
			}
			return result.ToArray();
		}
		public Member[] GetMembers(MemberFilter filter)
		{
			System.Type type = ((System.Type)this.Type);
			Collection.IList<Member> result = new Collection.Sorted.List<Member>();
			foreach (System.Reflection.MemberInfo member in type.GetMembers(filter.AsBindingFlags()))
			{
				Member m = Member.Create(this, type, member, filter);
				if (m.NotNull())
					result.Add(m);
			}
			return result.ToArray();
		}
		#endregion
		#region Get Fields
		public Field GetField(string name)
		{
			System.Type type = ((System.Type)this.Type);
			return Field.Create(this, type, type.GetField(name));
		}
		public Field GetField(string name, MemberFilter filter)
		{
			System.Type type = ((System.Type)this.Type);
			return Field.Create(this, type, type.GetField(name, filter.AsBindingFlags()));
		}
		public Field[] GetFields(MemberFilter filter)
		{
			System.Type type = ((System.Type)this.Type);
			Collection.IList<Field> result = new Collection.Sorted.List<Field>();
			foreach (System.Reflection.FieldInfo member in type.GetFields(filter.AsBindingFlags()))
			{
				Field f = Field.Create(this, type, member);
				if (f.NotNull())
					result.Add(f);
			}
			return result.ToArray();
		}
		#endregion
		#region Get Properties
		public Property GetProperty(string name)
		{
			System.Type type = ((System.Type)this.Type);
			return Property.Create(this, type, type.GetProperty(name));
		}
		public Property GetProperty(string name, MemberFilter filter)
		{
			System.Type type = ((System.Type)this.Type);
			return Property.Create(this, type, type.GetProperty(name, filter.AsBindingFlags()));
		}
		public Property[] GetProperties(MemberFilter filter)
		{
			System.Type type = ((System.Type)this.Type);
			Collection.IList<Property> result = new Collection.Sorted.List<Property>();
			foreach (System.Reflection.PropertyInfo member in type.GetProperties(filter.AsBindingFlags()))
			{
				Property p = Property.Create(this, type, member);
				if (p.NotNull())
					result.Add(p);
			}
			return result.ToArray();
		}
		#endregion
		#region Get Methods
		public Method GetMethod(string name)
		{
			System.Type type = ((System.Type)this.Type);
			return Method.Create(this, type, type.GetMethod(name));
		}
		public Method GetMethod(string name, MemberFilter filter)
		{
			System.Type type = ((System.Type)this.Type);
			return Method.Create(this, type, type.GetMethod(name, filter.AsBindingFlags()));
		}
		public Method[] GetMethods(MemberFilter filter)
		{
			System.Type type = ((System.Type)this.Type);
			Collection.IList<Method> result = new Collection.Sorted.List<Method>();
			foreach (System.Reflection.MethodInfo member in type.GetMethods(filter.AsBindingFlags()))
			{
				Method m = Method.Create(this, type, member);
				if (m.NotNull())
					result.Add(m);
			}
			return result.ToArray();
		}
		#endregion
		#region Object Overrides
		public override string ToString()
		{
			return this.Backend.ToString();
		}
		public override int GetHashCode()
		{
			return this.Backend.GetHashCode();
		}
		public override bool Equals(object other)
		{
			return other is Object && this.Equals(other as Object) || this.Backend.Equals(other);
		}
		#endregion
		#region IEquatable<Object> Members
		public bool Equals(Object other)
		{
			return other.NotNull() && this.Backend.Equals(other.Backend);
		}
		#endregion
	}
}
