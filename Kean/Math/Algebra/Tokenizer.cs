//
//  Tokenizer.cs
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
	public class Tokenizer : 
	Generic.IEnumerable<Expression>
	{
		IO.Text.Builder buffer;
		Collection.IQueue<Expression> queue = new Collection.Queue<Expression>();
		public bool Empty { get { return this.queue.Empty; } }
		public int Count { get { return this.queue.Count; } }
		bool nextMustBeUnaryOperator = true;
		bool NextMustBeUnaryOperator { get { return this.nextMustBeUnaryOperator && this.buffer.IsNull(); } set { this.nextMustBeUnaryOperator = value; } }
		Tokenizer()
		{
		}
		public Expression Peek()
		{
			return this.queue.Peek();
		}
		public Expression Dequeue()
		{
			return this.queue.Dequeue();
		}
		void Flush()
		{
			if (this.buffer.NotNull() && this.buffer.Length > 0)
			{
				string buffer = this.buffer;
				this.queue.Enqueue(char.IsDigit(buffer[0]) ? (Expression)Constant.Parse(buffer) : Variable.Create(buffer));
				this.NextMustBeUnaryOperator = false;
				this.buffer = null;
			}
		}
		void Enqueue(Expression token)
		{
			this.Flush();
			this.queue.Enqueue(token);
			this.NextMustBeUnaryOperator = token is BinaryOperator || token is LeftParenthesis;
		}
		public static Tokenizer Tokenize(string data)
		{
			Tokenizer result = new Tokenizer();
			if (data.NotNull())
				foreach (char c in data)
					switch (c)
					{
						case '-':
							result.Enqueue(result.NextMustBeUnaryOperator ? (Expression)new Negation() : new Subtraction());
							break;
						case '+':
							result.Enqueue(new Addition());
							break;
						case '*':
							result.Enqueue(new Multiplication());
							break;
						case '/':
							result.Enqueue(new Division());
							break;
						case '^':
							result.Enqueue(new Power());
							break;
						case '(':
							result.Enqueue(new LeftParenthesis());
							break;
						case ')':
							result.Enqueue(new RightParenthesis());
							break;
						default:
							if (char.IsWhiteSpace(c))
								result.Flush();
							else
								result.buffer += c;
							break;
					}
			result.Enqueue(null);
			return result;
		}
		#region IEnumerable implementation
		public Generic.IEnumerator<Expression> GetEnumerator()
		{
			var next = this.queue.Dequeue();
			yield return next;
		}
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			yield return this.queue.Dequeue();
		}
		#endregion
	}
}