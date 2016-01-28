// 
//  StringCastable.cs
//  
//  Author:
//       Anders Frisk <andersfrisk77@gmail.com>
//  
//  Copyright (c) 2011 Anders Frisk
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
using Reflect = Kean.Reflect;

namespace Kean.Platform.Settings.Parameter
{
	class StringCastable : 
		Abstract
	{
		public StringCastable(Reflect.Type type) : 
			base(type)
		{ }
		public override string AsString(object value)
		{
			return (string)StringCastable.GetToStringCast(this.Type).Invoke(null, System.Reflection.BindingFlags.Static, null, new object[] { value }, System.Globalization.CultureInfo.InvariantCulture);
		}
		public override object FromString(string value)
		{
			return StringCastable.GetFromStringCast(this.Type).Invoke(null, System.Reflection.BindingFlags.Static, null, new object[] { value }, System.Globalization.CultureInfo.InvariantCulture);
		}
		public override string Complete(string incomplete)
		{
			return incomplete;
		}
		public override string Help(string incomplete)
		{
			return "";
		}
		public static bool IsStringCastable(Reflect.Type type)
		{
			return StringCastable.GetFromStringCast(type).NotNull() && StringCastable.GetToStringCast(type).NotNull();
		}
		static System.Reflection.MethodInfo GetFromStringCast(Type type)
		{
			return type.GetMethod("op_Implicit", new Type[] { typeof(string) }) ?? type.GetMethod("op_Explicit", new Type[] { typeof(string) });
		}
		static System.Reflection.MethodInfo GetToStringCast(Type type)
		{
			System.Reflection.MethodInfo result = null;
			foreach (System.Reflection.MethodInfo method in type.GetMethods())
			{
				if (method.IsStatic && method.ReturnType == typeof(string) && (method.Name == "op_Implicit" || method.Name == "op_Explicit"))
				{
					System.Reflection.ParameterInfo[] parameters = method.GetParameters();
					if (parameters.Length == 1 && parameters[0].ParameterType == type)
					{
						result = method;
						break;
					}
				}
			}
			return result;
		}
	}
}
