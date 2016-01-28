// 
//  TypeExtension.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2012 Simon Mika
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

namespace Kean.Extension
{
	public static class TypeExtension
	{
		public static Func<string, object> FromStringCast(this Type me)
		{
			System.Reflection.MethodInfo result = me.GetMethod("op_Implicit", new Type[] { typeof(string) }) ?? me.GetMethod("op_Explicit", new Type[] { typeof(string) });
			return result.NotNull() ? (Func<string, object>)(value => result.Invoke(null, new object[] { value } )) : null;
		}
		public static Func<object, string> ToStringCast(this Type me)
		{
			System.Reflection.MethodInfo result = null;
			foreach (System.Reflection.MethodInfo method in me.GetMethods())
			{
				if (method.IsStatic && method.ReturnType == typeof(string) && (method.Name == "op_Implicit" || method.Name == "op_Explicit"))
				{
					System.Reflection.ParameterInfo[] parameters = method.GetParameters();
					if (parameters.Length == 1 && parameters[0].ParameterType == me)
					{
						result = method;
						break;
					}
				}
			}
			return result.NotNull() ? (Func<object, string>)(value => result.Invoke(null, new object[] { value }) as string) : null;
		}
		public static Func<byte[], object> FromBinaryCast(this Type me)
		{
			System.Reflection.MethodInfo result = me.GetMethod("op_Implicit", new Type[] { typeof(byte[]) }) ?? me.GetMethod("op_Explicit", new Type[] { typeof(byte[]) });
			return result.NotNull() ? (Func<byte[], object>)(value => result.Invoke(null, new object[] { value })) : null;
		}
		public static Func<object, byte[]> ToBinaryCast(this Type me)
		{
			System.Reflection.MethodInfo result = null;
			foreach (System.Reflection.MethodInfo method in me.GetMethods())
			{
				if (method.IsStatic && method.ReturnType == typeof(byte[]) && (method.Name == "op_Implicit" || method.Name == "op_Explicit"))
				{
					System.Reflection.ParameterInfo[] parameters = method.GetParameters();
					if (parameters.Length == 1 && parameters[0].ParameterType == me)
					{
						result = method;
						break;
					}
				}
			}
			return result.NotNull() ? (Func<object, byte[]>)(value => result.Invoke(null, new object[] { value }) as byte[]) : null;
		}
	}
}
