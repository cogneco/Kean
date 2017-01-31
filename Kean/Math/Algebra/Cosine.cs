//
//  Cosine.cs
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
using Single = Kean.Math.Single;

namespace Kean.Math.Algebra
{
	public class Cosine:
	Function
	{
		protected override string Symbol { get { return "cos"; } }
		public Cosine(Expression argument) : 
			base(argument)
		{
		}
		public override float Evaluate(params KeyValue<string, float>[] variables)
		{
			return Single.Cosine(this.Argument.Evaluate(variables));
		}
		public override Expression Derive(string variable)
		{
			return -(new Sine(this.Argument) * this.Argument.Derive(variable));
		}
		public override Expression Simplify()
		{
			return new Sine(this.Argument.Simplify());
		}
	}
}

