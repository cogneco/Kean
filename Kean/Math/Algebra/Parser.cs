//
//  Parser.cs
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
using Generic = System.Collections.Generic;

namespace Kean.Math.Algebra
{
	class Parser
	{
		public Parser()
		{
		}
		public Expression Parse(string expression)
		{
			var list = new Collection.List<Expression>();
			var tokens = Tokenizer.Tokenize(expression);
			while (!tokens.Empty)
				list.Add(tokens.Dequeue());
			return this.Parse(list);
		}
		Expression Parse(Generic.IEnumerable<Expression> tokens)
		{
			var enumerator = tokens.GetEnumerator();
			Expression result = null;
			if (enumerator.MoveNext())
				result = this.Parse(int.MaxValue, result, enumerator);
			return result;
		}
		Expression Parse(int precedence, Expression previous, Generic.IEnumerator<Expression> enumerator)
		{
			Expression result = null;
			Expression current = enumerator.Current;
			if (current is Constant || current is Variable)
				result = enumerator.MoveNext() ? this.Parse(precedence, current, enumerator) : current;
			else if (current is UnaryOperator && enumerator.MoveNext())
			{
				result = ((UnaryOperator)current).Build(this.Parse(current.Precedence, null, enumerator));
				result = this.Parse(precedence, result, enumerator);
			}
			else if (current is LeftParenthesis && enumerator.MoveNext())
			{
				result = this.Parse(int.MaxValue, null, enumerator);
				if (previous is Variable) // convert to function
					result = Function.Create(((Variable)previous).Name, result);
				result = enumerator.Current is RightParenthesis && enumerator.MoveNext() ? this.Parse(precedence, result, enumerator) : result;
			}
			else if (current is BinaryOperator && current.Precedence < precedence && enumerator.MoveNext())
			{
				result = ((BinaryOperator)current).Build(previous, this.Parse(current.Precedence, null, enumerator));
				result = this.Parse(precedence, result, enumerator);
			}
			else
				result = previous;
			return result;
		}
	}
}
