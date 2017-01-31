//
//  Addition.cs
//
//  Author:
//       Simon Mika <simon@mika.se>
//
//  Copyright (c) 2014 Simon Mika
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
using Kean.Collection.Extension;

namespace Kean.Math.Algebra
{
	public class Addition :
	BinaryOperator
	{
		public override int Precedence { get { return 4; } }
		protected override string Symbol { get { return "+"; } }
		internal Addition() :
			this(0, 0)
		{
		}
		public Addition(Expression left, Expression right) :
			base(left, right)
		{
		}
		public override float Evaluate(params KeyValue<string, float>[] variables)
		{
			return this.Left.Evaluate(variables) + this.Right.Evaluate(variables);
		}
		public override Expression Derive(string variable)
		{
			return this.Left.Derive(variable) + this.Right.Derive(variable);
		}
		public override Expression Simplify()
		{
			var terms = this.CollectTerms(this);
			Expression result;
			float number = 0;
			terms.Remove(term =>
			{
				bool r;
				if (r = term.Item2 is Constant)
				{
					if (term.Item1 == -1)
						number -= (term.Item2 as Constant).Value;
					else
						number += (term.Item2 as Constant).Value;
				}
				return r;
			});
			result = number;
			while (terms.Count > 0)
			{
				Tuple<float, Expression> nextItem = terms.Remove();
				Expression next = nextItem.Item2;
				float repitition = nextItem.Item1;
				terms.Remove(term =>
				{
					bool r;
					if (r = ((term.Item2).Equals(next)))
						repitition += term.Item1;

					return r;
				});
				if (result == 0)
				{
					if (repitition == -1)
						result = -next;
					else if (repitition == 1)
						result = next;
					else
						result = repitition * next;
				}
				else
				{
					if (repitition == -1)
						result -= next;
					else if (repitition == 1)
						result += next;
					else
						result += repitition * next;
				}
			}
			return result;
		}
		Collection.IList<Tuple<float, Expression>> CollectTerms(Expression current)
		{
			var result = new Collection.List<Tuple<float, Expression>>();
			if (current is Addition)
			{
				result.Add(this.CollectTerms((current as Addition).Left));
				result.Add(this.CollectTerms((current as Addition).Right));
			}
			else if (current is Subtraction)
			{
				result.Add(this.CollectTerms((current as Subtraction).Left));
				result.Add(this.CollectTerms((current as Subtraction).Right).Map(term => Tuple.Create(-term.Item1, term.Item2)));
			}
			else
			{

				current = current.Simplify();
				if (current is Multiplication && (current as Multiplication).Left is Constant)
					result.Add(Tuple.Create(((current as Multiplication).Left as Constant).Value, (current as Multiplication).Right));
				else if (current is Division)
				{
					current = current.Simplify();
					result.Add(Tuple.Create(1f, current));
				}
				else if (current is Negation)
					result.Add(Tuple.Create(-1f, (current as Negation).Argument));
				else if (current is Power)
				{
					current = current.Simplify();
					result.Add(Tuple.Create(1f, current));
				}
				else
					result.Add(Tuple.Create(1f, current));
			}
			return result;
		}
		#region Object Overrides
		public override bool Equals(Expression other)
		{
			return other is Addition && this.Left == (other as Addition).Left && this.Right == (other as Addition).Right;
		}
		public override int GetHashCode()
		{
			return this.Left.GetHashCode() ^ typeof(Addition).GetHashCode() ^ this.Right.GetHashCode();
		}
		#endregion
	}
}

