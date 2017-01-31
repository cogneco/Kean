//
//  Modulo.cs
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

namespace Kean.Math.Algebra
{
	public  class Modulo:
	BinaryOperator
	{
		public override int Precedence { get { return 3; } }
		protected override string Symbol{ get { return "%"; } }
		internal Modulo()
		{
		}
		public Modulo(Expression left, Expression right) :
			base(left, right)
		{
		}
		public override float Evaluate(params KeyValue<string, float>[] variables)
		{
			return this.Left.Evaluate(variables) % this.Right.Evaluate(variables);
		}
		public override Expression Derive(string variable)
		{
			throw new NotImplementedException();
		}
		public override Expression Simplify()
		{
			Expression result;
			Expression left = this.Left.Simplify();
			Expression right = this.Right.Simplify();
			if (left == right)
				result = 0;
			else if (left is Constant && (left as Constant).Value == 0)
				result = 0;
			else if ((left is Constant) && (right is Constant))
				result = (left as Constant).Value % (right as Constant).Value;
			else if ((left is Multiplication) && (left as Multiplication).Right == right)
				result = 0;
			else if (((left as Multiplication).Right == (right as Multiplication).Right) && ((left as Multiplication).Left is Constant) && ((right as Multiplication).Left is Constant))
				result = (((left as Multiplication).Left as Constant).Value % ((right as Multiplication).Left as Constant).Value) * (right as Multiplication).Right;
			else
				result = this;
			return result;
		}
	}
}

