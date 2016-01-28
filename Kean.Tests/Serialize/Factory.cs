// 
//  Factory.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2009-2012 Simon Mika
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
using NUnit.Framework;
using Kean;
using Kean.Extension;
using Kean.Collection.Extension;
using Kean.Reflect.Extension;

namespace Kean.Serialize.Test
{
	public abstract class Factory<T, V> :
		Kean.Test.Fixture<T>, 
		IFactory
			where T : Kean.Test.Fixture<T>, new()
			where V : Verifier, new()
	{
		V verifier;
		byte Byte { get { return 42; } }
		sbyte SignedByte { get { return -42; } }
		short Short { get { return -1337; } }
		ushort UnsignedShort { get { return 1337; } }
		int Integer { get { return -1337; } }
		uint UnsignedInteger { get { return 1337; } }
		long Long { get { return -1337; } }
		ulong UnsignedLong { get { return 1337; } }
		float Single { get { return -13.37f; } }
		double Double { get { return -13.37; } }
		decimal Decimal { get { return -13.37m; } }
		char Character { get { return 'k'; } }
		string String { get { return "This is Kean."; } }
		DateTime DateTime { get { return new DateTime(2111, 11, 11, 11, 11, 11, 111); } }
		DateTimeOffset DateTimeOffset { get { return new DateTimeOffset(this.DateTime, new TimeSpan(13, 37, 00)); } }
		TimeSpan TimeSpan { get { return new TimeSpan(1337, 11, 11, 11, 111); } }
		bool Boolean { get { return true; } }
		Data.Enumerator Enumerator { get { return Data.Enumerator.Second; } }
		public Serialize.Storage Storage { get; private set; }
		protected Factory()
		{
			this.verifier = new V() { Factory = this };
			this.Storage = this.verifier.Storage;
		}
		public virtual bool Store<O>(O value, Uri.Locator locator)
		{
			return this.Storage.Store(value, locator);
		}
		protected virtual void Test(Reflect.Type type)
		{
			this.VerifyStore(type);
			this.VerifyLoad(type);
		}
		protected void VerifyStore(Reflect.Type type)
		{
			this.verifier.VerifyStore(type);
		}
		protected void VerifyLoad(Reflect.Type type)
		{
			this.verifier.VerifyLoad(type);
		}
		public virtual string Name(Reflect.Type type)
		{
			return type.ShortName + (type.Arguments.Count > 0 ? "(" + type.Arguments.Map(a => this.Name(a)).Join(",") + ")" : "");
		}
		public U Create<U>()
		{
			return (U)(this.Create(typeof(U)) ?? default(U));
		}
		public object Create(Reflect.Type type)
		{
			object result;
			if (type.Category != Reflect.TypeCategory.Primitive && type.Implements<Data.IData>())
			{
				result = type.Create();
				(result as Data.IData).Initialize(this);
			}
			else if (type.Name == "System.Nullable" && type.Arguments.Count == 1)
				result = type.Create(this.Create(type.Arguments[0]));
			else if (type == typeof(byte))
				result = this.Byte;
			else if (type == typeof(sbyte))
				result = this.SignedByte;
			else if (type == typeof(short))
				result = this.Short;
			else if (type == typeof(ushort))
				result = this.UnsignedShort;
			else if (type == typeof(int))
				result = this.Integer;
			else if (type == typeof(uint))
				result = this.UnsignedInteger;
			else if (type == typeof(long))
				result = this.Long;
			else if (type == typeof(ulong))
				result = this.UnsignedLong;
			else if (type == typeof(float))
				result = this.Single;
			else if (type == typeof(double))
				result = this.Double;
			else if (type == typeof(decimal))
				result = this.Decimal;
			else if (type == typeof(char))
				result = this.Character;
			else if (type == typeof(string))
				result = this.String;
			else if (type == typeof(DateTime))
				result = this.DateTime;
			else if (type == typeof(DateTimeOffset))
				result = this.DateTimeOffset;
			else if (type == typeof(TimeSpan))
				result = this.TimeSpan;
			else if (type == typeof(bool))
				result = this.Boolean;
			else if (type == typeof(Data.Enumerator))
				result = this.Enumerator;
			else
				result = null;
			return result;
		}
		public void Verify(object value, string message, params object[] arguments)
		{
			Expect(value, Is.Not.Null);
			if (value is Data.IData)
				(value as Data.IData).Verify(this, message, arguments);
			else
			{
				Type type = value.GetType();
				if (type == typeof(bool))
					Expect(value, Is.EqualTo(this.Boolean), message, arguments);
				else if (type == typeof(int))
					Expect(value, Is.EqualTo(this.Integer), message, arguments);
				else if (type == typeof(float))
					Expect(value, Is.EqualTo(this.Single), message, arguments);
				else if (type == typeof(Data.Enumerator))
					Expect(value, Is.EqualTo(this.Enumerator), message, arguments);
				else if (type == typeof(string))
					Expect(value, Is.EqualTo(this.String), message, arguments);
				else if (type == typeof(byte))
					Expect(value, Is.EqualTo(this.Byte), message, arguments);
				else if (type == typeof(short))
					Expect(value, Is.EqualTo(this.Short), message, arguments);
				else if (type == typeof(ushort))
					Expect(value, Is.EqualTo(this.UnsignedShort), message, arguments);
				else if (type == typeof(int))
					Expect(value, Is.EqualTo(this.Integer), message, arguments);
				else if (type == typeof(uint))
					Expect(value, Is.EqualTo(this.UnsignedInteger), message, arguments);
				else if (type == typeof(long))
					Expect(value, Is.EqualTo(this.Long), message, arguments);
				else if (type == typeof(ulong))
					Expect(value, Is.EqualTo(this.UnsignedLong), message, arguments);
				else if (type == typeof(float))
					Expect(value, Is.EqualTo(this.Single), message, arguments);
				else if (type == typeof(double))
					Expect(value, Is.EqualTo(this.Double), message, arguments);
				else if (type == typeof(decimal))
					Expect(value, Is.EqualTo(this.Decimal), message, arguments);
				else if (type == typeof(char))
					Expect(value, Is.EqualTo(this.Character), message, arguments);
				else if (type == typeof(string))
					Expect(value, Is.EqualTo(this.String), message, arguments);
				else if (type == typeof(DateTime))
					Expect(value, Is.EqualTo(this.DateTime), message, arguments);
				else if (type == typeof(DateTimeOffset))
					Expect(value, Is.EqualTo(this.DateTimeOffset), message, arguments);
				else if (type == typeof(TimeSpan))
					Expect(value, Is.EqualTo(this.TimeSpan), message, arguments);
				else if (type == typeof(bool))
					Expect(value, Is.EqualTo(this.Boolean), message, arguments);
				else if (type == typeof(Data.Enumerator))
					Expect(value, Is.EqualTo(this.Enumerator), message, arguments);
			}
		}
	}
}
