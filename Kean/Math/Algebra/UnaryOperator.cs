//
//  UnaryOperator.cs
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
	public abstract class UnaryOperator :
	Expression
	{
		protected abstract string Symbol { get; }
		public Expression Argument { get; private set; }
		protected UnaryOperator()
		{
		}
		protected UnaryOperator(Expression argument)
		{
			this.Argument = argument;
		}
		internal UnaryOperator Build(Expression argument)
		{
			this.Argument = argument;
			return this;
		}
		#region Object Overrides
		public override string ToString()
		{
			return this.Symbol + this.Argument.ToString(this.Precedence);
		}
		public override bool Equals(Expression other)
		{
			return other is UnaryOperator && this.Symbol == ((UnaryOperator)other).Symbol && this.Argument == ((UnaryOperator)other).Argument;
		}
		public override int GetHashCode()
		{
			return this.Symbol.Hash() ^ this.Argument.Hash();
		}
		#endregion
	}
}

