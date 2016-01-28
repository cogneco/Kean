// 
//  StringCastable.cs
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
using Kean.Extension;
using Kean.Reflect.Extension;
namespace Kean.Serialize.Serializer
{
    public class StringCastable :
		ISerializer
    {
        public StringCastable()
        {
        }

        #region ISerializer Members

		public ISerializer Find(Reflect.Type type, bool deserialize)
        {
            return this.GetFromStringCast(type).NotNull() && this.GetToStringCast(type).NotNull() ? this : null;
        }
        public Data.Node Serialize(IStorage storage, Reflect.Type type, object data, Uri.Locator locator)
        {
            return new Data.String((string)this.GetToStringCast(data.Type()).Invoke(null, System.Reflection.BindingFlags.Static, null, new object[] { data }, System.Globalization.CultureInfo.InvariantCulture), data, type);
        }
        public object Deserialize(IStorage storage, Serialize.Data.Node data, object result)
        {
            return this.GetFromStringCast(data.Type).Invoke(null, System.Reflection.BindingFlags.Static, null, new object[] { data is Data.String ? (data as Data.String).Value : null }, System.Globalization.CultureInfo.InvariantCulture);
        }

        #endregion

        System.Reflection.MethodInfo GetFromStringCast(Type type)
        {
            return type.GetMethod("op_Implicit", new Type[] { typeof(string) }) ?? type.GetMethod("op_Explicit", new Type[] { typeof(string) });
        }
        System.Reflection.MethodInfo GetToStringCast(Type type)
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

