//
//  Constant.cs
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
using Single = Kean.Math.Single;

namespace Kean.Math.Algebra
{
	public class Constant :
	Expression
	{
		public override int Precedence { get { return 0; } }
		public float Value { get; private set; }
		string symbol;
		public Constant(float value)
		{
			this.Value = value;
		}
		public override float Evaluate(params KeyValue<string, float>[] variables)
		{
			return this.Value;
		}
		public override Expression Derive(string variable)
		{
			return 0f;
		}
		public override Expression Simplify()
		{
			return this;
		}
		public override bool Equals(Expression other)
		{
			return other is Constant && Single.Absolute(this.Value - ((Constant)other).Value) <= 0.000001f;
		}
		#region Object Overrides
		public override string ToString()
		{
			return this.symbol ?? this.Value.AsString();
		}
		public override int GetHashCode()
		{
			return this.Value.Hash();
		}
		#endregion
		#region Predefined Constants
		public static Constant Pi { get { return new Constant(Single.Pi) { symbol = "pi" }; } }
		public static Constant E { get { return new Constant(Single.E) { symbol = "e" }; } }
		#endregion
		#region Static Parse
		public static Constant Parse(string value)
		{
			return new Constant(value.Parse<float>());
		}
		#endregion
	}
}

