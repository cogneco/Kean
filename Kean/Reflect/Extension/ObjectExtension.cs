// 
//  ObjectExtension.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2010-2011 Simon Mika
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

using Kean.Extension;
using Kean.Collection.Extension;

namespace Kean.Reflect.Extension
{
	public static class ObjectExtension
	{
		#region Get Type
		public static Type Type(this object me)
		{
			return me.NotNull() ? me.GetType() : null;
		}
		#endregion
		#region Get/Set Properties
		public static void Set<T>(this object me, string property, T value)
		{
			me.Set(property, value);
		}
		public static T Get<T>(this object me, string property)
		{
			return (T)me.Get(property);
		}
		public static void Set(this object me, string property, object value)
		{
			me.GetType().GetProperty(property).SetValue(me, value, null);
		}
		public static object Get(this object me, string property)
		{
			return me.GetType().GetProperty(property).GetValue(me, null);
		}
		#endregion
		#region Invoke Methods
		public static object Invoke(this object me, string method, params object[] arguments)
		{
			return me.GetType().GetMethod(method).Invoke(me, arguments);
		}
		#endregion
		#region Get Attributes
		public static System.Attribute[] GetAttributes(this object me)
		{
			return me.GetType().GetCustomAttributes(true).Map(attribute => attribute as System.Attribute);
		}
		public static T[] GetAttributes<T>(this object me)
			where T : System.Attribute
		{
			return me.GetType().GetCustomAttributes(typeof(T) ,true).Map(attribute => attribute as T);
		}
		#endregion
		#region Get Members
		public static Member GetMember(this object me, string name)
		{
			return Member.Create(me, me.GetType(), name);
		}
		public static Member[] GetMember(this object me, string name, MemberFilter filter)
		{
			return Member.Create(me, me.GetType(), name, filter);
		}
		public static Member[] GetMembers(this object me)
		{
			System.Type type = me.GetType();
			Collection.IList<Member> result = new Collection.Sorted.List<Member>();
			foreach (System.Reflection.MemberInfo member in type.GetMembers())
			{
				Member m = Member.Create(me, type, member);
				if (m.NotNull())
					result.Add(m);
			}
			return result.ToArray();
		}
		public static Member[] GetMembers(this object me, MemberFilter filter)
		{
			System.Type type = me.GetType();
			Collection.IList<Member> result = new Collection.Sorted.List<Member>();
			foreach (System.Reflection.MemberInfo member in type.GetMembers(filter.AsBindingFlags()))
			{
				Member m = Member.Create(me, type, member, filter);
				if (m.NotNull())
					result.Add(m);
			}
			return result.ToArray();
		}
		#endregion
		#region Get Fields
		public static Field GetField(this object me, string name)
		{
			System.Type type = me.GetType();
			return Field.Create(me, type, type.GetField(name));
		}
		public static Field GetField(this object me, string name, MemberFilter filter)
		{
			System.Type type = me.GetType();
			return Field.Create(me, type, type.GetField(name, filter.AsBindingFlags()));
		}
		public static Field[] GetFields(this object me)
		{
			System.Type type = me.GetType();
			Collection.IList<Field> result = new Collection.Sorted.List<Field>();
			foreach (System.Reflection.FieldInfo member in type.GetFields())
			{
				Field f = Field.Create(me, type, member);
				if (f.NotNull())
					result.Add(f);
			}
			return result.ToArray();
		}
		public static Field[] GetFields(this object me, MemberFilter filter)
		{
			System.Type type = me.GetType();
			Collection.IList<Field> result = new Collection.Sorted.List<Field>();
			foreach (System.Reflection.FieldInfo member in type.GetFields(filter.AsBindingFlags()))
			{
				Field f = Field.Create(me, type, member);
				if (f.NotNull())
					result.Add(f);
			}
			return result.ToArray();
		}
		#endregion
		#region Get Properties
		public static Property GetProperty(this object me, string name)
		{
			System.Type type = me.GetType();
			return Property.Create(me, type, type.GetProperty(name));
		}
		public static Property GetProperty(this object me, string name, MemberFilter filter)
		{
			System.Type type = me.GetType();
			return Property.Create(me, type, type.GetProperty(name, filter.AsBindingFlags()));
		}
		public static Property[] GetProperties(this object me)
		{
			System.Type type = me.GetType();
			Collection.IList<Property> result = new Collection.Sorted.List<Property>();
			foreach (System.Reflection.PropertyInfo member in type.GetProperties())
			{
				Property p = Property.Create(me, type, member);
				if (p.NotNull())
					result.Add(p);
			}
			return result.ToArray();
		}
		public static Property[] GetProperties(this object me, MemberFilter filter)
		{
			System.Type type = me.GetType();
			Collection.IList<Property> result = new Collection.Sorted.List<Property>();
			foreach (System.Reflection.PropertyInfo member in type.GetProperties(filter.AsBindingFlags()))
			{
				Property p = Property.Create(me, type, member);
				if (p.NotNull())
					result.Add(p);
			}
			return result.ToArray();
		}
		#endregion
		#region Get Events
		public static Event GetEvent(this object me, string name)
		{
			System.Type type = me.GetType();
			return Event.Create(me, type, type.GetEvent(name));
		}
		public static Event GetEvent(this object me, string name, MemberFilter filter)
		{
			System.Type type = me.GetType();
			return Event.Create(me, type, type.GetEvent(name, filter.AsBindingFlags()));
		}
		public static Event[] GetEvents(this object me)
		{
			System.Type type = me.GetType();
			Collection.IList<Event> result = new Collection.Sorted.List<Event>();
			foreach (System.Reflection.EventInfo member in type.GetEvents())
			{
				Event p = Event.Create(me, type, member);
				if (p.NotNull())
					result.Add(p);
			}
			return result.ToArray();
		}
		public static Event[] GetEvents(this object me, MemberFilter filter)
		{
			System.Type type = me.GetType();
			Collection.IList<Event> result = new Collection.Sorted.List<Event>();
			foreach (System.Reflection.EventInfo member in type.GetEvents(filter.AsBindingFlags()))
			{
				Event p = Event.Create(me, type, member);
				if (p.NotNull())
					result.Add(p);
			}
			return result.ToArray();
		}
		#endregion
		#region Get Methods
		public static Method GetMethod(this object me, string name)
		{
			System.Type type = me.GetType();
			return Method.Create(me, type, type.GetMethod(name));
		}
		public static Method GetMethod(this object me, string name, MemberFilter filter)
		{
			System.Type type = me.GetType();
			return Method.Create(me, type, type.GetMethod(name, filter.AsBindingFlags()));
		}
		public static Method[] GetMethods(this object me)
		{
			System.Type type = me.GetType();
			Collection.IList<Method> result = new Collection.Sorted.List<Method>();
			foreach (System.Reflection.MethodInfo member in type.GetMethods())
			{
				Method m = Method.Create(me, type, member);
				if (m.NotNull())
					result.Add(m);
			}
			return result.ToArray();
		}
		public static Method[] GetMethods(this object me, MemberFilter filter)
		{
			System.Type type = me.GetType();
			Collection.IList<Method> result = new Collection.Sorted.List<Method>();
			foreach (System.Reflection.MethodInfo member in type.GetMethods(filter.AsBindingFlags()))
			{
				Method m = Method.Create(me, type, member);
				if (m.NotNull())
					result.Add(m);
			}
			return result.ToArray();
		}
		#endregion
	}
}

