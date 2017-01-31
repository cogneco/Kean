//
//  Multiplication.cs
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
	public class Multiplication:
	BinaryOperator
	{
		public override int Precedence{ get { return 3; } }
		protected override string Symbol { get { return "*"; } }
		internal Multiplication()
		{
		}
		public Multiplication(Expression left, Expression right) :
			base(left, right)
		{
		}
		public override float Evaluate(params KeyValue<string, float>[] variables)
		{
			return this.Left.Evaluate(variables) * this.Right.Evaluate(variables);
		}
		public override Expression Derive(string variable)
		{
			return this.Left.Derive(variable) * this.Right + this.Left * this.Right.Derive(variable);
		}
		public override Expression Simplify()
		{
			var terms = this.CollectFactors(this);
			Expression result;
			float constant = 1;
			terms.Remove(term =>
			{
				bool r;
				if (r = term.Item2 is Constant)
					constant *= (term.Item2 as Constant).Value;
				else if (r = term.Item2 is Negation && ((Negation)term.Item2).Argument is Constant)
					constant *= -((Constant)((Negation)term.Item2).Argument).Value;
				return r;
			});
			result = constant;
			while (terms.Count > 0)
			{
				Tuple<float, Expression> nextItem = terms.Remove();
				Expression next = nextItem.Item2;
				float repetition = nextItem.Item1;
				terms.Remove(term =>
				{
					bool r;
					if (r = ((term.Item2).Equals(next)))
						repetition += term.Item1;
					return r;
				});
				if (result == 1)
				{

					if (repetition == 1)
						result = next;
					else if (repetition == 0)
						result = 1;
					else
						result = next ^ repetition;
				}
				else
				{
					if (repetition == 1)
						result *= next;
					else if (repetition != 0)
						result *= next ^ repetition;
				}
			}
			return result;
		}
		Collection.IList<Tuple<float, Expression>> CollectFactors(Expression current)
		{
			var result = new Collection.Linked.List<Tuple<float, Expression>>();
			if (current is Multiplication)
			{
				result.Add(this.CollectFactors((current as Multiplication).Left));
				result.Add(this.CollectFactors((current as Multiplication).Right));
			}
			else if (current is Division)
			{
				result.Add(this.CollectFactors((current as Division).Left));
				result.Add(this.CollectFactors((current as Division).Right));
			}
			else
			{
				if (current is Power)
				{
					if ((current as Power).Left is Constant && (current as Power).Right is Constant)
					{
						current = current.Simplify();
						result.Add(Tuple.Create(1f, current));
					}
					else
					{
						Expression variable = (current as Power).Left;
						float value = ((current as Power).Right as Constant).Value;
						result.Add(Tuple.Create(value, variable));
					}
				}
				else
					result.Add(Tuple.Create(1f, current));
			}
			return result;
		}
	}
}

