//
//  BinaryOperator.cs
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
	public abstract class BinaryOperator :
	Expression
	{
		protected abstract string Symbol { get; }
		public Expression Left { get; private set; }
		public Expression Right { get; private set; }
		protected BinaryOperator() :
			this(0, 0)
		{
		}
		protected BinaryOperator(Expression left, Expression right)
		{
			this.Left = left;
			this.Right = right;
		}
		internal Expression Build(Expression left, Expression right)
		{
			this.Left = left;
			this.Right = right;
			return this;
		}
		#region Object Overrides
		public override string ToString()
		{
			return (this.Left.IsNull() ? "null" : this.Left.ToString(this.Precedence - 1)) + " " + this.Symbol + " " + (this.Right.IsNull() ? "null" : this.Right.ToString(this.Precedence));
		}
		public override bool Equals(Expression other)
		{
			return other is BinaryOperator && this.Symbol == ((BinaryOperator)other).Symbol && this.Left == ((BinaryOperator)other).Left && this.Right == (other as BinaryOperator).Right;
		}
		public override int GetHashCode()
		{
			return this.Left.Hash() ^ this.Symbol.Hash() ^ this.Right.Hash();
		}
		#endregion
	}
}
