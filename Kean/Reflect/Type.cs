//
//  TypeName.cs
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

using Kean.Extension;
using Kean.Collection.Extension;

namespace Kean.Reflect
{
	public class Type :
		System.IEquatable<Type>,
		System.IEquatable<string>,
		System.IEquatable<System.Type>
	{
		string assembly;
		public string Assembly
		{ 
			get { return this.assembly; }
			private set
			{
				this.assembly = value; 
				if (this.Parent.NotNull())
					this.Parent.Assembly = value;
			} 
		}
		string name;
		public string Name
		{
			get { return this.name; }
			private set
			{
				this.name = value;
				switch (this.name)
				{
					case "System.Boolean":
						this.ShortName = "bool";
						break;
					case "System.Byte":
						this.ShortName = "byte";
						break;
					case "System.SByte":
						this.ShortName = "sbyte";
						break;
					case "System.Char":
						this.ShortName = "char";
						break;
					case "System.Decimal":
						this.ShortName = "decimal";
						break;
					case "System.Int16":
						this.ShortName = "short";
						break;
					case "System.UInt16":
						this.ShortName = "ushort";
						break;
					case "System.Int32":
						this.ShortName = "int";
						break;
					case "System.UInt32":
						this.ShortName = "uint";
						break;
					case "System.Int64":
						this.ShortName = "long";
						break;
					case "System.UInt64":
						this.ShortName = "ulong";
						break;
					case "System.Single":
						this.ShortName = "float";
						break;
					case "System.Double":
						this.ShortName = "double";
						break;
					case "System.String":
						this.ShortName = "string";
						break;
					case "System.Object":
						this.ShortName = "object";
						break;
					default:
						this.ShortName = this.Name.NotEmpty() ? this.name.Split('.', ':').Last() : null;
						break;
				}
			}
		}
		public string ShortName { get; private set; }
		public Type Parent { get; private set; }
		Collection.IList<Type> arguments;
		public Collection.IReadOnlyVector<Type> Arguments { get; private set; }
		TypeCategory? category;
		public TypeCategory Category
		{
			get
			{
				if (!this.category.HasValue)
				{
					System.Type t = ((System.Type)this);
					if (t.IsValueType)
					{
						if (t.IsPrimitive)
							this.category = TypeCategory.Primitive;
						else if (t.IsEnum)
							this.category = TypeCategory.Enumeration;
						else
							this.category = TypeCategory.Structure;
					}
					else
					{
						if (t.IsPointer)
							this.category = TypeCategory.Pointer;
						else if (t.IsInterface)
							this.category = TypeCategory.Interface;
						else if (t.IsArray)
							this.category = TypeCategory.Array;
						else if (t.IsClass)
							this.category = TypeCategory.Class;
					}
				}
				return this.category.Value;
			}
		}
		#region Members
		Collection.IReadOnlyVector<FieldInformation> fields;
		public Collection.IReadOnlyVector<FieldInformation> Fields
		{
			get
			{
				if (this.fields.IsNull())
				{
					Collection.IList<FieldInformation> result = new Collection.Sorted.List<FieldInformation>();
					foreach (System.Reflection.FieldInfo member in ((System.Type)this).GetFields())
					{
						FieldInformation f = new FieldInformation(this, member);
						if (f.NotNull())
							result.Add(f);
					}
					this.fields = new Collection.Wrap.ReadOnlyVector<FieldInformation>(result);
				}
				return this.fields;
			}
		}
		Collection.IReadOnlyVector<PropertyInformation> properties;
		public Collection.IReadOnlyVector<PropertyInformation> Properties
		{
			get
			{
				if (this.properties.IsNull())
				{
					Collection.IList<PropertyInformation> result = new Collection.Sorted.List<PropertyInformation>();
					foreach (System.Reflection.PropertyInfo member in ((System.Type)this).GetProperties())
					{
						PropertyInformation p = new PropertyInformation(this, member);
						if (p.NotNull())
							result.Add(p);
					}
					this.properties = new Collection.Wrap.ReadOnlyVector<PropertyInformation>(result);
				}
				return this.properties;
			}
		}
		Collection.IReadOnlyVector<EventInformation> events;
		public Collection.IReadOnlyVector<EventInformation> Events
		{
			get
			{
				if (this.events.IsNull())
				{
					Collection.IList<EventInformation> result = new Collection.Sorted.List<EventInformation>();
					foreach (System.Reflection.EventInfo member in ((System.Type)this).GetEvents())
					{
						EventInformation e = new EventInformation(this, member);
						if (e.NotNull())
							result.Add(e);
					}
					this.events = new Collection.Wrap.ReadOnlyVector<EventInformation>(result);
				}
				return this.events;
			}
		}
		Collection.IReadOnlyVector<MethodInformation> methods;
		public Collection.IReadOnlyVector<MethodInformation> Methods
		{
			get
			{
				if (this.methods.IsNull())
				{
					Collection.IList<MethodInformation> result = new Collection.Sorted.List<MethodInformation>();
					foreach (System.Reflection.MethodInfo member in ((System.Type)this).GetMethods())
					{
						MethodInformation m = new MethodInformation(this, member);
						if (m.NotNull())
							result.Add(m);
					}
					this.methods = new Collection.Wrap.ReadOnlyVector<MethodInformation>(result);
				}
				return this.methods;
			}
		}
		#endregion
		System.Type type;
		#region Constructors
		Type()
		{
			this.arguments = new Collection.List<Type>();
			this.Arguments = new Collection.Wrap.ReadOnlyVector<Type>(this.arguments);
		}
		Type(System.Type type) :
			this()
		{
			if (type.IsNested)
			{
				this.Parent = new Type(type.DeclaringType);
				this.Name = this.Parent.Name + "+" + type.Name.Split(new char[] { '`' }, 2)[0];
			}
			else
				this.Name = type.Namespace + "." + type.Name.Split(new char[] { '`' }, 2)[0];
			this.Assembly = type.Assembly.GetName().Name;
			if (type.IsGenericType)
				foreach (System.Type t in type.GetGenericArguments())
					this.arguments.Add(t.FullName.IsEmpty() ? null : t);
		}
		Type(string name) :
			this()
		{
			string[] splitted = name.Split('+');
			if (splitted.Length > 1)
			{
				System.Text.StringBuilder parent = new System.Text.StringBuilder();
				int i;
				parent.Append(splitted[0]);
				for (i = 1; i < splitted.Length - 1; i++)
					parent.Append("+" + splitted[i]);
				if (this.Assembly.NotEmpty())
					parent.Append(" ").Append(this.Assembly);
				this.Parent = new Type(parent.ToString());
				this.Assembly = this.Parent.Assembly;
				this.Name = this.Parent.Name + "+";
				name = splitted[i];
			}
			int pointer = -1;
			while (++pointer < name.Length)
				switch (name[pointer])
				{
					default:
						this.Name += name[pointer];
						break;
					case '<':
						pointer++;
						int tail = name.Length;
						while (name[--tail] != '>')
							;
						foreach (string argument in name.Substring(pointer, tail - pointer).Split(','))
							this.arguments.Add(new Type(argument.Trim(' ')));
						pointer = tail;
						break;
					case ':':
						this.Assembly = this.Name;
						this.Name += '.';
						break;
					case ' ':
						this.Assembly = name.Substring(pointer + 1);
						pointer = name.Length;
						break;
				}
			string assembly = "mscorlib";
			switch (this.Name)
			{
			// http://msdn.microsoft.com/en-US/library/ya5y69ds%28v=VS.80%29.aspx
				case "bool":
					this.Name = "System.Boolean";
					break;
				case "byte":
					this.Name = "System.Byte";
					break;
				case "sbyte":
					this.Name = "System.SByte";
					break;
				case "char":
					this.Name = "System.Char";
					break;
				case "short":
					this.Name = "System.Int16";
					break;
				case "ushort":
					this.Name = "System.UInt16";
					break;
				case "int":
					this.Name = "System.Int32";
					break;
				case "uint":
					this.Name = "System.UInt32";
					break;
				case "long":
					this.Name = "System.Int64";
					break;
				case "ulong":
					this.Name = "System.UInt64";
					break;
				case "float":
					this.Name = "System.Single";
					break;
				case "double":
					this.Name = "System.Double";
					break;
				case "decimal":
					this.Name = "System.Decimal";
					break;
				case "string":
					this.Name = "System.String";
					break;
				case "object":
					this.Name = "System.Object";
					break;
				default:
					assembly = this.Assembly;
					break;
			}
			this.Assembly = assembly;
		}
		public Type(string assembly, string name, params Type[] arguments) :
			this()
		{
			this.Assembly = assembly;
			this.Name = name;
			this.arguments.Add(arguments);
		}
		#endregion
		public T Create<T>()
		{
			return (T)this.Create();
		}
		public object Create()
		{
			return System.Activator.CreateInstance(this);
		}
		public object Create(params object[] parameters)
		{
			System.Reflection.ConstructorInfo constructor = ((System.Type)this).GetConstructor(parameters.Map(parameter => parameter.GetType()));
			return constructor.NotNull() ? constructor.Invoke(parameters) : null;
		}
		#region Implemented Interfaces
		public bool Implements(Type type)
		{
			return type.Category == TypeCategory.Interface && this.GetImplementation(type).NotNull() || this.Inherits(type);
		}
		public bool Implements<T>()
		{
			return this.Implements(typeof(T));
		}
		public Type GetImplementation(Type type)
		{
			System.Type me = (System.Type)this;
			System.Type t = (System.Type)type;
			return me.NotNull() && t.NotNull() ? me.GetInterface(t.Name) : null;
		}
		public Type GetImplementation<T>()
		{
			return this.GetImplementation(typeof(T));
		}
		#endregion
		#region Inherited Classes
		public Type Base { get { return ((System.Type)this).BaseType.NotNull() ? ((Type)((System.Type)this).BaseType) : null; } }
		public bool Inherits<T>()
		{
			return this.Inherits(typeof(T));
		}
		public bool Inherits(Type type)
		{
			return this == type || ((System.Type)this).NotNull() && ((System.Type)this).BaseType.NotNull() && ((Type)((System.Type)this).BaseType).Inherits(type);
		}
		#endregion
		#region Get Attributes
		public System.Attribute[] GetAttributes()
		{
			return ((System.Type)this).GetCustomAttributes(true).Map(attribute => attribute as System.Attribute);
		}
		public T[] GetAttributes<T>()
			where T : System.Attribute
		{
			return ((System.Type)this).GetCustomAttributes(typeof(T), true).Map(attribute => attribute as T);
		}
		#endregion
		#region Object Overrides
		public override string ToString()
		{
			return this;
		}
		public override bool Equals(object other)
		{
			return other is Type && this.Equals(other as Type);
		}
		public override int GetHashCode()
		{
			return ((string)this).GetHashCode();
		}
		#endregion
		#region IEquatable<Typename>, IEquatable<string>, IEquatable<Type>
		public bool Equals(Type other)
		{
			return other.NotNull() && (string)this == (string)other;
		}
		public bool Equals(string other)
		{
			return (string)this == other;
		}
		public bool Equals(System.Type other)
		{
			return other.NotNull() && this == (Type)other;
		}
		#endregion
		#region Binary Operators
		public static bool operator ==(Type left, Type right)
		{
			return left.Same(right) || left.NotNull() && left.Equals(right);
		}
		public static bool operator !=(Type left, Type right)
		{
			return !(left == right);
		}
		public static bool operator ==(Type left, string right)
		{
			return left.Same(right) || left.NotNull() && left.Equals(right);
		}
		public static bool operator !=(Type left, string right)
		{
			return !(left == right);
		}
		public static bool operator ==(string left, Type right)
		{
			return right == left;
		}
		public static bool operator !=(string left, Type right)
		{
			return !(left == right);
		}
		public static bool operator ==(Type left, System.Type right)
		{
			return left.Same(right) || left.NotNull() && left.Equals(right);
		}
		public static bool operator !=(Type left, System.Type right)
		{
			return !(left == right);
		}
		public static bool operator ==(System.Type left, Type right)
		{
			return right == left;
		}
		public static bool operator !=(System.Type left, Type right)
		{
			return !(left == right);
		}
		#endregion
		#region Casts
		public static implicit operator Type(string value)
		{
			return value.IsNull() ? null : new Type(value);
		}
		public static implicit operator string(Type value)
		{
			string result = "";
			// http://msdn.microsoft.com/en-US/library/ya5y69ds%28v=VS.80%29.aspx
			switch (value.Name)
			{
				case "System.Boolean":
					result = "bool";
					break;
				case "System.Byte":
					result = "byte";
					break;
				case "System.SByte":
					result = "sbyte";
					break;
				case "System.Char":
					result = "char";
					break;
				case "System.Decimal":
					result = "decimal";
					break;
				case "System.Int16":
					result = "short";
					break;
				case "System.UInt16":
					result = "ushort";
					break;
				case "System.Int32":
					result = "int";
					break;
				case "System.UInt32":
					result = "uint";
					break;
				case "System.Int64":
					result = "long";
					break;
				case "System.UInt64":
					result = "ulong";
					break;
				case "System.Single":
					result = "float";
					break;
				case "System.Double":
					result = "double";
					break;
				case "System.String":
					result = "string";
					break;
				case "System.Object":
					result = "object";
					break;
				default:
					System.Text.StringBuilder resultBuilder = null;
					if (value.Arguments.Count > 0)
					{
						resultBuilder = new System.Text.StringBuilder().Append("<");
						bool first = true;
						foreach (Type name in value.Arguments)
						{
							if (!first)
								resultBuilder.Append(",");
							else
								first = false;
							resultBuilder.Append((string)name);
						}
						resultBuilder.Append(">");
					}
					if (value.Assembly.NotEmpty() && value.Name.StartsWith(value.Assembly))
						resultBuilder = new System.Text.StringBuilder(value.Assembly).Append(":").Append(value.Name.Substring(value.Assembly.Length + 1)).Append(resultBuilder);
					else
					{
						resultBuilder = new System.Text.StringBuilder(value.Name).Append(resultBuilder);
						if (value.Assembly.NotEmpty() && value.Assembly != "mscorlib")
							resultBuilder.AppendFormat(" {0}", value.Assembly);
					}
					result = resultBuilder.ToString();
					break;
			}
			return result;
		}
		public static implicit operator Type(System.Type type)
		{
			return type.IsNull() ? null : new Type(type);
		}
		public static implicit operator System.Type(Type type)
		{
			System.Type result = null;
			if (type.NotNull())
			{
				if (type.type.IsNull())
				{
					System.Text.StringBuilder name = new System.Text.StringBuilder(type.Name);
					if (type.Arguments.Count > 0)
					{
						name = name.AppendFormat("`{0}[", type.Arguments.Count);
						bool first = true;
						foreach (Type argument in type.Arguments)
						{
							if (first)
								first = false;
							else
								name.Append(",");
							name.AppendFormat("[{0}]", argument.NotNull() ? ((System.Type)argument).AssemblyQualifiedName : "");
						}
						name.Append("]");
					}
					if (type.Assembly.NotEmpty() && type.Assembly != "mscorlib")
						name.AppendFormat(", {0}", type.Assembly);
					type.type = System.Type.GetType(name.ToString(), false);
				}
				result = type.type;
			}
			return result;
		}
		#endregion
	}
}
