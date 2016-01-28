//
//  Table.cs
//
//  Author:
//       Simon Mika <smika@hx.se>
//
//  Copyright (c) 2012-2013 Simon Mika
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
//  You should have received data copy of the GNU Lesser General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using Kean;
using Kean.Extension;
using Collection = Kean.Collection;
using Kean.Collection.Extension;
using Uri = Kean.Uri;
using Serialize = Kean.Serialize;
using Reflect = Kean.Reflect;
using Kean.Reflect.Extension;
using Data = System.Data;
using IO = Kean.IO;
using Generic = System.Collections.Generic;
using Expressions = System.Linq.Expressions;

namespace Kean.DB.Sql
{
	public class Table<T> :
		DB.Table<T>
		where T : Item<T>, new()
	{
		Data.IDbConnection connection;
		string fieldString;
		public string FieldString
		{
			get
			{
				if (this.fieldString.IsNull())
					this.fieldString = this.Columns.Map(column => "`" + column.Key + "`").Join(", ");
				return this.fieldString;
			}
		}
		internal Table(Data.IDbConnection connection, string name) :
			base(name)
		{
			this.connection = connection;
		}
		string SqlType(KeyValue<string, Reflect.Type> field)
		{
			string result = "`" + field.Key + "` ";
			switch (field.Value)
			{
				case "bool":
					result += "boolean";
					break;
				case "byte":
					result += "tinyint UNSIGNED";
					break;
				case "sbyte":
					result += "tinyint";
					break;
				case "byte[]":
					result += "binary";
					break;
				case "System.DateTime":
					result += "datetime";
					break;
				case "decimal":
					result += "decimal";
					break;
				case "double":
					result += "double";
					break;
				case "System.Guid":
					result += "char(36)";
					break;
				case "short":
					result += "smallint";
					break;
				case "ushort":
					result += "smallint UNSIGNED";
					break;
				case "int":
					result += "int";
					break;
				case "uint":
					result += "int UNSIGNED";
					break;
				case "long":
					result += "bigint(20)";
					break;
				case "ulong":
					result += "bigint UNSIGNED";
					break;
				case "float":
					result += "float";
					break;
				case "string":
					result += "text";
					break;
				case "System.TimeSpan":
					result += "time";
					break;

			}
			return result;
		}
		Generic.IEnumerable<Serialize.Data.Leaf> Read(Data.IDataReader reader)
		{
			int ordinal = 0;
			foreach (KeyValue<string, Reflect.Type> field in this.Columns)
				yield return this.Read(reader, ordinal++, field);
		}
		Serialize.Data.Leaf Read(Data.IDataReader reader, int ordinal, KeyValue<string, Reflect.Type> field)
		{
			Serialize.Data.Leaf result = null;
			if (field.Value == typeof(bool))
				result = new Serialize.Data.Boolean(reader.GetBoolean(ordinal));
			else if (field.Value == typeof(byte))
				result = new Serialize.Data.Byte(reader.GetByte(ordinal));
			else if (field.Value == typeof(char))
				result = new Serialize.Data.Character(reader.GetChar(ordinal));
			else if (field.Value == typeof(DateTime))
				result = new Serialize.Data.DateTime(reader.GetDateTime(ordinal));
			else if (field.Value == typeof(decimal))
				result = new Serialize.Data.Decimal(reader.GetDecimal(ordinal));
			else if (field.Value == typeof(double))
				result = new Serialize.Data.Double(reader.GetDouble(ordinal));
        			//else if (field.Value == typeof(Guid))
        			//case typeof(Guid): result = new Serialize.Data.Guid(reader.GetGuid(ordinal)); break;
        			else if (field.Value == typeof(short))
				result = new Serialize.Data.Short(reader.GetInt16(ordinal));
			else if (field.Value == typeof(int))
				result = new Serialize.Data.Integer(reader.GetInt32(ordinal));
			else if (field.Value == typeof(long))
				result = new Serialize.Data.Long(reader.GetInt64(ordinal));
			else if (field.Value == typeof(sbyte))
				result = new Serialize.Data.SignedByte(unchecked((sbyte)reader.GetByte(ordinal)));
			else if (field.Value == typeof(float))
				result = new Serialize.Data.Single(reader.GetFloat(ordinal));
			else if (field.Value == typeof(string))
				result = new Serialize.Data.String(reader.IsDBNull(ordinal) ? null : reader.GetString(ordinal));
			else if (field.Value == typeof(TimeSpan))
				result = new Serialize.Data.TimeSpan(reader.GetDateTime(ordinal) - new DateTime());
			else if (field.Value == typeof(ushort))
				result = new Serialize.Data.UnsignedShort(unchecked((ushort)reader.GetInt16(ordinal)));
			else if (field.Value == typeof(uint))
				result = new Serialize.Data.UnsignedInteger(unchecked((uint)reader.GetInt32(ordinal)));
			else if (field.Value == typeof(ulong))
				result = new Serialize.Data.UnsignedLong(unchecked((ulong)reader.GetInt64(ordinal)));
			if (result.NotNull())
				result.Name = field.Key;
			return result;
		}
		#region implemented abstract members of Table
		protected override bool Create(Generic.IEnumerable<KeyValue<string, Reflect.Type>> columns)
		{
			bool result;
			if (result = this.connection.NotNull())
			{
				IO.Text.Builder query = new IO.Text.Builder("CREATE TABLE ");
				query += "`" + this.Name + "` (";
				foreach (var field in this.Columns)
					switch (field.Key)
					{
						default:
							query += this.SqlType(field) + ", ";
							break;
						case "key":
							query += "`key` bigint(20) NOT NULL AUTO_INCREMENT, ";
							break;
						case "_type":
							query += "`_type` varchar(255) DEFAULT NULL, ";
							break;
						case "_data":
							query += "`_data` longtext, ";
							break;
					}
				query += "PRIMARY KEY (`key`),";
				query += "UNIQUE KEY `key` (`key`)";
				query += ") DEFAULT CHARSET=utf8";
				Console.WriteLine(query);
				using (Data.IDbCommand command = this.connection.CreateCommand())
				{
					command.CommandText = query;
					result = command.ExecuteNonQuery() == 0;
				}
			}
			return result;
		}
		internal protected override int GetCount(Generic.IEnumerable<Expressions.Expression<Func<T, bool>>> filters, Sorting<T> sorting, int limit, int offset)
		{
			int result = -1;
			if (this.connection.NotNull())
			{
				IO.Text.Builder query = new IO.Text.Builder("SELECT COUNT(*) FROM ") + this.Name + QueryGenerator<T>.Generate(this.Database.Casing, filters, sorting, limit, offset);
				Console.WriteLine(query);
				using (Data.IDbCommand command = this.connection.CreateCommand())
				{
					command.CommandText = query;
					using (Data.IDataReader reader = command.ExecuteReader())
						if (reader.Read())
							result = reader.GetInt32(0);
				}
			}
			return result;
		}
		protected internal override int Delete(Generic.IEnumerable<Expressions.Expression<Func<T, bool>>> filters, Sorting<T> sorting, int limit, int offset)
		{
			int result = -1;
			if (this.connection.NotNull())
			{
				IO.Text.Builder query = new IO.Text.Builder("DELETE FROM ") + this.Name + QueryGenerator<T>.Generate(this.Database.Casing, filters, sorting, limit, offset);
				Console.WriteLine(query);
				using (Data.IDbCommand command = this.connection.CreateCommand())
				{
					command.CommandText = query;
					result = command.ExecuteNonQuery();
				}
			}
			return result;
		}
		protected override long Create(Generic.IEnumerable<Serialize.Data.Leaf> fields)
		{
			Collection.IList<string> columns = new Collection.List<string>();
			Collection.IList<string> values = new Collection.List<string>();
			foreach (Serialize.Data.Leaf field in fields)
				if (field.Name != "key")
				{
					columns.Add("`" + field.Name + "`");
					values.Add((field as Serialize.Data.Leaf).Text.AddDoubleQuotes());
				}
			IO.Text.Builder query = (IO.Text.Builder)"INSERT INTO " + this.Name + "(" + columns.Join(", ") + ") VALUES(" + values.Join(", ") + ")";
			Console.WriteLine(query);
			long result = -1;
			using (Data.IDbCommand command = this.connection.CreateCommand())
			{
				command.CommandText = query;
				command.ExecuteNonQuery();
			}
			using (Data.IDbCommand command = this.connection.CreateCommand())
			{
				command.CommandText = "SELECT LAST_INSERT_ID()";
				result = (long)(ulong)command.ExecuteScalar();
			}
			return result;
		}
		protected override Generic.IEnumerable<Generic.IEnumerable<Serialize.Data.Leaf>> ReadFields(Generic.IEnumerable<Expressions.Expression<Func<T, bool>>> filters, Sorting<T> sorting, int limit, int offset)
		{
			IO.Text.Builder query = (IO.Text.Builder)"SELECT " + this.FieldString + " FROM " + this.Name;
			query += QueryGenerator<T>.Generate(this.Database.Casing, filters, sorting, limit, offset);
			Console.WriteLine(query);
			Collection.List<Generic.IEnumerable<Serialize.Data.Leaf>> result = new Collection.List<Generic.IEnumerable<Serialize.Data.Leaf>>();
			using (Data.IDbCommand command = this.connection.CreateCommand())
			{
				command.CommandText = query;
				using (Data.IDataReader reader = command.ExecuteReader())
					while (reader.Read())
					{
						Collection.List<Serialize.Data.Leaf> r = new Collection.List<Serialize.Data.Leaf>();
						foreach (Serialize.Data.Leaf field in this.Read(reader))
							r.Add(field);
						result.Add(r);
					}
			}
			return result;
		}
		protected override int Update(Generic.IEnumerable<Expressions.Expression<Func<T, bool>>> filters, Sorting<T> sorting, int limit, int offset, Generic.IEnumerable<Serialize.Data.Leaf> fields)
		{
			IO.Text.Builder query = (IO.Text.Builder)"UPDATE " + this.Name + " SET " + fields.Map(field => "`" + field.Name + "` = " + (field as Serialize.Data.Leaf).Text.AddDoubleQuotes()).Join(", ");
			query += QueryGenerator<T>.Generate(this.Database.Casing, filters, sorting, limit, offset);
			Console.WriteLine(query);
			int result;
			using (Data.IDbCommand command = this.connection.CreateCommand())
			{
				command.CommandText = query;
				result = command.ExecuteNonQuery();
			}
			return result;
		}
		#endregion
	}
}

