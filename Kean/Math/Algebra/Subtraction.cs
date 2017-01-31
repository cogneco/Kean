//
//  Substraction.cs
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

namespace Kean.Math.Algebra
{
	public class Subtraction:
	BinaryOperator
	{
		public override int Precedence { get { return 4; } }
		protected override string Symbol { get { return "-"; } }
		internal Subtraction()
		{
		}
		public Subtraction(Expression left, Expression right) : 
			base(left, right)
		{
		}
		public override float Evaluate(params KeyValue<string, float>[] variables)
		{
			return this.Left.Evaluate(variables) - this.Right.Evaluate(variables);
		}
		public override Expression Derive(string variable)
		{
			return this.Left.Derive(variable) - this.Right.Derive(variable);
		}
		public override Expression Simplify()
		{
			return (this.Left.Simplify() + -this.Right.Simplify()).Simplify();
		}
	}
}

