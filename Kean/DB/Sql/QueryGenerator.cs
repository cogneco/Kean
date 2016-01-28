//
//  QueryGenerator.cs
//
//  Author:
//       Simon Mika <smika@hx.se>
//
//  Copyright (c) 2013 Simon Mika
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
using Kean.Serialize.Extension;
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
	class QueryGenerator<T>
	{
		IO.Text.Builder builder = new IO.Text.Builder();
		string parameter;
		Serialize.Casing casing;
		QueryGenerator()
		{
		}
		#region Generate
		void Generate(Expressions.Expression expression)
		{
			if (expression is Expressions.BinaryExpression)
				this.Generate(expression as Expressions.BinaryExpression);
			else if (expression is Expressions.MemberExpression)
				this.Generate(expression as Expressions.MemberExpression);
			else if (expression is Expressions.ConstantExpression)
				this.Generate(expression as Expressions.ConstantExpression);
			else
				Console.WriteLine(expression.Type());
		}
		void Generate(Expressions.BinaryExpression expression)
		{
			this.builder += "(";
			this.Generate(expression.Left);
			this.builder += ")";
			switch (expression.NodeType)
			{
				case Expressions.ExpressionType.Equal:
					this.builder += " = ";
					break;
				case Expressions.ExpressionType.GreaterThan:
					this.builder += " > ";
					break;
				case Expressions.ExpressionType.GreaterThanOrEqual:
					this.builder += " >= ";
					break;
				case Expressions.ExpressionType.LessThan:
					this.builder += " < ";
					break;
				case Expressions.ExpressionType.LessThanOrEqual:
					this.builder += " <= ";
					break;
				case Expressions.ExpressionType.AndAlso:
					this.builder += " AND ";
					break;
				case Expressions.ExpressionType.OrElse:
					this.builder += " OR ";
					break;
				default:
					Console.WriteLine(expression.NodeType);
					break;
			}
			this.builder += "(";
			this.Generate(expression.Right);
			this.builder += ")";
		}
		void Generate(Expressions.MemberExpression expression)
		{
			if (expression.Expression is Expressions.MemberExpression)
				this.Generate(expression.Expression as Expressions.MemberExpression);
			else if (expression.Expression is Expressions.ParameterExpression && (expression.Expression as Expressions.ParameterExpression).Name == this.parameter)
			{
				if (expression.Member.Name == "Key")
					this.builder += "`key`";
				else
				{
					IndexAttribute attribute = ((Reflect.Type)expression.Member.ReflectedType).Properties.Find(p => p.Name == expression.Member.Name).Attributes.Find(a => a is IndexAttribute) as IndexAttribute;
					this.builder += "`" + (attribute.Name ?? expression.Member.Name.Convert(Serialize.Casing.Pascal, this.casing)) + "`";
				}
			}
			else if (expression.Expression is Expressions.ConstantExpression)
				this.builder += (expression.Member as System.Reflection.FieldInfo).GetValue((expression.Expression as Expressions.ConstantExpression).Value).AsString().AddDoubleQuotes();

		}
		void Generate(Expressions.ConstantExpression expression)
		{
			this.builder += expression.Value.AsString().AddDoubleQuotes();
		}
		#endregion
		void Filter(Generic.IEnumerable<Expressions.Expression<Func<T, bool>>> filters)
		{
			if (filters.NotNull())
			{
				bool empty = true;
				foreach (var filter in filters)
					if (filter.NotNull())
					{
						this.builder += empty ? " WHERE (" : ") AND (";
						this.parameter = filter.Parameters[0].Name;
						this.Generate(filter.Body);
						empty = false;
					}
				if (!empty)
					this.builder += ")";
			}
		}
		void Sort(Sorting<T> sorting)
		{
			if (sorting.NotNull())
			{
				if (sorting.Previous.IsNull())
					this.builder += " ORDER BY ";
				else
				{
					this.Sort(sorting);
					this.builder += ", ";
				}
				this.parameter = sorting.Selector.Parameters[0].Name;
				this.Generate(sorting.Selector.Body);
				if (sorting.Descending)
					this.builder += " DESC";
			}
		}
		void Limit(int limit, int offset)
		{
			if (limit > 0)
			{
				this.builder += " LIMIT " + limit;
				if (offset > 0)
					this.builder += " OFFSET " + offset;
			}
		}
		#region Static Genearate
		public static IO.Text.Builder Generate(Serialize.Casing casing, Generic.IEnumerable<Expressions.Expression<Func<T, bool>>> filters, Sorting<T> sorting, int limit, int offset)
		{
			QueryGenerator<T> result = new QueryGenerator<T>();
			result.casing = casing;
			result.Filter(filters);
			result.Sort(sorting);
			result.Limit(limit, offset);
			return result.builder;
		}
		#endregion
	}
}

