//
//  Expression.cs
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
	public abstract class Expression :
	IEquatable<Expression>
	{
		public abstract int Precedence { get; }
		public float Evaluate()
		{
			return this.Evaluate(new KeyValue<string, float>[0]);
		}
		public abstract float Evaluate(params KeyValue<string, float>[] variables);
		public abstract Expression Derive(string variable);
		public abstract Expression Simplify();
		internal string ToString(int precedence)
		{
			string result = this.ToString();
			if (precedence < this.Precedence)
			//if (this.Precedence > 0)
				result = "(" + result + ")";
			return result;
		}
		#region Equality Overrides and Operators
		public override bool Equals(object other)
		{
			return this.Equals(other as Expression);
		}
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
		public abstract bool Equals(Expression other);
		public static bool operator ==(Expression left, Expression right)
		{
			return left.Same(right) || left.NotNull() && left.Equals(right);
		}
		public static bool operator !=(Expression left, Expression right)
		{
			return !left.Same(right) && (left.IsNull() || !left.Equals(right));
		}
		#endregion
		#region Operators
		public static Subtraction operator -(Expression left, Expression right)
		{
			return new Subtraction(left, right);
		}
		public static Addition operator +(Expression left, Expression right)
		{
			return new Addition(left, right);
		}
		public static Multiplication operator *(Expression left, Expression right)
		{
			return new Multiplication(left, right);
		}
		public static Division operator /(Expression left, Expression right)
		{
			return new Division(left, right);
		}
		public static Modulo operator %(Expression left, Expression right)
		{
			return new Modulo(left, right);
		}
		public static Power operator ^(Expression left, Expression right)
		{
			return new Power(left, right);
		}
		public static Negation operator -(Expression argument)
		{
			return new Negation(argument);
		}
		public static implicit operator Expression(float value)
		{
			return new Constant(value);
		}
		public static explicit operator Expression(string expression)
		{
			return new Parser().Parse(expression);
		}
		#endregion
	}
}
