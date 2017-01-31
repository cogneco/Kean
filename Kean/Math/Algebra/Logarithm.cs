//
//  Logarithm.cs
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
	public class Logarithm :
	Function
	{
		protected override string Symbol
		{ 
			get
			{ 
				return this.Base == 2f ? "lb" :
					this.Base == Single.E ? "ln" :
					this.Base == 10f ? "lg" : "log" + this.Base.AsString();
			} 
		}
		public float Base { get; private set; }
		public Logarithm(float @base, Expression argument) :
			base(argument)
		{
			this.Base = @base;
		}
		public override float Evaluate(params KeyValue<string, float>[] variables)
		{
			return Single.Logarithm(this.Argument.Evaluate(variables), this.Base);
		}
		public override Expression Derive(string variable)
		{
			return 1f / (this.Argument * new Logarithm(Single.E, this.Base)) * this.Argument.Derive(variable);
		}
		public override Expression Simplify()
		{
			Expression argument = this.Argument.Simplify();
			return argument is Constant && ((Constant)argument).Value == this.Base ? (Expression)1f : new Logarithm(this.Base, argument);
		}
	}
}

